using Microsoft.SharePoint;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PnuEServices
{
    public partial class AddPnuService : UserControl
    {
        #region properties
        public string SiteUrl { get; set; }
        public string WebUrl { get; set; } = "admin";
        public string ListName { get; set; } = "PnuEservices";
        public bool EditMode
        {
            get
            {
                if (ViewState["PnuServiceEditMode"] != null)
                    return (bool)(ViewState["PnuServiceEditMode"]);
                else
                    return false;


            }
            set
            {
                ViewState["PnuServiceEditMode"] = value;
            }
        }
        public List<Condition> ConditionsList
        {
            get
            {
                if (ViewState["conditions"] != null)
                    return (List<Condition>)(ViewState["conditions"]);
                else
                    return new List<Condition>();


            }
            set
            {
                ViewState["Condition"] = value;
            }
        }
        public List<Condition> ConditionsListEn
        {
            get
            {
                if (ViewState["conditionsEn"] != null)
                    return (List<Condition>)(ViewState["conditionsEn"]);
                else
                    return new List<Condition>();


            }
            set
            {
                ViewState["ConditionEn"] = value;
            }
        }
        public List<Document> DocumentsList
        {
            get
            {
                if (ViewState["documents"] != null)
                    return (List<Document>)(ViewState["documents"]);
                else
                    return new List<Document>();


            }
            set
            {
                ViewState["documents"] = value;
            }
        }
        public List<Document> DocumentsListEn
        {
            get
            {
                if (ViewState["documentsEn"] != null)
                    return (List<Document>)(ViewState["documentsEn"]);
                else
                    return new List<Document>();


            }
            set
            {
                ViewState["documentsEn"] = value;
            }
        }
        public List<Schedule> SchedulesList
        {
            get
            {
                if (ViewState["schedule"] != null)
                    return (List<Schedule>)(ViewState["schedule"]);
                else
                    return new List<Schedule>();


            }
            set
            {
                ViewState["schedule"] = value;
            }
        }
        public List<Schedule> SchedulesListEn
        {
            get
            {
                if (ViewState["scheduleEn"] != null)
                    return (List<Schedule>)(ViewState["scheduleEn"]);
                else
                    return new List<Schedule>();


            }
            set
            {
                ViewState["scheduleEn"] = value;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadCategories();
                    LoadBeneficieries();
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!EditMode)
                    Save();
                else
                    Edit();
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        void LoadCategories()
        {
            try
            {
                var Categories = Helper.GetChoicesLookup("PnuEservices", "ServicesCategories", SPContext.Current.Site.ID, "admin");
                var categoriesEn = Helper.GetChoicesLookup("PnuEservices", "ServicesCategoriesEn", SPContext.Current.Site.ID, "admin");

                ddl_Category.DataSource = Categories;
                ddl_Category.DataTextField = "Name";
                ddl_Category.DataValueField = "Value";
                ddl_Category.DataBind();

                ddl_Category.Items.Insert(0, new ListItem("--اختر--", "0"));

                ddlCategoryEn.DataSource = categoriesEn;
                ddlCategoryEn.DataTextField = "Name";
                ddlCategoryEn.DataValueField = "Value";
                ddlCategoryEn.DataBind();

                ddlCategoryEn.Items.Insert(0, new ListItem("--Select--", "0"));




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        void LoadBeneficieries()
        {
            try
            {
                var Beneficieries = Helper.GetMultipleChoicesLookup("PnuEservices", "Beneficiaries", SPContext.Current.Site.ID, "admin");

                var BeneficieriesEn = Helper.GetMultipleChoicesLookup("PnuEservices", "BeneficiariesEn", SPContext.Current.Site.ID, "admin");

                chk_Benef.DataSource = Beneficieries;
                chk_Benef.DataTextField = "Name";
                chk_Benef.DataValueField = "Value";
                chk_Benef.DataBind();

                chkBenfEn.DataSource = BeneficieriesEn;
                chkBenfEn.DataTextField = "Name";
                chkBenfEn.DataValueField = "Value";
                chkBenfEn.DataBind();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }

        protected void Save()
        {
            try
            {

                var jsonConditions = JsonConvert.SerializeObject(ConditionsList);
                var jsonConditionsEn = JsonConvert.SerializeObject(ConditionsListEn);
                var jsonDocuments = JsonConvert.SerializeObject(DocumentsList);
                var jsonDocumentsEn = JsonConvert.SerializeObject(DocumentsListEn);
                var jsonSchedule = JsonConvert.SerializeObject(SchedulesList);
                var jsonScheduleEn = JsonConvert.SerializeObject(SchedulesListEn);

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {

                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))//this.WebUrl
                        {
                            SPList PnuServiceList = web.Lists[this.ListName];

                            SPListItem listItem = PnuServiceList.Items.Add();


                            listItem["Title"] = txtServiceTitle.Text;
                            listItem["TitleEn"] = txt_ServiceTitleEn.Text;
                            listItem["Desc"] = txtServiceDEsc.Text;
                            listItem["DescEn"] = txtServiceDescEn.Text;
                            List<ListItem> selectedBenef = chk_Benef.Items.Cast<ListItem>()
                                                       .Where(li => li.Selected)
                                                       .ToList();

                            SPFieldMultiChoiceValue itemValue = new SPFieldMultiChoiceValue();
                            foreach (var i in selectedBenef)
                            {
                                itemValue.Add(i.Value);
                            }
                            listItem["Beneficiaries"] = itemValue;

                            List<ListItem> selectedBenefEn = chkBenfEn.Items.Cast<ListItem>()
                                                       .Where(li => li.Selected)
                                                       .ToList();

                            SPFieldMultiChoiceValue itemValueEn = new SPFieldMultiChoiceValue();
                            foreach (var i in selectedBenefEn)
                            {
                                itemValueEn.Add(i.Value);
                            }
                            
                            listItem["BeneficiariesEn"] = itemValueEn;
                            listItem["ServicesCategories"] = ddl_Category.SelectedValue ;
                            listItem["ServicesCategoriesEn"] = ddlCategoryEn.SelectedValue;
                            listItem["ItemOrder"] = Convert.ToInt32(txtServiceOrder.Text);
                            listItem["IsHome"] = rdb_IsHome.SelectedValue == "1" ? true :false ;
                            listItem["IsActive"] = rdb_IsActive.SelectedValue=="1" ? true : false;
                            listItem["ServiceUrl"] = txtServiceUrl.Text ; 
                            listItem["Conditions"] = jsonConditions;
                            listItem["ConditionsEn"] = jsonConditionsEn;
                            listItem["Documents"] = jsonDocuments;
                            listItem["DocumentsEn"] = jsonDocumentsEn; 
                            listItem["Schedule"] = jsonSchedule;
                            listItem["ScheduleEn"] = jsonScheduleEn;


                            if (fileUPload.PostedFile != null && fileUPload.HasFile)
                            {
                                Stream fStream = fileUPload.PostedFile.InputStream;
                                byte[] contents = new byte[fStream.Length];
                                fStream.Read(contents, 0, (int)fStream.Length);
                                fStream.Close();
                                fStream.Dispose();
                                SPAttachmentCollection attachments = listItem.Attachments;
                                string fileName = Path.GetFileName(fileUPload.PostedFile.FileName);
                                attachments.Add(fileName, contents);
                            }

                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            web.AllowUnsafeUpdates = false;


                            Msg.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_SccuessMsg");
                            Msg.Attributes.Add("class", "alert alert-success mb-4");


                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                Msg.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_FailMsg");
                Msg.Attributes.Add("class", "alert alert-danger mb-4");
            }
        }
        protected void Edit()
        {
            
            try
            {

                var jsonConditions = JsonConvert.SerializeObject(ConditionsList);
                var jsonDocuments = JsonConvert.SerializeObject(DocumentsList);
                var jsonSchedula = JsonConvert.SerializeObject(SchedulesList);

               
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {

                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList PnuServiceList = web.Lists[this.ListName];
                            SPQuery query = new SPQuery();
                            query.Query = "<Where>" +
                                           "<Eq>" +
                                               "<FieldRef Name='Email'/><Value Type='Text'></Value>" +
                                           "</Eq>" +
                                           "</Where>";

                            SPListItem listItem = PnuServiceList.GetItems(query)[0];

                           


                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            web.AllowUnsafeUpdates = false;

                           
                           // MsgAlert.InnerText = Helper.GetCustomFormsGlobalResourceValue("PnuInternetResources", "res_SccuessMsg");
                          //  MsgAlert.Attributes.Add("class", "alert alert-success mb-4");



                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

        }
        #region Conditions
        protected void AddNewCondition()
        {
            try
            {
                int index = 0;
                var lst = new List<Condition>();

                if (ConditionsList.Count > 0)
                {
                    var cond = new Condition
                    {
                        Id = (ConditionsList.Select(x => x.Id).Max()) + 1,
                        Order = Convert.ToInt32(txtCondOrder.Text),
                        Title = txt_Cond.Text

                    };

                    ConditionsList.Add(cond);
                    ViewState["conditions"] = ConditionsList;
                }
                else
                {
                    index += 1;
                    var cond = new Condition
                    {
                        Id = index,
                        Order = Convert.ToInt32(txtCondOrder.Text),
                        Title = txt_Cond.Text

                    };

                    lst.Add(cond);
                    ViewState["conditions"] = lst;
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void DeleteCondition(int Id)
        {
            try
            {
                var itemToRemove = ConditionsList.Single(r => r.Id == Id);
                ConditionsList.Remove(itemToRemove);
                ViewState["conditions"] = ConditionsList;
                BindRepeater();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void BindRepeater()
        {
            try
            {
                var CondList = (List<Condition>)ViewState["conditions"];
                rep_cond.DataSource = CondList;
                rep_cond.DataBind();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }
        protected void rep_cond_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    int id = Convert.ToInt32(e.CommandArgument.ToString());
                    DeleteCondition(id);

                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void btn_AddCond_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewCondition();
                BindRepeater();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        #endregion
        #region Conditions En
        protected void AddNewConditionEn()
        {
            try
            {
                int index = 0;
                var lst = new List<Condition>();

                if (ConditionsListEn.Count > 0)
                {
                    var cond = new Condition
                    {
                        Id = (ConditionsListEn.Select(x => x.Id).Max()) + 1,
                        Order = Convert.ToInt32(txtConOrderEn.Text),
                        Title = txtCondEn.Text

                    };

                    ConditionsListEn.Add(cond);
                    ViewState["conditionsEn"] = ConditionsListEn;
                }
                else
                {
                    index += 1;
                    var cond = new Condition
                    {
                        Id = index,
                        Order = Convert.ToInt32(txtConOrderEn.Text),
                        Title = txtCondEn.Text

                    };

                    lst.Add(cond);
                    ViewState["conditionsEn"] = lst;
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void DeleteConditionEn(int Id)
        {
            try
            {
                var itemToRemove = ConditionsListEn.Single(r => r.Id == Id);
                ConditionsListEn.Remove(itemToRemove);
                ViewState["conditionsEn"] = ConditionsListEn;
                BindRepeaterEn();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void BindRepeaterEn()
        {
            try
            {
                var CondList = (List<Condition>)ViewState["conditionsEn"];
                rep_CondEn.DataSource = CondList;
                rep_CondEn.DataBind();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }
        protected void rep_CondEn_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    int id = Convert.ToInt32(e.CommandArgument.ToString());
                    DeleteConditionEn(id);

                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void btnAddCondEn_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewConditionEn();
                BindRepeaterEn();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        #endregion
        #region Documents
        protected void AddNewDocument()
        {
            try
            {
                int index = 0;
                var lst = new List<Document>();

                if (DocumentsList.Count > 0)
                {
                    var doc = new Document
                    {
                        Id = (DocumentsList.Select(x => x.Id).Max()) + 1,
                        Order = Convert.ToInt32(txt_docOrder.Text),
                        Title = txt_DocTitle.Text

                    };

                    DocumentsList.Add(doc);
                    ViewState["documents"] = DocumentsList;
                }
                else
                {
                    index += 1;
                    var doc = new Document
                    {
                        Id = index,
                        Order = Convert.ToInt32(txt_docOrder.Text),
                        Title = txt_DocTitle.Text

                    };

                    lst.Add(doc);
                    ViewState["documents"] = lst;
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void DeleteDocument(int Id)
        {
            try
            {
                var itemToRemove = DocumentsList.Single(r => r.Id == Id);
                DocumentsList.Remove(itemToRemove);
                ViewState["documents"] = DocumentsList;
                BindDocumentRepeater();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void BindDocumentRepeater()
        {
            try
            {
                var docList = (List<Document>)ViewState["documents"];
                rep_Documents.DataSource = docList;
                rep_Documents.DataBind();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }
        protected void rep_Documents_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    int id = Convert.ToInt32(e.CommandArgument.ToString());
                    DeleteDocument(id);

                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void btn_AddDoc_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewDocument();
                BindDocumentRepeater();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        #endregion
        #region DocumentsEn
        protected void AddNewDocumentEn()
        {
            try
            {
                int index = 0;
                var lst = new List<Document>();

                if (DocumentsListEn.Count > 0)
                {
                    var doc = new Document
                    {
                        Id = (DocumentsListEn.Select(x => x.Id).Max()) + 1,
                        Order = Convert.ToInt32(txtDocOrderEn.Text),
                        Title = txtDoctitleEn.Text

                    };

                    DocumentsListEn.Add(doc);
                    ViewState["documentsEn"] = DocumentsListEn;
                }
                else
                {
                    index += 1;
                    var doc = new Document
                    {
                        Id = index,
                        Order = Convert.ToInt32(txtDocOrderEn.Text),
                        Title = txtDoctitleEn.Text

                    };

                    lst.Add(doc);
                    ViewState["documentsEn"] = lst;
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void DeleteDocumentEn(int Id)
        {
            try
            {
                var itemToRemove = DocumentsListEn.Single(r => r.Id == Id);
                DocumentsListEn.Remove(itemToRemove);
                ViewState["documentsEn"] = DocumentsListEn;
                BindDocumentRepeaterEn();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void BindDocumentRepeaterEn()
        {
            try
            {
                var docList = (List<Document>)ViewState["documentsEn"];
                rep_DocumentsEn.DataSource = docList;
                rep_DocumentsEn.DataBind();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }
        protected void rep_DocumentsEn_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    int id = Convert.ToInt32(e.CommandArgument.ToString());
                    DeleteDocumentEn(id);

                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void btnAddDocEn_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewDocumentEn();
                BindDocumentRepeaterEn();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        #endregion
        #region Schedule
        protected void AddNewSchedule()
        {
            try
            {
                int index = 0;
                var lst = new List<Schedule>();

                if (SchedulesList.Count > 0)
                {
                    var sched = new Schedule
                    {
                        Id = (SchedulesList.Select(x => x.Id).Max()) + 1,
                        Title = txt_procedure.Text,
                        Day = ddl_day.SelectedValue,
                        Date = ProcedureDate.SelectedDate.ToShortDateString(),
                        Order = 1

                    };

                    SchedulesList.Add(sched);
                    ViewState["schedule"] = SchedulesList;
                }
                else
                {
                    index += 1;
                    var sched = new Schedule
                    {
                        Id = index,
                        Title = txt_procedure.Text,
                        Day = ddl_day.SelectedValue,
                        Date = ProcedureDate.SelectedDate.ToShortDateString(),
                        Order = 1

                    };

                    lst.Add(sched);
                    ViewState["schedule"] = lst;
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void DeleteSchedule(int Id)
        {
            try
            {
                var itemToRemove = SchedulesList.Single(r => r.Id == Id);
                SchedulesList.Remove(itemToRemove);
                ViewState["schedule"] = SchedulesList;
                BindScheduleRepeater();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void BindScheduleRepeater()
        {
            try
            {
                var SchList = (List<Schedule>)ViewState["schedule"];
                rep_Schedule.DataSource = SchList;
                rep_Schedule.DataBind();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }
        protected void rep_Schedule_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    int id = Convert.ToInt32(e.CommandArgument.ToString());
                    DeleteSchedule(id);

                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void btn_AddProcedure_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewSchedule();
                BindScheduleRepeater();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        #endregion
        #region Schedule En
        protected void AddNewScheduleEn()
        {
            try
            {
                int index = 0;
                var lst = new List<Schedule>();

                if (SchedulesListEn.Count > 0)
                {
                    var sched = new Schedule
                    {
                        Id = (SchedulesListEn.Select(x => x.Id).Max()) + 1,
                        Title = txtProcEn.Text,
                        Day = ddl_dayEn.SelectedValue,
                        Date = ProcedureDateEn.SelectedDate.ToShortDateString(),
                        Order = 1

                    };

                    SchedulesListEn.Add(sched);
                    ViewState["scheduleEn"] = SchedulesListEn;
                }
                else
                {
                    index += 1;
                    var sched = new Schedule
                    {
                        Id = index,
                        Title = txtProcEn.Text,
                        Day = ddl_dayEn.SelectedValue,
                        Date = ProcedureDateEn.SelectedDate.ToShortDateString(),
                        Order = 1

                    };

                    lst.Add(sched);
                    ViewState["scheduleEn"] = lst;
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void DeleteScheduleEn(int Id)
        {
            try
            {
                var itemToRemove = SchedulesListEn.Single(r => r.Id == Id);
                SchedulesListEn.Remove(itemToRemove);
                ViewState["scheduleEn"] = SchedulesListEn;
                BindScheduleRepeaterEn();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void BindScheduleRepeaterEn()
        {
            try
            {
                var SchList = (List<Schedule>)ViewState["scheduleEn"];
                rep_ScheduleEn.DataSource = SchList;
                rep_ScheduleEn.DataBind();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }
       
        protected void rep_ScheduleEn_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    int id = Convert.ToInt32(e.CommandArgument.ToString());
                    DeleteScheduleEn(id);

                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
        protected void btnAddScheduleEn_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewScheduleEn();
                BindScheduleRepeaterEn();



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }










        #endregion

       
    }

}
