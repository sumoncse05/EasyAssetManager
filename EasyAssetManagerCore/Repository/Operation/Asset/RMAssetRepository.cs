using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace EasyAssetManagerCore.Repository.Operation.Asset
{
    public class RMAssetRepository : BaseRepository, IRMAssetRepository
    {
        public RMAssetRepository(OracleConnection connection) : base(connection)
        {
            
        }
        public IEnumerable<RM> GetRMList(string pvc_branchcode, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_branchcode", pvc_branchcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_userrmlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<RM>("pkg_liability_lov_manager.dpd_get_userrmlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<LoanProduct> GetLoanProduct(string pvc_sgid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_sgid", pvc_sgid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_loandetails", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<LoanProduct>("pkg_asset_manager.dpd_get_loandetails", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<Loan> GetAvailableLoan(string pvc_seg_id, string pvc_product_code,string pvc_origrm_code, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_seg_id", pvc_seg_id, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_product_code", pvc_product_code, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_oldrm_code", pvc_origrm_code, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_availableloan", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Loan>("pkg_asset_manager.dpd_get_availableloan", dyParam, commandType: CommandType.StoredProcedure);
        }

        public ResponseMessage SetRMAssignWithLoan(string pvc_loanslno, string pvc_rm_code, string pvc_effdate, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_loanslno", pvc_loanslno, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_rm_code", pvc_rm_code, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_effdate", pvc_effdate, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_asset_manager.dpd_set_assaignloantorm", dyParam);
            return res;

        }

        public IEnumerable<Branch> GetBranchList(string pvc_areacode, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_areacode", pvc_areacode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_allbranchlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Branch>("pkg_liability_lov_manager.dpd_get_areawisebranchlist", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<Area> GetAreaList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_areadtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Area>("pkg_liability_lov_manager.dpd_get_areadtl", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<Loan> GetUnAuthorizeLoanRM(string pvc_seg_id, string pvc_branchCode, string pvc_rmcode, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_seg_id", pvc_seg_id, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_branch_code", pvc_branchCode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_oldrm_code", pvc_rmcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_authorizeloan", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Loan>("pkg_asset_manager.dpd_get_authorizeloan", dyParam, commandType: CommandType.StoredProcedure);
        }

        public ResponseMessage SetRMAuthorized(string pvc_custlogslno, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_loanslno", pvc_custlogslno, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_asset_manager.dpd_set_authorizeloantorm", dyParam);
            return res;
        }

    }

    public interface IRMAssetRepository
    {
        IEnumerable<RM> GetRMList(string pvc_branchcode, string pvc_appuser);
        IEnumerable<Loan> GetAvailableLoan(string pvc_seg_id, string pvc_product_code, string pvc_origrm_code, string pvc_appuser);
        ResponseMessage SetRMAssignWithLoan(string pvc_loanlogslno, string pvc_rm_code, string pvc_effdate, string pvc_appuser);
        IEnumerable<Branch> GetBranchList(string pvc_areacode, string pvc_appuser);
        IEnumerable<Area> GetAreaList(string pvc_appuser);
        IEnumerable<Loan> GetUnAuthorizeLoanRM(string pvc_seg_id, string pvc_branchCode, string pvc_rmcode, string pvc_appuser);
        IEnumerable<LoanProduct> GetLoanProduct(string pvc_sgid, string pvc_appuser);
        ResponseMessage SetRMAuthorized(string pvc_loanlogslno, string pvc_appuser);
    }
}
