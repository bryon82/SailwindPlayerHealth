using HarmonyLib;

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

            [HarmonyPrefix]
            [HarmonyPatch("Start")]
            private static void AddPlayerHealth(PlayerNeeds __instance)
            {
                __instance.gameObject.AddComponent<PlayerHealth>();
            }            
        }

        [HarmonyPatch(typeof(PlayerNeedsUI))]
        private class PlayerNeedsUIPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("Awake")]
            private static void AddPlayerHealthUI(PlayerNeedsUI __instance)
            {
                PlayerHealthUI.SetupPlayerHealthUI(__instance);
                __instance.gameObject.AddComponent<PlayerHealthUI>();                
            }
        }

        [HarmonyPatch(typeof(ShipItemFood))]
        private class ShipItemFoodPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("EatFood")]
            private static void EatSpoiledFood(FoodState ___foodState)
            {
                if (___foodState.spoiled > 0.9f)
                    PlayerHealth.isSick = true;
            }
        }
    }
}
