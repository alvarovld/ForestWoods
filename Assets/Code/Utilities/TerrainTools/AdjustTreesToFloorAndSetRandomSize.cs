using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class AdjustTreesToFloorAndSetRandomSize : MonoBehaviour
    {
        void Adjust()
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                var child = transform.GetChild(i);
                child.position = TerrainHelper.AdjustPositionToFloor(child.position, -1.5f);
                SetRandomRotation(child);
                setRandomSizS(child);
            }
        }

        void setRandomSizS(Transform child)
        {
            var scale = child.localScale;
            scale *= Random.Range(0.6f, 1.4f);
            child.localScale = scale;
        }

        void SetRandomRotation(Transform child)
        {
            child.rotation = Quaternion.Euler(child.transform.rotation.eulerAngles.x,
                                                Random.Range(0, 360),
                                                child.transform.rotation.eulerAngles.z);
        }

        private void Start()
        {
            Adjust();
        }
    }
}
