using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace EserviceList.EserviceBrow
{
    public partial class EserviceBrowUserControl : UserControl
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
        private void BindservicesFav()
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
                        query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='Fav' /><Value Type='Boolean'>1</Value></Eq></And></Where>";

                        SPListItemCollection collitem = list.GetItems(query);
                        if (collitem != null)
                        {

                            rptprod.DataSource = collitem.GetDataTable();
                            rptprod.DataBind();
                            stuN = collitem.Count;

                            lblStNser.Text = stuN.ToString();


                            string[] array = new string[collitem.Count];
                            int sss = 0;
                            
                          
                            for (int i = 0; i< collitem.Count; i++)
                            {
                                
                                
                                if(collitem[i]!=null)
                                {
                                    SPListItem item = collitem[0];
                                    if (item["ARServiceName"] != null)
                                    {
                                        Top1.Text = item["ARServiceName"].ToString();
                                        toplink1 = item["ServiceURL"].ToString();
                                        tophref1.HRef = toplink1;

                                    }
                                }
                                if(collitem[i]!=null)
                                {
                                    SPListItem item2 = collitem[1];
                                    if (item2["ARServiceName"] != null)
                                    {
                                        Top2.Text = item2["ARServiceName"].ToString();
                                        toplink2 = item2["ServiceURL"].ToString();
                                        tophref2.HRef = toplink2;
                                    }
                                }
                                        
                                if(collitem[i] is null)
                                {
                                    break;
                                }
                                else 
                                if (collitem[i]!= null)
                                {
                                    SPListItem item3 = collitem[i];
                                    if (item3["ARServiceName"] != null)
                                    {
                                        Top3.Text = item3["ARServiceName"].ToString();
                                        toplink3 = item3["ServiceURL"].ToString();
                                        tophref3.HRef = toplink3;
                                    }
                                    else
                                        break;
                                }
                            
                                
                               

                                
                                
                               

                                
                              
                                

                            }
                            if (Top2.Text == Top3.Text)
                            {
                                tophref3.Visible = false;
                            }
                            //foreach (SPListItem item1 in collitem)
                            //{

                            //    array[0]=item1["ARServiceName"].ToString();
                            //    array[1]=item1["ARServiceName"].ToString();
                            //    Top1.Text = array[0];
                            //    Top2.Text = array[1];

                            //    //Top2.Text = item1["ARServiceName"].ToString()++;

                            //    //Top1.Text = item1["ARServiceName"].ToString();

                            //    for (int i = 0; i <= collitem.Count; i++)
                            //    {
                            //        // Top1.Text = item1["ARServiceName"].ToString();


                            //      //  array[i] = item1["ARServiceName"].ToString();


                            //        // Top1.Text = array[i];

                            //        //  arrPictureBox[i+1] = item1["ARServiceName"].ToString();
                            //        //Top2.Text = array[i];
                            //        //array[i + 1] = item1["ARServiceName"].ToString();

                            //        //Label1.Text = i.ToString();
                            //        //Label1.Text+= array[i];

                            //        //    arrPictureBox[i] = item1["ID"].ToString();
                            //        //    lblID.Text = arrPictureBox.ToString();
                            //    }


                            // //   Top2.Text = array[2];


                            //    // ss = item1["ID"].ToString();
                            //    //string[ss] AllID = { "" };
                            //    //AllID = {item1["ID"]};

                            //    // Session["PaID"] = ss;
                            //}


                            // Session["PaID"] = collitem[]
                        }
                    }
                }
            }
        }
        private void Bindservices()
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
                        query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='TargetGroup' /><Value Type='Choice'>Student</Value></Eq></And></Where>";

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
        private void BindservicesFaculty()
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

        private void BindservicesVis()
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

       

        protected void Search_Click1(object sender, EventArgs e)
        {
            Response.Redirect("E-ServiceSearch.aspx?eti=" + txtSearch.Text);
        }
    }

    
}
