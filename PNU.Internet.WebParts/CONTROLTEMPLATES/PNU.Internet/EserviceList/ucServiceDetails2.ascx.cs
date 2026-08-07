using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public partial class ucServiceDetails2 : UserControl
    {
        private int uid;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string testParameter = Request.QueryString["eti"];
                uid = Convert.ToInt32(testParameter);
                //   lblquerys.Text = testParameter;
                Bindservices();

                if (hrefUManual.HRef == "")
                {
                    hrefUManual.Attributes.Add("class", "btn btn-md btn-block py-2 main-btnD");
                    UserManP.Visible = false;

                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

        }
        private void Bindservices()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["EservicesList"];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            //query.Query = "<Where><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq></Where>";
                            query.Query = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + uid + "</Value></Eq></Where>";

                            

                            SPListItemCollection collitem = list.GetItems(query);
                            if (collitem != null)
                            {
                                

                                SPListItem _Data = collitem.GetItemById(uid);
                                EServicesList itemX = SPFactory.MapListItemsToClass<EServicesList>(_Data);
                                lblname.Text = SPFactory.GetLocalizedTitle(itemX.ARServiceName.ToString(), itemX.ENServiceName.ToString());
                                LabelBC.Text = SPFactory.GetLocalizedTitle(itemX.ARServiceName.ToString(), itemX.ENServiceName.ToString());

                                LblSerTitle.Text = SPFactory.GetLocalizedTitle(itemX.ARServiceName.ToString(), itemX.ENServiceName.ToString());
                                lblDesc.Text = SPFactory.GetLocalizedTitle(itemX.Desc.ToString(), itemX.Desc_EN.ToString());
                                lbltarget.Text = SPFactory.GetLocalizedTitle(itemX.DP_TargetGroup.ToString(), itemX.DP_TargetGroup_EN.ToString());
                                lblResPar.Text = SPFactory.GetLocalizedTitle(itemX.ResponsibleParty.ToString(), itemX.ResponsibleParty_EN.ToString());
                                lblDur.Text = SPFactory.GetLocalizedTitle(itemX.TimeDuration.ToString(), itemX.TimeDuration_EN.ToString());
                                lblChan.Text = SPFactory.GetLocalizedTitle(itemX.Channels.ToString(), itemX.Channels_EN.ToString());
                                hrefUManual.HRef = itemX.UserManualURL.ToString();
                                hreStart.HRef = itemX.URL.ToString();
                                hreLevelAgr.HRef = itemX.ServiceAgreement.ToString();


                                
                                
                                if (itemX._x0052_eq1.ToString() == "" && itemX.Req1_EN.ToString() == "")
                                {
                                    preq1.Visible = false;
                                }
                                else
                                    LblReq1.Text = SPFactory.GetLocalizedTitle(itemX._x0052_eq1.ToString(), itemX.Req1_EN.ToString());
                                if (itemX._x0052_eq2.ToString() == "" && itemX.Req2_EN.ToString() == "")
                                {
                                    preq2.Visible = false;
                                }
                                else
                                    LblReq2.Text = SPFactory.GetLocalizedTitle(itemX._x0052_eq2.ToString(), itemX.Req2_EN.ToString()); 
                                if (itemX._x0052_eq3.ToString() == "" && itemX.Req3_EN.ToString() == "")
                                {
                                    //preq3.Disabled = true;
                                    preq3.Visible = false;
                                }
                                else
                                    LblReq3.Text = SPFactory.GetLocalizedTitle(itemX._x0052_eq3.ToString(), itemX.Req3_EN.ToString());
                            }
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

    }
}
