using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net.Mail;
using System.Net;

using System.Data.Entity;
using LoanManagement.Domain;

namespace LoanManagement.Website
{
    public partial class RegistrationSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["newID"] != null)
            {
                int id = Convert.ToInt32((string)Session["newID"]);
                using (var ctx = new finalContext())
                {
                    var clt = ctx.Clients.Find(id);
                    string email = clt.Email;
                    string link = "http://localhost:7234/Confirmation.aspx?id=" + clt.TrackingNumber;
                    var exp = ctx.iClientExpirations.Find(id);
                    lblContent.Text = "A Confirmation Email has been sent to " + email + "\n Your currently registered account will be deleted if not confirmed on or before " + exp.ExpirationDate;
                    string message = "You have to click the following link in order to activate your account: " + link;

                    MailMessage msg = new MailMessage();
                    msg.To.Add(email);
                    msg.From = new MailAddress("aldrinarciga@gmail.com"); //See the note afterwards...
                    msg.Body = message;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential("aldrinarciga@gmail.com", "312231212131");
                    smtp.Send(msg);
                    Session["newID"] = null;
                }

            }
            else
            {
                lblContent.Text = "Account has been successfully registered. You can now login to use our online features.";
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("/Index.aspx");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Login.aspx");
        }
    }
}