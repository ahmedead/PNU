using Microsoft.Office.Server.Auditing;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Departments;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity
{
    public partial class ucUniversityPresidents : UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               LoadData();
            }
        }

        private void LoadData()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        var initializer = new SharePointListInitializer();
                        initializer.EnsureLists(web);

                        var repo = new PresidentRepository();

                        var profile = repo.GetProfile(web);
                        var speech = repo.GetSpeechItems(web);
                        var contacts = repo.GetContacts(web);
                        var memberships = repo.GetMemberships(web);
                        var awards = repo.GetAwards(web);
                        var experiences = repo.GetExperiences(web);


                        //litPresidentName.Text = profile.PresidentNameAr;
                        //litPresidentTitle.Text = profile.PresidentTitleAr;
                        //litUniversityName.Text = profile.UniversityNameAr;
                        litBiographyTitle.Text = profile[0].BiographyTitleAr;
                        litBiographyText.Text = profile[0].BiographyTextAr;

                        //imgPresident.ImageUrl = profile.ImageUrl;
                        //lnkExternal.NavigateUrl = profile.ExternalLink;

                        if(profile != null && profile.Count != 0)
                        {
                            rptSpeech.DataSource = profile;
                            rptSpeech.DataBind();

                            rptProfile.DataSource = profile;
                            rptProfile.DataBind();
                        }
                        
                        
                        
                        if (contacts != null && contacts.Count != 0)
                        {
                            rptContacts.DataSource = contacts;
                            rptContacts.DataBind();
                        }
                        
                        if (memberships != null && memberships.Count != 0)
                        {
                            rptMemberships.DataSource = memberships;
                            rptMemberships.DataBind();
                        }
                        
                        if (awards != null && awards.Count != 0)
                        {
                            rptAwards.DataSource = awards;
                            rptAwards.DataBind();
                        }
                        
                        if (experiences != null && experiences.Count != 0)
                        {
                            rptExperiences.DataSource = experiences;
                            rptExperiences.DataBind();
                        }
                        
                    }

                }
            });


            
        }

        protected void rptContacts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var item = (ContactItem)e.Item.DataItem;
            var lnk = (HyperLink)e.Item.FindControl("lnkContact");

            lnk.Text = item.ContactValue;

            switch ((item.ContactType ?? "").ToLower())
            {
                case "email":
                    lnk.NavigateUrl = "mailto:" + item.ContactValue;
                    lnk.Attributes["dir"] = "ltr";
                    break;

                case "phone":
                    lnk.NavigateUrl = "tel:" + item.ContactValue;
                    lnk.Attributes["dir"] = "ltr";
                    break;

                case "link":
                    lnk.NavigateUrl = item.ContactValue;
                    lnk.Target = "_blank";
                    lnk.Attributes["dir"] = "ltr";
                    break;

                default:
                    lnk.NavigateUrl = "#";
                    break;
            }
        }


    }




}
