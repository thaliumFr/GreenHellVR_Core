using HarmonyLib;
using UnityEngine;

namespace GreenHellVR_Core
{
    [HarmonyPatch(typeof(MainMenu), "UpdateState")]
    class MainMenu_UpdateState_Fix
    {
        static bool Prefix(ref MainMenuState ___m_State, MainMenu __instance)
        {
            bool SkipIntro = ConfigManager.Instance.ConfigSkipIntro.Value;
            if (SkipIntro && (___m_State == MainMenuState.CompanyLogo || ___m_State == MainMenuState.SecondCompanyLogo || ___m_State == MainMenuState.GameLogo))
            {
                ___m_State = MainMenuState.MainMenu;
                VrInputManager.Get().enabled = false;
                GreenHellGame.GetVRCanvasHelper().SetVrCanvasToPositon(MainMenuManager.Get().m_RectTransform, new Vector3(82.178f, 97.683f, 182.291f), new Vector3(0f, -135f, 0f));
                __instance.m_BG.gameObject.SetActive(value: true);
                __instance.m_CompanyLogo.gameObject.SetActive(value: false);
                __instance.m_SecondCompanyLogo.gameObject.SetActive(value: false);
                __instance.m_GameLogo.gameObject.SetActive(value: false);
                __instance.m_Buttons.SetActive(value: true);
                Color black = __instance.m_BG.color;
                black.a = 1f;
                __instance.m_BG.color = black;
                GreenHellGame.Instance.m_Settings.ApplySettings();
                GreenHellGame.Instance.m_InitialSequenceComplete = SkipIntro;
                Plugin.Log.LogInfo("Intro skipped");
            }

            Logger.Log("UpdateState fix");
            return true;
        }
    }
}
