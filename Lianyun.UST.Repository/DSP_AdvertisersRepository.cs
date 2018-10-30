using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Linq;
using System.Linq;


namespace Lianyun.UST.Repository
{
    public class DSP_AdvertisersRepository : BaseRepository<DSP_Advertisers>, IDSP_AdvertisersRepository
    {
        public DSP_AdvertisersRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext,logger) { }
        public DSP_Advertisers GetDspAdvertisersByEmail(string email)
        {
           return this.Find(o => o.LoginEmail == email);
        }
    }
}
