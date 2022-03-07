using System.Collections.Generic;
using UnityEngine;
using FootFallNoise = GameData.Enums.FootFallNoise;
using ItemEnum = GameData.Enums.Items;
public static class PlayerInfo
{
    public static bool lightOn = false;
    public static Vector3 position;
    public static Quaternion rotation;

    public static readonly Dictionary<FootFallNoise, float> movementNoises = new Dictionary<FootFallNoise, float>()
    {
        {FootFallNoise.NoNoise, 0 },
        {FootFallNoise.NormalNoise, 100f },
        {FootFallNoise.LoudNoise, 250f }
    };
}
