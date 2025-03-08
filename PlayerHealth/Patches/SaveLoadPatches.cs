using HarmonyLib;
using ModSaveBackups;
using System;

namespace PlayerHealth
{
    internal class SaveLoadPatches
    {
        [HarmonyPatch(typeof(SaveLoadManager))]
        private class SaveLoadManagerPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("SaveModData")]
            public static void DoSaveGamePatch()
            {
                var saveContainer = new PlayerHealthSaveContainer();

                saveContainer.health = PlayerHealth.health;
                saveContainer.isSick = PlayerHealth.isSick;
                saveContainer.sickTimer = PlayerHealth.sickTimer;

                ModSave.Save(Plugin.instance.Info, saveContainer);
            }

            [HarmonyPostfix]
            [HarmonyPatch("LoadModData")]
            public static void LoadModDataPatch()
            {
                if (!ModSave.Load(Plugin.instance.Info, out PlayerHealthSaveContainer saveContainer))
                {
                    Plugin.logger.LogWarning("Save file loading failed. If this is the first time loading this save with this mod, this is normal.");
                    return;
                }

                PlayerHealth.health = saveContainer.health;
                PlayerHealth.isSick = saveContainer.isSick;
                PlayerHealth.sickTimer = saveContainer.sickTimer;
            }
        }
    }

    [Serializable]
    public class PlayerHealthSaveContainer
    {
        internal float health;
        internal bool isSick;
        internal float sickTimer;
    }
}

