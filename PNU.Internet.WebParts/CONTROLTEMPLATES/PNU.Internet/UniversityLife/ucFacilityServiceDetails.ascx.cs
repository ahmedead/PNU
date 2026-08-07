using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Fields;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers
{
    public partial class ucFacilityServiceDetails : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.Request.QueryString["ServiceId"] == null)
                    return;
                GetData();
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

            


        }
        private bool GetData()
        {
            bool retVal = false;
            try
            {
                
                if (Page.Request.QueryString["ServiceId"] == null)
                    return false;

                var RequestId = Convert.ToInt32(Page.Request.QueryString["ServiceId"]);

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPList requestsList = web.Lists["FacilitiesServices"];

                                SPListItem item = requestsList.GetItemById(RequestId);

                                if (item != null)
                                {
                                    string div = @"<div class='col-lg-6 order-lg-0 order-1  '>
                                        <div class='ps-lg-4 mt-5 '>
                                            <div class='d-flex align-items-start  mb-4 mt-5'>
                                                <h1
                                                    class='title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-5 me-md-4 flex-shrink-0'>
                                                    @Title
                                                    <span class='px-2 position-absolute mt-1 h2 text-primary'>•</span>
                                                </h1>
                                            </div>
                                            <p class='fs-5 mb-5 text-muted'>@Desc</p>
                                        </div>
                                    </div>
                                    <div class='col-12 col-lg-6 mt-0 pb-md-5 pb-2   mb-md-5 mb-2 text-end'>
                                        <div class=' position-relative ms-4 pattern-behind-img'>

                                            <img src='@Image' class='d-block w-100 rounded-4 object-fit-cover img-fluid ms-auto d-block mx-5' alt='...'>
                                        </div>
                                    </div>";

                                    div = div.Replace("@Title", item["Title"] == null ? "" : item["Title"].ToString());
                                    div = div.Replace("@Desc", item["Desc"] == null ? "" : item["Desc"].ToString());


                                    if (item["PublishingRollupImage"] != null)
                                    {
                                        ImageFieldValue PublishingRollupImage = (ImageFieldValue)item["PublishingRollupImage"];
                                        div = div.Replace("@Image", PublishingRollupImage.ImageUrl);
                                    }

                                    divData.InnerHtml = div;
                                    if (item["Facility"] != null)
                                    {
                                        SPFieldLookupValue SingleValue = new SPFieldLookupValue(item["Facility"].ToString());

                                        int FacilityId = SingleValue.LookupId;
                                        SPQuery query = new SPQuery();
                                        query.Query = string.Concat(
                                             @"<Where>
                                      <And>
                                         <Eq>
                                            <FieldRef Name='Facility' />
                                            <Value Type='Lookup'>" + FacilityId + @"</Value>
                                         </Eq>
                                         <Neq>
                                            <FieldRef Name='ID' />
                                            <Value Type='Counter'>" + RequestId + @"</Value>
                                         </Neq>
                                      </And>
                                   </Where>
                                <OrderBy><FieldRef Name='ItemOrder' Ascending='False' /></OrderBy>");


                                        SPListItemCollection items = requestsList.GetItems(query);

                                        if (items != null && items.Count > 0)
                                        {
                                            List<clsServices> _AllItems = SPFactory.MapListItemsToClass<clsServices>(items);
                                            if (_AllItems != null && _AllItems.Count > 0)
                                                _AllItems[0].ClassName = " active";
                                            rptServices.DataSource = _AllItems;
                                            rptServices.DataBind();
                                        }

                                    }



                                }




                                if (item != null)
                                {



                                }
                            }
                        }
                    }
                });

                return retVal;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
       
            return false;
        }
        
    }
}
