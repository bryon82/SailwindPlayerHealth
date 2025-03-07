using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace PlayerHealth
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.raddude82.playerhealth";
        public const string PLUGIN_NAME = "PlayerHealth";
        public const string PLUGIN_VERSION = "1.0.0";

        internal static Plugin instance;
        internal static ManualLogSource logger;

        private void Awake()
        {
            instance = this;
            logger = Logger;

            AssetsLoader.LoadAssets();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);            
        }
    }
}

