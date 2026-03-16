using AIs;
using BepInEx.Logging;
using Enums;
using GreenHellVR_Core_Include.Items.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GreenHellVR_Core.Systems
{
    internal class LocalizationSystem
    {
        public static Dictionary<string, string> localizationDictionary = [];
        private static bool isInitialized = false;

        private static void AddLocalization(string key, string value)
        {
            if (!localizationDictionary.ContainsKey(key))
            {
                localizationDictionary.Add(key, value);
            }
        }

        private static void RemoveLocalization(string key)
        {
            if (localizationDictionary.ContainsKey(key))
            {
                localizationDictionary.Remove(key);
            }
        }

        public static void ReadAllLocalizationFiles()
        {
            if (isInitialized) return;
            string directoryPath = Directory.GetParent(GHVRC_Objects.BundlesFolder).FullName;

            if (!Directory.Exists(directoryPath))
            {
                Plugin.Log.LogError($"Localization directory not found: {directoryPath}");
                return;
            }

            foreach (var dir in Directory.GetDirectories(directoryPath))
            {
                Plugin.Log.LogInfo($"Looking fo Localization: {dir}");
                string[] files = Directory.GetFiles(dir, "*Localization.xml");
                foreach (string file in files)
                {
                    ReadLocalizationFile(file);
                }
            }
            isInitialized = true;
        }

        public static void ReadLocalizationFile(string filePath)
        {
            var xmlStr = File.ReadAllText(filePath);

            XmlDocument document = new();
            document.LoadXml(xmlStr);

            Language language = GreenHellGame.Instance.m_Settings.m_Language;

            XmlNode root = document["Localization"];
            string languageStr = "en";

            switch (language)
            {
                case Language.English:
                    languageStr = "en";
                    break;
                case Language.French:
                    languageStr = "fr";
                    break;
                case Language.Italian:
                    languageStr = "it";
                    break;
                case Language.German:
                    languageStr = "de";
                    break;
                case Language.Spanish:
                    languageStr = "es";
                    break;
                case Language.ChineseTraditional:
                    languageStr = "cht";
                    break;
                case Language.ChineseSimplyfied:
                    languageStr = "chs";
                    break;
                case Language.Portuguese:
                    languageStr = "pt";
                    break;
                case Language.Polish:
                    languageStr = "pl";
                    break;
                case Language.Japanese:
                    languageStr = "jp";
                    break;
                case Language.Korean:
                    languageStr = "ko";
                    break;
                case Language.Czech:
                    languageStr = "cz";
                    break;
                default:
                    Plugin.Log.LogWarning($"Unsupported language: {language}. Defaulting to first one.");
                    break;
            }

            XmlNode languageNode = root.SelectSingleNode($"Language[@Name='{languageStr}']") ?? root.FirstChild;

            root = languageNode;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "String")
                {
                    string key = node.Attributes["Name"].Value;
                    string value = node.InnerText;
                    AddLocalization(key, value);
                }
            }

            Plugin.Log.LogInfo($"Localization file read successfully. Found {localizationDictionary.Count} entries.");
        }
    }
}
