using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Operation.Asset;
using System;
using System.Collections.Generic;

namespace EasyAssetManagerCore.BusinessLogic.Operation.Asset
{
    public class ReportManager : BaseService, IReportManager
    {
        private readonly IReportRepository reportRepository;
        public ReportManager()
        {
            reportRepository = new ReportRepository(Connection);
        }
        public IEnumerable<AstDailyStatus> AssetAtGlance(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            try
            {
                return reportRepository.AssetAtGlance(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "ReportManager-AssetAtGlance", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<AstDailyStatus> AreawiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            try
            {
                return reportRepository.AreawiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "ReportManager-AreawiseReport", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        
        public IEnumerable<AstDailyStatus> BranchwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
    {
        try
        {
            return reportRepository.BranchwiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);

        }
        catch (Exception ex)
        {
            Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "ReportManager-BranchwiseReport", ex.Message + "|" + ex.StackTrace.TrimStart());
            return null;
        }
        }
        public IEnumerable<AstDailyStatus> RmwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            try
            {
                return reportRepository.RmwiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "ReportManager-RmwiseReport", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<AstDailyStatus> BstwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            try
            {
                return reportRepository.BstwiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "ReportManager-BstwiseReport", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<AstDailyStatus> ProductwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            try
            {
                return reportRepository.ProductwiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "ReportManager-ProductwiseReport", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<AstDailyStatus> YearwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            try
            {
                return reportRepository.YearwiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "ReportManager-YearwiseReport", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<AstDailyStatus> ClientwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            try
            {
                return reportRepository.ClientwiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "ReportManager-ClientwiseReport", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
    }
    public interface IReportManager
    {
        IEnumerable<AstDailyStatus> AssetAtGlance(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session);
        IEnumerable<AstDailyStatus> AreawiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session);
        IEnumerable<AstDailyStatus> BranchwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session);
        IEnumerable<AstDailyStatus> RmwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session);
        IEnumerable<AstDailyStatus> BstwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session);
        IEnumerable<AstDailyStatus> ProductwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session);
        IEnumerable<AstDailyStatus> YearwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session);
        IEnumerable<AstDailyStatus> ClientwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session);
    }
}
