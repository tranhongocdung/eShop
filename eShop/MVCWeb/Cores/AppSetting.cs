using System.Linq;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores
{
    public static class AppSetting
    {
        public static string Get(string configKey)
        {
            var db = new DbAppContext();
            var config = db.Configs.FirstOrDefault(o => o.Id == configKey);
            if (config == null)
            {
                return "Config not found!";
            }
            return config.Value ?? "";
        }
    }
}