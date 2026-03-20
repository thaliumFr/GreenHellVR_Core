using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenHellVR_Core.Systems
{
    public static class CraftingSystems
    {
        public static List<Receipe> ModReceipes {get; private set; }
        internal static bool isInitialized = false;

        public static void AddReceipe(ItemID result, Dictionary<ItemID, int> ingredients)
        {
            ModReceipes ??= [];
            ModReceipes.Add(new Receipe { Result = result, Ingredients = ingredients });
        }




        public struct Receipe
        {
            public ItemID Result;

            /// <summary>
            /// ItemID and quantity of the ingredients needed to craft the item
            /// </summary>
            public Dictionary<ItemID, int> Ingredients;
        }
    }


}
