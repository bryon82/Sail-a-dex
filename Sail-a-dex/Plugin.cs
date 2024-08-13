using System.Reflection;
using BepInEx;
using HarmonyLib;
using BepInEx.Logging;
using SailwindModdingHelper;
using BepInEx.Configuration;

namespace sailadex
{
    [BepInPlugin(PLUGIN_ID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.app24.sailwindmoddinghelper", "2.0.3")]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_ID = "com.raddude82.sailadex";
        public const string PLUGIN_NAME = "Sail-A-Dex";
        public const string PLUGIN_VERSION = "1.1.0";

        internal static Plugin instance;
        internal static ManualLogSource logger;
        internal static ConfigEntry<bool> fishNamesHidden;
        internal static ConfigEntry<bool> portNamesHidden;
        internal static ConfigEntry<bool> fishCaughtUIEnabled;
        internal static ConfigEntry<bool> portsVisitedUIEnabled;

        internal static Harmony harmony;

        private void Awake()
        {
            instance = this;
            logger = Logger;
            fishNamesHidden = Config.Bind("Settings", "Hide Fish Names Before Caught", true, "true = the fish names will be hidden before being caught for the first time.");
            portNamesHidden = Config.Bind("Settings", "Hide Port Names Before Visited", false, "true = the port names will be hidden before visited for the first time.");
            fishCaughtUIEnabled = Config.Bind("Settings", "Enable Fish Caught UI", true, "true = the UI for how many fish you caught will be enabled. Setting to false, continuing a game where previously enabled, and then saving will erase all previous recorded fish caught progress.");
            portsVisitedUIEnabled = Config.Bind("Settings", "Enable Ports Visited UI", true, "true = the UI which ports you have visited will be enabled. Setting to false, continuing a game where previously enabled, and then saving will erase all previous recorded port visit progress.");
            harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_ID);            
        }

        private void OnDestroy()
        {
            logger.LogInfo($"Destroying and unpatching {PLUGIN_ID}");
            harmony.UnpatchSelf();
        }
    }
}
