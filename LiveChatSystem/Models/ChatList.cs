using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiveChatSystem.Models
{
    public class ChatList
    {
        
        public int LoanUpdateID { get; set; }
        public string ClientId { get; set; }
        public string LoanNo { get; set; }
        public string RecipientName { get; set; }
        public string Message { get; set; }
        public DateTime DateTimeReceived { get; set; }
        public string RecepientInitials { get; set; }
        public string RecipientPhoneNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public string ColorCode { get; set; }
        public string RecipientId { get; set; }
        public string Email { get; set; }
        public int RecipientRole { get; set; }
        
        
    }
}