using System.Collections.Generic;
using ItemEnum = GameData.Enums.Items;

namespace GameData
{
    namespace Converters
    {
        public class TagToItem
        {
            public static ItemEnum Get(string tag)
            {
                switch (tag)
                {
                    case Tags.FlashLight:
                        return ItemEnum.Flashlight;
                    case Tags.Stone:
                        return ItemEnum.Stone;
                    case Tags.Stick:
                        return ItemEnum.Stick;
                    case Tags.Rag:
                        return ItemEnum.Rag;
                    case Tags.FireTorch:
                        return ItemEnum.FireTorch;
                    case Tags.Fish:
                        return ItemEnum.Fish;
                    case Tags.CookedFish:
                        return ItemEnum.CookedFish;
                    case Tags.BlueBerry:
                        return ItemEnum.Blueberry;
                    case Tags.Rope:
                        return ItemEnum.Rope;
                    case Tags.EmptyCan:
                        return ItemEnum.EmptyCan;
                    case Tags.FilledCan:
                        return ItemEnum.FilledCan;
                    case Tags.FilledBoiledCan:
                        return ItemEnum.FilledBoiledCan;
                    case Tags.Branch:
                        return ItemEnum.Branch;
                    case Tags.Bed:
                        return ItemEnum.Bed;
                    default:
                        return ItemEnum.NullItem;
                }
            }
        }
        public class ItemToTag
        {
            public static string Get(ItemEnum item)
            {
                switch (item)
                {
                    case ItemEnum.Flashlight:
                        return Tags.FlashLight;
                    case ItemEnum.Stone:
                        return Tags.Stone;
                    case ItemEnum.Stick:
                        return Tags.Stick;
                    case ItemEnum.Rag:
                        return Tags.Rag;
                    case ItemEnum.FireTorch:
                        return Tags.FireTorch;
                    case ItemEnum.Fish:
                        return Tags.Fish;
                    case ItemEnum.CookedFish:
                        return Tags.CookedFish;
                    case ItemEnum.Blueberry:
                        return Tags.BlueBerry;
                    case ItemEnum.Rope:
                        return Tags.Rope;
                    case ItemEnum.FilledBoiledCan:
                        return Tags.FilledBoiledCan;
                    case ItemEnum.Branch:
                        return Tags.Branch;
                    default:
                        return "";
                }
            }
        }

    }

}