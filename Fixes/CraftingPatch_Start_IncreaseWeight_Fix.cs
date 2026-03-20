using HarmonyLib;

namespace GreenHellVR_Core.Fixes
{
    [HarmonyPatch(typeof(ItemsManager), "Start")]
    internal class CraftingPatch_Start_IncreaseWeight_Fix
    {
        private static void Prefix(ItemsManager __instance)
        {
            InventoryBackpack.Get().m_MaxWeight = ConfigManager.Instance.BackpackMaxWeight.Value;
            if (ConfigManager.Instance.ConfigUnlockAllItems.Value) ItemsManager.Get().UnlockAllItemsInNotepad();
        }
    }
}
