using Enums;
using GreenHellVR_Core_Include.Items.Objects;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GreenHellVR_Core.Items
{
    public static class GHVRC_ItemsManager
    {
        public static readonly List<ItemID> OGItemIDs = [.. ((ItemID[])Enum.GetValues(typeof(ItemID)))];
        private static readonly List<ModItemID> OGItemsIDsList = [.. OGItemIDs.Select((id, index) => new ModItemID { ID = (int)id, Name = id.ToString() })];
        public static Dictionary<int, ItemInfo> OGItemInfos;

        public static Dictionary<ModItemID, ItemInfo> modItemIDs;

        public static Dictionary<ModItemID, ItemInfo> AllItemsIds;

        public static void Initialize()
        {
            OGItemInfos = Traverse.Create(typeof(ItemsManager)).Field("m_ItemInfos").GetValue<Dictionary<int, ItemInfo>>();
            modItemIDs = [];
            AllItemsIds = [];

            for (int i = 0; i < OGItemInfos.Count; i++)
            {
                ItemInfo item = OGItemInfos[i];
                ModItemID modItemID = ConvertItemID(item.m_ID);

                AllItemsIds.Add(modItemID, item);
            }

            foreach (var itemId in modItemIDs)
            {
                AllItemsIds.AddItem(itemId);
            }
        }

        public static ModItemID ConvertItemID(ItemID id)
        {
            return new ModItemID { ID = (int)id, Name = id.ToString() };
        }

        public static void ListAllItems()
        {
            StringBuilder sb = new();
            sb.AppendLine("Listing all item IDs:");
            foreach (ModItemID itemID in OGItemsIDsList)
            {
                sb.AppendLine($"ID: {itemID.ID}, Name: {itemID.Name}");
            }

            File.WriteAllText(Path.Combine(GHVRC_Objects.BundlesFolder, "AllItemIDs.txt"), sb.ToString());
            Plugin.Log.LogInfo(sb.ToString());
        }

        public static Item SpawnGHVRItem(ItemID id, Transform pos)
        {
            Item item = ItemsManager.Get().CreateItem(id, true, pos);
            Plugin.Log.LogInfo(id.ToString() + " has been created");
            return item;
        }

        public static Item SpawnGHVRItem(ItemID id, Vector3 pos, Quaternion rot)
        {
            Item item = ItemsManager.Get().CreateItem(id, true, pos, rot);
            Plugin.Log.LogInfo(id.ToString() + " has been created");
            return item;
        }

        public static Item CreateItem(ItemID item_id)
        {
            CraftingManager craftingManager = CraftingManager.Get();
            Item item = ItemsManager.Get().CreateItem(item_id, false, craftingManager.CraftSpawnPosition.position, craftingManager.CraftSpawnPosition.rotation);
            CalcHealth(item);
            Player.Get().AddKnownItem(item_id);
            EventsManager.OnEvent(global::Enums.Event.Craft, 1, (int)item_id);
            Skill.Get<CraftingSkill>().OnSkillAction();
            ItemsManager.Get().OnCreateItem(item_id);
            return item;
        }

        public static void CalcHealth(Item item)
        {
            float playerHealthMul = Skill.Get<CraftingSkill>().GetPlayerHealthMul();
            float itemHealthMul = Skill.Get<CraftingSkill>().GetItemHealthMul(item);
            float num = Mathf.Clamp01(playerHealthMul + itemHealthMul + Skill.Get<CraftingSkill>().m_InitialHealthMul);
            item.m_Info.m_Health = item.m_Info.m_MaxHealth * num;
            if (item.m_Info.IsTorch())
            {
                ((Torch)item).m_Fuel = num;
            }
        }

    }
}
