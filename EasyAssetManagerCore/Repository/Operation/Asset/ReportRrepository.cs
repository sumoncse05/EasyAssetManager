using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace EasyAssetManagerCore.Repository.Operation.Asset
{
    public class ReportRepository : BaseRepository, IReportRepository
    {
        public ReportRepository(OracleConnection connection) : base(connection)
        {

        }
        public IEnumerable<AstDailyStatus> AssetAtGlance(string pvc_loantype, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_segid", pvc_loantype, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_areacode", pvc_areacode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_branchcode", pvc_branchcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_rmcode", pvc_rmcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_workdate", System.Convert.ToDateTime(pvc_todate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_assetatglance", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AstDailyStatus>("dpg_reports_manager.dpd_get_userrmlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<AstDailyStatus> AreawiseReport(string pvc_loantype, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_segid", pvc_loantype, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_areacode", pvc_areacode, OracleMappingType.Varchar2, ParameterDirection.Input);
            //dyParam.Add("pvc_branchcode", pvc_branchcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            //dyParam.Add("pvc_rmcode", pvc_rmcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_workdate", System.Convert.ToDateTime(pvc_todate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_areawisedailystatus", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AstDailyStatus>("dpg_reports_manager.dpd_get_areawisedailystatus", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<AstDailyStatus> BranchwiseReport(string pvc_loantype, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_segid", pvc_loantype, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_areacode", pvc_areacode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_branchcode", pvc_branchcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            //dyParam.Add("pvc_rmcode", pvc_rmcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_workdate", System.Convert.ToDateTime(pvc_todate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_branchwisedailystatus", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AstDailyStatus>("dpg_reports_manager.dpd_get_branchwisedailystatus", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<AstDailyStatus> RmwiseReport(string pvc_loantype, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_segid", pvc_loantype, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_areacode", pvc_areacode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_branchcode", pvc_branchcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_rmcode", pvc_rmcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_workdate", System.Convert.ToDateTime(pvc_todate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_rmassetdailystatus", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AstDailyStatus>("dpg_reports_manager.dpd_get_rmassetdailystatus", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<AstDailyStatus> BstwiseReport(string pvc_loantype, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_segid", pvc_loantype, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_areacode", pvc_areacode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_branchcode", pvc_branchcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_rmcode", pvc_rmcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_workdate", System.Convert.ToDateTime(pvc_todate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_bstwise", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AstDailyStatus>("dpg_reports_manager.dpd_get_userrmlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<AstDailyStatus> ProductwiseReport(string pvc_loantype, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_segid", pvc_loantype, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_areacode", pvc_areacode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_branchcode", pvc_branchcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_rmcode", pvc_rmcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_workdate", System.Convert.ToDateTime(pvc_todate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_productwise", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AstDailyStatus>("dpg_reports_manager.dpd_get_userrmlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<AstDailyStatus> YearwiseReport(string pvc_loantype, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_segid", pvc_loantype, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_areacode", pvc_areacode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_branchcode", pvc_branchcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_rmcode", pvc_rmcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_workdate", System.Convert.ToDateTime(pvc_todate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_yearwise", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AstDailyStatus>("dpg_reports_manager.dpd_get_userrmlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<AstDailyStatus> ClientwiseReport(string pvc_loantype, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_segid", pvc_loantype, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_areacode", pvc_areacode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_branchcode", pvc_branchcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_rmcode", pvc_rmcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_workdate", System.Convert.ToDateTime(pvc_todate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_clientwise", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AstDailyStatus>("dpg_reports_manager.dpd_get_userrmlist", dyParam, commandType: CommandType.StoredProcedure);
        }
    }
    public interface IReportRepository 
    {
        IEnumerable<AstDailyStatus> AssetAtGlance(string pvc_loanType, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser);
        IEnumerable<AstDailyStatus> AreawiseReport(string pvc_loanType, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser);
        IEnumerable<AstDailyStatus> BranchwiseReport(string pvc_loanType, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser);
        IEnumerable<AstDailyStatus> RmwiseReport(string pvc_loanType, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser);
        IEnumerable<AstDailyStatus> BstwiseReport(string pvc_loanType, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser);
        IEnumerable<AstDailyStatus> ProductwiseReport(string pvc_loanType, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser);
        IEnumerable<AstDailyStatus> YearwiseReport(string pvc_loanType, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser);
        IEnumerable<AstDailyStatus> ClientwiseReport(string pvc_loanType, string pvc_rmcode, string pvc_areacode, string pvc_branchcode, string pvc_todate, string pvc_appuser);
    }
}
