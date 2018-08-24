using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Infrastructure.Config
{
    public interface IConfiguration
    {

        string EmailVerifyAddress { get; }
        string SmtpServer { get;  }
        string EmailFromAddress { get; }
        string EmailFromDisplayName { get;  }
        string EmailFromPassword { get;  }
    }
}
