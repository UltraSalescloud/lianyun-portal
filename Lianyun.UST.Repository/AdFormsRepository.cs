using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Lianyun_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Repository
{
    public class AdFormsRepository : BaseRepository<AdForms>, IAdFormsRepository
    {
        public AdFormsRepository(LianyunContext lianyunContext, ILogger logger) : base(lianyunContext,logger) { }
        public List<AdForms> SelectAll()
        {
            //var v = this.GetListBy(o => o.ID == o.ID);//this.GetListBy(o => o.IsDelete == false);
            //return v.ToList();
            // this.DB.Database.SqlQuery<AdForms>("SELECT * FROM AdForms").ToList();
            return this.DbSet.ToList<AdForms>();
        }


        public List<string> GetFormsName()
        {
            string sql = @"SELECT Name From AdForms WHERE IsDelete = 0 ORDER BY AdSpaceType ";
            return this.DB.Database.SqlQuery<string>(sql).ToList();
        }
        public List<AdForms> GetAdSpaceType()
        {
            string sql = @"SELECT * From AdForms WHERE IsDelete = 0 ORDER BY AdSpaceType ";
            return this.DB.Database.SqlQuery<AdForms>(sql).ToList();
        }
    }
}
