using BepInEx.Configuration;

namespace GreenHellVR_Core
{
    internal class ConfigManager
    {
        public static ConfigManager Instance { get; private set; }

        public ConfigEntry<bool> ConfigSkipIntro;
        public ConfigEntry<bool> ConfigFullUI;

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

            Instance.ConfigFullUI = Instance.plugin.Config.Bind(
                "Extra",
                "FullUI",
                false);
        }
    }
}
