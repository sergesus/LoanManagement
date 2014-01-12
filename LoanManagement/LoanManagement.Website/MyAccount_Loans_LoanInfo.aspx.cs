using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LoanManagement.Domain;
using System.Data.Entity;

namespace LoanManagement.Website
{
    public partial class MyAccount_Loans_LoanInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Session["Service"] = null;
            Session["iService"] = null;
            Session["UpdateChecker"] = null;
            if (Session["ID"] == null)
            {
                Response.Redirect("/Login.aspx");
            }

            try
            {
                int lID = Convert.ToInt32(Request.QueryString["LoanID"]);
                using (var ctx = new newerContext())
                {
                    var c = ctx.Loans.Where(x => x.LoanID == lID).Count();
                    if (c > 0)
                    {
                        var lon = ctx.Loans.Find(lID);
                        lblID.Text = lon.LoanID.ToString();
                        lblTOL.Text = lon.Service.Name;
                        lblType.Text = lon.Service.Type.ToString();
                        lblTerm.Text = lon.Term.ToString();
                        lblAmtApplied.Text = lon.LoanApplication.AmountApplied.ToString("N2");
                        lblStatus.Text = lon.Status;
                        if (lon.Status == "Declined" || lon.Status == "Applied")
                        {
                            return;
                        }
                        if (lon.Status == "Approved")
                        {
                            string s = lon.ApprovedLoan.AmountApproved.ToString();
                            double n = Convert.ToDouble(s);
                            lblAmtApproved.Text = n.ToString("N2");
                            lblDtReleasing.Text = lon.ApprovedLoan.ReleaseDate.ToString().Split(' ')[0];
                            return;
                        }
                        pnlHidden.Visible = true;
                        lblAmtReleased.Text = lon.ReleasedLoan.Principal.ToString("N2");

                        if (lon.Status == "Under Collection")
                        {
                            double remain = lon.PassedToCollector.RemainingBalance;
                            lblRemaining.Text = remain.ToString("N2");
                            var pys = from py in ctx.CollectionInfoes
                                      where py.LoanID == lID
                                      select new { TotalCollection = py.TotalCollection, DateCollected = py.DateCollected };
                            dgHistory.Visible = true;
                            lblPH.Visible = true;
                            dgHistory.DataSource = pys.ToList();
                            dgHistory.DataBind();
                            return;
                        }

                        if (lon.Service.Department == "Financing")
                        {
                            var rmn = from rm in ctx.FPaymentInfo
                                      where rm.LoanID == lID && rm.PaymentStatus == "Cleared"
                                      select rm;
                            double r = 0;
                            foreach (var item in rmn)
                            {
                                r = r + item.Amount;
                            }
                            double remain = lon.ReleasedLoan.TotalLoan - r;
                            lblRemaining.Text = remain.ToString("N2");
                            var l = ctx.FPaymentInfo.Where(x => x.LoanID == lID && (x.PaymentStatus == "Pending" || x.PaymentStatus == "Due/Pending")).First();
                            lblPaymentAmt.Text = lon.ReleasedLoan.MonthlyPayment.ToString("N2");
                            lblNextPaymentDt.Text = l.PaymentDate.ToString().Split(' ')[0];
                            var pys = from py in ctx.FPaymentInfo
                                      where py.LoanID == lID
                                      select new { No = py.PaymentNumber, ChequeNumber = py.ChequeInfo, Amount = py.Amount, PaymentDate = py.PaymentDate, Status = py.PaymentStatus };
                            dgHistory.Visible = true;
                            lblPH.Visible = true;
                            dgHistory.DataSource = pys.ToList();
                            dgHistory.DataBind();
                        }
                        else
                        {
                            try
                            {
                                var pys = from py in ctx.MPaymentInfoes
                                          where py.LoanID == lID
                                          select new { No = py.PaymentNumber, Amount = py.Amount, Balance = py.ExcessBalance, TotalBalance = py.TotalBalance, TotalAmount = py.TotalAmount, TotalPayment = py.TotalPayment, PaymentDate = py.PaymentDate, Status = py.PaymentStatus };
                                dgHistory.Visible = true;
                                lblPH.Visible = true;
                                dgHistory.DataSource = pys.ToList();
                                dgHistory.DataBind();
                                var re1 = ctx.MPaymentInfoes.Where(x => x.LoanID == lID && x.PaymentStatus == "Pending").First();
                                double remain1 = re1.RemainingLoanBalance;
                                lblRemaining.Text = remain1.ToString("N2");
                                lblNextPaymentDt.Text = re1.DueDate.ToString().Split(' ')[0];
                                lblPaymentAmt.Text = re1.TotalAmount.ToString("N2");
                            }
                            catch (Exception)
                            {
                                lblRemaining.Text = "0.00";
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("MyAccount_Loans.aspx");
                    }
                }
            }
            catch (Exception)
            {
                if (Session["ID"] == null)
                    Response.Redirect("/Login.aspx");
                else
                    Response.Redirect("MyAccount_Loans.aspx");
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

        protected void dgHistory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}