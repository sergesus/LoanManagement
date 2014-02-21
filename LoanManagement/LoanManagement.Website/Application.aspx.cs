using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.Entity;
using LoanManagement.Domain;
using System.IO;

namespace LoanManagement.Website
{
    public partial class Application : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblTime.Text = DateTime.Now.ToString("MMM dd, yyyy | hh:mm tt");
                using (var ctx = new finalContext())
                {
                    var set = ctx.OnlineSettings.Find(1);
                    lblVisitor.Text = set.Visitor.ToString();
                }
                Session["iService"] = null;
                if (Session["ID"] != null)
                {
                    Session["ref"] = "true";
                    using (var ctx = new finalContext())
                    {
                        int n = Convert.ToInt32(Session["ID"]);
                        var c = ctx.Loans.Where(x => x.ClientID == n && (x.Status == "Applied" || x.Status == "Under Collection" || x.Status == "Closed Account" || x.Status == "Released")).Count();
                        if (c > 0)
                        {
                            lblCheck.Text = "You still have an existing loan application OR a loan that is not yet finished. You cannot apply for another loan.";
                            lblCheck.Visible = true;
                            pnlForm.Visible = !true;
                            return;
                        }

                        pnlForm.Visible = true;
                        lnkBtn.Visible = false;
                        if (Session["Service"] == null)
                        {
                            checkTOL();
                            checkMode();
                            cmbTOL_SelectedIndexChanged(null, null);
                        }
                        c = ctx.TemporaryLoanApplications.Where(x => x.ClientID == n).Count();
                        if (c > 0)
                        {
                            lblCheck.Text = "You still have a pending online application that needs to be confirmed. You cannot apply for another loan, but you are able to update it.";
                            lblCheck.Visible = true;
                            pnlForm.Visible = true;
                            if (Session["UpdateChecker"] == null)
                            {
                                var lon = ctx.TemporaryLoanApplications.Where(x => x.ClientID == n).First();
                                cmbMode.Text = lon.Mode;
                                cmbTOL.Text = lon.Service.Name;
                                txtAmt.Text = lon.AmountApplied.ToString("N2");
                                txtTerm.Text = lon.Term.ToString();
                                Session["UpdateChecker"] = "1";
                            }
                            //return;
                        }
                    }
                }
                else
                {
                    pnlForm.Visible = !true;
                    lnkBtn.Visible = !false;
                }
            }
            catch (Exception)
            {
                //Response.Redirect("/Index.aspx");
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

        private void checkTOL()
        {
            try
            {
                using (var ctx = new finalContext())
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
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
            }
        }

        private void checkMode()
        {
            try
            {
                using (var ctx = new finalContext())
                {
                    int n = Convert.ToInt32(Session["ID"]);
                    var c = ctx.TemporaryLoanApplications.Where(x => x.ClientID == n).Count();
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
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ID"] == null)
                {
                    Response.Redirect("/Index.aspx");
                }
                if (txtCaptcha.Text == "")
                {
                    lblCaptcha.Visible = true;
                    return;
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
                using (var ctx = new finalContext())
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
                    int n = Convert.ToInt32(Session["ID"]);
                    var c = ctx.TemporaryLoanApplications.Where(x => x.ClientID == n).Count();
                    if (c > 0)
                    {
                        using (var ictx = new finalContext())
                        {
                            int num = Convert.ToInt32(Session["ID"]);
                            var ln = ictx.TemporaryLoanApplications.Where(x => x.ClientID == num).First();
                            ln.Mode = cmbMode.Text;
                            ln.AmountApplied = Convert.ToDouble(txtAmt.Text);
                            ln.Term = Convert.ToInt32(txtTerm.Text);
                            ln.ServiceID = ser.ServiceID;
                            ictx.Entry(ln).State = System.Data.EntityState.Modified;
                            ictx.SaveChanges();

                            Session["tempLoan"] = ln.TemporaryLoanApplicationID.ToString();
                            Response.Redirect("/ApplicationSuccess.aspx");
                            //Response.Redirect("/MyAccount_Loans.aspx");
                            return;
                        }
                    }

                    TemporaryLoanApplication lon = new TemporaryLoanApplication { AmountApplied = Convert.ToDouble(txtAmt.Text), ClientID = Convert.ToInt32(Session["ID"]), DateApplied = DateTime.Now.Date, ExpirationDate = DateTime.Now.Date.AddMonths(1), Mode = cmbMode.Text, ServiceID = Convert.ToInt32(Session["Service"]), Term = Convert.ToInt32(txtTerm.Text) };
                    ctx.TemporaryLoanApplications.Add(lon);
                    ctx.SaveChanges();
                    Session["tempLoan"] = lon.TemporaryLoanApplicationID.ToString();
                    string folderName = @"F:\Loan Files\Applications Online";
                    string pathString = System.IO.Path.Combine(folderName, "Application " + Session["tempLoan"]);
                    if (!Directory.Exists(pathString))
                    {
                        System.IO.Directory.CreateDirectory(pathString);
                    }
                    else
                    {
                        Directory.Delete(pathString, true);
                        System.IO.Directory.CreateDirectory(pathString);
                    }
                    Response.Redirect("/ApplicationSuccess.aspx");

                }
            }
            catch (Exception)
            {
                //Response.Redirect("/Index.aspx");
            }
        }

        protected void cmbTOL_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                checkMode();
            }
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Session["Service"] = null;
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
            }
        }
    }
}