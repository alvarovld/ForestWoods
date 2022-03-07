using UnityEngine;
using System.Collections;
using System;

namespace Utils
{
    public static class ItemHelper
    {
        public static bool IsItem(string tag)
        {
            var items = Enum.GetValues(typeof(GameData.Enums.Items));
            foreach (var item in items)
            {
                if (item.ToString().Equals(tag))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
