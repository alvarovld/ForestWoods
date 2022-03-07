using System;
using UnityEngine;

public class FadeCamera : MonoBehaviour
{
    public AnimationCurve fadeCurve; 

    private float alpha = 1;
    private Texture2D texture;
    private bool done;
    private float time;
    Action callback;

    private void Awake()
    {
        done = true;
    }

    public void Reset()
    {
        done = false;
        alpha = 1;
        time = 0;
    }

    public void Fade(Action callback)
    {
        this.callback = callback;
        Reset();
    }

    public void OnGUI()
    {
        if (done)
        {
            return;
        }

        if (texture == null)
        {
            texture = new Texture2D(1, 1);
        }

        texture.SetPixel(0, 0, new Color(0, 0, 0, alpha));
        texture.Apply();

        time += Time.deltaTime;
        alpha = fadeCurve.Evaluate(time);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
       
        if(alpha >= 2)
        {
            callback();
        }

        if (alpha <= 0)
        {
            done = true;
        }
    }
}