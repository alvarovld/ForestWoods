using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System;
using System.Security.Cryptography;

namespace Utils
{
    public class SystemRandom
    {

        [ThreadStatic]
        System.Random random = null;

        public SystemRandom(int seed)
        {
            random = new System.Random(seed);
        }

        Quaternion Rotation()
        {
            return UnityEngine.Random.rotation;
        }

        public float Range(float min, float max)
        {
            float nextDouble = (float)random.NextDouble();
            return nextDouble * (max - min) + min;
        }
    }
}

public static class StaticRandom
{
    static int seed = 11122;

    public static void Initialize(int s)
    {
        seed = s;
    }

    static readonly System.Threading.ThreadLocal<System.Random> random =
        new System.Threading.ThreadLocal<System.Random>(() => new System.Random(seed));


    public static int Rand()
    {
        return random.Value.Next();
    }

    public static float Range(float min, float max)
    {
        return (float)random.Value.NextDouble() * max + min;
    }

}