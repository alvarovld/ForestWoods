using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public static class GameObjectRefs
    {
        public static GameObject sceneParent;
        public static Transform player;
        public static Camera camera;
        public static GameObject canvas;

        [RuntimeInitializeOnLoadMethod]
        static void Start()
        {
            if (GameSceneHandler.GetInstance().parent == null)
            {
                Debug.LogError("Scene Handler not attached to an object in the current initial scene");
                return;
            }
            sceneParent = GameSceneHandler.GetInstance().parent;
            camera = Camera.main;
            if(camera == null)
            {
                camera = GameObject.FindGameObjectWithTag(GameData.Tags.MainCamera).GetComponent<Camera>();
            }
            player = GameObject.FindGameObjectWithTag(GameData.Tags.Player).transform;
            canvas = GameObject.Find("Canvas");
            if(canvas == null)
            {
                canvas = new GameObject("Canvas");
                canvas.AddComponent<Canvas>();
                canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            }

        }


        public static Transform GetChildByTag(GameObject obj, string tag)
        {
            foreach (Transform child in obj.transform)
            {
                if (child.gameObject.tag.Equals(tag))
                {
                    return child;
                }
            }
            Debug.LogError("[GameObjectFinder] Child not found, tag: "+tag+" Object tag: "+obj.tag);
            return null;
        }
    }
}
