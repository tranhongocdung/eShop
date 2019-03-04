using System.Web.Configuration;

namespace MVCWeb.Libraries
{
    public class Config
    {
        public static int StartYear
        {
            get { return 2016; }
        }
        public static int CarTrackingPageSize
        {
            get { return ConvertType.ToInt32(WebConfigurationManager.AppSettings["CarTrackingPageSize"], 10); }
        }
        public static int EmployeePageSize
        {
            get { return ConvertType.ToInt32(WebConfigurationManager.AppSettings["EmployeePageSize"], 10); }
        }
    }
}