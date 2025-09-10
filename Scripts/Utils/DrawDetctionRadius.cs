using UnityEngine;

namespace DoDoDoIt
{
    public class DrawDetectionRadius
    {
        public float detectionRadius = 20f; // 범위 반경
        public int segments = 50; // 원을 그릴 때 사용할 세그먼트 수
        public Color drawColor = Color.red; // 라인 색상

        // 범위를 그리기 위한 메서드
        public void DrawCircle(Vector3 center)
        {
            float angle = 0f;
            for (int i = 0; i < segments; i++)
            {
                float x1 = Mathf.Sin(angle) * detectionRadius;
                float z1 = Mathf.Cos(angle) * detectionRadius;
                float x2 = Mathf.Sin(angle + (2 * Mathf.PI / segments)) * detectionRadius;
                float z2 = Mathf.Cos(angle + (2 * Mathf.PI / segments)) * detectionRadius;

                Vector3 point1 = new Vector3(center.x + x1, center.y, center.z + z1);
                Vector3 point2 = new Vector3(center.x + x2, center.y, center.z + z2);

                Debug.DrawLine(point1, point2, drawColor);
                angle += (2 * Mathf.PI / segments);
            }
        }
    }
}