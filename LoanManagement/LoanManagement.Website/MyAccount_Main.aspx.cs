using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.Entity;
using LoanManagement.Domain;
namespace LoanManagement.Website
{
    public partial class MyAccount_Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (var ctx = new finalContext())
                {
                    var set = ctx.OnlineSettings.Find(1);
                    lblVisitor.Text = set.Visitor.ToString();
                }
                lblTime.Text = DateTime.Now.ToString("MMM dd, yyyy | hh:mm tt");
                Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Session["Service"] = null;
                Session["UpdateChecker"] = null;
                Session["iService"] = null;
                if (Session["ID"] == null)
                {
                    Response.Redirect("/Login.aspx");
                }
                using (var ctx = new finalContext())
                {
                    int cID = Convert.ToInt32(Session["ID"]);
                    var clt = ctx.Clients.Find(cID);
                    lblBirthday.Text = clt.Birthday.ToString().Split(' ')[0];
                    var con = ctx.ClientContacts.Where(x => x.ClientID == cID).First();
                    lblContact.Text = con.Contact.ToString();
                    lblEmail.Text = clt.Email;
                    lblGender.Text = clt.Sex;
                    lblName.Text = clt.LastName + ", " + clt.FirstName + " " + clt.MiddleName + " " + clt.Suffix;
                    lblSSS.Text = clt.SSS;
                    lblStatus.Text = clt.Status;
                    lblTIN.Text = clt.TIN;

                    if (clt.isConfirmed == true)
                        lblType.Text = "Verified";
                    else
                        lblType.Text = "Pending";
                }
            }
            catch (Exception)
            {
                if (Session["ID"] == null)
                    Response.Redirect("/Login.aspx");
                else
                    Response.Redirect("/Index.aspx");
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("MMM dd yyyy, | hh:mm tt");
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