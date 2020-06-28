using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.Repository.Operation.Asset;

namespace EasyAssetManagerCore.BusinessLogic.Operation.Asset
{
    public class RMAssetManager : BaseService, IRMAssetManager
    {
        private readonly IRMAssetRepository rmRepository;
        public RMAssetManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            rmRepository = new RMAssetRepository(Connection);
        }
    }

    public interface IRMAssetManager
    {

    }
}
