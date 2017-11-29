using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TeamsToDoApp.Utils
{
    public static class AppSettings
    {
        public static readonly string BaseUrl;
        public static readonly string ConnectorAppId;

        static AppSettings()
        {
            BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            ConnectorAppId = ConfigurationManager.AppSettings["ConnectorAppId"];
        }
    }
}