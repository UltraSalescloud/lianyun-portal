using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Lianyun_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Repository
{
    public class AdsOperationRepository : BaseRepository<AdsOperation>, IAdsOperationRepository
    {
        public AdsOperationRepository(LianyunContext lianyunContext, ILogger logger) : base(lianyunContext, logger) { }
    }
}
