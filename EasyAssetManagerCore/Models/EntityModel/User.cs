using EasyAssetManagerCore.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class User
    {
        public User()
        {
            Message = new Message();
        }
        public string USER_ID { get; set; }
        public string USER_PASS { get; set; }
        public string USER_NAME { get; set; }
        public string USER_TYPE { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string FINGER_DATA { get; set; }
        public string BRANCH_CODE { get; set; }
        public string DEPT_CODE { get; set; }
        public string EMP_ID { get; set; }
        public string AGENT_ID { get; set; }
        public string OUTLET_ID { get; set; }
        public string MOD_ID { get; set; }
        public string ACTIVE { get; set; }
        public string BLOCKED { get; set; }
        public string OTP_REQ { get; set; }
        public string CLIENT_IP { get; set; }
        public string BIND_IP { get; set; }
        public string PASS_EXP_DATE { get; set; }
        public string LOGGEDIN { get; set; }
        public string LOCKED { get; set; }
        public Nullable<decimal> TOT_SUCCESS_CNT { get; set; }
        public Nullable<decimal> TOT_FAILED_CNT { get; set; }
        public Nullable<decimal> CONS_FAILED_CNT { get; set; }
        public string LAST_LOGIN_TIME { get; set; }
        public string INS_BY { get; set; }
        public string INS_DATE { get; set; }
        public string UPD_BY { get; set; }
        public string UPD_DATE { get; set; }
        public string AUTH_BY { get; set; }
        public string AUTH_DATE { get; set; }
        public string OTP_REQ_TYPE { get; set; }
        public string OTP_USER_TYPE { get; set; }
        public string OTP_USER_REF_NO { get; set; }
        public string OTP_REQ_REF_NO { get; set; }
        public string OTP_REQ_STATUS { get; set; }
        public string BRANCH_NAME { get; set; }
        public string DEPT_NAME { get; set; }
        public string AGENT_NAME { get; set; }
        public string USER_DESC { get; set; }
        public string REQ_BY { get; set; }
        public string REQ_DATE { get; set; }
        public string RST_DATE { get; set; }
        public string RST_BY { get; set; }
        public string RST_SLNO { get; set; }
        public string INACTIVE_REASON { get; set; }
        public string INACTV_BY { get; set; }
        public string INACTV_DATE { get; set; }
        public string ACTV_SLNO { get; set; }
        public string USER_STATUS { get; set; }
        public string STATION_IP { get; set; }
        public string LOGIN_TIME { get; set; }
        public string SESSION_ID { get; set; }
        public string hf_user_status { get; set; }

        public Message  Message { get; set; }
    }
}
