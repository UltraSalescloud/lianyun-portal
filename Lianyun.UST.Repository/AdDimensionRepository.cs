using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Lianyun_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Repository
{
    public class AdDimensionRepository : BaseRepository<AdDimension>, IAdDimensionRepository
    {
        public AdDimensionRepository(LianyunContext lianyunContext, ILogger logger) : base(lianyunContext,logger) { }
        public List<AdDimension> SelectByAdFormsCode(string AdFormsCode, string sRatio)
        {
            var list = this.GetListBy(o => o.AdFormsCode == AdFormsCode && sRatio.IndexOf(o.Ratio) >= 0, o => o.Height, true);
            return list.OrderByDescending(o => o.Width).OrderByDescending(o => o.Width).ToList();
        }
    }
}
