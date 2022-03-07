using UnityEngine;
using Utils;
using FootFallNoise = GameData.Enums.FootFallNoise;

public static class FootFallNoiseFromPlayer
{
    public static float Get(Transform me)
    {
        var distance = (me.position - GameObjectRefs.player.transform.position).magnitude;
        var noise = PlayerMovementNoise.Get();
        if (noise == FootFallNoise.Unknown)
        {
            Debug.LogError("[FootFallNoise] Unknown player movement");
            return 0;
        }
        return PlayerInfo.movementNoises[noise] / distance;
    }


    private static class PlayerMovementNoise
    {
        public static FootFallNoise Get()
        {
            if (IsRunning())
            {
                return FootFallNoise.NormalNoise;
            }
            else if (IsSprinting())
            {
                return FootFallNoise.LoudNoise;
            }
            else if (IsIdle())
            {
                return FootFallNoise.NoNoise;
            }
            return FootFallNoise.Unknown;
        }
        static bool IsRunning()
        {
            if ((Input.GetKey("w") ||
               Input.GetKey("d") ||
               Input.GetKey("s") ||
               Input.GetKey("a")) &&
               !Input.GetKey("left shift"))
            {
                return true;
            }
            return false;
        }

        static bool IsIdle()
        {
            if (Input.GetKey("w") ||
                Input.GetKey("d") ||
                Input.GetKey("s") ||
                Input.GetKey("a"))
            {
                return false;
            }

            return true;
        }

        static bool IsSprinting()
        {
            if ((Input.GetKey("w") ||
               Input.GetKey("d") ||
               Input.GetKey("s") ||
               Input.GetKey("a")) &&
               Input.GetKey("left shift"))
            {
                return true;
            }
            return false;
        }

    }
}