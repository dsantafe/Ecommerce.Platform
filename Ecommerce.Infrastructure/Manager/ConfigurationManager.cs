namespace Ecommerce.Infrastructure.Manager
{
    using RockLib.Configuration;

    public static class ConfigurationManager
    {
        public static string GetValue(string key)
        {
            try
            {
                string value = string.Empty;
                value = Config.AppSettings[key];
                return value;
            }
            catch (Exception) { return key; }
        }
    }
}
