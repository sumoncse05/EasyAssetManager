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

        public FileProcessManager()
        {
            fileProcessRepository = new FileProcessRepository(Connection);
        }
        public Message ProcessFile(int businessYear, int file_Type, string filepath, AppSession session)
        {
            Logging.WriteToLog(session.User.StationIp, session.User.user_id, "processExcell", "Excell application created.");
            switch (file_Type)
            {
                case (int)FileType.AST_RM_PORTFOLIO_TMP:
                    Message = Process_LOAN_PORTFOLIO(filepath, session, FileType.AST_RM_PORTFOLIO_TMP.ToString(), businessYear);
                    break;
                case (int)FileType.AST_LOAN_TARGET_TMP:
                    Message = Process_LOAN_TARGET(filepath, session, FileType.AST_LOAN_TARGET_TMP.ToString(), businessYear);
                    break;
                case (int)FileType.AST_LOAN_CL_TMP:
                    Message = Process_LOAN_CL(filepath, session, FileType.AST_LOAN_CL_TMP.ToString(), businessYear);
                    break;
                case (int)FileType.AST_LOAN_WO_STATUS_TEMP:
                    Message = Process_LOAN_WO(filepath, session, FileType.AST_LOAN_WO_STATUS_TEMP.ToString(), businessYear);
                    break;
            }
            return Message;
        }
        private Message Process_LOAN_WO(string filepath, AppSession session, string tableName, int businessYear)
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
                                    SEG_ID = wooksheet.Cells[i, 1].Text.Trim(),
                                    SEG_NAME = wooksheet.Cells[i, 2].Text.Trim(),
                                    AREA_CODE = wooksheet.Cells[i, 3].Text.Trim(),
                                    AREA_NAME = wooksheet.Cells[i, 4].Text.Trim(),
                                    BRANCH_CODE = wooksheet.Cells[i, 5].Text.Trim(),
                                    BRANCH_NAME = wooksheet.Cells[i, 6].Text.Trim(),
                                    PRODUCT_CODE = wooksheet.Cells[i, 7].Text.Trim(),
                                    PRODUCT_DESC = wooksheet.Cells[i, 8].Text.Trim(),
                                    LOAN_AC_NUMBER = wooksheet.Cells[i, 9].Text.Trim(),
                                    OS_AMOUNT = valid(wooksheet.Cells[i, 10].Text.Trim(), "LOAN_OUTSTANDING", "Number"),
                                    WO_AMOUNT = valid(wooksheet.Cells[i, 11].Text.Trim(), "WO_AMOUNT", "Number"),
                                    WO_DATE = Convert.ToDateTime(wooksheet.Cells[i, 12].Text.Trim()),
                                    INS_BY = session.User.user_id,
                                    INS_DATE = DateTime.Now
                                };
                                portFolios.Add(portFolio);
                            }
                            if (portFolios.Count > 0)
                            {
                                var row = fileProcessRepository.Process_LOAN_WO(portFolios);
                                if (row > 0)
                                {
                                    var response = fileProcessRepository.SetProcess_LOAN_WO(fileProcessID, businessYear, session.User.user_id);
                                    if (response.pvc_status == "40999")
                                    {
                                        MessageHelper.Success(Message, "Data upload and process successfully....");
                                    }
                                    else
                                    {
                                        MessageHelper.Error(Message, response.pvc_statusmsg);
                                    }

                                }
                                else
                                {
                                    MessageHelper.Error(Message, "No data process...");
                                }
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
        private Message Process_LOAN_PORTFOLIO(string filepath, AppSession session, string tableName, int businessYear)
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
                            var portFolios = new List<AST_RM_PORTFOLIO_TMP>();
                            for (var i = 2; i <= rowCount; i++)
                            {
                                var portFolio = new AST_RM_PORTFOLIO_TMP
                                {
                                    File_Process_ID = fileProcessID,
                                    AREA_CODE = wooksheet.Cells[i, 1].Text.Trim(),
                                    AREA_NAME = wooksheet.Cells[i, 2].Text.Trim(),
                                    BRANCH_CODE = wooksheet.Cells[i, 3].Text.Trim(),
                                    BRANCH_NAME = wooksheet.Cells[i, 4].Text.Trim(),
                                    RM_CODE = wooksheet.Cells[i, 5].Text.Trim(),
                                    RM_NAME = wooksheet.Cells[i, 6].Text.Trim(),
                                    LOAN_AC_NUMBER = valid(wooksheet.Cells[i, 7].Text.Trim(), "LOAN_AC_NUMBER", "Number"),
                                    EFF_DATE = Convert.ToDateTime(wooksheet.Cells[i, 8].Text.Trim()),
                                    INS_BY = session.User.user_id,
                                    INS_DATE = DateTime.Now
                                };
                                portFolios.Add(portFolio);
                            }
                            if (portFolios.Count > 0)
                            {
                                var row = fileProcessRepository.Process_LOAN_PORTFOLIO(portFolios);
                                if (row > 0)
                                {
                                    var response = fileProcessRepository.SetProcess_LOAN_PORTFOLIO(fileProcessID, businessYear, session.User.user_id);
                                    if (response.pvc_status == "40999")
                                    {
                                        MessageHelper.Success(Message, "Data upload and process successfully....");
                                    }
                                    else
                                    {
                                        MessageHelper.Error(Message, response.pvc_statusmsg);
                                    }

                                }
                                else
                                {
                                    MessageHelper.Error(Message, "No data process...");
                                }
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
        private Message Process_LOAN_TARGET(string filepath, AppSession session, string tableName, int businessYear)
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
                                    SEG_ID = wooksheet.Cells[i, 1].Text.Trim(),
                                    SEG_NAME = wooksheet.Cells[i, 2].Text.Trim(),
                                    AREA_CODE = wooksheet.Cells[i, 3].Text.Trim(),
                                    AREA_NAME = wooksheet.Cells[i, 4].Text.Trim(),
                                    BRANCH_CODE = wooksheet.Cells[i, 5].Text.Trim(),
                                    BRANCH_NAME = wooksheet.Cells[i, 6].Text.Trim(),
                                    RM_CODE = wooksheet.Cells[i, 7].Text.Trim(),
                                    RM_NAME = wooksheet.Cells[i, 8].Text.Trim(),
                                    BST_CODE = wooksheet.Cells[i, 9].Text.Trim(),
                                    BST_NAME = wooksheet.Cells[i, 10].Text.Trim(),
                                    OS_TARGET_AMT = valid(wooksheet.Cells[i, 11].Text.Trim(), "OS_TARGET_AMT", "Number"),
                                    DISB_TARGET_AMT = valid(wooksheet.Cells[i, 12].Text.Trim(), "DISB_TARGET_AMT", "Number"),
                                    INC_TARGET_AMT = valid(wooksheet.Cells[i, 13].Text.Trim(), "INC_TARGET_AMT", "Number"),
                                    INS_BY = session.User.user_id,
                                    INS_DATE = DateTime.Now
                                };
                                portFolios.Add(portFolio);
                            }
                            if (portFolios.Count > 0)
                            {
                                var row = fileProcessRepository.Process_LOAN_TARGET(portFolios);
                                if (row > 0)
                                {
                                    var response = fileProcessRepository.SetProcess_LOAN_TARGET(fileProcessID, businessYear, session.User.user_id);
                                    if (response.pvc_status == "40999")
                                    {
                                        MessageHelper.Success(Message, "Data upload and process successfully....");
                                    }
                                    else
                                    {
                                        MessageHelper.Error(Message, response.pvc_statusmsg);
                                    }

                                }
                                else
                                {
                                    MessageHelper.Error(Message, "No data process...");
                                }
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
        private Message Process_LOAN_CL(string filepath, AppSession session, string tableName, int businessYear)
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
                                    AREA_CODE = wooksheet.Cells[i, 1].Text.Trim(),
                                    AREA_NAME = wooksheet.Cells[i, 2].Text.Trim(),
                                    BRANCH_CODE = wooksheet.Cells[i, 3].Text.Trim(),
                                    BRANCH_NAME = wooksheet.Cells[i, 4].Text.Trim(),
                                    RM_CODE = wooksheet.Cells[i, 5].Text.Trim(),
                                    RM_NAME = wooksheet.Cells[i, 6].Text.Trim(),
                                    BST_CODE = wooksheet.Cells[i, 7].Text.Trim(),
                                    BST_NAME = wooksheet.Cells[i, 8].Text.Trim(),
                                    LOAN_AC_NUMBER = valid(wooksheet.Cells[i, 9].Text.Trim(), "LOAN_AC_NUMBER", "Number"),
                                    CL_STATUS = valid(wooksheet.Cells[i, 10].Text.Trim(), "CL_STATUS", "Digit"),
                                    EFF_DATE = Convert.ToDateTime(wooksheet.Cells[i, 11].Text.Trim()),
                                    INS_BY = session.User.user_id,
                                    INS_DATE = DateTime.Now
                                };
                                portFolios.Add(portFolio);
                            }
                            if (portFolios.Count > 0)
                            {
                                var row = fileProcessRepository.Process_LOAN_CL(portFolios);
                                if (row > 0)
                                {
                                    var response = fileProcessRepository.SetProcess_LOAN_CL(fileProcessID, businessYear, session.User.user_id);
                                    if (response.pvc_status == "40999")
                                    {
                                        MessageHelper.Success(Message, "Data upload and process successfully....");
                                    }
                                    else
                                    {
                                        MessageHelper.Error(Message, response.pvc_statusmsg);
                                    }

                                }
                                else
                                {
                                    MessageHelper.Error(Message, "No data process...");
                                }
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
                case "Digit":
                    decimal digit = 0;
                    canConvert = strVal != "" ? strVal.Length <= 2 : true;
                    if (canConvert == false)
                        throw new Exception(fieldName + " contains invalid Number: " + strVal);
                    break;
                default:
                    break;
            }

            return strVal;
        }

        public IEnumerable<AST_LOAN_WO_STATUS_TEMP> Getloanwo(string loan_number, string are_code, string branch_code, string wo_date, string pvc_appuser)
        {
            return fileProcessRepository.Getloanwo(loan_number,are_code, branch_code, wo_date, pvc_appuser);
        }

        public Message Set_LOAN_WO(AST_LOAN_WO_STATUS_TEMP loan_wo, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                var response = fileProcessRepository.Set_LOAN_WO(loan_wo, session.User.user_id);
                if (response.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "Data process successfully....");
                }
                else
                {
                    MessageHelper.Error(Message, response.pvc_statusmsg);
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

        public IEnumerable<AST_LOAN_CL_TMP> Getloancl(string loan_number, string cl_status, string eff_date, string pvc_appuser)
        {
            return fileProcessRepository.Getloancl(loan_number, cl_status, eff_date, pvc_appuser);
        }

        public Message Set_LOAN_CL(AST_LOAN_CL_TMP loan_wo, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                var response = fileProcessRepository.Set_LOAN_CL(loan_wo, session.User.user_id);
                if (response.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "Data process successfully....");
                }
                else
                {
                    MessageHelper.Error(Message, response.pvc_statusmsg);
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
    }

    public enum FileType
    {
        AST_RM_PORTFOLIO_TMP = 1,
        AST_LOAN_TARGET_TMP = 2,
        AST_LOAN_CL_TMP = 3,
        AST_LOAN_WO_STATUS_TEMP = 4
    }
    public static class ExcelColumn
    {
        public static List<string> LOAN_CL = new List<string> { "AREA_CODE", "AREA_NAME", "BRANCH_CODE", "BRANCH_NAME", "RM_CODE", "RM_NAME", "BST_CODE", "BST_NAME", "LOAN_AC_NUMBER", "CL_STATUS", "EFF_DATE" };
        public static List<string> LOAN_PORTFOLIO = new List<string> { "AREA_CODE", "AREA_NAME", "BRANCH_CODE", "BRANCH_NAME", "RM_CODE", "RM_NAME", "LOAN_AC_NUMBER", "EFF_DATE" };
        public static List<string> LOAN_TARGET = new List<string> { "SEG_ID", "SEG_NAME", "AREA_CODE", "AREA_NAME", "BRANCH_CODE", "BRANCH_NAME", "RM_CODE", "RM_NAME", "BST_CODE", "BST_NAME", "OS_TARGET_AMT", "DISB_TARGET_AMT", "INC_TARGET_AMT" };
        public static List<string> LOAN_WO = new List<string> { "SEG_ID", "SEG_NAME", "AREA_CODE", "AREA_NAME", "BRANCH_CODE", "BRANCH_NAME", "PRODUCT_CODE", "PRODUCT_DESC", "LOAN_AC_NUMBER", "OS_AMOUNT", "WO_AMOUNT", "WO_DATE" };
    }
    public interface IFileProcessManager
    {
        Message ProcessFile(int businessYear, int file_Type, string filepath, AppSession session);

        IEnumerable<AST_LOAN_WO_STATUS_TEMP> Getloanwo(string loan_number, string are_code, string branch_code, string wo_date, string pvc_appuser);
        Message Set_LOAN_WO(AST_LOAN_WO_STATUS_TEMP loan_wo, AppSession session);
        IEnumerable<AST_LOAN_CL_TMP> Getloancl(string loan_number, string cl_status, string eff_date, string pvc_appuser);
        Message Set_LOAN_CL(AST_LOAN_CL_TMP loan_wo, AppSession session);
    }
}
