using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Lianyun_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Repository
{
    public class AdsEndedOrPausedRepository : BaseRepository<AdsEndedOrPaused>, IAdsEndedOrPausedRepository
    {
        public AdsEndedOrPausedRepository(LianyunContext lianyunContext,ILogger logger) : base(lianyunContext,logger) { }
    }
}
