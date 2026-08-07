using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public partial class ucServiceDetailsOld : UserControl
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
                                SPItem itemX = collitem.GetItemById(uid);
                                if (itemX["ARServiceName"] != null)
                                {
                                    lblname.Text = itemX["ARServiceName"].ToString();
                                }
                                if (itemX["ARServiceName"] != null)
                                {
                                    LabelBC.Text = itemX["ARServiceName"].ToString();
                                }
                                if (itemX["ARServiceName"] != null)
                                {
                                    LblSerTitle.Text = itemX["ARServiceName"].ToString();
                                }
                                if (itemX["Desc"] != null)
                                {
                                    lblDesc.Text = itemX["Desc"].ToString();
                                }
                                if (itemX["DP_TargetGroup"] != null)
                                {
                                    lbltarget.Text = itemX["DP_TargetGroup"].ToString();
                                }
                                if (itemX["ResponsibleParty"] != null)
                                {
                                    lblResPar.Text = itemX["ResponsibleParty"].ToString();
                                }
                                if (itemX["TimeDuration"] != null)
                                {
                                    lblDur.Text = itemX["TimeDuration"].ToString();
                                }
                                if (itemX["Channels"] != null)
                                {
                                    lblChan.Text = itemX["Channels"].ToString();
                                }
                                if (itemX["UserManualURL"] != null)
                                {
                                    hrefUManual.HRef = itemX["UserManualURL"].ToString();
                                }
                                if (itemX["URL"] != null)
                                {
                                    hreStart.HRef = itemX["URL"].ToString();
                                }
                                if (itemX["ServiceAgreement"] != null)
                                {
                                    hreLevelAgr.HRef = itemX["ServiceAgreement"].ToString();
                                }
                                if (itemX["Req1"] == null)
                                {
                                    //  preq1.Disabled = true;
                                    preq1.Visible = false;
                                }
                                else
                                if (itemX["Req1"] != null)
                                {
                                    LblReq1.Text = itemX["Req1"].ToString();

                                }
                                if (itemX["Req2"] == null)
                                {
                                    //preq2.Disabled = true;
                                    preq2.Visible = false;
                                }
                                else
                                if (itemX["Req2"] != null)
                                {
                                    LblReq2.Text = itemX["Req2"].ToString();

                                }
                                if (itemX["Req3"] == null)
                                {
                                    //preq3.Disabled = true;
                                    preq3.Visible = false;
                                }
                                else
                                if (itemX["Req3"] != null)
                                {
                                    LblReq3.Text = itemX["Req3"].ToString();

                                }
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
