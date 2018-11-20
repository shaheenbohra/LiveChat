using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LiveChatSystem.Hubs;

namespace LiveChatSystem.Models
{
    public class ChatRepository
    {
        String strConnString = ConfigurationManager.ConnectionStrings["DbLiveChat"].ConnectionString;

        public IEnumerable<ChatHistory> GetAllMessages(string userId, string clientId,string recepientPhoneNumber, string recipientId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = "0";
                recepientPhoneNumber = "0";
                clientId = "0";
            }
            var messages = new List<ChatHistory>();
            DataTable dtChatList = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetAllMessagesFromRecipient";
            cmd.Parameters.Add("@clientId", SqlDbType.VarChar).Value = clientId;
            cmd.Parameters.Add("@userId", SqlDbType.VarChar).Value = userId;
            cmd.Parameters.Add("@recepientPhoneNumber", SqlDbType.VarChar).Value = recepientPhoneNumber;
            cmd.Parameters.Add("@recipientId", SqlDbType.VarChar).Value = recipientId;
            cmd.Connection = con;
            try
            {
                con.Open();

                reader = cmd.ExecuteReader();

                dtChatList.Load(reader);
                if (dtChatList.Rows.Count > 0)
                {


                    messages.Add(item: new ChatHistory
                    {
                        Message = (string)dtChatList.Rows[0]["Message"],
                        Timestamp = Convert.ToDateTime(dtChatList.Rows[0]["Timestamp"]),
                        RecipientName = (string)dtChatList.Rows[0]["RecipientName"]
                    });
                }

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
            
            //using (var connection = new SqlConnection(strConnString))
            //{
            //    connection.Open();

            //    using (var command = new SqlCommand(@"SELECT TOP 1 [Message], [Timestamp] FROM [dbo].[ChatHistory]  ORDER BY [ChatId] desc", connection))

            //   // using (var command = new SqlCommand(@"SELECT TOP 1 [Message], [Timestamp], [UserId], [ClientId],[IsSentByUser],[RecipientPhoneNumber],[RecipientRole] FROM [dbo].[ChatHistory] WHERE UserId="+userId+" AND clientid="+clientId+" AND RecipientPhoneNumber ="+recepientPhoneNumber+" ORDER BY [ChatId] desc", connection))
            //    {
            //        command.Notification = null;

            //        var dependency = new SqlDependency(command);
            //        dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

            //        if (connection.State == System.Data.ConnectionState.Closed)
            //            connection.Open();

            //        var reader = command.ExecuteReader();
            //        DataTable dt = new DataTable();
            //        dt.Load(reader);

            //        if (dt.Rows.Count > 0)
            //        {


            //            messages.Add(item: new ChatHistory
            //            {
            //                Message = (string)dt.Rows[0]["Message"],
            //                Timestamp = Convert.ToDateTime(dt.Rows[0]["Timestamp"])
            //            });
            //        }
            //    }

            //}
            return messages;


        }

        //private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        //{
        //    if (e.Type == SqlNotificationType.Change)
        //    {
        //        ChatHub.SendMessages();
        //    }
        //}
    }
}