using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameSceneHandler : MonoBehaviour
{
    static GameSceneHandler instance;
    [HideInInspector]
    public GameObject parent;
    [HideInInspector]
    public bool isGameSceneActive;


    private void Awake()
    {
        isGameSceneActive = true; // TODO: Change this when adding Start Menu
        parent = GetObjectsParent();
        
        DontDestroyOnLoad(parent);
    }


    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            instance = this;
        }
    }

    public void EnableGameScene()
    {
        for (int i = 0; i < parent.transform.childCount; ++i)
        {
            var child = parent.transform.GetChild(i).gameObject;
            child.SetActive(true);
        }
        isGameSceneActive = true;
    }

    public void DisableGameScene()
    {
        for (int i = 0; i < parent.transform.childCount; ++i)
        {
            if (parent.transform.GetChild(i).gameObject.tag.Equals(GameData.Tags.SceneHandler))
            {
                continue;
            }
            parent.transform.GetChild(i).gameObject.SetActive(false);
        }
        isGameSceneActive = false;
    }

    public static GameSceneHandler GetInstance()
    {
        return instance;
    }

    GameObject GetObjectsParent()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        GameObject parent = new GameObject();
        parent.name = "Parent";


        foreach (GameObject go in allObjects)
        {
            if (go.activeInHierarchy && go.transform.parent == null)
            {
                go.transform.SetParent(parent.transform);
            }
        }
        return parent;
    }

}
