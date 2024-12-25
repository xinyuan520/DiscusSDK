namespace Discus.SDK.Core.Configuration
{
    public class MysqlConfig
    {
        public const string Name = "Mysql";

        public string MasterConnectionString { get; set; } = string.Empty;

        public string[] SlaveConnectionStrings { get; set; } = Array.Empty<string>();
    }
}
