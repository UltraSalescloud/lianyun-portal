using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using Lianyun.UST.Model.Repositories;

namespace Lianyun.UST.Repository
{
    public class AdToSceneDistrictRepository : BaseRepository<DSP_AdToSceneDistrict>, IAdToSceneDistrictRepository
    {
       public AdToSceneDistrictRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext, logger) { }

    }
}
