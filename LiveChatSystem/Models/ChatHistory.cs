using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiveChatSystem.Models
{
    public class ChatHistory
    {
        public int LoanNumber { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhoneNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public bool SentFromMobile { get; set; }
        public string Message { get; set; }
        public string SendBy { get; set; }
        public string DocName { get; set; }
        public string DocType { get; set; }
        public string DocSize { get; set; }
        public string ConnectionId { get; set; }
        public bool IsSentByUser { get; set; }
        public string LoanNo { get; set; }

    }
}