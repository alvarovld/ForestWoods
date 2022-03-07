using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DayNightCycle : MonoBehaviour
{
    public float dayDuration;
    [Range(0,24)]
    public int startTime;
    [Range(1,100)]
    public int sunSetPortionOfTheDay;
    float rotationInterval;
    float rotationRate;

    [Header("Intensity")]
    public float sunSetAngleFromZero;


    public float lightIntensityChangeRate;
    public float maxLight;

    [Header("Color")]
    public Color ambientColor;
    public Color sunsetColor;

    float rChange;
    float gChange;
    float bChange;
    Light lightObj;

    int nOfSunSetRotMovements;

    float xRotation;
    Color currentColor;


    public void StartCycle()
    {
        xRotation = 0;
        rotationRate = 0.04f;

        CalculateRotationIntervals();
        lightObj = GetComponent<Light>();
        lightObj.color = sunsetColor;
        currentColor = lightObj.color;

        rChange = ambientColor.r - sunsetColor.r;
        gChange = ambientColor.g - sunsetColor.g;
        bChange = ambientColor.b - sunsetColor.b;

        StartCoroutine(MoveSun());
    }

    void CalculateRotationIntervals()
    {
        int numberOfRotations = Mathf.RoundToInt(dayDuration / rotationRate);
        rotationInterval = 360f / numberOfRotations;

        float sunSetDuration = dayDuration * sunSetPortionOfTheDay / 100;

        int numberOfSunSetRotations = Mathf.RoundToInt(sunSetDuration / rotationRate);
        nOfSunSetRotMovements = numberOfSunSetRotations;

        lightIntensityChangeRate = 1f / numberOfSunSetRotations;
    }




    float ChangeColorBasedOnIncreasingRate(float increasingRate, float color)
    {
        return color += increasingRate / nOfSunSetRotMovements;
    }

    bool IsSunRise()
    {
        return (xRotation > 0 &&
                xRotation < sunSetAngleFromZero &&
                lightObj.intensity < maxLight);
    }

    bool IsSunSet()
    {
        return xRotation > (180 - sunSetAngleFromZero) &&
               xRotation < 180 &&
               lightObj.intensity != 0;
    }

    void ChangeRotation()
    {
        xRotation += rotationInterval;
        transform.rotation = Quaternion.Euler(xRotation, 0, 0);
    }

    void HandleSunSetAndRaiseLight()
    {
        if (IsSunRise())
        {
            lightObj.intensity += lightIntensityChangeRate;
            ChangeColorToDay();
        }
        else if (IsSunSet())
        {
            lightObj.intensity -= lightIntensityChangeRate;
            ChangeColorToNight();
        }
        AdjustLightColorWhenNeeded();
    }

    void AdjustLightColorWhenNeeded()
    {
        if(lightObj.intensity == 0)
        {
            lightObj.color = sunsetColor;
            return;
        }
        if(lightObj.intensity == 1)
        {
            lightObj.color = ambientColor;
        }
    }


    IEnumerator MoveSun()
    {
        yield return new WaitForSeconds(rotationRate);

        ChangeRotation();
        HandleSunSetAndRaiseLight();

        if(xRotation >= 360)
        {
            xRotation = 0;
        }

        StartCoroutine(MoveSun());
    }


    void ChangeColorToNight()
    {
        ChangeColor(-1);
    }

    void ChangeColorToDay()
    {
        ChangeColor(1);
    }

    void ChangeColor(int direction)
    {
        currentColor.r = ChangeColorBasedOnIncreasingRate(direction * rChange, currentColor.r);
        currentColor.g = ChangeColorBasedOnIncreasingRate(direction * gChange, currentColor.g);
        currentColor.b = ChangeColorBasedOnIncreasingRate(direction * bChange, currentColor.b);

        lightObj.color = currentColor;
    }
}
