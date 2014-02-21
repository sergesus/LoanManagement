﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.Entity;
using LoanManagement.Domain;

namespace LoanManagement.Website
{
    public partial class ApplicationSuccess : System.Web.UI.Page
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
                if (Session["ID"] == null)
                {
                    Response.Redirect("/Index.aspx");
                }
                Session["UpdateChecker"] = null;
                Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Session["Service"] = null;
                if (Session["tempLoan"] != null)
                {
                    int lID = Convert.ToInt32(Session["tempLoan"]);
                    using (var ctx = new finalContext())
                    {
                        var lon = ctx.TemporaryLoanApplications.Find(lID);
                        lblContent.Text = "Your loan application has been successfully applied. Please visit our branch ON or BEFORE " + lon.ExpirationDate.ToString().Split(' ')[0] + " to submit the requirements, provide other information and to confirm of the application. Thankyou";
                        Session["tempLoan"] = null;
                        Session["ref"] = "false";
                    }
                }
                else
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