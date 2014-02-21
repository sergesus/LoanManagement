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
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblTime.Text = DateTime.Now.ToString("MMM dd yyyy, | hh:mm tt");
                Session["Service"] = null;
                Session["UpdateChecker"] = null;
                Session["iService"] = null;

                using (var ctx = new finalContext())
                {
                    var set = ctx.OnlineSettings.Find(1);
                    lblVisitor.Text = set.Visitor.ToString();
                }

                using(var ctx = new finalContext())
                {
                    var set = ctx.OnlineSettings.Find(1);
                    lblAbout.Text = set.AboutDescription.Replace("\n", "<br />"); ;
                    lblMission.Text = set.MissionVision.Replace("\n", "<br />"); ;
                    lblContact.Text = set.ContactInfo.Replace("\n", "<br />"); ;
                }
            }
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
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

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("MMM dd yyyy, | hh:mm tt");
        }
    }
}