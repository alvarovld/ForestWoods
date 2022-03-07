using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;
using Utils;
using Keys = GameData.Keys;

public class InputHandler : MonoBehaviour
{
    private static InputHandler instance;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            instance.gameObject.SetActive(true);
            Destroy(gameObject);
            return;
        }
    }

    public static InputHandler GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        InputEventManager.GetInstance().SetNewEvent(HandleOpenCloseInventory, Keys.INVENTORY, false);
    }

    public void CloseInventory()
    {
        GameObjectRefs.player.gameObject.GetComponent<FirstPersonController>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObjectRefs.canvas.SetActive(true);

        SceneManager.UnloadSceneAsync("Inventory", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        GameSceneHandler.GetInstance().isGameSceneActive = true;
    }


    public void OpenInventory()
    {
        GameObjectRefs.player.gameObject.GetComponent<FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameObjectRefs.canvas.SetActive(false);


        Transform player = GameObjectRefs.player;
        SceneManager.LoadScene("Inventory", LoadSceneMode.Additive);
        PlayerInfo.position = player.transform.position;
        PlayerInfo.rotation = player.transform.rotation;
        GameSceneHandler.GetInstance().isGameSceneActive = false;
    }

    void HandleOpenCloseInventory()
    {
        if(!GameSceneHandler.GetInstance().isGameSceneActive)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }


    void HandleMeleeAttack()
    {
        if (Input.GetMouseButtonDown(Keys.ATTACK))
        {
            //GetComponent<MeleeAttackHandler>().Attack();
        }    
    }
}
