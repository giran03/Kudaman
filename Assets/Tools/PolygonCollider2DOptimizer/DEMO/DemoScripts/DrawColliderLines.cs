using UnityEngine;

// just for previewing polygoncollider2D lines

namespace Unitycoder.Extras
{
    public class DrawColliderLines : MonoBehaviour
    {
        static Material lineMaterial;

        bool isEnabled = false;

        PolygonCollider2D pc;

        void Awake()
        {
            if (gameObject.GetComponent<SpriteRenderer>() == null) return;
            if (gameObject.GetComponent<PolygonCollider2D>() == null) return;

            pc = gameObject.GetComponent<PolygonCollider2D>();

            isEnabled = true;
        }

        static void CreateLineMaterial()
        {
            if (!lineMaterial)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                var shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                lineMaterial.SetInt("_ZWrite", 0);
            }
        }

        // Will be called after all regular rendering is done
        public void OnRenderObject()
        {
            if (!isEnabled) return;

            if (pc == null)
            {
                pc = gameObject.GetComponent<PolygonCollider2D>();
                if (pc == null) return;
            }

            CreateLineMaterial();
            // Apply the line material
            lineMaterial.SetPass(0);

            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
            GL.MultMatrix(transform.localToWorldMatrix);

            // Draw lines
            GL.Begin(GL.LINES);
            GL.Color(Color.green);

            for (int i = 0; i < pc.pathCount; i++)
            {
                Vector2[] path = pc.GetPath(i);


                for (int j = 1; j < path.Length; j++)
                {

                    GL.Vertex3(path[j - 1].x, path[j - 1].y, transform.position.z);
                    GL.Vertex3(path[j].x, path[j].y, transform.position.z);
                }

                // connect last line
                GL.Vertex3(path[path.Length - 1].x, path[path.Length - 1].y, transform.position.z);
                GL.Vertex3(path[0].x, path[0].y, transform.position.z);
            }

            GL.End();
            GL.PopMatrix();
        }
    }

}
