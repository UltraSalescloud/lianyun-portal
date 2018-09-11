using Donson.Lomark.DSP.Infrastructure.Logging;
using Donson.Lomark.DSP.Model.Lomark_DSP_Entities;
using Donson.Lomark.DSP.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donson.Lomark.DSP.Repository
{
    public class AdPortraitExpressionRepository : BaseRepository<DSP_AdPortraitExpression>, IAdPortraitExpressionRepository
    {
        public AdPortraitExpressionRepository(Lomark_DSPContext lomark_DSPContext, ILogger logger) : base(lomark_DSPContext, logger) { }
    }
}
