using UnityEngine;
using UnityEngine.UI;

// only for debugging purposes, displays polygoncollider2D stats
// NOTE: stats do not update correctly for ResetPolygonCollider2D(), probably misses the update by 1 frame..

namespace Unitycoder.Extras
{
    public class GetSpriteColliderStats : MonoBehaviour
    {

        public GameObject target;

        void Start()
        {
            GetStats();
        }

        void GetStats()
        {
            var statsText = GetComponent<Text>();

            var pc = target.GetComponent<PolygonCollider2D>();

            var pathCount = pc.pathCount;
            var vertexCount = pc.GetTotalPointCount();

            statsText.text = ("POLYGONCOLLIDER2D\nPathCount=" + pathCount + "\nVertexCount=" + vertexCount);
        }

        public void UpdateColliderStats()
        {
            GetStats();
        }
    }
}