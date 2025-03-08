using HarmonyLib;
using UnityEngine;

namespace PlayerHealth
{
    internal class ContinuePatches
    {
        [HarmonyPatch(typeof(StartMenu))]
        private class StartMenuPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("Awake")]
            private static void AddContinueUI(StartMenu __instance)
            {
                ContinueUI.SetupContinueUI(__instance);
                __instance.gameObject.AddComponent<ContinueUI>();
            }

            [HarmonyPrefix]
            [HarmonyPatch("LateUpdate")]
            private static bool SkipLateUpdateIfIsDead(StartMenu __instance)
            {
                if (PlayerHealth.isDead)                
                    return false;                
                return true;
            }
        }

        [HarmonyPatch(typeof(PlayerNeeds))]
        private class PlayerNeedsPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("LateUpdate")]
            private static bool SkipLateUpdateIfIsDead()
            {
                if (PlayerHealth.isDead)
                    return false;
                return true;
            }
        }

        [HarmonyPatch(typeof(PlayerNeedsUI))]
        private class PlayerNeedsUIPatches
        {

            [HarmonyPrefix]
            [HarmonyPatch("ToggleUI")]
            private static bool SkipToggleUIIfIsDead()
            {
                if (PlayerHealth.isDead)
                    return false;
                return true;
            }

            [HarmonyPrefix]
            [HarmonyPatch("PlayWarning")]
            private static bool SkipPlayWarningIfIsDead()
            {
                if (PlayerHealth.isDead)
                    return false;
                return true;
            }
        }

        [HarmonyPatch(typeof(MissionListUI))]
        private class MissionListUIPatches
        {

            [HarmonyPrefix]
            [HarmonyPatch("ToggleMenu")]
            private static bool SkipToggleMenuIfIsDead()
            {
                if (PlayerHealth.isDead)
                    return false;
                return true;
            }
        }
    }
}
