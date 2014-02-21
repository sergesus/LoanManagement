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
    public partial class Login : System.Web.UI.Page
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
                Session["iService"] = null;
                Session["UpdateChecker"] = null;
                if (Session["ID"] != null)
                {
                    Response.Redirect("/Index.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("MMM dd yyyy, | hh:mm tt");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ctx = new finalContext())
                {
                    var ctr1 = ctx.Clients.Where(x => x.Username == txtUsername.Text && x.Password == txtPassword.Text && x.isRegistered == true).Count();
                    if (ctr1 > 0)
                    {
                        var clt = ctx.Clients.Where(x => x.Username == txtUsername.Text && x.Password == txtPassword.Text && x.isRegistered == true).First();
                        Session["ID"] = clt.ClientID;
                        Session["NAME"] = clt.LastName + ", " + clt.FirstName;
                        Response.Redirect("/Index.aspx");
                    }
                    else
                    {
                        lclCheck.Visible = true;
                    }
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

        protected void LinkButton1_Click1(object sender, EventArgs e)
        {
            //Response.Redirect("/RegisterClient.aspx");
        }
    }
}