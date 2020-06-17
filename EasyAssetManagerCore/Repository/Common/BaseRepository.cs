using Oracle.ManagedDataAccess.Client;

namespace EasyAssetManagerCore.Repository.Common
{
    public class BaseRepository
    {
        public OracleConnection Connection { get; set; }
        public OracleTransaction Transaction { get; set; }

        public BaseRepository(OracleConnection connection, OracleTransaction transaction = null)
        {
            Connection = connection;
            Transaction = transaction;
        }
    }
}
