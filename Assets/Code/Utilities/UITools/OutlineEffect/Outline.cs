using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace OutlineCamera
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Renderer))]
    public class Outline : MonoBehaviour
    {
        public Renderer Renderer { get; private set; }

        public int color;
        public bool eraseRenderer;

        [HideInInspector]
        public int originalLayer;
        [HideInInspector]
        public Material[] originalMaterials;

        private void Awake()
        {
            Renderer = GetComponent<Renderer>();
        }

        public void ApplyOutline()
        {
			IEnumerable<OutlineEffect> effects = Camera.allCameras.AsEnumerable()
				.Select(camera => camera.GetComponent<OutlineEffect>())
				.Where(outlineEffect => outlineEffect != null);

			foreach (OutlineEffect effect in effects)
            {
                effect.AddOutline(this);
            }
        }

        private void OnDisable()
        {
            RemoveOutline();
        }

        public void RemoveOutline()
        {
			IEnumerable<OutlineEffect> effects = Camera.allCameras.AsEnumerable()
				.Select(camera => camera.GetComponent<OutlineEffect>())
				.Where(effect => effect != null);

			foreach (OutlineEffect effect in effects)
            {
                effect.RemoveOutline(this);
            }
        }
    }
}