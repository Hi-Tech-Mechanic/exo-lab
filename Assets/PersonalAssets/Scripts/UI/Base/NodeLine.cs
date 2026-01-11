namespace ExoLab.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class NodeLine : MaskableGraphic
    {
        public Vector2 startPoint;
        public Vector2 endPoint;
        public float thickness = 3f;
        public int segments = 20;

        private void Update()
        {
            this.SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            // Рассчитываем контрольные точки для S-образного изгиба
            float horizontalDistance = Mathf.Abs(endPoint.x - startPoint.x);
            float offset = horizontalDistance * 0.5f;

            Vector2 control1 = startPoint + Vector2.right * offset;
            Vector2 control2 = endPoint - Vector2.right * offset;

            Vector2 prevPos = startPoint;

            for (int i = 1; i <= segments; i++)
            {
                float t = i / (float)segments;
                Vector2 currentPos = this.CalculateBezier(startPoint, control1, control2, endPoint, t);

                this.DrawLineSegment(prevPos, currentPos, vh);
                prevPos = currentPos;
            }
        }

        private Vector2 CalculateBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            return Mathf.Pow(1 - t, 3) * p0 +
                   3 * Mathf.Pow(1 - t, 2) * t * p1 +
                   3 * (1 - t) * Mathf.Pow(t, 2) * p2 +
                   Mathf.Pow(t, 3) * p3;
        }

        private void DrawLineSegment(Vector2 start, Vector2 end, VertexHelper vh)
        {
            Vector2 dir = (end - start).normalized;
            Vector2 normal = new Vector2(-dir.y, dir.x) * thickness * 0.5f;

            UIVertex v = UIVertex.simpleVert;
            v.color = color;

            int index = vh.currentVertCount;

            v.position = start - normal; vh.AddVert(v);
            v.position = start + normal; vh.AddVert(v);
            v.position = end + normal; vh.AddVert(v);
            v.position = end - normal; vh.AddVert(v);

            vh.AddTriangle(index, index + 1, index + 2);
            vh.AddTriangle(index, index + 2, index + 3);
        }
    }
}
