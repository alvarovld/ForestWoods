using UnityEngine;

public class ObjectAmountTracker : MonoBehaviour
{
    private static ObjectAmountTracker instance = null;

    ObjectAmountList<string> objectTrackerList = new ObjectAmountList<string>();

    const string fileName = "ObjectAmount.txt";

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            instance.gameObject.SetActive(true);
            Destroy(gameObject);
        }

        objectTrackerList = new ObjectAmountList<string>();
        //LoadPreLoadedObjects();
    }

    public void AddOneObject(string item)
    {
        objectTrackerList.AddOneObject(item);
    }

    public static ObjectAmountTracker GetInstance()
    {
        return instance;
    }

    void Serialize()
    {
        Serializer.Serialize(objectTrackerList, fileName);
    }

    ObjectAmountList<string> Deserialize()
    {
        return Serializer.Deserialize<ObjectAmountList<string>>(fileName);
    }

    void LoadPreLoadedObjects()
    {
        var obejctList = Deserialize();
        if(obejctList == null)
        {
            Debug.Log("No object to load");
            return;
        }
        ObjectPoolManager.GetInstance().LoadPreloadedObjects(obejctList);
    }


    private void OnApplicationQuit()
    {
        Serialize();
    }

}
