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
    public partial class Confirmation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string tnum = Request.QueryString["id"];
                using (var ctx = new finalContext())
                {
                    var clt = ctx.Clients.Where(x => x.TrackingNumber == tnum).First();
                    if (clt.isRegistered == true)
                    {
                        Response.Redirect("/Index.aspx");
                    }
                    else
                    {
                        clt.isRegistered = true;
                        var exp = ctx.iClientExpirations.Find(clt.ClientID);
                        lblContent.Text = "Your currently registered account will be deleted if not confirmed on or before " + exp.ExpirationDate + "\n Please visit our office to confirm this account regarding the information. Thank You.";
                        Session["newID"] = null;
                        ctx.SaveChanges();
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
    }
}