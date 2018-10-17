using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Repository
{
    public class AdToPeopleKeywordsRepository : BaseRepository<DSP_ADToPeopleKeywords>, IAdToPeopleKeywordsRepository
    {
        public AdToPeopleKeywordsRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext, logger) { }

    }
}
