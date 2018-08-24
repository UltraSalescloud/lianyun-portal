using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lianyun.UST.Infrastructure.Core
{
    public interface IStartUpTask
    {
        void OnStartUp();

        int Order { get; }
    }
}