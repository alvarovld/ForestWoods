
using UnityEngine;

namespace GameData
{
    public class Tags
    {
        public const string MainCamera = "MainCamera";
        public const string Obstacle = "Obstacle";
        public const string Enemy = "Enemy";
        public const string Player = "Player";
        public const string Stone = "Stone";
        public const string Stick = "Stick";
        public const string Tree = "Tree";
        public const string FlashLight = "Flashlight";
        public const string Rag = "Rag";
        public const string FireTorch = "FireTorch";
        public const string Terrain = "Terrain";
        public const string Fish = "Fish";
        public const string CookedFish = "CookedFish";
        public const string CampFire = "CampFire";
        public const string BlueBerry = "Blueberry";
        public const string Rope = "Rope";
        public const string EmptyCan = "EmptyCan";
        public const string FilledCan = "FilledCan";
        public const string FilledBoiledCan = "FilledBoiledCan";
        public const string Branch = "Branch";
        public const string Bed = "Bed";
        public const string SceneHandler = "SceneHandler";
        public const string ActionSwitcherText = "ActionSwitcherText";
        public const string AvailableRecipeUI = "AvailableRecipeUI";
        public const string DrinkText = "DrinkText";
        public const string IngredientsUI = "IngredientsUI";
        public const string CraftText = "CraftText";
        public const string CampFireBluePrint = "CampFireBluePrint";
        public const string BedBluePrint = "BedBluePrint";

    }

    public class Keys
    {
        public const string INVENTORY = "i";
        public const int ATTACK = 0;
        public const string RESET_CRAFT = "r";
        public const string PICK_UP_OBJECT = "e";
        public const string CROUCH = "left alt";
        public const string ONOFF_LIGHT = "r";
        public const string COOK = "e";
        public const string ACTION_SWITCHER = "e";
        public const string ACTION = "f";
        public const string CRAFT = "f";
        public const string CANCEL_CRAFT = "r";
        public const string RESET_INVENTORY_ITEMS = "r";

    }

    // This Parameters will overwrite given parameters on inspector
    public static class Parameters
    {
        public static Vector3 itemtextOffset = new Vector3(0, 2, 0);
        public static Vector3 campFireTextOffset = new Vector3(0, 4, 0);
        public static Color textColor = new Color(1f, 0.9696858f, 0f);
        public static Color outlineColor = textColor;
        public const float bluePrintOffset = 10;
    }

    public class TerrainData
    {
        public static float groundLevel;
    }

    public class Enums
    {
        public enum Items
        {
            NullItem = 0,
            Flashlight = 2,
            Stick = 3,
            Stone = 4,
            Rag = 5,
            FireTorch = 6,
            Fish = 7,
            CookedFish = 8,
            Blueberry = 9,
            Rope = 10,
            EmptyCan = 11,
            FilledCan = 12,
            FilledBoiledCan = 13,
            Branch = 14, 
            Bed = 15, 
            Campfire = 16
        }

        public enum FootFallNoise
        {
            Unknown = 0,
            NoNoise = 1,
            NormalNoise = 2,
            LoudNoise = 3
        }

        public enum BehaviourEnum
        {
            Unknown = 0,
            Attack = 1,
            RunBackwards = 2,
            GetCrazy = 3,
            KeepGuardStand = 4,
            KeepGuardRight = 5,
            KeepGuardLeft = 6,
            WalkFree = 7,
            Idle = 8,
            PerformAttack = 9,
            WalkToCampfire = 10,
            KeepGuard = 11
        }

    }
}