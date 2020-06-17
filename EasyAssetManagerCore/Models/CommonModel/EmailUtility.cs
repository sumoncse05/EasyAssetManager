using System.Net.Mail;
using System;
using System.Net.Mime;
using System.Net;

public sealed class EmailUtility
{
    SmtpClient smtp;
    MailMessage mail;

    public EmailUtility()
    {
        smtp = new SmtpClient("192.168.5.116");
        //smtp.Credentials = new NetworkCredential("islammdsyf@ebl-bd.com", "Ebl12345");
        mail = new MailMessage();
    }
    public bool SendMail(string from_email, string to_email, string subject, string body)
    {
        try
        {
            mail.From = new MailAddress(from_email);
            mail.To.Add(to_email);
            mail.Subject = subject;
            ContentType mimeType = new System.Net.Mime.ContentType("text/html");
            AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
            mail.AlternateViews.Add(alternate);

            smtp.Send(mail);
            return true;
        }
        catch (Exception ex)
        {
            //log err
            throw ex;
            //return false;
        }
    }
    public string[] SendMail(string user_ref_no, string from_email, string to_email,string cc_email, string attachment, string subject, string body)
    {
        string[] statusMsg = new string[] {"40999","","" };
        try
        {
            mail.From = new MailAddress(from_email);
            mail.To.Add(to_email);
            mail.Subject = subject;
            ContentType mimeType = new System.Net.Mime.ContentType("text/html");
            AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
            mail.AlternateViews.Add(alternate);

            smtp.Send(mail);
            
        }
        catch (Exception ex)
        {
            //log err
            statusMsg= new string[] { "40999", "", ex.Message.Replace("'", "") };            
        }
        return statusMsg;
    }
}
