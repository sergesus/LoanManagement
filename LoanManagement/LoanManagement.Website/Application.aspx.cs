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
    public partial class Application : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["iService"] = null;
            if (Session["ID"] != null)
            {
                Session["ref"] = "true";
                using (var ctx = new newerContext())
                {
                    int n = Convert.ToInt32(Session["ID"]);
                    var c = ctx.TemporaryLoanApplications.Where(x => x.ClientID == n).Count();
                    if (c > 0)
                    {
                        lblCheck.Text = "You still have a pending online application that needs to be confirmed. You cannot apply for another loan.";
                        lblCheck.Visible = true;
                        pnlForm.Visible = !true;
                        return;
                    }

                    c = ctx.Loans.Where(x => x.ClientID == n && (x.Status=="Applied" || x.Status=="Under Collection" || x.Status=="Closed Account" || x.Status=="Released")).Count();
                    if (c > 0)
                    {
                        lblCheck.Text = "You still have an existing loan application OR a loan that is not yet finished. You cannot apply for another loan.";
                        lblCheck.Visible = true;
                        pnlForm.Visible = !true;
                        return;
                    }
                }
                pnlForm.Visible = true;
                lnkBtn.Visible = false;
                if (Session["Service"] == null)
                {
                    checkTOL();
                    checkMode();
                    cmbTOL_SelectedIndexChanged(null, null);
                }
            }
            else
            {
                pnlForm.Visible = !true;
                lnkBtn.Visible = !false;
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

        private void checkTOL()
        {
            using (var ctx = new newerContext())
            {
                var tol = from t in ctx.Services
                          where t.Active == true
                          select t;

                cmbTOL.Items.Clear();

                foreach (var itm in tol)
                {
                    cmbTOL.Items.Add(itm.Name);
                }
            }
        }

        private void checkMode()
        {
            using (var ctx = new newerContext())
            {
                string value = cmbTOL.Text;
                var ser = ctx.Services.Where(x => x.Name == value).First();
                Session["Service"] = ser.ServiceID.ToString();

                cmbMode.Items.Clear();

                if (ser.Department == "Financing")
                {
                    cmbMode.Items.Add("Monthly");
                    cmbMode.Items.Add("Semi-Monthly");
                    cmbMode.Items.Add("One-Time Payment");
                }
                else if (ser.Department == "Micro Business")
                {
                    cmbMode.Items.Add("Semi-Monthly");
                    cmbMode.Items.Add("Weekly");
                    cmbMode.Items.Add("Daily");
                }

                double ded = ser.AgentCommission;
                var de = from d in ctx.Deductions
                         where d.ServiceID == ser.ServiceID
                         select d;

                foreach (var itm in de)
                {
                    ded = ded + itm.Percentage;
                }
                lblDed.Text = "Total Deduction: " + ded + "%";
                lblInt.Text = "Total Interest: " + ser.Interest + "%";
                lblAmt.Text = "(Min. of " + ser.MinValue.ToString("C") + " and Max. of " + ser.MaxValue.ToString("C") + ")";
                lblTerm.Text = "(Min. of " + ser.MinTerm + " month(s) and Max. of " + ser.MaxTerm + " month(s))";
             }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (Session["ref"] == "false")
            {
                Response.Redirect("/Application.aspx");
            }
            CaptchaControl1.ValidateCaptcha(txtCaptcha.Text);
            if (!CaptchaControl1.UserValidated)
            {
                lblCaptcha.Visible = true;
                return;
            }
            else
            {
                lblCaptcha.Visible = !true;
            }
            using(var ctx = new newerContext())
            {
                double val = Convert.ToDouble(txtAmt.Text);
                var ser = ctx.Services.Find(Convert.ToInt32(Session["Service"]));
                if (val > ser.MaxValue || val < ser.MinValue)
                {
                    lblError.Text = "Invalid Loan Amount";
                    lblError.Visible = true;
                    return;
                }
                val = Convert.ToDouble(txtTerm.Text);
                if (val > ser.MaxTerm || val < ser.MinTerm)
                {
                    lblError.Text = "Invalid Desired Term";
                    lblError.Visible = true;
                    return;
                }

                TemporaryLoanApplication lon = new TemporaryLoanApplication { AmountApplied = Convert.ToDouble(txtAmt.Text), ClientID = Convert.ToInt32(Session["ID"]), DateApplied = DateTime.Now.Date, ExpirationDate = DateTime.Now.Date.AddMonths(1), Mode = cmbMode.Text, ServiceID = Convert.ToInt32(Session["Service"]), Term = Convert.ToInt32(txtTerm.Text) };
                ctx.TemporaryLoanApplications.Add(lon);
                ctx.SaveChanges();
                Session["tempLoan"] = lon.TemporaryLoanApplicationID.ToString();
                Response.Redirect("/ApplicationSuccess.aspx");
            }
        }

        protected void cmbTOL_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkMode();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Session["Service"] = null;
            Response.Redirect(Request.RawUrl);
        }
    }
}