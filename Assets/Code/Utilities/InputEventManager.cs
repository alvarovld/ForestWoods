using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

public class InputEventManager : MonoBehaviour
{
    static InputEventManager instance;
    private struct KeyInputEvent
    {
        public string key;
        public Action action;
        public bool oneShot;
    }

    private struct MouseInputEvent
    {
        public int button;
        public Action action;
        public bool oneShot;
    }

    private static ICollection<KeyInputEvent> keyEvents = new List<KeyInputEvent>();
    private static ICollection<MouseInputEvent> mouseEvents = new List<MouseInputEvent>();

    private void Awake()
    {
        if (instance == null)
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



    public void SetNewEvent(Action ev, string key, bool oneShot)
    {
        KeyInputEvent inputEv = new KeyInputEvent
        {
            key = key,
            action = ev,
            oneShot = oneShot,
        };

        if(keyEvents.Contains(inputEv))
        {
            return;
        }

        keyEvents.Add(inputEv);
    }

    public void SetNewEvent(Action ev, int mouseButton, bool oneShot)
    {
        MouseInputEvent inputEv = new MouseInputEvent
        {
            button = mouseButton,
            action = ev,
            oneShot = oneShot,
        };
        mouseEvents.Add(inputEv);
    }


    public bool RemoveEvent(Action ev, string key, bool oneShot)
    {
        KeyInputEvent inputEv = new KeyInputEvent
        {
            key = key,
            action = ev,
            oneShot = oneShot,
        };

        if(keyEvents.Contains(inputEv))
        {
            keyEvents.Remove(inputEv);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RemoveEvent(Action ev, int mouseButton, bool oneShot)
    {
        MouseInputEvent inputEv = new MouseInputEvent
        {
            button = mouseButton,
            action = ev,
            oneShot = oneShot,
        };

        if (mouseEvents.Contains(inputEv))
        {
            mouseEvents.Remove(inputEv);
            return true;
        }
        else
        {
            return false;
        }
    }



    public static InputEventManager GetInstance()
    {
        return instance;
    }

    private void Update()
    {
        foreach (var it in keyEvents.Reverse())
        {
            if (Input.GetKeyDown(it.key))
            {
                it.action?.Invoke();
                if (it.oneShot || it.action == null)
                {
                    keyEvents.Remove(it);
                }
            }
        }

        foreach (var it in mouseEvents.Reverse())
        {
            if (Input.GetMouseButtonDown(it.button))
            {
                it.action?.Invoke();
                if (it.oneShot)
                {
                    mouseEvents.Remove(it);
                }
            }
        }
    }
}
