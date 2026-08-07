using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using Portal.Main.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News
{
    public partial class ucRequestDetails : UserControl
    {
        private string _facultyNameEn = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["RequestId"] == null)
                return;
            if (!IsPostBack)
            {
                NewsUsers newsUsers = new NewsUsers();
                newsUsers = busclsNewsUsers.GetCurrentUser();

                if (newsUsers == null)
                {
                    dvForm.Attributes.Add("class", "d-none");
                    hMsg.Attributes.Add("class", "block");

                    return;
                }
                dv_userAction.Visible = false;
                divReturnStep.Visible = false;

                RequestsList obj = GetRequestsById();
                if (obj.RequestStatus == "Pending")
                {
                    if (obj.NextRequestStatus != "")
                    {
                        bool canAct = false;
                        if (newsUsers.IsAdmin)
                            canAct = true;
                        else if (newsUsers.WorkFlowSteps.Contains(obj.NextRequestStatus))
                            canAct = true;

                        if (canAct)
                        {
                            dv_userAction.Visible = true;
                            // إتاحة إرجاع الخبر إلى أي مرحلة سابقة (ما عدا المرحلة الحالية)
                            divReturnStep.Visible = true;
                            BindReturnSteps(obj.NextRequestStatus);
                        }
                    }
                }
                else if (obj.RequestStatus == "Rejected")
                {
                    dv_userAction.Visible = true;
                    divSubmit.Visible = false;
                    if (obj.UserComments != null && obj.UserComments.ToString() != "")
                        txtComments.Text = obj.UserComments.ToString();

                    // السماح للأدمن بإعادة خبر مرفوض إلى إحدى مراحل سير العمل
                    if (newsUsers.IsAdmin)
                    {
                        divReturnStep.Visible = true;
                        BindReturnSteps("");
                    }
                }
                else if (obj.RequestStatus == "Approved")
                {
                    // السماح للأدمن بسحب خبر معتمد/منشور وإرجاعه إلى مرحلة سابقة (تحرير / تدقيق ...)
                    if (newsUsers.IsAdmin)
                    {
                        dv_userAction.Visible = true;
                        divSubmit.Visible = false;
                        divReturnStep.Visible = true;
                        BindReturnSteps("");
                    }
                }

                // إخفاء لوحة "إرجاع الخبر" إذا كانت الجهة الطالبة هي المركز الإعلامي
                if (string.Equals(_facultyNameEn.Trim(), "Media Center", StringComparison.OrdinalIgnoreCase))
                    divReturnStep.Visible = false;


            }
        }
        private RequestsList GetRequestsById()
        {

            RequestsList obj = new RequestsList();
            if (Page.Request.QueryString["RequestId"] == null)
                return null;

            var RequestId = Convert.ToInt32(Page.Request.QueryString["RequestId"]);

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                        {
                            SPList requestsList = web.Lists["RequestsList"];
                            SPListItem item = requestsList.GetItemById(RequestId);

                            obj = SPFactory.MapListItemsToClass<RequestsList>(item);
                            opt_IsHome.SelectedValue = "False";
                            if (item["IsHome"] != null)
                            {
                                if (item["IsHome"].ToString() == "1")
                                {
                                    opt_IsHome.SelectedValue = "True";
                                }

                            }


                            if (item["FacultyName"] != null)
                            {
                                lblFacultyName.Text = Convert.ToString(item["FacultyName"]);
                                if (item["FacultyName"].ToString() != "Main")
                                {
                                    btnAskForEdit.Visible = true;
                                }

                            }

                            if (item["FacultyName_EN"] != null)
                                _facultyNameEn = Convert.ToString(item["FacultyName_EN"]);

                            txtDate.Text = Convert.ToString(item["MediaDate"]);
                            txtTitle.Text = Convert.ToString(item["Title"]);
                            txtSummary.Text = Convert.ToString(item["Summary"]);
                            txtDetails.Text = Convert.ToString(item["MediaContent"]);

                            txtTitleEn.Text = Convert.ToString(item["Title_EN"]);
                            txtSummaryEn.Text = Convert.ToString(item["Summary_EN"]);
                            txtDetailsEn.Text = Convert.ToString(item["MediaContent_EN"]);



                            if (item["VideoURL"] != null)
                                txtVideoURL.Text = item["VideoURL"].ToString();

                            if (item.Attachments.Count > 0)
                            {
                                SPAttachmentCollection attachments = item.Attachments;

                                SPFile file = SPContext.Current.Web.GetFile(attachments.UrlPrefix + attachments[0]);
                                Image1.ImageUrl = file.ServerRelativeUrl;
                            }

                            if (item["AdditionalFileURL"] != null)
                            {
                                divAdditionalFileURL.Visible = true;
                                AdditionalFileURLLink.NavigateUrl = SPContext.Current.Site.Url + item["AdditionalFileURL"].ToString();
                                AdditionalFileURLLink.Text = "رابط الملف";
                            }

                        }
                    }
                }
            });



            return obj;



        }

        /// <summary>
        /// تعبئة قائمة المراحل التي يمكن إرجاع الخبر إليها:
        /// - أثناء سير العمل (Pending): المراحل "السابقة" فقط حسب ItemOrder
        /// - للأدمن على خبر معتمد/مرفوض (currentStep = ""): جميع المراحل
        /// </summary>
        private void BindReturnSteps(string currentStep)
        {
            try
            {
                List<NewsWorkflowSteps> steps = busclsNewsWorkflowSteps.GetAllSteps();

                // ترتيب المرحلة الحالية — لعرض المراحل السابقة لها فقط
                int currentOrder = int.MaxValue;
                if (!string.IsNullOrEmpty(currentStep))
                {
                    foreach (NewsWorkflowSteps s in steps)
                    {
                        if (s != null && s.WorkflowStepName == currentStep)
                        {
                            currentOrder = ParseOrder(s.ItemOrder);
                            break;
                        }
                    }
                }

                ddlReturnStep.Items.Clear();
                ddlReturnStep.Items.Add(new ListItem("-- اختر المرحلة --", ""));

                foreach (NewsWorkflowSteps step in steps)
                {
                    if (step == null || string.IsNullOrEmpty(step.WorkflowStepName))
                        continue;
                    if (step.WorkflowStepName == currentStep)
                        continue;
                    // نعرض المراحل السابقة فقط أثناء سير العمل
                    if (ParseOrder(step.ItemOrder) >= currentOrder)
                        continue;

                    ddlReturnStep.Items.Add(new ListItem(step.WorkflowStepName, step.WorkflowStepName));
                }

                // إذا لم توجد مراحل سابقة نخفي اللوحة (مثال: الخبر في أول مرحلة)
                if (ddlReturnStep.Items.Count <= 1)
                    divReturnStep.Visible = false;
            }
            catch (Exception)
            {
                divReturnStep.Visible = false;
            }
        }

        private int ParseOrder(string itemOrder)
        {
            double order;
            if (double.TryParse(itemOrder, out order))
                return (int)order;
            return int.MaxValue - 1;
        }

        /// <summary>
        /// إرجاع الخبر إلى مرحلة محددة: الحالة تعود Pending والمرحلة القادمة = المرحلة المختارة
        /// مع إرسال إشعار بريدي لمقدم الطلب وللأدمن
        /// </summary>
        private bool ReturnToStep(string targetStep)
        {
            bool retVal = false;

            if (Page.Request.QueryString["RequestId"] == null)
                return false;
            if (string.IsNullOrEmpty(targetStep))
                return false;

            var RequestId = Convert.ToInt32(Page.Request.QueryString["RequestId"]);
            string requesterUser = "";
            string newTitle = "";

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                    {
                        SPList requestsList = web.Lists["RequestsList"];
                        SPListItem item = requestsList.GetItemById(RequestId);

                        if (item == null)
                            return;

                        newTitle = Convert.ToString(item["Title"]);
                        if (item["RequesterEmail"] != null)
                            requesterUser = Convert.ToString(item["RequesterEmail"]);

                        item["RequestStatus"] = "Pending";
                        item["NextRequestStatus"] = targetStep;
                        item["UserComments"] = txtComments.Text;

                        web.AllowUnsafeUpdates = true;
                        item.Update();
                        web.AllowUnsafeUpdates = false;
                        retVal = true;
                    }
                }
            });

            if (!retVal)
                return false;

            var toMail = requesterUser != "" ? requesterUser : "mhelrefaie@pnu.edu.sa";
            var groupUsers = Helper.GetAdminUsers();
            List<string> ccMail = new List<string>();
            foreach (var u in groupUsers)
            {
                ccMail.Add(u.UserEmail);
            }

            var subjectMail = $"المركز الإعلامي - تمت إعادة الخبر إلى مرحلة {targetStep}";

            string CommonTemplate = PortalHelper.GetEmailTemplate("CommonTemplate");
            CommonTemplate = CommonTemplate.Replace("{Title}", subjectMail)
                .Replace("{LinkTitle}", "رابط الخبر ")
                .Replace("{LinkURL}", SPContext.Current.Web.Url + "/Pages/RequestDetails.aspx?RequestId=" + RequestId);

            string AddNewTemplate = PortalHelper.GetEmailTemplate("ModifyRejectNewTemplate");

            Hashtable values = new Hashtable();
            values.Add("{requesterName}", requesterUser);
            values.Add("{txtTitle}", txtTitle.Text);
            values.Add("{MediaContent}", txtDetails.Text);
            values.Add("{txtCommentsTitle}", "سبب الإرجاع");
            values.Add("{txtComments}", txtComments.Text);

            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    AddNewTemplate = AddNewTemplate.Replace(key, values[key].ToString());
                }
            }
            CommonTemplate = CommonTemplate.Replace("{Table}", AddNewTemplate);

            StringBuilder bodyMail = new StringBuilder();
            bodyMail.Append(CommonTemplate);

            EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());
            return retVal;
        }

        protected void btnReturnToStep_Click(object sender, EventArgs e)
        {
            // التعليق (سبب الإرجاع) واختيار المرحلة إلزاميان
            rfvFaculties.Enabled = true;
            rfvFaculties.ValidationGroup = "ReturnRequest";
            rfvReturnStep.Enabled = true;

            Page.Validate("ReturnRequest");

            if (Page.IsValid)
            {
                var result = ReturnToStep(ddlReturnStep.SelectedValue);
                if (result)
                    dv_userAction.Visible = false;
                else
                    dv_userAction.Visible = true;

                Response.Redirect("requestlist.aspx", false);
            }
        }

        private bool CompleteTask(string userAction)
        {
            bool retVal = false;

            if (Page.Request.QueryString["RequestId"] == null)
                return false;

            var RequestId = Convert.ToInt32(Page.Request.QueryString["RequestId"]);
            string requesterUser = "";
            string newTitle = "";
            string currentStepName = "";
            string nextStepName = "";
            bool isFinalApproval = false;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                        {
                            SPList requestsList = web.Lists["RequestsList"];

                            SPListItem item = requestsList.GetItemById(RequestId);

                            if (item == null)
                                return;

                            newTitle = Convert.ToString(item["Title"]);
                            if (item["RequesterEmail"] != null)
                                requesterUser = Convert.ToString(item["RequesterEmail"]);
                            if (userAction == "Approved")
                            {
                                if (item["NextRequestStatus"] != null && item["NextRequestStatus"].ToString() != "")
                                {
                                    // المرحلة الحالية التي تمت الموافقة فيها
                                    currentStepName = item["NextRequestStatus"].ToString();

                                    NewsWorkflowSteps NextStep = busclsNewsWorkflowSteps.GetNextStep(item["NextRequestStatus"].ToString());
                                    if (NextStep != null)
                                    {
                                        // موافقة مرحلية: الانتقال إلى المرحلة التالية
                                        nextStepName = NextStep.WorkflowStepName;
                                        item["NextRequestStatus"] = NextStep.WorkflowStepName;
                                    }
                                    else
                                    {
                                        // لا توجد مرحلة تالية: هذه الموافقة النهائية
                                        isFinalApproval = true;
                                        item["NextRequestStatus"] = "";
                                        item["RequestStatus"] = userAction;
                                    }
                                }
                                else
                                {
                                    isFinalApproval = true;
                                    item["RequestStatus"] = userAction;
                                }

                            }
                            else
                                item["RequestStatus"] = userAction;

                            item["UserComments"] = txtComments.Text;
                            //item["IsHome"] = Convert.ToInt32(opt_IsHome.SelectedValue);

                            if (opt_IsHome.SelectedValue == "True")
                                item["IsHome"] = 1;
                            else
                                item["IsHome"] = 0;


                            web.AllowUnsafeUpdates = true;
                            item.Update();
                            web.AllowUnsafeUpdates = false;
                            retVal = true;
                        }
                    }
                }
            });




            var toMail = requesterUser != "" ? requesterUser : "mhelrefaie@pnu.edu.sa";
            var groupUsers = Helper.GetAdminUsers();
            List<string> ccMail = new List<string>();
            foreach (var u in groupUsers)
            {
                ccMail.Add(u.UserEmail);
            }

            string action = "";
            if (userAction == "Approved")
            {
                action = "الموافقة";
            }
            else if (userAction == "Rejected")
            {
                action = "الرفض";
            }
            else if (userAction == "ModificationRequest")
            {
                action = "طلب تعديل";
            }
            else
            {
                action = "إعادة";
            }

            string subjectMail;
            if (userAction == "Approved" && !isFinalApproval && nextStepName != "")
            {
                // موافقة مرحلية: نذكر المرحلة الحالية والمرحلة التالية
                subjectMail = $"المركز الإعلامي - تمت الموافقة على الخبر في مرحلة {currentStepName} وانتقاله إلى مرحلة {nextStepName}";
            }
            else if (userAction == "Approved" && isFinalApproval)
            {
                subjectMail = "المركز الإعلامي - تمت الموافقة النهائية على نشر الخبر";
            }
            else
            {
                subjectMail = $"المركز الإعلامي - تم {action} علي نشر الخبر ";
            }
            StringBuilder bodyMail = new StringBuilder();

            string CommonTemplate = PortalHelper.GetEmailTemplate("CommonTemplate");
            CommonTemplate = CommonTemplate.Replace("{Title}", subjectMail)
            .Replace("{LinkTitle}", "رابط الخبر ");

            Hashtable values = new Hashtable();
            values.Add("{requesterName}", requesterUser);
            values.Add("{txtTitle}", txtTitle.Text);
            values.Add("{MediaContent}", txtDetails.Text);

            string AddNewTemplate = "";

            if (userAction == "ModificationRequest")
            {
                AddNewTemplate = PortalHelper.GetEmailTemplate("ModifyRejectNewTemplate");
                CommonTemplate = CommonTemplate.Replace("{LinkURL}", SPContext.Current.Web.Url + "/Pages/EditNewRequest.aspx?RequestId=" + RequestId);

                values.Add("{txtCommentsTitle}", "سبب طلب التعديل");
                values.Add("{txtComments}", txtComments.Text);
            }
            else if (userAction == "Rejected")
            {
                AddNewTemplate = PortalHelper.GetEmailTemplate("ModifyRejectNewTemplate");
                CommonTemplate = CommonTemplate.Replace("{LinkURL}", SPContext.Current.Web.Url + "/Pages/RequestDetails.aspx?RequestId=" + RequestId);
                values.Add("{txtCommentsTitle}", "سبب الرفض");
                values.Add("{txtComments}", txtComments.Text);
            }
            else
            {
                AddNewTemplate = PortalHelper.GetEmailTemplate("AddNewTemplate");
                CommonTemplate = CommonTemplate.Replace("{LinkURL}", SPContext.Current.Web.Url + "/Pages/RequestDetails.aspx?RequestId=" + RequestId);
            }

            // replace values in design template
            foreach (string key in values.Keys)
            {
                if (values[key] != null)
                {
                    // replace variable in template design
                    AddNewTemplate = AddNewTemplate.Replace(key, values[key].ToString());
                }
            }
            CommonTemplate = CommonTemplate.Replace("{Table}", AddNewTemplate);
            bodyMail.Append(CommonTemplate);

            EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());
            return retVal;
        }

        protected void btnAskForEdit_Click(object sender, EventArgs e)
        {

            // Enable validation for txtComments
            rfvFaculties.Enabled = true;
            rfvFaculties.ValidationGroup = "EditRequest";

            // Trigger validation
            Page.Validate("EditRequest");

            if (Page.IsValid)
            {
                var result = CompleteTask("ModificationRequest");
                if (result)
                    dv_userAction.Visible = false;
                else
                    dv_userAction.Visible = true;

                Response.Redirect("requestlist.aspx", false);
            }

        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            var result = CompleteTask("Approved");
            if (result)
                dv_userAction.Visible = false;
            else
                dv_userAction.Visible = true;

            Response.Redirect("requestlist.aspx", false);
        }
        protected void btnReject_Click(object sender, EventArgs e)
        {
            // Enable validation for txtComments
            rfvFaculties.Enabled = true;
            rfvFaculties.ValidationGroup = "RejectRequest";

            // Trigger validation
            Page.Validate("RejectRequest");

            if (Page.IsValid)
            {
                var result = CompleteTask("Rejected");
                if (result)
                    dv_userAction.Visible = false;
                else
                    dv_userAction.Visible = true;


                Response.Redirect("requestlist.aspx", false);
            }

        }

        protected void brnEdit_Click(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["RequestId"] == null)
                return;

            var RequestId = Convert.ToInt32(Page.Request.QueryString["RequestId"]);
            Response.Redirect("EditNews.aspx?RequestId=" + RequestId);
        }
    }




}