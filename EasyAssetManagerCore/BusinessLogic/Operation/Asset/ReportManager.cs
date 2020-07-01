using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Operation.Asset;
using System;
using System.Collections.Generic;
using System.Data;

namespace EasyAssetManagerCore.BusinessLogic.Operation.Asset
{
    public class ReportManager : BaseService, IReportManager
    {
        private readonly IReportRepository reportRepository;
        public ReportManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            reportRepository = new ReportRepository(Connection);
        }
        public IEnumerable<AstDailyStatus> AssetAtGlance(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            return reportRepository.AssetAtGlance(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
        }
        public IEnumerable<AstDailyStatus> AreawiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            return reportRepository.AreawiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
        }
        public IEnumerable<AstDailyStatus> BranchwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            return reportRepository.BranchwiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
        }
        public IEnumerable<AstDailyStatus> RmwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            return reportRepository.RmwiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
        }
        public IEnumerable<AstDailyStatus> BstwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            return reportRepository.BstwiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
        }
        public IEnumerable<AstDailyStatus> ProductwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            return reportRepository.ProductwiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
        }
        public IEnumerable<AstDailyStatus> YearwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            return reportRepository.YearwiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
        }
        public IEnumerable<AstDailyStatus> ClientwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate, AppSession session)
        {
            return reportRepository.ClientwiseReport(loanType, rmCode, areaCode, branchCode, todate, session.User.user_id);
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
