using UnityEngine;
using System.Collections.Generic;

namespace Unitycoder.Runtime
{
    public class RuntimeSpriteOptimizer : MonoBehaviour
    {

        public enum ScalingMode
        {
            EstimateNeighbors, // Best if collider has holes inside
            StandardScaling,
            NormalizedScaling,
            NormalizedScalingLocalCenter // Best if no holes in collider
        }


        ScalingMode scalingMode = ScalingMode.EstimateNeighbors;
        float angleThreshold = 18.0f; // 18 seems good values
        float shrinkMultiplier = 0.98f; // 0.98 seems good value
        int origVertCount, cleanVertCount, removedVertPercent;


        public void OptimizeCollider()
        {
            OptimizePolygon2DCollider(gameObject);
        }

        public void ScaleCollider()
        {
            ScaleColliders(gameObject, scaleDown: true);
        }

        public void KeepLargestColliderPathOnly()
        {
            KeepLargestPath(gameObject);
        }

        public void RemoveInnerColliderPaths()
        {
            RemoveInternalPathsComplex(gameObject);
        }


        public void ResetCollider()
        {
            ResetPolygonCollider2D(gameObject);
        }

        // optimizes given gameobject
        void OptimizePolygon2DCollider(GameObject go)
        {
            // validate selected object
            if (go.GetComponent<SpriteRenderer>() == null || go.GetComponent<PolygonCollider2D>() == null)
            {
                return; // no polygon2D collider
            }

            PolygonCollider2D pc = go.GetComponent<PolygonCollider2D>();
            List<Vector2> newVerts = new List<Vector2>();

            for (int i = 0; i < pc.pathCount; i++)
            {
                if (pc.GetPath(i).Length < 4) continue; // path needs atleast 4 vertices

                Vector2[] path = pc.GetPath(i);
                float angle1 = 0;
                float angle2 = 0;
                newVerts.Clear();
                int mx = path.Length;
                Vector2 currentDir = (path[0] - path[1]).normalized;
                angle1 = Vector2.Angle(path[0] - path[1], currentDir);
                for (int j = 0; j < mx; j++)
                {
                    int sPrev = (((j - 1) % mx) + mx) % mx;
                    int sNext = (((j + 1) % mx) + mx) % mx;
                    angle1 = Vector2.Angle(path[sPrev] - path[j], currentDir);
                    angle2 = Vector2.Angle(path[j] - path[sNext], currentDir);
                    if (angle1 > angleThreshold || angle2 > angleThreshold)
                    {
                        currentDir = (path[j] - path[sNext]).normalized;
                        newVerts.Add(path[j]);
                    }
                } // loop each vert in path

                // update collider
                pc.SetPath(i, newVerts.ToArray());
            }
        }

        void ScaleColliders(GameObject go, bool scaleDown)
        {
            // validate selected object
            if (go.GetComponent<SpriteRenderer>() == null || go.GetComponent<PolygonCollider2D>() == null)
            {
                return; // no polygon2D collider
            }

            PolygonCollider2D pc = go.GetComponent<PolygonCollider2D>();
            List<Vector2> newVerts = new List<Vector2>();

            // loop all paths
            for (int i = 0; i < pc.pathCount; i++)
            {
                Vector2[] path = pc.GetPath(i);

                newVerts.Clear();

                // shrink
                int mx = path.Length;
                for (int s = 0; s < mx; s++)
                {
                    switch (scalingMode)
                    {
                        case ScalingMode.EstimateNeighbors:
                            int sPrev = (((s - 1) % mx) + mx) % mx;
                            int sNext = (((s + 1) % mx) + mx) % mx;
                            Vector2 pdir = (path[sPrev] - path[sNext]).normalized;
                            if (scaleDown)
                            {
                                newVerts.Add(path[s] - GetPerpendicular(pdir) * (1 - shrinkMultiplier));
                            }
                            else
                            {
                                newVerts.Add(path[s] + GetPerpendicular(pdir) * (1 - shrinkMultiplier));
                            }
                            break;
                        case ScalingMode.StandardScaling:
                            if (scaleDown)
                            {
                                newVerts.Add(path[s] * shrinkMultiplier);
                            }
                            else
                            {
                                newVerts.Add(path[s] * (1 + (1 - shrinkMultiplier)));
                            }
                            break;
                        case ScalingMode.NormalizedScaling:
                            if (scaleDown)
                            {
                                newVerts.Add(path[s] - path[s].normalized * (1 - shrinkMultiplier));
                            }
                            else
                            {
                                newVerts.Add(path[s] + path[s].normalized * (1 - shrinkMultiplier));
                            }
                            break;
                        case ScalingMode.NormalizedScalingLocalCenter:
                            Vector2 center = GetCentroid(path);
                            if (scaleDown)
                            {
                                Vector2 temp = (path[s] - center) - (path[s] - center).normalized * (1 - shrinkMultiplier);
                                newVerts.Add(temp + center);
                            }
                            else
                            {
                                Vector2 temp = path[s] - center + (path[s] - center).normalized * (1 - shrinkMultiplier);
                                newVerts.Add(temp + center);
                            }
                            break;
                        default:
                            Debug.LogWarning("Unknown scaling mode:" + scalingMode);
                            break;
                    }

                    // TODO: scale based on local path center?


                } // looped each vert
                  // update collider
                pc.SetPath(i, newVerts.ToArray());
            } // each path

            // cleanup
            newVerts.Clear();
        } // ScaleColliders


        // removes paths, only largest is left
        void KeepLargestPath(GameObject go)
        {
            if (go.GetComponent<PolygonCollider2D>() != null)
            {
                PolygonCollider2D pc = go.GetComponent<PolygonCollider2D>();

                if (pc.pathCount > 1)
                {
                    // get bounds of each path
                    int biggestPath = -1;
                    float biggestSize = -1;
                    for (int i = 0; i < pc.pathCount; i++)
                    {
                        origVertCount += pc.GetPath(i).Length;

                        // get bounds
                        float xMin = Mathf.Infinity;
                        float yMin = Mathf.Infinity;
                        float xMax = -Mathf.Infinity;
                        float yMax = -Mathf.Infinity;

                        // find edges

                        for (int j = 0; j < pc.GetPath(i).Length; j++)
                        {
                            xMin = Mathf.Min(xMin, pc.GetPath(i)[j].x);
                            xMax = Mathf.Max(xMax, pc.GetPath(i)[j].x);
                            yMin = Mathf.Min(yMin, pc.GetPath(i)[j].y);
                            yMax = Mathf.Max(yMax, pc.GetPath(i)[j].y);
                        }

                        float area = (xMax - xMin) * (yMax - yMin);

                        if (area > biggestSize)
                        {
                            biggestSize = area;
                            biggestPath = i;
                        }
                    }

                    // assign it
                    pc.SetPath(0, pc.GetPath(biggestPath));
                    //						pc.SetPath (0, pc.GetPath(0));
                    pc.pathCount = 1;

                    cleanVertCount += pc.GetTotalPointCount();

                }
            } // if pol2D collider

        } // keeplargetspath

        // remove paths less than amount of verts
        void FilterPaths(GameObject go, int filterAmount)
        {
            // this gameobject
            if (go.GetComponent<PolygonCollider2D>() != null)
            {
                PolygonCollider2D pc = go.GetComponent<PolygonCollider2D>();

                // have more than 1?
                if (pc.pathCount > 1)
                {
                    origVertCount = pc.GetTotalPointCount();
                    List<int> keep = new List<int>();

                    // check each path
                    for (int i = 0; i < pc.pathCount; i++)
                    {
                        if (pc.GetPath(i).Length >= filterAmount)
                        {
                            if (!keep.Contains(i))
                            {
                                keep.Add(i);
                            }
                        }
                    } // each path

                    List<Vector2[]> keepPaths = new List<Vector2[]>();
                    for (int i = 0; i < keep.Count; i++)
                    {
                        keepPaths.Add(pc.GetPath(keep[i]));
                    }
                    for (int i = 0; i < keepPaths.Count; i++)
                    {
                        pc.SetPath(i, keepPaths[i]);
                    }
                    pc.pathCount = keepPaths.Count;
                }
            } // if pol2D collider
        } // filterpaths

        // removes internal paths (point inside polygon test)
        void RemoveInternalPathsComplex(GameObject go)
        {
            {
                // this gameobject
                if (go.GetComponent<PolygonCollider2D>() != null)
                {
                    PolygonCollider2D pc = go.GetComponent<PolygonCollider2D>();
                    origVertCount += pc.GetTotalPointCount();

                    // more than 1
                    if (pc.pathCount > 1)
                    {
                        List<int> keep = new List<int>();

                        // check each path
                        for (int i = 0; i < pc.pathCount; i++)
                        {
                            // compare each path
                            for (int j = 0; j < pc.pathCount; j++)
                            {
                                // if not the same path
                                if (i != j)
                                {
                                    bool isInside = false;

                                    // compare each point of inner path to outer path
                                    for (int k = 0; k < pc.GetPath(j).Length; k++)
                                    {
                                        // is it inside anything?
                                        if (IsPointInPolygon(pc.GetPath(i), pc.GetPath(j)[k]))
                                        {
                                            if (keep.Contains(j))
                                            {
                                                keep.Remove(j);
                                            }
                                            isInside = true;
                                            break;
                                        }
                                        else
                                        {
                                            //isInside = false;
                                        }
                                    }

                                    // if we didnt find any verts inside
                                    if (!isInside)
                                    {
                                        // then add to keep paths list
                                        if (!keep.Contains(j))
                                        {
                                            keep.Add(j);
                                        }
                                    }

                                }

                            }
                        } // each path

                        // now take those paths that are for keeps
                        List<Vector2[]> keepPaths = new List<Vector2[]>();
                        keepPaths.Clear();
                        for (int i = 0; i < keep.Count; i++)
                        {
                            keepPaths.Add(pc.GetPath(keep[i]));
                        }
                        pc.pathCount = keepPaths.Count;
                        for (int i = 0; i < keepPaths.Count; i++)
                        {
                            pc.SetPath(i, keepPaths[i]);
                        }


                    } // more than 1 path
                } // if pol2D collider
            } // for each go
        } // removeinternalpathscomplex

        void ResetPolygonCollider2D(GameObject go)
        {
            if (go.GetComponent<PolygonCollider2D>() != null)
            {
                bool isTrigger = go.GetComponent<PolygonCollider2D>().isTrigger;
                PhysicsMaterial2D physMat = go.GetComponent<PolygonCollider2D>().sharedMaterial;

                Destroy(go.GetComponent<PolygonCollider2D>());
                go.AddComponent<PolygonCollider2D>();

                go.GetComponent<PolygonCollider2D>().isTrigger = isTrigger;
                go.GetComponent<PolygonCollider2D>().sharedMaterial = physMat;
            }
        }


        //http://www.java2s.com/Code/CSharp/Development-Class/PerpendicularVector2.htm
        Vector2 GetPerpendicular(Vector2 original)
        {
            float x = original.x;
            float y = original.y;
            y = -y;
            return new Vector2(y, x);
        }

        // http://stackoverflow.com/a/16841009
        Vector2 GetCentroid(Vector2[] points)
        {
            float area = 0.0f;
            float Cx = 0.0f;
            float Cy = 0.0f;
            float tmp = 0.0f;
            int k;

            for (int i = 0; i <= (points.Length - 1); i++)
            {
                k = (i + 1) % ((points.Length - 1) + 1);
                tmp = points[i].x * points[k].y - points[k].x * points[i].y;
                area += tmp;
                Cx += (points[i].x + points[k].x) * tmp;
                Cy += (points[i].y + points[k].y) * tmp;
            }
            area *= 0.5f;
            Cx *= 1.0f / (6.0f * area);
            Cy *= 1.0f / (6.0f * area);

            return new Vector2(Cx, Cy);
        }

        //http://dominoc925.blogspot.fi/2012/02/c-code-snippet-to-determine-if-point-is.html
        private bool IsPointInPolygon(Vector2[] polygon, Vector2 point)
        {
            bool isInside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&
                    (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x))
                {
                    isInside = !isInside;
                }
            }
            return isInside;
        }

    }
}
