using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using Lianyun.UST.Model.Repositories;

namespace Lianyun.UST.Repository
{
    public class AdToDistrictRepository : BaseRepository<DSP_AdToDistrict>,IAdToDistrictRepository
    {
         public AdToDistrictRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext, logger) { }
    }
}
