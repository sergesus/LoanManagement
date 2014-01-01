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
            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Session["Service"] = null;
            Session["UpdateChecker"] = null;
            Session["iService"] = null;
            if (Session["ID"] == null)
            {
                Response.Redirect("/Index.aspx");
            }
            using (var ctx = new newerContext())
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