using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using Portal.Main.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm
{
    public partial class ucRegServices : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    //LoadSectionPrograms();
                    LoadCollegeServices();
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }





        }


        private void LoadCollegeServices()
        {
            try
            {
                

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {

                            SPList list = web.Lists["RegAdmServices"];
                            if (list != null)
                            {
                                //SPQuery query = new SPQuery();
                                //query.Query = $@"<Where>
                                //             <Eq>
                                //                <FieldRef Name='COLL_CODE' />
                                //                <Value Type='Text'>{Source}</Value>
                                //             </Eq>
                                //       </Where>";

                                //SPListItemCollection collitem = list.GetItems(query);

                                SPListItemCollection collitem = list.GetItems();

                                if (collitem != null && collitem.Count > 0)
                                {

                                    List<RegServices> _AllData = new List<RegServices>();
                                    _AllData = SPFactory.MapListItemsToClass<RegServices>(collitem);


                                    rptServices.DataSource = _AllData;
                                    rptServices.DataBind();

                                }
                                else
                                {
                                    pnlData.Visible = false;
                                }

                            }

                        }
                    }
                });




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }


    }


    public class RegServices
    {
        public string Title { get; set; }
        public string Title_EN { get; set; }

        private string _description;

        public string Description
        {
            get
            {

                if (_description == null)
                {
                    return string.Empty;
                }


                return _description.Length > 600 ? _description.Substring(0, 600) : _description;
            }
            set
            {

                _description = value;
            }
        }

        private string _description_EN;

        public string Description_EN
        {
            get
            {

                if (_description_EN == null)
                {
                    return string.Empty;
                }


                return _description_EN.Length > 600 ? _description_EN.Substring(0, 600) : _description_EN;
            }
            set
            {

                _description_EN = value;
            }
        }


        public string Url { get; set; }



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

        public string COLL_CODE { get; set; }






    }

}
