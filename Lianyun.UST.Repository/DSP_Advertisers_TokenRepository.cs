using Lianyun.UST.Model.Repositories;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lianyun.UST.Infrastructure.Logging;

namespace Lianyun.UST.Repository
{
    public class DSP_Advertisers_TokenRepository : BaseRepository<DSP_Advertisers_Token>, IDSP_Advertisers_TokenRepository
    {
        public DSP_Advertisers_TokenRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext,logger) { }
    }
}
