using Microsoft.SharePoint;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.Programs
{
    public partial class ucObjectivesOutcomes : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                    BindDataU();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

        }
        public void BindDataU()
        {
            try
            {
                if (Request.QueryString["ProgramCode"] == null)
                    return;
                string ProgramCode = Request.QueryString["ProgramCode"].ToString();

                List<AllObjectivesOutcomes> _AllData = new List<AllObjectivesOutcomes>();
                string ProgramObjectivesListsDefinations = "ProgramObjectivesListsDefinations";
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("admin"))
                    {
                        SPList lstProgramObjectivesListsDefinations = web.Lists.TryGetList(ProgramObjectivesListsDefinations);

                        SPQuery query = new SPQuery();
                        query.Query = $@"<Where>
                                          <Eq>
                                             <FieldRef Name='Visibility' />
                                             <Value Type='Boolean'>1</Value>
                                          </Eq>
                                       </Where>
                                       <OrderBy>
                                          <FieldRef Name='ItemOrder' Ascending='True' />
                                       </OrderBy>";

                        SPListItemCollection items = lstProgramObjectivesListsDefinations.GetItems(query);

                        if (items == null || items.Count == 0)
                        {
                            return;
                        }
                        _AllData = SPFactory.MapListItemsToClass<AllObjectivesOutcomes>(items);
                        if (_AllData == null || _AllData.Count == 0)
                        {
                            rptMaster.DataSource = null;
                            rptMaster.DataBind();
                            return;
                        }
                        for (int i = 0; i < _AllData.Count; i++)
                        {
                            SPList lstObjective = web.Lists.TryGetList(_AllData[i].ListName);

                            query = new SPQuery();
                            query.Query = $@"<Where>
      <And>
         <Eq>
            <FieldRef Name='Visibility' />
            <Value Type='Boolean'>1</Value>
         </Eq>
         <Eq>
            <FieldRef Name='ProgramCode' />
            <Value Type='Text'>{ProgramCode}</Value>
         </Eq>
      </And>
   </Where>
   <OrderBy>
      <FieldRef Name='ItemOrder' Ascending='True' />
   </OrderBy>";

                            SPListItemCollection Objitems = lstObjective.GetItems(query);

                            if (Objitems != null && Objitems.Count > 0)
                            {
                                List<ObjectivesOutcomes> objectivesOutcomes = new List<ObjectivesOutcomes>();
                                objectivesOutcomes = SPFactory.MapListItemsToClass<ObjectivesOutcomes>(Objitems);
                                _AllData[i].ObjectivesOutcomes = objectivesOutcomes;
                            }
                        }


                        rptMaster.DataSource = _AllData;
                        rptMaster.DataBind();

                    }
                }






                
                
                




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

        }

    }


    public class AllObjectivesOutcomes
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string ListName { get; set; }

        public List<ObjectivesOutcomes> ObjectivesOutcomes { get; set; }
        
    }

    public class ObjectivesOutcomes
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string ItemOrder { get; set; }
        public string Visibility { get; set; }
        public string ProgramCode { get; set; }
        public string Numbering { get; set; }
        
    }

}
