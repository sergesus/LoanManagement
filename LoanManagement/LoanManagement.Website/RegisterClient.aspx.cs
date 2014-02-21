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
    public partial class RegisterClient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var ctx = new finalContext())
            {
                var set = ctx.OnlineSettings.Find(1);
                lblVisitor.Text = set.Visitor.ToString();
            }
            lblTime.Text = DateTime.Now.ToString("MMM dd, yyyy | hh:mm tt");
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

        protected void btnRegister_Click1(object sender, EventArgs e)
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
                DateTime bDay = Convert.ToDateTime(txtBirthday.Text);

                using (var ctx = new finalContext())
                {
                    var ctr = ctx.Clients.Where(x => x.FirstName == txtFirstName.Text && x.LastName == txtLastName.Text && x.MiddleName == txtMiddleName.Text && x.Suffix == txtSuffix.Text && x.Birthday == bDay).Count();
                    if (ctr > 0)
                    {
                        lblExists.Visible = true;
                        return;
                    }
                    else
                    {

                        string age = "0";
                        int years = DateTime.Now.Year - bDay.Year;
                        if (bDay.AddYears(years) > DateTime.Now) ;
                        years--;

                        age = years.ToString();
                        int iAge = Convert.ToInt32(age);
                        if (iAge < 18 || iAge > 65)
                        {
                            lblExists.Text = "Age must be between 18 an 65";
                            lblExists.Visible = true;
                            return;
                        }
                        if (txtPassword.Text != txtConfirm.Text)
                        {
                            lblExists.Text = "Passwords didn't match";
                            lblExists.Visible = true;
                            return;
                        }

                        if (txtPassword.Text.Length < 8)
                        {
                            lblExists.Text = "Password length must be at least 8";
                            lblExists.Visible = true;
                            return;
                        }

                        if (txtUsername.Text.Length < 8)
                        {
                            lblExists.Text = "Username length must be at least 8";
                            lblExists.Visible = true;
                            return;
                        }

                        if(txtBirthday.Text == "" || txtEmail.Text == "" || txtFirstName.Text == "" || txtLastName.Text == "" || txtUsername.Text == "")
                        {
                            lblExists.Text = "Please input all required fields";
                            lblExists.Visible = true;
                            return;
                        }

                        var c = ctx.Clients.Where(x => x.Username == txtUsername.Text).Count();
                        if (c > 0)
                        {
                            lblExists.Text = "Username has been already used";
                            lblExists.Visible = true;
                            return;
                        }

                        c = ctx.Clients.Where(x => x.Email == txtEmail.Text).Count();
                        if (c > 0)
                        {
                            lblExists.Text = "Email has been already used";
                            lblExists.Visible = true;
                            return;
                        }
                        var result = "asd";
                        do
                        {
                            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                            var random = new Random();
                            result = new string(
                                Enumerable.Repeat(chars, 15)
                                          .Select(s => s[random.Next(s.Length)])
                                          .ToArray());
                            c = ctx.Clients.Where(x => x.TrackingNumber == result).Count();
                        } while (c > 0);
                        Client clt = new Client { Active = true, Birthday = bDay, Email = txtEmail.Text, FirstName = txtFirstName.Text, isConfirmed = false, LastName = txtLastName.Text, MiddleName = txtMiddleName.Text, Password = txtPassword.Text, Sex = cmbGender.Text, SSS = txtSSS.Text, Username = txtUsername.Text, Status = cmbStatus.Text, Suffix = txtSuffix.Text, TIN = txtTIN.Text, isRegistered = false, TrackingNumber = result };
                        ClientContact con = new ClientContact { ContactNumber = 1, Primary = true, Contact = txtContact.Text };
                        iClientExpiration exp = new iClientExpiration { ExpirationDate = DateTime.Now.AddMonths(1) };

                        ctx.Clients.Add(clt);
                        ctx.ClientContacts.Add(con);
                        ctx.iClientExpirations.Add(exp);
                        ctx.SaveChanges();
                        Session["newID"] = clt.ClientID.ToString();
                        Response.Redirect("/RegistrationSuccess.aspx");

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