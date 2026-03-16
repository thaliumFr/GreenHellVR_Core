using AIs;
using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GreenHellVR_Core.Items
{
    class ModItemInfo : ItemInfo
    {
        public string ItemName { get; private set; }
        private ModItemID CItemID;

        public ModItemInfo(string ItemName, ModItemID itemID)
        {
            this.ItemName = ItemName;
            this.m_ID = ItemID.None;
            CItemID = itemID;
        }
    }
}
