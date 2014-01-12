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
    public partial class RegisterUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string str = Request.QueryString["id"];
                if (str != null)
                {
                    txtTN.Text = str;
                    btnCheck_Click(null, null);
                }
            }
            catch (Exception)
            { return; }
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

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
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

                using (var ctx = new newerContext())
                {
                    var clt = ctx.Clients.Where(x => x.TrackingNumber == txtTN.Text).First();
                    var c = ctx.Clients.Where(x => x.Username == txtUsername.Text).Count();
                    if (c > 0)
                    {
                        lblCheck.Text = "Username has been already used";
                        lblCheck.Visible = true;
                        return;
                    }
                    else
                    {
                        if (txtPassword.Text != txtConfirm.Text)
                        {
                            lblCheck.Text = "Password didn't match";
                            lblCheck.Visible = true;
                            return;
                        }

                        if (txtPassword.Text.Length < 8)
                        {
                            lblCheck.Text = "Password length must be at least 8";
                            lblCheck.Visible = true;
                            return;
                        }

                        if (txtUsername.Text.Length < 8)
                        {
                            lblCheck.Text = "Username length must be at least 8";
                            lblCheck.Visible = true;
                            return;
                        }

                        if (txtUsername.Text == "" || txtPassword.Text == "")
                        {
                            lblCheck.Text = "Please input all required fields";
                            lblCheck.Visible = true;
                            return;
                        }
                    }
                    clt.Username = txtUsername.Text;
                    clt.Password = txtPassword.Text;
                    clt.isRegistered = true;
                    ctx.SaveChanges();
                    Response.Redirect("/RegistrationSuccess.aspx");
                }
            }
            catch (Exception)
            {
                //Response.Redirect("/Index.aspx");
            }
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ctx = new newerContext())
                {
                    var c = ctx.Clients.Where(x => x.TrackingNumber == txtTN.Text).Count();
                    if (c > 0)
                    {
                        c = ctx.Clients.Where(x => x.TrackingNumber == txtTN.Text && x.isRegistered == true).Count();
                        if (c > 0)
                        {
                            lblCheck.Text = "Tracking number has been already used";
                            lblCheck.Visible = true;
                        }
                        else
                        {
                            txtTN.Enabled = false;
                            btnCheck.Enabled = false;
                            txtCaptcha.Enabled = true;
                            txtConfirm.Enabled = true;
                            txtPassword.Enabled = true;
                            txtUsername.Enabled = true;
                            btnRegister.Enabled = true;
                        }
                    }
                    else
                    {
                        if (txtTN.Text != "")
                        {
                            lblCheck.Text = "Tracking number that you've entered is invalid";
                            lblCheck.Visible = true;
                        }
                        else
                        {
                            lblCheck.Text = "Please enter the tracking number that was sent to your email";
                            lblCheck.Visible = true;
                        }
                    }

                }
            }
            catch (Exception)
            {
                //Response.Redirect("/Index.aspx");
            }
        }

        
    }
}