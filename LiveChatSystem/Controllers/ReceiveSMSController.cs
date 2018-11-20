using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.AspNet.Common;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;
using LiveChatSystem.DataLayer;
using System.Data;
using LiveChatSystem.Hubs;

namespace LiveChatSystem.Controllers
{
    public class ReceiveSMSController : TwilioController
    {
        // GET: ReceiveSMS
        
        public TwiMLResult Index(SmsRequest incomingMessage)
        {
            //string clientId = string.Empty;
            //string UserId = string.Empty;
            //string recepientName = string.Empty;
            //string loanNumber = string.Empty;
            //string RecepientRole = string.Empty;
            //string shortUrl = string.Empty;
            //string connectionid = string.Empty;
            //string email = string.Empty;
            //string LoanNo = string.Empty;
            //var objUserChat = new UserChat();
            //var objSaveMessage = new ChatWithTwilio();

            //Message response if by chat only (Wrong Area, should be in ChatHub)
            //var messagingResponse = new MessagingResponse();
            //messagingResponse.Message("Your Response: " +
            //                          incomingMessage.Body, incomingMessage.AccountSid, incomingMessage.From);

            //Save Message
            //objSaveMessage.SaveMessage(recepientName, UserId, false, incomingMessage.Body, clientId, incomingMessage.From, true, loanNumber, incomingMessage.To, Convert.ToInt32(RecepientRole),shortUrl, LoanNo,connectionid);
            //Query Database for # of messages
            //DataTable dtCount= objSaveMessage.GetPhoneNumberInstances(incomingMessage.From);
            ////if > 1 then
            //if (Convert.ToInt32(dtCount.Rows[0][0]) > 1)
            //{
            //    //var objSendmsg = new ChatWithTwilio();
            //    string msg = "Please reply using the link provided, thank you.";
            //    string recipientId = recepientName.Trim() + incomingMessage.From.Trim();
            //    objSaveMessage.SendMessage(msg, incomingMessage.To, incomingMessage.From, loanNumber);
            //}
            //else
            //{
            //    DataTable dtUserDetails = objUserChat.GetAllUserDetailsForRecvdMsg(incomingMessage.From, incomingMessage.To);
            //    if (dtUserDetails.Rows.Count > 0)
            //    {
            //        DataRow row = dtUserDetails.Rows[0];
            //        clientId = row["ClientId"].ToString();
            //        recepientName = row["RecipientName"].ToString();
            //        loanNumber = row["LoanNumber"].ToString();
            //        RecepientRole = row["RecipientRole"].ToString();
            //        UserId = row["UserId"].ToString();
            //        shortUrl = row["ShortUrl"].ToString();
            //        connectionid = row["ConnectionId"].ToString();
            //        email = row["Email"].ToString();
            //        LoanNo = row["LoanNo"].ToString();
            //    }

            //    //Check if user is online (Not working correctly)
            //    string userStatus = objUserChat.GetUserStatus(incomingMessage.To);

            //    if (userStatus == "0")
            //    {
            //        objUserChat.SendMail(recepientName, loanNumber, incomingMessage.Body, email, LoanNo);
            //    }
            //    else
            //    {
            //        string recipientId = recepientName.Trim() + incomingMessage.From.Trim();
            //        ChatHub.SendMessages(UserId, clientId, incomingMessage.From, recipientId);
            //    }
            //}
            //return TwiML(messagingResponse);
            var messagingResponse = new MessagingResponse();
            messagingResponse.Message("Please reply using the link provided, thank you!", incomingMessage.From, incomingMessage.To);
            return new TwiMLResult(messagingResponse);
        }

    }
}