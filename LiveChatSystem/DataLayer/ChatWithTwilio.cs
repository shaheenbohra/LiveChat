using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace LiveChatSystem.DataLayer
{
    public class ChatWithTwilio
    {
        String strConnString = ConfigurationManager.ConnectionStrings["DbLiveChat"].ConnectionString;
        public DataTable GetTotalNoOfMsg(string RecipientPhoneNumber, string UserPhoneNumber, string LoanNumber)
        {

            DataTable dtMsgs = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetTotalNoOfMsg";
            cmd.Parameters.Add("@RecipientPhoneNumber", SqlDbType.VarChar).Value = RecipientPhoneNumber;
            cmd.Parameters.Add("@UserPhoneNumber", SqlDbType.VarChar).Value = UserPhoneNumber;
            cmd.Parameters.Add("@LoanNumber", SqlDbType.VarChar).Value = LoanNumber;
            cmd.Connection = con;
            try
            {
                con.Open();

                reader = cmd.ExecuteReader();

                dtMsgs.Load(reader);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return dtMsgs;
        }
        public DataTable GetTotalNoOfMsgFromMobile(string RecipientPhoneNumber, string UserPhoneNumber, string LoanNumber)
        {

            DataTable dtMsgs = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetTotalNoOfMsgByMobile";
            cmd.Parameters.Add("@RecipientPhoneNumber", SqlDbType.VarChar).Value = RecipientPhoneNumber;
            cmd.Parameters.Add("@UserPhoneNumber", SqlDbType.VarChar).Value = UserPhoneNumber;
            cmd.Parameters.Add("@LoanNumber", SqlDbType.VarChar).Value = LoanNumber;
            cmd.Connection = con;
            try
            {
                con.Open();

                reader = cmd.ExecuteReader();

                dtMsgs.Load(reader);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return dtMsgs;
        }

        public DataTable GetPhoneNumberInstances(string UserPhoneNumber)
        {

            DataTable dtMsgs = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetPhoneNumberInstances";
            cmd.Parameters.Add("@UserPhoneNumber", SqlDbType.VarChar).Value = UserPhoneNumber;
            cmd.Connection = con;
            try
            {
                con.Open();

                reader = cmd.ExecuteReader();

                dtMsgs.Load(reader);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return dtMsgs;
        }

        public String SendMessage(string sendMessage, string from /*= "+17342940754"*/, string To /*= "+971545473919"*/, string loanNumber = "0", string shortUrl = "")
        {
            string returnMsg = string.Empty;
            
            string accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"].ToString();
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"].ToString();

            TwilioClient.Init(accountSid, authToken);
            try
            {
                var message = MessageResource.Create(
                    to: new PhoneNumber(To),
                    from: new PhoneNumber(from),
                    body: sendMessage);
                returnMsg = "1";
            }
            catch (Exception ex)
            {
                returnMsg = ex.Message.ToString();
            }



            return returnMsg;
        }
        public void SaveMessage(string RecipientName, string UserId, bool IsSentByUser, string message, string clientId , string recepientPhoneNumber, bool sentFromMobile, string loanNumber, string userPhoneNumber, int RecepientRole, string shortUrl, string LoanNo, string connectionid="")
        {
            //if (string.IsNullOrEmpty(shortUrl))
            //{
            //    shortUrl = Path.GetRandomFileName();
            //    shortUrl = shortUrl.Replace(".", "");
            //}
            try
            {
                SqlConnection cn = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand("SaveMessage", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RecipientName", RecipientName);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@IsSentByUser", IsSentByUser);
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters.AddWithValue("@clientId", clientId);
                cmd.Parameters.AddWithValue("@recepientPhoneNumber", recepientPhoneNumber);
                cmd.Parameters.AddWithValue("@userPhoneNumber", userPhoneNumber);
                cmd.Parameters.AddWithValue("@SentFromMobile", sentFromMobile);
                cmd.Parameters.AddWithValue("@LoanNumber", loanNumber);
                cmd.Parameters.AddWithValue("@RecipientRole", RecepientRole);
                cmd.Parameters.AddWithValue("@ShortUrl", shortUrl);
                cmd.Parameters.AddWithValue("@ConnectionId", connectionid);
                cmd.Parameters.AddWithValue("@RecipientId", RecipientName.Trim()+recepientPhoneNumber.Trim());
                cmd.Parameters.AddWithValue("@LoanNo", LoanNo);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {

            }
        }
        public void SaveReceivedMessage(string message, string clientId, string recepientPhoneNumber, string userPhoneNumber)
        {
            try
            {
                SqlConnection cn = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand("SaveReceivedMessage", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters.AddWithValue("@clientId", clientId);
                cmd.Parameters.AddWithValue("@recepientPhoneNumber", recepientPhoneNumber);
                cmd.Parameters.AddWithValue("@userPhoneNumber", userPhoneNumber);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

            }
            catch (Exception ex)
            {

            }
        }
    }
}