using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace LiveChatSystem.DataLayer
{
    public class UserChat
    {
        String strConnString = ConfigurationManager.ConnectionStrings["DbLiveChat"].ConnectionString;
        public DataTable GetUserChatList(string ClientId, string UserId, string LoanGUID)
        {

            DataTable dtChatList = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetAllUserConversationList";
            cmd.Parameters.Add("@ClientId", SqlDbType.VarChar).Value = ClientId;
            cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = UserId;
            cmd.Parameters.Add("@LoanGUID", SqlDbType.VarChar).Value = LoanGUID;
            cmd.Connection = con;
            try
            {
                con.Open();

                reader = cmd.ExecuteReader();

                dtChatList.Load(reader);

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
            return dtChatList;
        }

        public DataTable GetRecepientUserConversationByShortUrl(string shortUrl)
        {

            DataTable dtConversation = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetChatMessagesForRecepientByShortUrl";
            cmd.Parameters.Add("@shortUrl", SqlDbType.VarChar).Value = shortUrl;

            cmd.Connection = con;
            try
            {
                con.Open();

                reader = cmd.ExecuteReader();

                dtConversation.Load(reader);

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
            return dtConversation;
        }
        public DataTable GetRecepientUserConversation(string loanNumber, string RecipientPhoneNumber, string RecipientId, string loanNo)
        {

            DataTable dtConversation = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetChatMessagesForRecepient";
            cmd.Parameters.Add("@LoanNumber", SqlDbType.VarChar).Value = loanNumber;
            cmd.Parameters.Add("@RecipientPhoneNumber", SqlDbType.VarChar).Value = RecipientPhoneNumber;
            cmd.Parameters.Add("@RecipientId", SqlDbType.VarChar).Value = RecipientId;
            cmd.Parameters.Add("@loanNo", SqlDbType.VarChar).Value = loanNo;
            cmd.Connection = con;
            try
            {
                con.Open();

                reader = cmd.ExecuteReader();

                dtConversation.Load(reader);

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
            return dtConversation;
        }
        public DataTable GetAllUploadedDocsUser(string loanNumber)
        {

            DataTable dtDocuments = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetDocumentsForUser";
            cmd.Parameters.Add("@LoanNumber", SqlDbType.VarChar).Value = loanNumber;

            cmd.Connection = con;
            try
            {
                con.Open();

                reader = cmd.ExecuteReader();

                dtDocuments.Load(reader);

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
            return dtDocuments;
        }
        public DataTable GetAllMessageTemplates()
        {

            DataTable dtMessage = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetAllMessageTemplates";


            cmd.Connection = con;
            try
            {
                con.Open();

                reader = cmd.ExecuteReader();

                dtMessage.Load(reader);

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
            return dtMessage;
        }
        public string SendMail(string recipientName,string loanNumber,string message, string UserEmail, string LoanNo)
        {
            var AccountName = ConfigurationManager.AppSettings["EmailAccountName"].ToString();
            var AccountPwd = ConfigurationManager.AppSettings["EmailAccountPassWord"].ToString();
            string isSent = "0";
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(AccountName,AccountPwd);
                String body = "Hello,\n You recieved a new message from " + recipientName + " regarding Loan number "+LoanNo +".\n The message says:\n" + message;
                String subject = "From MM chat system: Message recieved from " + recipientName;
                MailMessage mm = new MailMessage(AccountName, UserEmail, subject, body);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);

                             
                isSent = "1";
            }
            catch (Exception ex)
            {

            }
            return isSent;
        }
        public string GetUserStatus(string UserPhoneNumber)
        {

            string status = string.Empty;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetUserStatus";
            cmd.Parameters.Add("@UserPhoneNumber", SqlDbType.VarChar).Value = UserPhoneNumber;

            cmd.Connection = con;
            try
            {
                con.Open();

                 status = cmd.ExecuteScalar().ToString();

               

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
            return status;
        }

        public DataTable GetLenderName(string ClientID)
        {

            DataTable dtlender = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetLenderName";
            cmd.Parameters.Add("@CLientID", SqlDbType.VarChar).Value = ClientID;

            cmd.Connection = con;
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                dtlender.Load(reader);
                



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
            return dtlender;
        }
        public DataTable GetAllUserDetailsForRecvdMsg(string ReceivedFrom, string SendTo)
        {

            DataTable dtMessage = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetUserDetailsFromReceivedMessage";
            cmd.Parameters.Add("@ReceivedFrom", SqlDbType.VarChar).Value = ReceivedFrom;
            cmd.Parameters.Add("@SendTo", SqlDbType.VarChar).Value = SendTo;

            cmd.Connection = con;
            try
            {
                con.Open();

                reader = cmd.ExecuteReader();

                dtMessage.Load(reader);

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
            return dtMessage;
        }
        public int SaveDocs(string docName, string docPath, string docSize, string docType, string ShortUrl,string ClientID,string LoanNo,string BlobFileName="")
        {
            int rowid = 0;
            try
            {
                SqlConnection cn = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand("SaveDocument", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DocName", docName);
                cmd.Parameters.AddWithValue("@DocPath", docPath);
                cmd.Parameters.AddWithValue("@DocSize", docSize);
                cmd.Parameters.AddWithValue("@DocType", docType);
                cmd.Parameters.AddWithValue("@ShortUrl", ShortUrl);
                cmd.Parameters.AddWithValue("@ClientID", ClientID);
                cmd.Parameters.AddWithValue("@LoanNo", LoanNo);
                cmd.Parameters.AddWithValue("@BlobFileName", BlobFileName);
                cn.Open();
                rowid = (int)cmd.ExecuteScalar();
                cn.Close();

            }
            catch (Exception ex)
            {

            }
            return rowid;

        }
        public void UpdateDocs(int DocId, string DocPath,string BlobFileName)
        {
            try
            {
                SqlConnection cn = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand("UpdateDocs", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DocId", DocId);
                cmd.Parameters.AddWithValue("@DocPath", DocPath);
                cmd.Parameters.AddWithValue("@BlobFileName", BlobFileName);
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