using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
