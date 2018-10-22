using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Lianyun_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Repository
{
    public class DistrictConfigRepository : BaseRepository<DistrictConfig>, IDistrictConfigRepository
    {
        public DistrictConfigRepository(LianyunContext lianyunContext, ILogger logger) : base(lianyunContext, logger) { }

        public List<DistrictConfig> GetAllList()
        {
            //var v = this.GetListBy(o => o.ID == o.ID);//this.GetListBy(o => o.IsDelete == false);
            //return v.ToList();
            return this.DbSet.ToList<DistrictConfig>();
        }

        public List<DistrictConfig> GetDistrictConfigListBy(string Name, string CityCode)
        {
            List<DistrictConfig> lst = new List<DistrictConfig>();
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(CityCode))
                lst = this.GetListBy(o => o.Name == Name && o.CityCode == CityCode);
            else if (!string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(CityCode))
                lst = this.GetListBy(o => o.Name == Name);
            else if (string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(CityCode))
                lst = this.GetListBy(o => o.CityCode == CityCode);
            else
                lst = this.DbSet.ToList<DistrictConfig>();
            return lst;
        }
    }
}
