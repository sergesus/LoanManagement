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
    public partial class MyAccount_Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Session["Service"] = null;
                Session["iService"] = null;
                if (Session["ID"] == null)
                {
                    Response.Redirect("/Login.aspx");
                }
                using (var ctx = new finalContext())
                {
                    int cID = Convert.ToInt32(Session["ID"]);
                    var clt = ctx.Clients.Find(cID);
                    lblUsername.Text = clt.Username;
                }
            }
            catch (Exception)
            {
                Response.Redirect("/Login.aspx");
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtConfirm.Text != txtNew.Text)
                {
                    lblCheck.Text = "Password didn't match";
                    lblCheck.Visible = true;
                    return;
                }

                if (txtNew.Text.Length < 8)
                {
                    lblCheck.Text = "Password length must be at least 8";
                    lblCheck.Visible = true;
                    return;
                }

                using (var ctx = new finalContext())
                {
                    int cID = Convert.ToInt32(Session["ID"]);
                    var clt = ctx.Clients.Find(cID);
                    if (clt.Password != txtCurrent.Text)
                    {
                        lblCheck.Text = "Current Password is wrong";
                        lblCheck.Visible = true;
                        return;
                    }

                    else
                    {
                        clt.Password = txtNew.Text;
                        ctx.SaveChanges();
                        lblCheck.Text = "Password has been successfully changed!";
                        lblCheck.Visible = true;
                    }
                }
            }
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
            }
        }
    }
}