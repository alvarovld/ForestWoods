using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class ActionSwitcher : MonoBehaviour
{
    ActionSwitcherTextWrapper actionSwitcherTextWrapper;

    public SortedList actions = new SortedList();
    Vector3 offset;
    public Transform target;
    KeyValuePair<string, Action> currentAction;
    Dictionary<string, Action> actionExecutedCallbackList;
    public bool executeOnlyOnce;

    private void Awake()
    {
        executeOnlyOnce = false;
        actionExecutedCallbackList = new Dictionary<string, Action>();
    }

    public void RemoveAllActionsExcept(string key)
    {
        if(actions.Contains(key))
        {
            currentAction = new KeyValuePair<string, Action>(key, (Action)actions[key]);
        }
        else
        {
            return;
        }
        foreach(var actionKey in actions.Keys)
        {
            if(!actionKey.Equals(key))
            {
                actions.Remove(key);
                UpdateActionTextSwitcherComponents();
            }
        }
    }

    private void OnDisable()
    {
        ResetToDefault();
    }

    public void RemoveAllActions()
    {
        actions.Clear();
        UpdateActionTextSwitcherComponents();
        currentAction = new KeyValuePair<string, Action>();
        actionExecutedCallbackList.Clear();
    }

    void ResetToDefault()
    {
        actions.Clear();
        UpdateActionTextSwitcherComponents();
        actionSwitcherTextWrapper.ReturnToPool();
        actionExecutedCallbackList.Clear();
    }

    private void Update()
    {
        if (currentAction.Key == null)
        {
            return;
        }

        if (Input.GetKeyDown(GameData.Keys.ACTION_SWITCHER))
        {
            SwitchAction();
            return;
        }
        if (Input.GetKeyDown(GameData.Keys.ACTION))
        {
            ExecuteAction();
        }
    }

    void ExecuteAction()
    {
        actionSwitcherTextWrapper.EnableAllExceptSwitchButton();
        currentAction.Value();

        if (currentAction.Key != null && actionExecutedCallbackList.ContainsKey(currentAction.Key))
        {
            actionExecutedCallbackList[currentAction.Key]?.Invoke();
        }

        if (executeOnlyOnce)
        {
            RemoveActionIfExists(currentAction.Key);
            if (actions.Count == 0)
            {
                ResetToDefault();
            }
        }
    }

    private void OnGUI()
    {
        if (actionSwitcherTextWrapper != null && 
            !actionSwitcherTextWrapper.IsEmpty())
        {
            DrawActionTextUI();
        }
    }

    public void AddActionIfNotExist(string text, Action action)
    {
        AddActionIfNotExist(text, action, null);
    }

    public void AddActionIfNotExist(string text, Action action, Action callback)
    {
        if (actionSwitcherTextWrapper == null)
        {
            actionSwitcherTextWrapper = new ActionSwitcherTextWrapper();
        }

        Vector3 textPos = GameObjectRefs.camera.WorldToScreenPoint(target.position + offset);

        if(actionSwitcherTextWrapper.IsEmpty())
        {
            actionSwitcherTextWrapper.FillFromPool(textPos);
        }

        if (actions.ContainsKey(text))
        {
            return;
        }

        currentAction = new KeyValuePair<string, Action>(text, action);

        if (callback != null)
        {
            actionExecutedCallbackList.Add(text, callback);
        }

        actions.Add(text, action);
        UpdateActionTextSwitcherComponents();
    }

    public void RemoveActionIfExists(string text)
    {
        if(text == null)
        {
            return;
        }
        if (!actions.ContainsKey(text))
        {
            return;
        }
        
        actions.Remove(text);
        UpdateActionTextSwitcherComponents();

        actionExecutedCallbackList.Remove(text);

        if(actions.Count == 0)
        {
            actionSwitcherTextWrapper.Enabled(false);
        }

        SwitchAction();
    }

    public void SetPositionProperties(Transform target, Vector3 offset)
    {
        this.target = target;
        this.offset = offset;
    }


    void UpdateActionTextSwitcherComponents()
    {
        if (actions.Count == 0)
        {
            actionSwitcherTextWrapper.DisableAllButtons();
            return;
        }
        if (actions.Count == 1)
        {
            actionSwitcherTextWrapper.EnableAllExceptSwitchButton();
        }
        else
        {
            actionSwitcherTextWrapper.EnableAllButtons();
        }
    }

    void DrawActionTextUI()
    {
        actionSwitcherTextWrapper.SetActionText(currentAction.Key);
        actionSwitcherTextWrapper.SetPosition(GameObjectRefs.camera.WorldToScreenPoint(target.position + offset));
        actionSwitcherTextWrapper.Show();
    }

    void SwitchAction()
    {
        int index = actions.IndexOfKey(currentAction.Key) + 1;

        if(actions.Count == 0)
        {
            currentAction = new KeyValuePair<string, Action>();
            return;
        }

        if(index >= actions.Count || index < 0)
        {
            index = 0;
        }

        currentAction = new KeyValuePair<string, Action>((string)actions.GetKey(index), (Action)actions.GetByIndex(index));
    }

    private class ActionSwitcherTextWrapper
    {
        GameObject obj;
        GameObject switchText;
        GameObject actionText;
        GameObject selectText;

        public ActionSwitcherTextWrapper()
        {
        }

        public void Enabled(bool value)
        {
            obj.SetActive(value);
        }

        public bool IsEmpty()
        {
            return obj == null;
        }

        public void FillFromPool(Vector3 position)
        {
            if(obj)
            {
                return;
            }
            obj = ObjectPoolManager.GetInstance().GetDisabledObjectFromPool(GameData.Tags.ActionSwitcherText, position);
            obj.transform.SetParent(GameObjectRefs.canvas.transform);

            switchText = obj.transform.GetChild(0).gameObject;
            actionText = obj.transform.GetChild(1).gameObject;
            selectText = obj.transform.GetChild(2).gameObject;

            SetTextColors();
        }

        void SetTextColors()
        {
            var texts = obj.transform.GetComponentsInChildren<Text>();

            foreach (var text in texts)
            {
                text.color = GameData.Parameters.textColor;
            }
        }

        public void Show()
        {
            obj.SetActive(true);
        }

        public void ReturnToPool()
        {
            if(!obj)
            {
                return;
            }

            DisableAllButtons();
            ObjectPoolManager.GetInstance().ReturnObjectToPoolDisabling(obj);
            obj = null;
            switchText = null;
            actionText = null;
            selectText = null;
        }

        bool IsAnyObjectNull()
        {
            return (obj == null || selectText == null || switchText == null || actionText == null);
        }

        public void EnableAllExceptSwitchButton()
        {
            if(IsAnyObjectNull())
            {
                return;
            }
            Enable(false, true, true);
        }

        public void EnableAllButtons()
        {
            if (IsAnyObjectNull())
            {
                return;
            }
            Enable(true, true, true);
        }

        public void DisableAllButtons()
        {
            if (IsAnyObjectNull())
            {
                return;
            }
            Enable(false, false, false);
        }

        public void SetActionText(string text)
        {
            actionText.GetComponent<Text>().text = text;
            obj.transform.GetComponent<HorizontalLayoutGroup>().enabled = false;
            obj.transform.GetComponent<HorizontalLayoutGroup>().enabled = true;
        }

        public void SetPosition(Vector3 position)
        {
            obj.transform.position = position;
        }

        void Enable(bool switchT, bool select, bool action)
        {
            switchText.SetActive(switchT);
            selectText.SetActive(select);
            actionText.SetActive(action);
        }

    }

}

