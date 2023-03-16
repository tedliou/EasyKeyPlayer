using IniParser;
using IniParser.Model;
using System.IO;
using UnityEngine;

namespace IMDLAB.EasyKeyPlayer
{
    public class ConfigManager : MonoBehaviour
    {
        internal static ConfigManager Instance { get; private set; }

        /// <summary>
        /// INI File Path
        /// </summary>
        private const string PATH = "config.ini";
        /// <summary>
        /// INI Section: Screen
        /// </summary>
        private const string SCR = "screen";
        private const string SCR_WIDTH = "width";
        private const string SCR_HEIGHT = "height";
        private const string SCR_FULLSCREEN = "fullscreen";
        /// <summary>
        /// INI Default Value
        /// </summary>
        private IniData DefaultConfig {
            get {
                var data = new IniData();
                data[SCR][SCR_WIDTH] = "1920";
                data[SCR][SCR_HEIGHT] = "1080";
                data[SCR][SCR_FULLSCREEN] = "true";

                return data;
            }
        }

        private FileIniDataParser Parser;
        private IniData Config;

        private void Awake()
        {
            Instance = this;
            Parser = new FileIniDataParser();
            ReadFile();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void ReadFile()
        {
            if (CheckFileIntegrity())
            {
                Config = Parser.ReadFile(PATH);
            }
        }

        private bool CheckFileIntegrity()
        {
            if (!File.Exists(PATH))
            {
                ResetAll();
            }

            return true;
        }

        private void ResetAll()
        {
            var data = DefaultConfig;
            Parser.WriteFile(PATH, data);
            Config = data;
        }

        private void ResetValue(string section, string property)
        {
            Config[section][property] = DefaultConfig[section][property];
            Parser.WriteFile(PATH, Config);
        }

        private Vector2Int GetDefaultSize()
        {
            var width = int.Parse(DefaultConfig[SCR][SCR_WIDTH]);
            var height = int.Parse(DefaultConfig[SCR][SCR_HEIGHT]);
            return new Vector2Int(width, height);
        }

        internal Vector2Int GetScreenSize()
        {
            var size = GetDefaultSize();
            if (int.TryParse(Config[SCR][SCR_WIDTH], out int width))
            {
                size.x = width;
            }
            else
            {
                ResetValue(SCR, SCR_WIDTH);
            }

            if (int.TryParse(Config[SCR][SCR_HEIGHT], out int height))
            {
                size.y = height;
            }
            else
            {
                ResetValue(SCR, SCR_HEIGHT);
            }

            return size;
        }

        internal bool GetFullScreen()
        {
            if (bool.TryParse(Config[SCR][SCR_FULLSCREEN], out bool mode))
            {
                return mode;
            }
            else
            {
                ResetValue(SCR, SCR_FULLSCREEN);
            }

            return false;
        }
    }
}
