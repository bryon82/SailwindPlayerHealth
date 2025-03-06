using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerHealth
{
    internal class HealthPatches
    {
        [HarmonyPatch(typeof(PlayerNeeds))]
        private class PlayerNeedsPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("PassOut")]
            private static bool SkipPassOut()
            {
                return false;
            }
        }
    }
}
