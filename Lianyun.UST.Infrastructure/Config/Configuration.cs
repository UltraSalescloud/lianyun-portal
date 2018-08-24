using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Lianyun.UST.Infrastructure.Logging;

namespace Lianyun.UST.Infrastructure.Config
{
    public class Configuration : IConfiguration
    {
        private ILogger logger = null;

        public Configuration(ILogger logger)
        {
            this.logger = logger;
        }

        private string GetConfigurationSetting(string key)
        {
            string value = ConfigurationManager.AppSettings.Get(key);

            if (value == null)
            {
                logger.Error(typeof(Configuration), string.Format("AppSetting: {0} is not configured.", key));
            }

            return value;
        }

        #region IConfiguration Members
        public string SmtpServer
        {
            get
            {
                return GetConfigurationSetting("SmtpServer");
            }
        }



        public string EmailFromAddress
        {
            get
            {
                return GetConfigurationSetting("EmailFromAddress");
            }
        }

        public string EmailFromDisplayName
        {
            get
            {
                return GetConfigurationSetting("EmailFromDisplayName");
            }
        }
        public string EmailFromPassword
        {
            get
            {
                return GetConfigurationSetting("EmailFromPassword");
            }
        }

        public string EmailVerifyAddress
        {
            get
            {
                return GetConfigurationSetting("EmailVerifyAddress");
            }
        }

        #endregion
    }
}
