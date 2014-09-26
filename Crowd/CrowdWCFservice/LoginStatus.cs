using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Data.Entity;
using System.Runtime.Serialization;
using Portal.Repository;
using Portal.Model;

namespace CrowdWCFservice
{
    public class LoginStatus : ApiController
    {
        public LoginStatus() { }

        public LoginStatus(string token)
        {

        }
        private string _token = string.Empty;
        private string _message = string.Empty;
        private bool _success = false;

        //[DataMember]
        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }

        //[DataMember]
        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        //[DataMember]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// Return Token Object with value
        /// </summary>
        /// <param name="pUseId"></param>
        /// <returns></returns>

        public Token GetUserToken(string PEmail)
        {
            Token objToken = new Token();

            var db = new UnitOfWork();
            User user = new User();
            //user = db.tblUsers.Get().FirstOrDefault(l => l.Email == PEmail.Trim() && (l.TokenExpireTime > DateTime.Now || l.TokenExpireTime == null));
            user = db.User.Get().FirstOrDefault(l => l.Email == PEmail.Trim() && l.TokenExpireTime > DateTime.Now);
            if (user != null)
            {
                objToken.Email = Convert.ToString(user.Email);
                // objToken.UserType = Convert.ToInt16(obj.UserType);
                objToken.TokenExpiretime = Convert.ToDateTime(user.TokenExpireTime);
                objToken.TokenValue = Convert.ToString(user.DeviceToken);
            }
            return objToken;
        }


        public static Token.tbl_TokenInfo ValidateToken(string token, string userID)
        {
            var db = new UnitOfWork();
            User obj = db.User.Get().FirstOrDefault(s => s.Token == token && s.ID == Convert.ToInt64(userID));

            if (obj != null)
            {
                //===============Not using token expire time================
                //if (obj.TokenExpireTime > DateTime.Now)
                //{
                obj.TokenExpireTime = DateTime.Now.AddMinutes(30.00);
                obj.Token = token;
                db.User.Update(obj);
                db.SaveChanges();
                return EncryptionDecryption.Encoding.DeserializeXmlString<CrowdWCFservice.Token.tbl_TokenInfo>(EncryptionDecryption.Encoding.GetDecrypt(token));
                //}
                //else
                //{
                //    return null;
                //}
                //===========================End============================
            }
            return null;
        }


    }
}