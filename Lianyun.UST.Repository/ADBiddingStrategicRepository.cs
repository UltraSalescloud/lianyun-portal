using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using Lianyun.UST.Model.Repositories;
using Lianyun.UST.Infrastructure.Logging;

namespace Lianyun.UST.Repository
{
    public class ADBiddingStrategicRepository : BaseRepository<DSP_ADBiddingStrategic>, IADBiddingStrategicRepository
    {
        public ADBiddingStrategicRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext,logger) { }
    }
}
