using LiveChatSystem.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using LiveChatSystem.Models;
using System.IO;
using LiveChatSystem.Hubs;

namespace LiveChatSystem.Controllers
{
    public class UserController : Controller
    {
        public ActionResult trychat()
        {
            //if (Session["UserName"] != null && Session["UserId"] != null)
            //{
            //    ViewBag.UserName = Session["UserName"].ToString();
            //    ViewBag.UserId = Session["UserId"].ToString();
            //}
            return View();
            

        }

        public ActionResult GetMessages( string userid, string clientId , string recepientPhoneNumber, string recipientId)
        {
            ChatRepository _chatRepository = new ChatRepository();
            return Json(_chatRepository.GetAllMessages(userid, clientId, recepientPhoneNumber, recipientId), JsonRequestBehavior.AllowGet);
           // return PartialView("_chatView", _chatRepository.GetAllMessages());
        }

        //Action called when recepient conversation is loaded with user
        public ActionResult UserChatScreen(string clientId, string recepientName, string recepientPhoneNumber, int loanNumber, string userPhoneNumber, int RecepientRole)
        {
            //ViewBag.clientId = clientId;
            //ViewBag.recepientName = recepientName;
            //ViewBag.recepientPhoneNumber = recepientPhoneNumber;
            //ViewBag.loanNumber = loanNumber;
            //ViewBag.userPhoneNumber = userPhoneNumber;
            //ViewBag.RecepientRole = RecepientRole;

            //var objRecepientUserConversation = new UserChat();
            //DataTable dtMessages = objRecepientUserConversation.GetAllMessageTemplates();
            //DataRow newRow = dtMessages.NewRow();
            //newRow[0] = "0";
            //newRow[1] = "Select message";
            //dtMessages.Rows.InsertAt(newRow, 0);
            //ViewBag.MessagesTemplates = new SelectList(dtMessages.AsDataView(), "ID", "MessageTemplate");//dtMessages.AsEnumerable().ToList();
            //DataTable dtUserConversation = objRecepientUserConversation.GetRecepientUserConversation(Convert.ToString(loanNumber), recepientPhoneNumber);
            //var myEnumerable = dtUserConversation.AsEnumerable();

            //List<ChatHistory> lstConversation =
            //    (from item in myEnumerable
            //     select new ChatHistory
            //     {
            //         RecipientName = item.Field<string>("RecipientName"),
            //         Message = item.Field<string>("Message"),
            //         Timestamp = item.Field<DateTime>("Timestamp"),
            //     }).ToList();
            //return View(lstConversation);
            return View();
        }
      
        public ActionResult _chatView(string clientId, string recepientName, string recepientPhoneNumber, int loanNumber, string userPhoneNumber, int RecepientRole, string userid, string ResultRole, string RecipientId, string loanNo)
        {
            ViewBag.clientId = clientId;
            ViewBag.recepientName = recepientName;
            ViewBag.recepientPhoneNumber = recepientPhoneNumber;
            ViewBag.loanNumber = loanNumber;
            ViewBag.userPhoneNumber = userPhoneNumber;
            ViewBag.RecepientRole = RecepientRole;
            ViewBag.UserId = userid;
            ViewBag.ResultRole = ResultRole;
            ViewBag.LoanNo = loanNo;
            var objUserConversationList = new UserChat();
            DataTable dtMessages = objUserConversationList.GetAllMessageTemplates();
            DataRow newRow = dtMessages.NewRow();
            newRow[0] = "0";
            newRow[1] = "Select message";
            dtMessages.Rows.InsertAt(newRow, 0);
            ViewBag.MessagesTemplates = new SelectList(dtMessages.AsDataView(), "ID", "MessageTemplate");//dtMessages.AsEnumerable().ToList();

            var objRecepientUserConversation = new UserChat();
            if (ResultRole == "0")
            {
                loanNumber = 0;
            }
            DataTable dtUserConversation = objRecepientUserConversation.GetRecepientUserConversation(Convert.ToString(loanNumber), recepientPhoneNumber, RecipientId,loanNo);
            var myEnumerable = dtUserConversation.AsEnumerable();

            List<ChatHistory> lstConversation =
                (from item in myEnumerable
                 select new ChatHistory
                 {
                     RecipientName = item.Field<string>("RecipientName"),
                     Message = item.Field<string>("Message"),
                     Timestamp = item.Field<DateTime>("Timestamp"),
                     IsSentByUser = item.Field<bool>("IsSentByUser")
                 }).ToList();
            // return Json(lstConversation, JsonRequestBehavior.AllowGet);
                       ViewBag.lstConversations = lstConversation;
            return PartialView("_chatView", ViewBag.lstConversations);

        }

        public int sendvcard(string recepientPhoneNumber, string userPhoneNumber, string userid, string clientId)
        {
            var objUser = new UserChat();
            DataTable userdetails = objUser.GetUserDetailsFromUserID(userid, clientId);
            string firstName = userdetails.Rows[0]["FirstName"].ToString();
            string LastName = userdetails.Rows[0]["LastName"].ToString();
            string PhoneNumber = userdetails.Rows[0]["PhoneNumber"].ToString();
            string Email = userdetails.Rows[0]["Email"].ToString();




            var vCard = new System.Text.StringBuilder();
            vCard.Append("BEGIN:VCARD");
            vCard.AppendLine();
            vCard.Append("VERSION:2.1");
            vCard.AppendLine();

            // Name
            vCard.Append($"N:  {LastName};{firstName};");
            vCard.AppendLine();

            vCard.Append($"FN:{firstName} {LastName}");
            vCard.AppendLine();
            vCard.Append("TEL");
            vCard.Append(";");
            vCard.Append("CELL");
            vCard.Append(";");
            vCard.Append("VOICE:");
            vCard.Append(PhoneNumber);
            vCard.AppendLine();

            // Email
            vCard.Append("EMAIL");
            vCard.Append(";");
            vCard.Append("PREF");
            vCard.Append(";");
            vCard.Append("INTERNET:");
            vCard.Append(Email);
            vCard.AppendLine();
            vCard.Append("END:VCARD");

            string result = vCard.ToString();
            string fname = "/" + PhoneNumber + ".vcf";

            string filename = Server.MapPath(fname);
            FileInfo info = new FileInfo(filename);
            using (StreamWriter writer = info.CreateText())
            {
                writer.WriteLine(result);

            }

            var objChatWithTwilio = new ChatWithTwilio();

            objChatWithTwilio.SendMMS(filename, userPhoneNumber, recepientPhoneNumber);

            return 1;

        }

        //Action called when message is send on User chat screen
        public JsonResult SendMessageByUser(bool isSentByUser,string message, string clientId, string recepientName, string recepientPhoneNumber, string loanNumber, string userPhoneNumber, int RecepientRole, string userid, string shortUrl="", bool sentFromMobile = true, string connectionid= "", string Email="", string loanNo="")
        {
            var objUserChat = new UserChat();
            List<string> Result = new List<string>();
            string recipientId = string.Empty;
            var objChatWithTwilio = new ChatWithTwilio();
            if (string.IsNullOrEmpty(shortUrl))
            {
                shortUrl = Path.GetRandomFileName();
                shortUrl = shortUrl.Replace(".", "").Substring(0,5);
            }
            string smsmessage = message;
            var objChat = new ChatWithTwilio();
            string userStatus = objUserChat.GetUserStatus(userPhoneNumber);
            recipientId = recepientName.Trim() + recepientPhoneNumber.Trim();
           // if (userStatus == "False" && isSentByUser == false)
            //{
              //  objUserChat.SendMail(recepientName, loanNumber, message,Email, loanNo);
            //}
            if ((isSentByUser == false) && (sentFromMobile == true))
            {
                DataTable dtNoOfMstgs = objChat.GetTotalNoOfMsg(recepientPhoneNumber, userPhoneNumber, loanNumber);
                if (dtNoOfMstgs.Rows.Count > 1)
                {
                    smsmessage = " (Reply-> mtgmsg.co/C/?u=" + shortUrl + ")";
                    message = " (Reply-> mtgmsg.co/C/?u=" + shortUrl + ")";
                }
            }
            else if(isSentByUser ==true) {
                smsmessage = message+ " (Reply-> mtgmsg.co/C/?u=" + shortUrl + " )";
            }
            string isReceived = "";
            if ((isSentByUser == false) && (sentFromMobile == false))
            {
                isReceived = "1";
            }
            else
            {
                isReceived = objChatWithTwilio.SendMessage(smsmessage, userPhoneNumber, recepientPhoneNumber, loanNumber, shortUrl);
            }
            if (isReceived == "1")
            {
                objChatWithTwilio.SaveMessage(recepientName,userid,isSentByUser, message, clientId, recepientPhoneNumber, sentFromMobile, loanNumber, userPhoneNumber, RecepientRole,shortUrl, loanNo, connectionid);
                if(isSentByUser==false)
                {
                    ChatHub.SendMessages(userid, clientId, recepientPhoneNumber, recipientId, loanNo, recepientName, loanNumber, message, Email, Convert.ToString(TempData.Peek("LoanGUID")));
                }

            }
            Result.Add(recipientId);
            Result.Add(isReceived);
            Result.Add(message);

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        //Action called when all recepient are listed with whom user has done conversation
        public ActionResult UserChatList(string ClientID="", string UserID="", string LoanGUID="")
        {
            //ClientID = "3011138444";
            //UserID = "ccl1229137";
            //LoanGUID = "{d0150da9-5852-4f5f-9f2c-6fa35119bcff}";
            Session["UserName"] = ClientID;
            Session["UserId"] = UserID;
            TempData["LoanGUID"] = LoanGUID;
            var objUserConversationList = new UserChat();
            DataTable dtUserChat = objUserConversationList.GetUserChatList(ClientID, UserID, LoanGUID);
            var myEnumerable = dtUserChat.AsEnumerable();
           
            List<ChatList> myClassList =
                (from item in myEnumerable
                 select new ChatList
                 {
                     LoanUpdateID = item.Field<Int32>("LoanUpdateID"),
                     ClientId = item.Field<string>("ClientId"),
                     RecipientName = item.Field<string>("RecipientName"),
                     Message = item.Field<string>("Message"),
                     DateTimeReceived = item.Field<DateTime>("DateTimeReceived"),
                     RecepientInitials = item.Field<string>("RecepientInitials"),
                     RecipientPhoneNumber = item.Field<string>("RecipientPhoneNumber"),
                     PhoneNumber = item.Field<string>("PhoneNumber"),
                     UserId = UserID,
                     ColorCode = item.Field<string>("ColorCode"),
                     RecipientRole = item.Field<int>("RecipientRole"),
                     RecipientId = item.Field<string>("RecipientId"),
                     LoanNo = item.Field<string>("LoanNo"),
                 //    Email = item.Field<string>("Email"),
                 }).ToList();
            return View(myClassList);
            //  return View();
        }

    }
}