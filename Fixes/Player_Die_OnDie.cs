using HarmonyLib;

namespace GreenHellVR_Core
{

    [HarmonyPatch(typeof(Player), nameof(Player.Die))]
    class Player_Die_OnDie
    {
        static void Postfix()
        {
            Plugin.instance.OnDie();
        }
    }
}
