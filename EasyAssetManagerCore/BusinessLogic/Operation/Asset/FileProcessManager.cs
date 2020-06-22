using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.Data;
using EasyAssetManagerCore.Repository.Operation.Asset;

namespace EasyAssetManagerCore.BusinessLogic.Operation.Asset
{
    public class FileProcessManager : BaseService, IFileProcessManager
    {
        private readonly IFileProcessRepository fileProcessRepository;

        public FileProcessManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            fileProcessRepository = new FileProcessRepository(Connection);
        }
        public Message ProcessFile(int businessYear,int file_Type, string filepath, AppSession session)
        {
            Logging.WriteToLog(session.User.StationIp, session.User.user_id, "processExcell", "Excell application created.");
            switch (file_Type)
            {
                case (int)FileType.AST_LOAN_PORTFOLIO_TMP:
                    Message = Process_LOAN_PORTFOLIO(filepath, session, FileType.AST_LOAN_PORTFOLIO_TMP.ToString());
                    break;
                case (int)FileType.AST_LOAN_TARGET_TMP:
                    Message = Process_LOAN_TARGET(filepath, session, FileType.AST_LOAN_TARGET_TMP.ToString());
                    break;
                case (int)FileType.AST_LOAN_CL_TMP:
                    Message = Process_LOAN_CL(filepath, session, FileType.AST_LOAN_CL_TMP.ToString());
                    break;
                case (int)FileType.AST_LOAN_WO_STATUS_TEMP:
                    Message = Process_LOAN_WO(filepath, session, FileType.AST_LOAN_WO_STATUS_TEMP.ToString());
                    break;
            }
            return Message;
        }

        private Message Process_LOAN_WO(string filepath, AppSession session, string tableName)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                fileProcessRepository.DeleteTable(tableName, session.User.user_id);
                var random = new Random();
                var fileProcessID = random.Next(10000);
                using (var package = new ExcelPackage(new FileInfo(@filepath)))
                {
                    var totalWorkSheet = package.Workbook.Worksheets.Count;
                    var isProcess = true;
                    for (var index = 1; index <= totalWorkSheet; index++)
                    {
                        var wooksheet = package.Workbook.Worksheets[index];
                        var xx = wooksheet.Name;
                        int rowCount = wooksheet.Dimension.Rows;
                        int columnCount = wooksheet.Dimension.Columns;
                        for (var i = 1; i <= 1; i++)
                        {
                            for (var j = 1; j <= columnCount; j++)
                            {
                                if (wooksheet.Cells[i, j].Text.Trim() == null && !ExcelColumn.LOAN_WO.Contains(wooksheet.Cells[i, j].Text.Trim()))
                                {
                                    isProcess = false;
                                }
                            }
                        }
                        if (isProcess)
                        {
                            var portFolios = new List<AST_LOAN_WO_STATUS_TEMP>();
                            for (var i = 2; i <= rowCount; i++)
                            {
                                var portFolio = new AST_LOAN_WO_STATUS_TEMP
                                {
                                    File_Process_ID = fileProcessID,
                                    AREA_CODE = wooksheet.Cells[i, 1].Text.Trim(),
                                    AREA_NAME = wooksheet.Cells[i, 2].Text.Trim(),
                                    LOAN_NUMBER = wooksheet.Cells[i, 3].Text.Trim(),
                                    LOAN_OUTSTANDING = wooksheet.Cells[i, 4].Text.Trim(),
                                    WO_AMOUNT = wooksheet.Cells[i, 5].Text.Trim(),
                                    INS_BY = session.User.user_id,
                                    INS_DATE = DateTime.Now
                                };
                                portFolios.Add(portFolio);
                            }
                            if (portFolios.Count > 0)
                            {
                                var row = fileProcessRepository.Process_LOAN_WO(portFolios);
                            }
                            else
                            {
                                MessageHelper.Error(Message, "No rows found this excel file.");
                            }
                        }
                        else
                        {
                            MessageHelper.Error(Message, "File structure is not valid. Invalid Columns.");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return Message;
        }

        public Message Process_LOAN_PORTFOLIO(string filepath, AppSession session,string tableName)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                fileProcessRepository.DeleteTable(tableName, session.User.user_id);
                var random = new Random();
                var fileProcessID = random.Next(10000);
                using (var package = new ExcelPackage(new FileInfo(@filepath)))
                {
                    var totalWorkSheet = package.Workbook.Worksheets.Count;
                    var isProcess = true;
                    for (var index = 1; index <= totalWorkSheet; index++)
                    {
                        var wooksheet = package.Workbook.Worksheets[index];
                        var xx = wooksheet.Name;
                        int rowCount = wooksheet.Dimension.Rows;
                        int columnCount = wooksheet.Dimension.Columns;
                        for (var i = 1; i <= 1; i++)
                        {
                            for (var j = 1; j <= columnCount; j++)
                            {
                                if (wooksheet.Cells[i, j].Text.Trim() == null && !ExcelColumn.LOAN_PORTFOLIO.Contains(wooksheet.Cells[i, j].Text.Trim()))
                                {
                                    isProcess = false;
                                }
                            }
                        }
                        if (isProcess)
                        {
                            var portFolios = new List<AST_LOAN_PORTFOLIO_TMP>();
                            for (var i = 2; i <= rowCount; i++)
                            {
                                var portFolio = new AST_LOAN_PORTFOLIO_TMP
                                {
                                    File_Process_ID = fileProcessID,
                                    ID_of_Area= wooksheet.Cells[i, 1].Text.Trim(),
                                    Name_of_Area = wooksheet.Cells[i, 2].Text.Trim(),
                                    Brn_Code = wooksheet.Cells[i, 3].Text.Trim(),
                                    Branch_Name = wooksheet.Cells[i, 4].Text.Trim(),
                                    ID_of_RM = wooksheet.Cells[i, 5].Text.Trim(),
                                    Name_of_RM = wooksheet.Cells[i, 6].Text.Trim(),
                                    Loan_Acct_No = wooksheet.Cells[i, 7].Text.Trim(),
                                    INS_BY=session.User.user_id,
                                    INS_DATE=DateTime.Now
                                };
                                portFolios.Add(portFolio);
                            }
                            if (portFolios.Count > 0)
                            {
                                var row = fileProcessRepository.Process_LOAN_PORTFOLIO(portFolios);                          }
                            else
                            {
                                MessageHelper.Error(Message, "No rows found this excel file.");
                            }
                        }
                        else
                        {
                            MessageHelper.Error(Message, "File structure is not valid. Invalid Columns.");
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            
            return Message;
        }
        public Message Process_LOAN_TARGET(string filepath, AppSession session, string tableName)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                fileProcessRepository.DeleteTable(tableName, session.User.user_id);
                var random = new Random();
                var fileProcessID = random.Next(10000);
                using (var package = new ExcelPackage(new FileInfo(@filepath)))
                {
                    var totalWorkSheet = package.Workbook.Worksheets.Count;
                    var isProcess = true;
                    for (var index = 1; index <= totalWorkSheet; index++)
                    {
                        var wooksheet = package.Workbook.Worksheets[index];
                        var xx = wooksheet.Name;
                        int rowCount = wooksheet.Dimension.Rows;
                        int columnCount = wooksheet.Dimension.Columns;
                        for (var i = 1; i <= 1; i++)
                        {
                            for (var j = 1; j <= columnCount; j++)
                            {
                                if (wooksheet.Cells[i, j].Text.Trim() == null && !ExcelColumn.LOAN_TARGET.Contains(wooksheet.Cells[i, j].Text.Trim()))
                                {
                                    isProcess = false;
                                }
                            }
                        }
                        if (isProcess)
                        {
                            var portFolios = new List<AST_LOAN_TARGET_TMP>();
                            for (var i = 2; i <= rowCount; i++)
                            {
                                var portFolio = new AST_LOAN_TARGET_TMP
                                {
                                    File_Process_ID = fileProcessID,
                                    ID_of_Area = wooksheet.Cells[i, 1].Text.Trim(),
                                    Name_of_Area = wooksheet.Cells[i, 2].Text.Trim(),
                                    Brn_Code = wooksheet.Cells[i, 3].Text.Trim(),
                                    Branch_Name = wooksheet.Cells[i, 4].Text.Trim(),
                                    ID_of_RM = wooksheet.Cells[i, 5].Text.Trim(),
                                    Name_of_RM = wooksheet.Cells[i, 6].Text.Trim(),
                                    ID_of_BST = wooksheet.Cells[i, 7].Text.Trim(),
                                    Name_of_BST = wooksheet.Cells[i, 8].Text.Trim(),
                                    Out_Standing_Amount = wooksheet.Cells[i, 9].Text.Trim(),
                                    Disbursed_Amount = wooksheet.Cells[i, 10].Text.Trim(),
                                    INS_BY = session.User.user_id,
                                    INS_DATE = DateTime.Now
                                };
                                portFolios.Add(portFolio);
                            }
                            if (portFolios.Count > 0)
                            {
                                var row = fileProcessRepository.Process_LOAN_TARGET(portFolios);
                            }
                            else
                            {
                                MessageHelper.Error(Message, "No rows found this excel file.");
                            }
                        }
                        else
                        {
                            MessageHelper.Error(Message, "File structure is not valid. Invalid Columns.");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return Message;
        }
        public Message Process_LOAN_CL(string filepath, AppSession session, string tableName)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                fileProcessRepository.DeleteTable(tableName, session.User.user_id);
                var random = new Random();
                var fileProcessID = random.Next(10000);
                using (var package = new ExcelPackage(new FileInfo(@filepath)))
                {
                    var totalWorkSheet = package.Workbook.Worksheets.Count;
                    var isProcess = true;
                    for (var index = 1; index <= totalWorkSheet; index++)
                    {
                        var wooksheet = package.Workbook.Worksheets[index];
                        var xx = wooksheet.Name;
                        int rowCount = wooksheet.Dimension.Rows;
                        int columnCount = wooksheet.Dimension.Columns;
                        for (var i = 1; i <= 1; i++)
                        {
                            for (var j = 1; j <= columnCount; j++)
                            {
                                if (wooksheet.Cells[i, j].Text.Trim() == null && !ExcelColumn.LOAN_CL.Contains(wooksheet.Cells[i, j].Text.Trim()))
                                {
                                    isProcess = false;
                                }
                            }
                        }
                        if (isProcess)
                        {
                            var portFolios = new List<AST_LOAN_CL_TMP>();
                            for (var i = 2; i <= rowCount; i++)
                            {
                                var portFolio = new AST_LOAN_CL_TMP
                                {
                                    File_Process_ID = fileProcessID,
                                    ID_of_Area = wooksheet.Cells[i, 1].Text.Trim(),
                                    Name_of_Area = wooksheet.Cells[i, 2].Text.Trim(),
                                    Brn_Code = wooksheet.Cells[i, 3].Text.Trim(),
                                    Branch_Name = wooksheet.Cells[i, 4].Text.Trim(),
                                    ID_of_RM = wooksheet.Cells[i, 5].Text.Trim(),
                                    Name_of_RM = wooksheet.Cells[i, 6].Text.Trim(),
                                    ID_of_BST = wooksheet.Cells[i, 7].Text.Trim(),
                                    Name_of_BST = wooksheet.Cells[i, 8].Text.Trim(),
                                    Loan_Acct_No = wooksheet.Cells[i, 9].Text.Trim(),
                                    Classification_TYPE = wooksheet.Cells[i, 10].Text.Trim(),
                                    INS_BY = session.User.user_id,
                                    INS_DATE = DateTime.Now
                                };
                                portFolios.Add(portFolio);
                            }
                            if (portFolios.Count > 0)
                            {
                                var row = fileProcessRepository.Process_LOAN_CL(portFolios);
                            }
                            else
                            {
                                MessageHelper.Error(Message, "No rows found this excel file.");
                            }
                        }
                        else
                        {
                            MessageHelper.Error(Message, "File structure is not valid. Invalid Columns.");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return Message;
        }

        public IEnumerable<District> GetDistrictList(string div_code, string pvc_appuser)
        {
            return fileProcessRepository.GetDistrictList(div_code, pvc_appuser);
        }

        public IEnumerable<Division> GetDivisionList(string pvc_appuser)
        {
            return fileProcessRepository.GetDivisionList(pvc_appuser);
        }

        public IEnumerable<Thana> GetThanaList(string div_code, string dist_code, string pvc_appuser)
        {
            return fileProcessRepository.GetThanaList(div_code, dist_code, pvc_appuser);
        }

        protected string valid(object val, string fieldName, string fieldType)
        {
            string strVal = (val != DBNull.Value ? val.ToString().Trim() : Convert.ToString(0).Trim());

            bool canConvert = false;
            switch (fieldType)
            {
                case "String":
                    switch (fieldName)
                    {
                        case "DR_CR":
                            if (strVal != "C" && strVal != "D")
                                throw new Exception(fieldName + " contains invalid value:" + strVal + "; Required Value C or D;");
                            break;
                        default:
                            break;
                    }
                    break;
                case "Number":
                    decimal number = 0;
                    canConvert = strVal != "" ? decimal.TryParse(strVal, out number) : true;
                    if (canConvert == false)
                        throw new Exception(fieldName + " contains invalid Number: " + strVal);
                    break;
                default:
                    break;
            }

            return strVal;
        }


    }

    public enum FileType
    {
        AST_LOAN_PORTFOLIO_TMP = 1,
        AST_LOAN_TARGET_TMP = 2,
        AST_LOAN_CL_TMP = 3,
        AST_LOAN_WO_STATUS_TEMP = 4
    }
    public static class ExcelColumn
    {
        public static List<string> LOAN_CL  = new List<string> { "ID_of_Area", "Name_of_Area", "Brn_Code", "Branch_Name", "ID_of_RM", "Name_of_RM", "ID_of_BST", "Name_of_BST", "Loan_Acct_No", "Classification_TYPE" };
        public static List<string> LOAN_PORTFOLIO = new List<string> { "ID_of_Area", "Name_of_Area", "Brn_Code", "Branch_Name", "ID_of_RM", "Name_of_RM", "Loan_Acct_No" };
        public static List<string> LOAN_TARGET = new List<string> { "ID_of_Area", "Name_of_Area", "Brn_Code", "Branch_Name", "ID_of_RM", "Name_of_RM", "ID_of_BST", "Name_of_BST", "Out_Standing_Amount", "Disbursed_Amount" };
        public static List<string> LOAN_WO = new List<string> { "AREA_CODE", "AREA_NAME", "LOAN_NUMBER", "LOAN_OUTSTANDING", "WO_AMOUNT" };
    }
    public interface IFileProcessManager
    {
        IEnumerable<Division> GetDivisionList(string pvc_appuser);
        IEnumerable<District> GetDistrictList(string div_code, string pvc_appuser);
        IEnumerable<Thana> GetThanaList(string div_code, string dist_code, string pvc_appuser);
        Message ProcessFile(int businessYear, int file_Type, string filepath, AppSession session);
    }
}
