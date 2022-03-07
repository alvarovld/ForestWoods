using UnityEngine;

namespace Utils
{
    public static class CameraView
    {
        public static bool PositionInCameraView(Vector3 position)
        {
            var point = GameObjectRefs.camera.WorldToViewportPoint(position);
            return point.x >= -0.1 && point.x <= 2 && point.y >= -1 && point.y <= 2 && point.z > -1;
        }
    }
}