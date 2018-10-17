using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Repository
{
    public class AdToScheduleRepository : BaseRepository<DSP_AdToSchedule>, IAdToScheduleRepository
    {
        public AdToScheduleRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext,logger) { }
    }
}
