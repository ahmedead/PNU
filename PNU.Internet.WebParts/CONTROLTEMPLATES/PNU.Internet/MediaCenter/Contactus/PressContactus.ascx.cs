using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Contactus
{
    public partial class PressContactus : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("/admin"))
                            {
                                SPList requestsList = web.Lists["MediaContactus"];

                                SPListItem listItem = requestsList.Items.Add();

                                listItem["Title"] = TextBoxName.Text;
                                listItem["MediaEntity"] = TextBoxSide.Text;
                                listItem["MediaEmail"] = TextBoxEmail.Text;
                                listItem["MediaMessage"] = TextBoxDesc.Text;
                                listItem["MediaPhone"] = TextBoxMobile.Text;


                                web.AllowUnsafeUpdates = true;
                                listItem.Update();
                                web.AllowUnsafeUpdates = false;

                            }
                        }
                    }
                });

                //send email to admin user .

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        
    }
}
