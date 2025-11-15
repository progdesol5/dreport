using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RHReportScheduler.Utilities
{
    public static class EmailHelper
    {
        public static void SendEmail(string subject, string body, string email)
        {
            if (String.IsNullOrEmpty(email))
                return;
            //string emails = email.Replace(';', ',');
            //2.The Destination email Addresses
            MailAddressCollection TO_addressList = new MailAddressCollection();
            //3.Prepare the Destination email Addresses list
            foreach (var curr_address in email.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                //MailAddress mytoAddress = new MailAddress(curr_address, "Custom display name");
                TO_addressList.Add(curr_address);
            }

          //  string[] all = email.ToString().Split(';');
          //  for (int a = 0; a <= all.Count() - 1; a++)
            {
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                msg.Subject = subject;// "Daily Report for " + DateTime.Now;
                msg.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["FromEmailAddress"]);
               // msg.To.Add(new System.Net.Mail.MailAddress(emails));
                msg.To.Add(TO_addressList.ToString());
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.Body = body;
                msg.IsBodyHtml = true;
                msg.Priority = System.Net.Mail.MailPriority.Normal;
                System.Net.Mail.SmtpClient smpt = new System.Net.Mail.SmtpClient();
                smpt.UseDefaultCredentials = false;
                smpt.Host = ConfigurationManager.AppSettings["EmailHost"];//for google required smtp.gmail.com and must be check Google Account setting https://myaccount.google.com/lesssecureapps?pli=1 ON//"mail.digital53.com";
                smpt.Port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                smpt.EnableSsl = true;
                smpt.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailUserName"], ConfigurationManager.AppSettings["EmailPassword"]);
                smpt.Send(msg);
            }
        }
    }
}
