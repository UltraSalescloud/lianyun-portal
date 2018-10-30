using Lianyun.UST.Infrastructure.Enums;
using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Repository
{
    public class DSP_AdToCreativeMD5LogRepository : BaseRepository<DSP_AdToCreativeMD5Log>, IDSP_AdToCreativeMD5LogRepository
    {
        public DSP_AdToCreativeMD5LogRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext, logger) { }

       
    }
}
