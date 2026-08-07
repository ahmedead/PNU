using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.util.collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using Portal.Main.Helper;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter
{
    public partial class ucResearchPaths : UserControl
    {

        public string ListName { get; set; }
        public int TabsCount = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!Page.IsPostBack)
                {

                    var ResearchTrack = 0;
                    div1.Visible = false;
                    div2.Visible = false;
                    div3.Visible = false;

                    if (Page.Request.QueryString["ResearchTrack"] != null)
                    {
                        ResearchTrack = Page.Request.QueryString["ResearchTrack"].ToInt();
                        BindTwoLevelResearchTracks(ResearchTrack);
                        div2.Visible = true;

                    }

                    if (Page.Request.QueryString["ChildResearchTrack"] != null)
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab(2);", true);
                        ResearchTrack = Convert.ToInt32(Page.Request.QueryString["ChildResearchTrack"]);
                        BindChildResearchTracks(ResearchTrack);
                        
                        div3.Visible = true;

                    }
                    if (Page.Request.QueryString["ResearchTrack"] == null && Page.Request.QueryString["ChildResearchTrack"] == null)
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTab", $"activeTab(2);", true);
                        BindResearchTracks();
                        div1.Visible = true;
                    }

                        
                    



                }


                

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }

        private void BindTwoLevelResearchTracks(int researchTrack)
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {

                    using (SPWeb web = site.OpenWeb())
                    {
                        

                        SPList list = web.Lists["ResearchTracks"];
                       

                        SPListItem _Item = list.Items.GetItemById(researchTrack);


                        AllResearchTracks _itemData = SPFactory.MapListItemsToClass<AllResearchTracks>(_Item);

                        if(_itemData != null)
                        {
                            ltr2.Text = _itemData.Title;
                        }
                        SPList _CollegesList = web.Lists["ResearchTracksLevelTwo"];
                        SPQuery query = new SPQuery();
                        query.Query = string.Concat(
                                         $@"<Where>
                                              <Eq>
                                                 <FieldRef Name='ResearchTracks' />
                                                 <Value Type='Lookup'>{_itemData.Title}</Value>
                                              </Eq>
                                           </Where>
                                        <OrderBy>
                                                  <FieldRef Name='ItemOrder' Ascending='True' />
                                               </OrderBy>");

                        SPListItemCollection _AllData = _CollegesList.GetItems(query);

                        List<AllResearchTracksLevelTwo> _Data = SPFactory.MapListItemsToClass<AllResearchTracksLevelTwo>(_AllData);


                        if (_Data != null)
                        {
                            rptCourses.DataSource = _Data;
                            rptCourses.DataBind();
                        }
                    }
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }

        private void BindChildResearchTracks(int researchTrack)
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["ResearchTracksLevelTwo"];


                        SPListItem _Item = list.Items.GetItemById(researchTrack);


                        AllResearchTracks _itemData = SPFactory.MapListItemsToClass<AllResearchTracks>(_Item);

                        if (_itemData != null)
                        {
                            ltr3.Text = _itemData.Title;
                        }

                        SPList _CollegesList = web.Lists["ResearchTracksLevelThree"];
                        SPQuery query = new SPQuery();
                        query.Query = string.Concat(
                                         $@"<Where>
                                              <Eq>
                                                 <FieldRef Name='ResearchTracksLevelTwo' />
                                                 <Value Type='Lookup'>{_itemData.Title}</Value>
                                              </Eq>
                                           </Where>
                                        <OrderBy>
                                                  <FieldRef Name='ItemOrder' Ascending='True' />
                                               </OrderBy>");

                        SPListItemCollection _AllData = _CollegesList.GetItems(query);

                        List<AllResearchTracks> _Data = SPFactory.MapListItemsToClass<AllResearchTracks>(_AllData);


                        if (_Data != null)
                        {
                            rptCourses.DataSource = _Data;
                            rptCourses.DataBind();
                        }
                    }
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void BindResearchTracks()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {


                        SPList _CollegesList = web.Lists["ResearchTracks"];
                        SPQuery query = new SPQuery();
                        query.Query = string.Concat(
                                         @"<OrderBy>
                                                  <FieldRef Name='ItemOrder' Ascending='True' />
                                               </OrderBy>");

                        SPListItemCollection _AllData = _CollegesList.GetItems(query);

                        List<AllResearchTracks> _Data = SPFactory.MapListItemsToClass<AllResearchTracks>(_AllData);


                        if (_Data != null)
                        {
                            rptCourses.DataSource = _Data;
                            rptCourses.DataBind();
                        }
                    }
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }

        
        

    }


    public class AllResearchTracks
    {
        public string Title { get; set; }


        public string DescriptionDisplay
        {
            get
            {

                if (_description == null)
                {
                    return string.Empty;
                }


                return _description.Length > 600 ? _description.Substring(0, 300) : _description;
            }

        }
        
        private string _description;

        public string Description
        {
            get
            {

                if (_description == null)
                {
                    return string.Empty;
                }


                return _description;
            }
            set
            {

                _description = value;
            }
        }


        
        private string _ImageUrl;

        public string ImageUrl
        {
            get
            {

                if (_ImageUrl == null)
                {
                    return PortalHelper.IsArabic ? "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" : "/Style Library/NewStyle/UI5/images/pnu-logo-en.svg";
                }
                return _ImageUrl;
            }
            set
            {
                _ImageUrl = value;
            }
        }

        private string _LinkUrl;

        public string LinkUrl
        {
            get
            {

                if (_LinkUrl == null)
                {
                    return "default.aspx?ResearchTrack=" + ID;
                }
                return _LinkUrl;
            }
            set
            {
                _LinkUrl = value;
            }
        }

        public string ItemOrder { get; set; }
        

        

        private string _PublishingRollupImage;

        public string PublishingRollupImage
        {
            get
            {

                if (_PublishingRollupImage == null)
                {
                    return PortalHelper.IsArabic ? "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" : "/Style Library/NewStyle/UI5/images/pnu-logo-en.svg";
                }
                return _PublishingRollupImage;
            }
            set
            {
                _PublishingRollupImage = value;
            }
        }

        public string ID { get; set; }

        

        public string DisplayImage
        {
            get
            {
                if (PublishingRollupImage != null && PublishingRollupImage != "")
                    return PublishingRollupImage;
                else
                {

                    return PortalHelper.IsArabic ? "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" : "/Style Library/NewStyle/UI5/images/pnu-logo-en.svg";
                }

            }


        }







    }

    public class AllResearchTracksLevelTwo
    {
        public string Title { get; set; }


        public string DescriptionDisplay
        {
            get
            {

                if (_description == null)
                {
                    return string.Empty;
                }


                return _description.Length > 600 ? _description.Substring(0, 300) : _description;
            }

        }

        private string _description;

        public string Description
        {
            get
            {

                if (_description == null)
                {
                    return string.Empty;
                }


                return _description;
            }
            set
            {

                _description = value;
            }
        }



        private string _ImageUrl;

        public string ImageUrl
        {
            get
            {

                if (_ImageUrl == null)
                {
                    return PortalHelper.IsArabic ? "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" : "/Style Library/NewStyle/UI5/images/pnu-logo-en.svg";
                }
                return _ImageUrl;
            }
            set
            {
                _ImageUrl = value;
            }
        }

        private string _LinkUrl;

        public string LinkUrl
        {
            get
            {

                if (_LinkUrl == null)
                {
                    return "default.aspx?ChildResearchTrack=" + ID;
                }
                return _LinkUrl;
            }
            set
            {
                _LinkUrl = value;
            }
        }

        public string ItemOrder { get; set; }




        private string _PublishingRollupImage;

        public string PublishingRollupImage
        {
            get
            {

                if (_PublishingRollupImage == null)
                {
                    return PortalHelper.IsArabic ? "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" : "/Style Library/NewStyle/UI5/images/pnu-logo-en.svg";
                }
                return _PublishingRollupImage;
            }
            set
            {
                _PublishingRollupImage = value;
            }
        }

        public string ID { get; set; }



        public string DisplayImage
        {
            get
            {
                if (PublishingRollupImage != null && PublishingRollupImage != "")
                    return PublishingRollupImage;
                else
                {

                    return PortalHelper.IsArabic ? "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" : "/Style Library/NewStyle/UI5/images/pnu-logo-en.svg";
                }

            }


        }







    }

}
