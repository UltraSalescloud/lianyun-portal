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
    public class AdToOptimizeRepository:BaseRepository<DSP_AdToOptimize>,IAdToOptimizeRepository
    {
        public AdToOptimizeRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext, logger) { }
        
    }
}
