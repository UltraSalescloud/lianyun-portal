using System;
using System.Collections.Generic;
using System.Linq;

namespace Lianyun.UST.Infrastructure.Email
{

    public interface IEmail
    {
        bool SendEmail(string to, string subject, string message);
        bool SendEmail(string to, string cc, string bcc, string subject, string message);
        bool SendEmail(string[] to, string[] cc, string[] bcc, string subject, string message);

    }
}
