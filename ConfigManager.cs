using BepInEx.Configuration;

namespace GreenHellVR_Core
{
    internal class ConfigManager
    {
        public static ConfigManager Instance { get; private set; }

        public ConfigEntry<bool> ConfigSkipIntro;
        public ConfigEntry<float> BackpackMaxWeight;
        public ConfigEntry<bool> ConfigFullUI;
        public ConfigEntry<bool> ConfigUnlockAllItems;
        public ConfigEntry<bool> ConfigMonkeySpawnDebug;

        private Plugin plugin;

        public static void Awake()
        {
            Instance ??= new ConfigManager();
            Instance.plugin ??= Plugin.instance;
        }

        public static void LoadConfig()
        {
            Instance.ConfigSkipIntro = Instance.plugin.Config.Bind(
                "Miscellaneous",
                "SkipIntro",
                true);

            Instance.BackpackMaxWeight = Instance.plugin.Config.Bind(
                "Miscellaneous",
                "BackpackMaxWeight",
                100f);

            Instance.ConfigFullUI = Instance.plugin.Config.Bind(
                "Extra",
                "FullUI",
                false);

            Instance.ConfigUnlockAllItems = Instance.plugin.Config.Bind(
                "Extra",
                "UnlockAllItems",
                false);

            Instance.ConfigMonkeySpawnDebug = Instance.plugin.Config.Bind(
                "Debug",
                "MonkeySpawnDebug",
                false);

        }
    }
}
