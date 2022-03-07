using UnityEngine;

namespace Utils
{
    public static class TerrainHelper
    {
        const float height = 2000;
        const float maxSearchDistance = 2300;

        static RaycastHit[] GetVerticalRaycastedObjects(Vector2 coordinates)
        {
            Vector3 origin = new Vector3(coordinates.x, height, coordinates.y);
            Ray ray = new Ray(origin, Vector3.down);

            return Physics.RaycastAll(ray, maxSearchDistance, LayerMask.GetMask("Terrain"));
        }

        public static Vector3 AdjustPositionToFloor(Vector3 position)
        {
            return AdjustPositionToFloor(position, 0);
        }

        public static Vector3 AdjustPositionToFloor(GameObject obj)
        {
            return AdjustPositionToFloor(obj.transform.position);
        }

        public static Vector3 AdjustPositionToFloor(Vector2 planePosition)
        {
            return AdjustPositionToFloor(new Vector3(planePosition.x, 0, planePosition.y), 0);
        }

        public static Vector3 AdjustPositionToFloor(Vector3 position, float yOffset)
        {
            var hits = GetVerticalRaycastedObjects(new Vector2(position.x, position.z));

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.tag.Equals(GameData.Tags.Terrain))
                {
                    return new Vector3(position.x, hit.point.y + yOffset, position.z);
                }
            }

            /*Debug.LogWarning("[TerrainHelper] [adjustPositionToFloor] Terrain not found, position: " +
                position + " floorOffset: " + yOffset + ". Maybe you have to increase the search distance, current is: "+ maxSearchDistance);*/
            return position;
        }

        public static float GetDistanceToFloor(Vector3 position)
        {
            var hits = GetVerticalRaycastedObjects(new Vector2(position.x, position.z));

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.tag.Equals(GameData.Tags.Terrain))
                {
                    return (hit.point - position).magnitude;
                }
            }
            Debug.LogWarning("[TerrainHelper] [getDistanceToFloor] Terrain not found");
            return -1;
        }

        public static Vector3 GetFloorProjection(Vector3 position)
        {
            var hits = GetVerticalRaycastedObjects(new Vector2(position.x, position.z));

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.tag.Equals(GameData.Tags.Terrain))
                {
                    return hit.point;
                }
            }
            Debug.LogWarning("[TerrainHelper] [getYProjectionPointOnFloor] Terrain not found");
            return Vector3.zero;
        }
    }

}
