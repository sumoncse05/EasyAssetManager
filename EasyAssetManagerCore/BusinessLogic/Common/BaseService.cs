using EasyAssetManagerCore.Model.CommonModel;
using Microsoft.AspNetCore.Http;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace EasyAssetManagerCore.BusinessLogic.Common
{
    public abstract class BaseService : IDisposable
    {
        private readonly IHttpContextAccessor contextAccessor;
        public OracleConnection Connection { get; set; }
        public OracleTransaction Transaction { get; set; }
        public AppSession Sessoin { get; set; }
        public Message Message { get; set; }

        public BaseService(int connectionstring)
        {

            switch (connectionstring)
            {
                case 1:
                    Connection = new OracleConnection(ApplicationConstant.EbankConnectionString);
                    break;
                case 2:
                    Connection = new OracleConnection(ApplicationConstant.EsecConnectionString);
                    break;
                case 3:
                    Connection = new OracleConnection(ApplicationConstant.CbsConnectionString);
                    break;
                case 4:
                    Connection = new OracleConnection(ApplicationConstant.RemitConnectionString);
                    break;
                default:
                    Connection = new OracleConnection();
                    break;

            }
                    
            Message = new Message();
            //if (Connection.State != System.Data.ConnectionState.Open)
            //{
            //    try
            //    {
            //        Connection.Open();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception(ex.Message);
            //        throw new Exception("Failed to connect with database. please contact with system administrator.");
            //    }
            //}
        }

        public void Dispose()
        {
        }

        public void ConnectionOpen()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Close();
                Connection.Open();
            }


        }
        public void ConnectionClose()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();

        }
    }

    public enum ConnectionStringEnum
    {
        EbankConnectionString=1,
        EsecConnectionString=2,
        CbsConnectionString=3,
        RemitConnectionString=4
    }
}
