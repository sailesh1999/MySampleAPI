using EventsApi.Models;
using EventsApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace EventsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrCodeController : ControllerBase
    {
        private IMongoCollection<User> _userCollection;
        public QrCodeController(IEventsDatabaseSettings settings,IMongoClient mongoClient){
            var client=mongoClient;
            var database = client.GetDatabase(settings.DatabaseName);
            _userCollection = database.GetCollection<User>(settings.UsersCollectionName);
        }
        [HttpGet]
        public ActionResult SendQRCodes(){
            try{
                foreach(var user in _userCollection.Find(User=>true).ToList()){
                    GenerateQRCode(user.UserId);
                    SendEmail(user);
                }
                //GenerateQRCode();
                //SendEmail();
                return StatusCode(200);
            }
            catch(Exception ex){
                return StatusCode(500,ex);
            }    
        }

        private void GenerateQRCode(double userId){
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(userId.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save(userId.ToString()+".bmp");
        }
         protected void SendEmail(User user)  
        {  
            var toEmailID = user.Email;
            try  
            {  
                Console.WriteLine("Sending Email to "+toEmailID);
                //MailSettings.SMTPServer = Convert.ToString("smtp.gmail.com");
                MailMessage message = new MailMessage();  
                message.To.Add(toEmailID);// Email-ID of Receiver  
                message.Subject = "Your ID | Please show at the event venue";// Subject of Email  
                message.From = new System.Net.Mail.MailAddress("sailesh2929@gmail.com");// Email-ID of Sender  
                AlternateView imgView=AlternateView.CreateAlternateViewFromString(
                    "<b>Your ID for the event: </b>"+user.UserId.ToString()+"<br/><img src=cid:QRCode height=400 width=400>"
                    ,null,
                    "text/html");
                LinkedResource lr = new LinkedResource(user.UserId.ToString()+".bmp");
                lr.ContentId="QRCode";
                imgView.LinkedResources.Add(lr);
                message.AlternateViews.Add(imgView);
                message.Body=lr.ContentId;            
                // message.IsBodyHtml = true;  
                // message.AlternateViews.Add(Mail_Body());  
                SmtpClient SmtpMail = new SmtpClient();
                SmtpMail.Credentials = new System.Net.NetworkCredential("sailesh2929@gmail.com", "supersailesh123");
                SmtpMail.Host = "smtp.gmail.com";//name or IP-Address of Host used for SMTP transactions  
                SmtpMail.Port = 587;//Port for sending the mail  
                // SmtpMail.Credentials = new System.Net.NetworkCredential("", "");//username/password of network, if apply  
                // SmtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;  
                SmtpMail.EnableSsl = true;  
                // SmtpMail.ServicePoint.MaxIdleTime = 0;
                // SmtpMail.ServicePoint.SetTcpKeepAlive(true, 2000, 2000);  
                // message.BodyEncoding = Encoding.Default;  
                // message.Priority = MailPriority.High;  
                SmtpMail.Send(message); //Smtpclient to send the mail message  
                Console.WriteLine("Email sent to "+toEmailID+" successful");
            }  
            catch (Exception)  
            { 
                Console.WriteLine("Failed sending email to "+toEmailID);
                throw new Exception();
            }  
        }              
    }
}