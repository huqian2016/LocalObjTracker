using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IniParser;
using IniParser.Model;


namespace LocalObjTracker
{
    public class AppConfig
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string SecurityToken { get; set; } = "";
        public string OrgType { get; set; } = "Production";
        public int MainFormWidth { get; set; } = 1400;
        public int MainFormHeight { get; set; } = 720;
        public int MainFormX { get; set; } = 100;
        public int MainFormY { get; set; } = 100;
    }


    public class ConfigManager
    {
        private static readonly string configFilePath = "settings.ini";
        private static ConfigManager instance;
        public AppConfig Config { get; private set; }

        private ConfigManager()
        {
            LoadConfig();
        }

        public static ConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigManager();
                }
                return instance;
            }
        }

        private void LoadConfig()
        {
            if (!File.Exists(configFilePath))
            {
                Config = new AppConfig();
                SaveConfig();
            }
            else
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(configFilePath);

                Config = new AppConfig
                {
                    Username = data["InitWizardForm"]["Username"],
                    Password = DecodeBase64(data["InitWizardForm"]["Password"]),
                    SecurityToken = data["InitWizardForm"]["SecurityToken"],
                    OrgType = data["InitWizardForm"]["OrgType"],
                    MainFormWidth = int.Parse(data["MainForm"]["Width"]),
                    MainFormHeight = int.Parse(data["MainForm"]["Height"]),
                    MainFormX = int.Parse(data["MainForm"]["X"]),
                    MainFormY = int.Parse(data["MainForm"]["Y"])
                };
            }
        }

        public void SaveConfig()
        {
            var parser = new FileIniDataParser();
            IniData data = new IniData();

            data["InitWizardForm"]["Username"] = Config.Username;
            data["InitWizardForm"]["Password"] = EncodeBase64(Config.Password);
            data["InitWizardForm"]["SecurityToken"] = Config.SecurityToken;
            data["InitWizardForm"]["OrgType"] = Config.OrgType;
            data["MainForm"]["Width"] = Config.MainFormWidth.ToString();
            data["MainForm"]["Height"] = Config.MainFormHeight.ToString();
            data["MainForm"]["X"] = Config.MainFormX.ToString();
            data["MainForm"]["Y"] = Config.MainFormY.ToString();

            parser.WriteFile(configFilePath, data);
        }

        private string EncodeBase64(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private string DecodeBase64(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }

}
