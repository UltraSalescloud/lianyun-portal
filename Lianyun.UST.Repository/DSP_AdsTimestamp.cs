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
    public class DSP_AdsTimestampRepository : BaseRepository<DSP_AdsTimestamp>, IDSP_AdsTimestampRepository
    {
        public DSP_AdsTimestampRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext, logger) { }

       
    }
}
