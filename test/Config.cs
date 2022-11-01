namespace test
{
    [System.Serializable]
    public class Config
    {
        public string service { get; set; }
        public List<object> data { get; set; } = new List<object>();
        public int version { get; set; } = 1;
        public int usingServicesAmount { get => users.Count;}
        public List<string> users { get; set; } = new List<string>();  
        public Config(string serviceName) => service = serviceName;
        public Config(string serviceName, params object[] objects)
        {
            service = serviceName;
            foreach (var obj in objects)
            {
                data.Add(obj);
            }
        }
        public Config() => service = "";
    }
}