using System.Text.Json;

namespace test
{
    public static class FileMaster
    {
        private static List<string> _fileNames = new();
        private static List<Config> _configs = new();
        private static readonly string _savingDirectory = Path.GetFullPath(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\Configs"));

        public static void UpdateFiles()
        {
            if (Directory.Exists(_savingDirectory))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Reading saved configs...");
                Console.ResetColor();
                foreach (var file in Directory.EnumerateFiles(_savingDirectory, "*.json"))
                {
                    var filename = Path.GetFileNameWithoutExtension(file);
                    _fileNames.Add(filename);
                    var f = File.ReadAllText(file);
                    var conf = JsonSerializer.Deserialize<Config>(f);
                    if(conf != null)_configs.Add(conf);
                    Console.WriteLine("Config {0} added", filename);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Folder not found. Creating new one...");
                Console.ResetColor();
                Directory.CreateDirectory(_savingDirectory);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Operation completed successfully\n");
            Console.ResetColor();
        }

        public static bool IsExist(string name) => _fileNames.Exists(x => x == name);

        public static Config GetConfig(string name)
        {
            if (!IsExist(name))return new Config("File with this name doesn't exist");
            var conf = _configs.Find(x => x.service == name);
            return conf ?? new Config(name);
        }

        public static byte SaveConfig(Config config, bool rewrite = false)
        {
            if (IsExist(config.service)) return 0;

            var filePath = Path.GetFullPath(Path.Combine(_savingDirectory, config.service + ".json"));
            var fileText = JsonSerializer.Serialize(config);
            using (var file = File.CreateText(filePath))
            {
                file.Write(fileText);
            }
            _fileNames.Add(config.service);
            _configs.Add(config);
            return 1;
        }
        public static byte SaveConfig(string json)
        {
            var conf = JsonSerializer.Deserialize<Config>(json);
            return conf != null ? SaveConfig(conf) : (byte)2;
        }
        public static byte SaveConfig(JsonElement json) => SaveConfig(json.ToString());

        public static (byte, string) UpdateConfig(Config config)
        {
            if (!IsExist(config.service))return (0, "");
            var originalConfig = GetConfig(config.service);
            config.version = originalConfig.version;
            do
            {
                config.version++;
            }
            while (IsExist(config.service + "_v" + config.version));
            config.service += ("_v" + config.version);
            SaveConfig(config);
            return (1, config.service);
        }
        public static (byte, string) UpdateConfig(string json)
        {
            var conf = JsonSerializer.Deserialize<Config>(json);
            return conf != null ? UpdateConfig(conf) : ((byte)2, "");
        }
        public static (byte, string) UpdateConfig(JsonElement json) => UpdateConfig(json.ToString());

        public static byte DeleteConfig(string name)
        {
            if(!IsExist(name))return 0;
            var conf = GetConfig(name);
            if (conf.usingServicesAmount > 0)
            {
                return 1;
            }
            var filePath = Path.GetFullPath(Path.Combine(_savingDirectory, name + ".json"));
            File.Delete(filePath);
            _fileNames.Remove(name);
            _configs.Remove(conf);
            return 2;
        }

        public static List<object> AddUser(string userId, string service)
        {
            if (!IsExist(service)) return new List<object>();
            var conf = GetConfig(service);
            conf.users.Add(userId);
            ReWriteConfigSave(conf);
            return conf.data;
        }
        public static string DelUser(string userId, string service)
        {
            if (!IsExist(service)) return "This server doesn't exist";
            var conf = GetConfig(service);
            if (!conf.users.Exists(x => x == userId))return "User not found";
            conf.users.Remove(userId);
            ReWriteConfigSave(conf);
            return "Success";
        }
        private static void ReWriteConfigSave(Config config)
        {
            var filePath = Path.GetFullPath(Path.Combine(_savingDirectory, config.service + ".json"));
            var fileText = JsonSerializer.Serialize(config);
            File.WriteAllText(filePath, fileText);
        }
    }
}
