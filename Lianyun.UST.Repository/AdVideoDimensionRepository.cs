using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Lianyun_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Repository
{
    public class AdVideoDimensionRepository : BaseRepository<AdVideoDimension>, IAdVideoDimensionRepository
    {
        public AdVideoDimensionRepository(LianyunContext lianyunContext, ILogger logger) : base(lianyunContext, logger) { }
        public List<AdVideoDimension> SelectByAdFormsCode(string AdFormsCode)
        {
            var list = this.GetListBy(o => o.AdFormsCode == AdFormsCode, o => o.MinHeight, true);
            return list.OrderByDescending(o => o.MinWidth).OrderByDescending(o => o.MinWidth).ToList();
        }
    }
}
