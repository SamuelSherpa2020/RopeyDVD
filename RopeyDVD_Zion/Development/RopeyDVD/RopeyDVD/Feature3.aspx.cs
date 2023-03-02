/*
 In this class, all the necessary code for feature 3 will be covered.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RopeyDVD
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie ck = Request.Cookies["ACK"]; // Getting data from cookies
            if (ck == null)
            {
                Response.Redirect("Login.aspx"); //loads Login.aspx page
            }

            if (Session["username"] != null)
            {
                lblUserName.Text = Session["username"].ToString(); //Getting username from session.
            }
            // below, all the lastname of members are extracted from the database and added in a dropdown box.
            if (!Page.IsPostBack)
            {
                GlobalConnection gc = new GlobalConnection();
                SqlCommand cmd = new SqlCommand();

                string memberLastname = "Select * from Member";

                SqlDataAdapter dtd = new SqlDataAdapter(memberLastname, gc.cn);
                DataTable dcdt = new DataTable();
                dtd.Fill(dcdt);
                foreach (DataRow dr in dcdt.Rows)
                {
                    member_lastname_dropdownlist.Items.Add(dr["LastName"].ToString());
                }
                DisplayDetails();
            }
        }

        // in the below code: SQL query is inserted to view a list of all Dvd title and it's copynumber which have been
        // loaned in the last 31days.
       public void DisplayDetails()
        {
 GlobalConnection gc = new GlobalConnection();
            SqlCommand cmd = new SqlCommand();
            string actorData = "Select at.LastName, dt.DvDTitle, dc.CopyNumber from Member at join Loan ln on at.MemberNumber = ln.MemberNumber join DvdCopy dc on ln.CopyNumber = dc.CopyNumber join DvdTitle dt on dc.DvdNumber = dt.DvdNumber where ln.DateOut>=GETDATE()-31 ANd at.LastName='"+member_lastname_dropdownlist.SelectedItem.Text +"'";
            SqlDataAdapter dtd = new SqlDataAdapter(actorData, gc.cn);
            DataTable dcdt = new DataTable();

            dtd.Fill(dcdt);
            Displayfeature3_details.DataSource = dcdt;
            Displayfeature3_details.DataBind();
        }

        protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayDetails(); // calling method DisplayDetails so that when the lastname is selected from the dropdownlist, it changes the data in the table.
        }

        protected void Displayfeature3_details_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e) // logs out user from the system.
        {
            Session.Clear();
            HttpCookie lo = Request.Cookies["ACK"];
            lo.Expires = DateTime.Now.AddMilliseconds(-10);
            Response.Cookies.Add(lo);
            Response.Redirect("Login.aspx");
        }
    }
}