using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public partial class ucEserviceBrowOld : UserControl
    {
        private int ss;
        //  string[] arrPictureBox = new string[2];

        private int stuN;
        private int facN;
        private int visN;
        private int TotalN;
        private string toplink1;
        private string toplink2;
        private string toplink3;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindservicesFav();
                    Bindservices();
                    BindservicesFaculty();
                    BindservicesVis();
                    TotalN = stuN + facN + visN;
                    lblTotal.Text = TotalN.ToString();

                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

        }
        private void BindservicesFav()
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
                            query.Query = @"<Where><And><Eq><FieldRef Name='Active' />
                            <Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='Fav' />
                            <Value Type='Boolean'>1</Value></Eq></And></Where>";

                            query.RowLimit = 3;

                            SPListItemCollection collitem = list.GetItems(query);
                            if (collitem != null)
                            {

                                rptprod.DataSource = collitem.GetDataTable();
                                rptprod.DataBind();
                                stuN = collitem.Count;

                                lblStNser.Text = stuN.ToString();


                                string[] array = new string[collitem.Count];
                                // int sss = 0;


                                if(collitem.Count > 0)
                                {
                                    if (collitem[0] != null)
                                    {
                                        SPListItem item = collitem[0];
                                        if (item["ARServiceName"] != null)
                                        {
                                            Top1.Text = item["ARServiceName"].ToString();
                                            toplink1 = item["ServiceURL"].ToString();
                                            tophref1.HRef = toplink1;

                                        }
                                    }
                                }
                                if (collitem.Count > 1)
                                {
                                    if (collitem[1] != null)
                                    {
                                        SPListItem item2 = collitem[1];
                                        if (item2["ARServiceName"] != null)
                                        {
                                            Top2.Text = item2["ARServiceName"].ToString();
                                            toplink2 = item2["ServiceURL"].ToString();
                                            tophref2.HRef = toplink2;
                                        }
                                    }
                                }
                                if (collitem.Count > 2)
                                {
                                    if (collitem[2] != null)
                                    {
                                        SPListItem item3 = collitem[2];
                                        if (item3["ARServiceName"] != null)
                                        {
                                            Top3.Text = item3["ARServiceName"].ToString();
                                            toplink3 = item3["ServiceURL"].ToString();
                                            tophref3.HRef = toplink3;
                                        }

                                    }
                                }
                                if (Top2.Text == Top3.Text)
                                {
                                    tophref3.Visible = false;
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
                            query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='TargetGroup' /><Value Type='Choice'>Student</Value></Eq></Where>";

                            SPListItemCollection collitem = list.GetItems(query);
                            if (collitem != null)
                            {

                                rptprod.DataSource = collitem.GetDataTable();
                                rptprod.DataBind();
                                stuN = collitem.Count;

                                lblStNser.Text = stuN.ToString();


                                string[] array = new string[collitem.Count];

                                foreach (SPListItem item1 in collitem)
                                {


                                    for (int i = 0; i < ss; i++)
                                    {
                                        array[i] = item1["ID"].ToString();
                                        //Label1.Text = i.ToString();
                                        //Label1.Text+= array[i];

                                        //    arrPictureBox[i] = item1["ID"].ToString();
                                        //    lblID.Text = arrPictureBox.ToString();
                                    }
                                    // ss = item1["ID"].ToString();
                                    //string[ss] AllID = { "" };
                                    //AllID = {item1["ID"]};

                                    // Session["PaID"] = ss;
                                }


                                // Session["PaID"] = collitem[]
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
        private void BindservicesFaculty()
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
                            query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='TargetGroup' /><Value Type='Choice'>FacultyMember</Value></Eq></And></Where>";

                            SPListItemCollection collitem = list.GetItems(query);
                            if (collitem != null)
                            {

                                RepFMem.DataSource = collitem.GetDataTable();
                                RepFMem.DataBind();
                                facN = collitem.Count;

                                LblFaNSer.Text = facN.ToString();

                                string[] array = new string[collitem.Count];

                                foreach (SPListItem item1 in collitem)
                                {


                                    for (int i = 0; i < ss; i++)
                                    {
                                        array[i] = item1["ID"].ToString();

                                    }

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

        private void BindservicesVis()
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
                            query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='TargetGroup' /><Value Type='Choice'>Visitor</Value></Eq></And></Where>";

                            SPListItemCollection collitem = list.GetItems(query);
                            if (collitem != null)
                            {

                                RepVisitor.DataSource = collitem.GetDataTable();
                                RepVisitor.DataBind();

                                visN = collitem.Count;
                                LblVisN.Text = visN.ToString();

                                string[] array = new string[collitem.Count];

                                foreach (SPListItem item1 in collitem)
                                {


                                    for (int i = 0; i < ss; i++)
                                    {
                                        array[i] = item1["ID"].ToString();
                                    }
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



        protected void Search_Click1(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("E-ServiceSearch.aspx?eti=" + txtSearch.Text);
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

    }
}
