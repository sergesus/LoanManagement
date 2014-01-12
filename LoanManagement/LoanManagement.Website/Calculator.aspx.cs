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
    public partial class Calculator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session["UpdateChecker"] = null;
                Session["Service"] = null;
                if (Session["iService"] == null)
                {
                    checkTOL();
                    checkMode();
                    cmbTOL_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
            }
        }

        protected void cmbTOL_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkMode();
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
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
            }
        }

        private void checkMode()
        {
            try
            {
                using (var ctx = new newerContext())
                {
                    string value = cmbTOL.Text;
                    var ser = ctx.Services.Where(x => x.Name == value).First();
                    Session["iService"] = ser.ServiceID.ToString();

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

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ctx = new newerContext())
                {
                    double val = Convert.ToDouble(txtAmt.Text);
                    var ser = ctx.Services.Find(Convert.ToInt32(Session["iService"]));
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
                    double ded = ser.AgentCommission;
                    var dec = from de in ctx.Deductions
                              where de.ServiceID == ser.ServiceID
                              select de;
                    foreach (var item in dec)
                    {
                        ded = ded + item.Percentage;
                    }
                    double Deduction = ded / 100;
                    Deduction = (Convert.ToDouble(txtAmt.Text) * Deduction);
                    double NetProceed = (Convert.ToDouble(txtAmt.Text)) - Deduction;
                    double TotalInt = (ser.Interest / 100) * Convert.ToInt32(txtTerm.Text);
                    double WithInt = (Convert.ToDouble(txtAmt.Text)) + (Convert.ToDouble(txtAmt.Text) * TotalInt);

                    double Payment = 0;
                    string md = cmbMode.Text;
                    if (md == "Monthly")
                    {
                        Payment = WithInt / Convert.ToInt32(txtTerm.Text);
                    }
                    else if (md == "Semi-Monthly")
                    {
                        Payment = WithInt / (Convert.ToInt32(txtTerm.Text) * 2);
                    }
                    else if (md == "Weekly")
                    {
                        Payment = WithInt / (Convert.ToInt32(txtTerm.Text) * 4);
                    }
                    else if (md == "Daily")
                    {
                        Payment = WithInt / ((Convert.ToInt32(txtTerm.Text) * 4) * 7);
                    }
                    else if (md == "One-Time Payment")
                    {
                        NetProceed = NetProceed - ((Convert.ToDouble(txtAmt.Text) * TotalInt));
                        WithInt = Convert.ToDouble(txtAmt.Text);
                        Payment = Convert.ToDouble(txtAmt.Text);
                    }

                    txtAmmortization.Text = Payment.ToString("N2");
                    txtBalance.Text = WithInt.ToString("N2");
                    txtDeduction.Text = Deduction.ToString("N2");
                    txtNet.Text = NetProceed.ToString("N2");

                }
            }
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                Session["iService"] = null;
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
            }
        }
    }
}