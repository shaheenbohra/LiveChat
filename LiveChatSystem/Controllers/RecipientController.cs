using LiveChatSystem.DataLayer;
using LiveChatSystem.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LiveChatSystem.Controllers
{
    public class RecipientController : Controller
    {
        private static string _connectionString = "DefaultEndpointsProtocol=https;AccountName=mmchat;AccountKey=CVkj1lLncXn4c3/Y9nei94fdeDS07kYo1gdFl2FvRnbkbaQ8G4y0sCI6DytnYkTKBot0pzl2A5ocXnMlvbgVDA==;EndpointSuffix=core.windows.net";
        // GET: Recepient
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Helloworld()
        {

            return View();
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string size = files[i].ContentLength.ToString();
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Documents/"), fname);
                        var path = Path.Combine(Server.MapPath("~/Documents/"), fname);
                        file.SaveAs(fname);
                        var objSaveDoc = new UserChat();
                     //   objSaveDoc.SaveDocs(file.FileName, path, size, "test", Convert.ToString(TempData["shortUrl"]));
                    }

                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        [Route("Chat"), Route("C")]
        public ActionResult RecipientChat(string shortUrl, string u)
        {
            if (u != null)
                shortUrl = u;
            var objRecepientUserConversation = new UserChat();
            DataTable dtUserConversation = objRecepientUserConversation.GetRecepientUserConversationByShortUrl(shortUrl);
            ViewBag.shortUrl = shortUrl;
            TempData["shortUrl"] = shortUrl;
            List<ChatHistory> lstConversation = new List<ChatHistory>();
            if (dtUserConversation.Rows.Count > 0)
            {
                ViewBag.loanNumber = dtUserConversation.Rows[0]["LoanNumber"];
                ViewBag.loanNo = dtUserConversation.Rows[0]["LoanNo"];
                TempData["LoanNo"]= dtUserConversation.Rows[0]["LoanNo"];
                TempData["loanNumber"] = dtUserConversation.Rows[0]["LoanNumber"];
                ViewBag.clientId = dtUserConversation.Rows[0]["Clientid"];
                ViewBag.recepientName = dtUserConversation.Rows[0]["RecipientName"];
                ViewBag.recepientPhoneNumber = dtUserConversation.Rows[0]["RecipientPhoneNumber"];
                //ViewBag.IsSentByUser = dtUserConversation.Rows[0]["IsSentByUser"];
                ViewBag.userPhoneNumber = dtUserConversation.Rows[0]["UserPhoneNumber"];
                ViewBag.RecepientRole = dtUserConversation.Rows[0]["RecipientRole"];
                TempData["RecepientRole"]= dtUserConversation.Rows[0]["RecipientRole"];
                ViewBag.UserId = dtUserConversation.Rows[0]["UserId"];
                ViewBag.ResultRole = "0";//Need to check and get
                ViewBag.ConnectionId = dtUserConversation.Rows[0]["ConnectionId"];
                DataTable dtlender = objRecepientUserConversation.GetLenderName(ViewBag.clientId);
                ViewBag.companyName = dtlender.Rows[0]["CompanyName"];
                ViewBag.WebsiteHeaderPreference = dtlender.Rows[0]["WebsiteHeaderPreference"];
                ViewBag.LogoFileName = dtlender.Rows[0]["LogoFileName"];
                TempData["UserId"] = dtUserConversation.Rows[0]["UserId"];
                TempData["recepientPhoneNumber"] = dtUserConversation.Rows[0]["RecipientPhoneNumber"];
                TempData["clientId"] = dtUserConversation.Rows[0]["Clientid"];
                ViewBag.RecipientId = dtUserConversation.Rows[0]["RecipientId"];
                ViewBag.Email = dtUserConversation.Rows[0]["Email"];
                var myEnumerable = dtUserConversation.AsEnumerable();

                lstConversation =
                    (from item in myEnumerable
                     select new ChatHistory
                     {
                         RecipientName = item.Field<string>("RecipientName"),
                         Message = item.Field<string>("Message"),
                         Timestamp = item.Field<DateTime>("Timestamp"),
                         SendBy = item.Field<string>("SendBy"),
                         IsSentByUser = item.Field<bool>("IsSentByUser"),
                         LoanNo = item.Field<string>("LoanNo")

                     }).ToList();
            }
            return View(lstConversation);
        }
        [Route("DocUpload")]
        public ActionResult DocsUpload(string logoFileName, string companyName, string websitePreference, string shortUrl, string u)
        {
            if (u != null)
                shortUrl = u;
            ViewBag.companyName = companyName;
            ViewBag.WebsiteHeaderPreference = websitePreference;
            ViewBag.LogoFileName = logoFileName;
            ViewBag.shortUrl = shortUrl;
            TempData["shortUrl"] = shortUrl;

            return View();
        }
        [HttpPost]
        public JsonResult SaveFile()
        {
            string shorturl = Convert.ToString(TempData.Peek("shortUrl"));
            string ClientID = Convert.ToString(TempData.Peek("ClientID"));
            string LoanNo = Convert.ToString(TempData.Peek("LoanNo"));
            string RecepientRole = Convert.ToString(TempData.Peek("RecepientRole"));
            var objUserChat = new UserChat();
            var result = "0";
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        //fname = Path.Combine(Server.MapPath("~/Documents/"), fname);
                        //file.SaveAs(fname);
                        System.IO.Stream fs = files[i].InputStream;
                        System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                        Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);



                        string base64 = base64String.Substring(base64String.IndexOf(',') + 1);
                        string blobPath = "";
                        base64 = base64.Trim('\0');
                        string BlobFileName = LoanNo + "_" + RecepientRole + "_";
                        double size = (files[i].ContentLength / 1024f) / 1024f;
                        string ftype = fname.Split('.').Last();
                        int rowid = objUserChat.SaveDocs(fname, blobPath, Math.Round(size, 2) + "MB", files[i].ContentType, shorturl, ClientID, LoanNo, BlobFileName);
                        BlobFileName = BlobFileName + rowid.ToString()+"."+ftype;
                        blobPath = UploadImage(base64, BlobFileName);
                        objUserChat.UpdateDocs(rowid, blobPath, BlobFileName);
                    }
                    // Returns message that successfully uploaded  
                    result = "1";
                    return Json(result);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        [HttpPost]
        public JsonResult SaveImage(List<FileObject> fileObject, string shortUrl, string u)
        {
            if (u != null)
                shortUrl = u;
            var objUserChat = new UserChat();
            var result = "0";
            try
            {
                foreach (var file in fileObject)
                {
                    string base64 = file.fileContent.Substring(file.fileContent.IndexOf(',') + 1);

                    base64 = base64.Trim('\0');
                    string blobPath = UploadImage(base64, file.fileName);
                    //objUserChat.SaveDocs(file.fileName, blobPath, file.fileSize, file.fileType, shorturl);
                    //save in db

                }
                result = "1";
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }

        public static string UploadImage(string imageData, string filename)
        {

            CloudBlobContainer container = GetContainer("documents");
            byte[] imageBytes = Convert.FromBase64String(imageData);
            CloudBlockBlob blob = container.GetBlockBlobReference(filename);
            blob.Properties.ContentType = "Jpeg";
            blob.UploadFromByteArray(imageBytes, 0, imageBytes.Length);
            return blob.StorageUri.PrimaryUri.AbsoluteUri;
        }

        private static CloudStorageAccount GetConnection()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);
            return storageAccount;
        }

        //private static string GetFileName(ImageType imageType, int cardId)
        //{
        //    switch (imageType)
        //    {
        //        case ImageType.Cover:
        //            return string.Format("Cover-{0}.jpeg", cardId);
        //        case ImageType.Profile:
        //            return string.Format("Profile-{0}.jpeg", cardId);
        //        default:
        //            return null;
        //    }


        //}
        private static CloudBlobContainer GetContainer(string containerName)
        {
            CloudStorageAccount storageAccount = GetConnection();
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            return container;
        }

    }

}