using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiveChatSystem.Models
{
    public class FileObject
    {
        public string fileName { get; set; }
        public string fileContent { get; set; }
        public string fileSize { get; set; }
        public string fileType { get; set; }
    }
}