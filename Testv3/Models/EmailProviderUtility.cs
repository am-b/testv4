using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Configuration;

namespace Testv3.Models
{
    public class EmailProviderUtility: SmtpClient
    {
        public string UserName { get; set; }
        //All other attributes are inherited from the SmtpClient class.

        public EmailProviderUtility() :
            base(ConfigurationManager.AppSettings["Host"],Int32.Parse(ConfigurationManager.AppSettings["Port"]))
        {
            EnableSsl = true;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
            Credentials = credentials;

        }

        public void SendNotificationEmail(List<String> StudentEmails)
        {
            //Use a dummy email as the email receiver.
            MailMessage email = new MailMessage(new MailAddress("teambbl.mmccis@gmail.com", "(do not reply)"),
            new MailAddress("lfname404@gmail.com"));

            //MailMessage email = new MailMessage(new MailAddress("no-reply@mmccguidance.co", "(do not reply)"),
            //new MailAddress("teambbl.mmccis@gmail.com"));



            //Add each student in BCC list
            foreach (var Email in StudentEmails)
            {
                MailAddress bcc = new MailAddress(Email);
                email.Bcc.Add(bcc);
            }

            email.Subject = "Your Account has been created.";
            email.Body = "Your MMCC account has been created. Your default password is [Your last name & your student number (Lastname&123456) ]<br />Please change your password after your first login. ";
            email.IsBodyHtml = true;

            using (var mailClient = new EmailProviderUtility())
            {
                mailClient.Send(email);
            }
            //



        }

    }
}