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
    public partial class wfIndex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblTime.Text = DateTime.Now.ToString("MMM dd, yyyy | hh:mm tt");
                Session["Service"] = null;
                Session["UpdateChecker"] = null;
                Session["iService"] = null;

                if (Session["Visit"] == null)
                {
                    Session["Visit"] = "Visited";
                    using (var ctx = new finalContext())
                    {
                        var set = ctx.OnlineSettings.Find(1);
                        set.Visitor = set.Visitor + 1;
                        ctx.SaveChanges();
                    }

                }
                using (var ctx = new finalContext())
                {
                    var set = ctx.OnlineSettings.Find(1);
                    lblDesc.Text = set.HomeDescription.Replace("\n", "<br />"); ;
                    lblVisitor.Text = set.Visitor.ToString();
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