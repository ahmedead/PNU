using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Controls.Common;
using Portal.Main.Helper;
using Portal.Main.WebApp.Controls.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Forms
{
    public partial class ucNoraNabtaker : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()

            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {

                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;

                        string listName = "NoraNabtaker";
                        SPList list = null;

                        // Check if the list exists
                        if (!web.Lists.Cast<SPList>().Any(l => l.Title == listName))
                        {
                            // Create list
                            Guid listId = web.Lists.Add(listName, "List to collect challenges and opportunities", SPListTemplateType.GenericList);
                            list = web.Lists[listId];

                            // Rename default Title field
                            SPField titleField = list.Fields.GetFieldByInternalName("Title");
                            titleField.Title = "الاسم";
                            titleField.Update();

                            //StringCollection strings = new StringCollection();
                            //strings.Add("");
                            //strings.Add("");
                            // Add required fields
                            list.Fields.Add("Department", SPFieldType.Text, false);
                            list.Fields.Add("Workplace", SPFieldType.Text, false);
                            list.Fields.Add("EntryType", SPFieldType.Choice, false,false,
                                new StringCollection { "تحدٍ قائم", "فرصة قابلة للاستثمار" });
                            list.Fields.Add("Description", SPFieldType.Note, false);
                            list.Fields.Add("ImpactScope", SPFieldType.Choice, false,
                                false,
                                new StringCollection {
                            "على مستوى مجموعة أفراد",
                            "على مستوى القسم فقط",
                            "على مستوى الإدارة / الكلية",
                            "على مستوى الجامعة ككل"
                                });
                            list.Fields.Add("ImpactEvidence", SPFieldType.Note, false);
                            list.Fields.Add("ImpactConsequences", SPFieldType.Note, false);
                            list.Fields.Add("ProposedIdea", SPFieldType.Note, false);
                            list.Fields.Add("Participate", SPFieldType.Choice, false,
                                false,
                                new StringCollection { "نعم", "لا", "حسب التفرغ / المهام" });
                            list.Fields.Add("SelfEvaluation", SPFieldType.Choice, false,
                                false,
                                new StringCollection {
                            "منخفض (تحسين محلي بسيط)",
                            "متوسط (فرق على مستوى إدارة / كلية)",
                            "عالٍ (تحول على مستوى الجامعة)"
                                });
                            list.Fields.Add("Generalizability", SPFieldType.Choice, false,
                                false,
                                new StringCollection {
                            "نعم، ويمكن تعميمه على جهات أخرى",
                            "لا، الحل خاص ببيئة محددة",
                            "محتمل، حسب ظروف الجهة الأخرى"
                                });
                            list.Fields.Add("AdditionalIdea", SPFieldType.Note, false);

                            // Update view to show fields
                            SPView view = list.DefaultView;
                            view.ViewFields.Add("Department");
                            view.ViewFields.Add("Workplace");
                            view.ViewFields.Add("EntryType");
                            view.ViewFields.Add("Description");
                            view.ViewFields.Add("ImpactScope");
                            view.ViewFields.Add("ImpactEvidence");
                            view.ViewFields.Add("ImpactConsequences");
                            view.ViewFields.Add("ProposedIdea");
                            view.ViewFields.Add("Participate");
                            view.ViewFields.Add("SelfEvaluation");
                            view.ViewFields.Add("Generalizability");
                            view.ViewFields.Add("AdditionalIdea");
                            view.Update();
                        }
                        else
                        {
                            list = web.Lists[listName];
                        }

                        // Add new item
                        SPListItem item = list.Items.Add();
                        item["Title"] = txtName.Text;
                        item["Department"] = txtDepartment.Text;
                        item["Workplace"] = txtWorkplace.Text;
                        item["EntryType"] = rbType.SelectedValue;
                        item["Description"] = txtDescription.Text;
                        item["ImpactScope"] = rbImpactLevel.SelectedValue;
                        item["ImpactEvidence"] = txtImpactEvidence.Text;

                        var selectedImpacts = string.Join("; ", cbImpact.Items.Cast<ListItem>()
                            .Where(i => i.Selected).Select(i => i.Text));

                        //if (!string.IsNullOrWhiteSpace(txtOtherImpact.Text))
                        //    selectedImpacts += "; " + txtOtherImpact.Text;

                        item["ImpactConsequences"] = selectedImpacts;

                        item["ProposedIdea"] = txtSolutionIdea.Text;
                        item["Conditions"] = txtConditions.Text;


                        //item["Participate"] = rbParticipation.SelectedValue;
                        item["SelfEvaluation"] = rbImpactLevelSelf.SelectedValue;
                        item["Generalizability"] = rbApplicability.SelectedValue;
                        //item["AdditionalIdea"] = txtAdditionalIdea.Text;

                        var selectedcbImpact = string.Join("; ", cbImpactIfIgnored.Items.Cast<ListItem>()
                            .Where(i => i.Selected).Select(i => i.Text));

                        if (!string.IsNullOrWhiteSpace(txtOtherImpact.Text))
                            selectedcbImpact += "; " + txtOtherImpact.Text;
                        item["ChallengeImpact"] = selectedcbImpact;


                        item["Email"] = txtEmail.Text;
                        item["PhoneNo"] = txtMobileNo.Text;

                        item.Update();
                        web.AllowUnsafeUpdates = false;

                        pnlData.Visible = false;
                        lblSuccessMessage.Text = SPFactory.GetPNUresResource("SavedSuccessfully");
                        lblSuccessMessage.Visible = true;  // Make the success message visible
                        lblException.Visible = false;
                        btnSubmit.Visible = false;


                        var smtpSettings = PortalHelper.GetSPSettingList(new string[] { "NoraNabtakerFromEmail" });


                        if (smtpSettings != null)
                        {
                            if (Convert.ToString(smtpSettings["NoraNabtakerFromEmail"]) != "")
                            {
                                var toMail = Convert.ToString(smtpSettings["NoraNabtakerFromEmail"]);
                                //var toMail = "aesharaf@pnu.edu.sa";

                                List<string> ccMail = new List<string>();
                                //ccMail.Add("aesharaf@pnu.edu.sa");


                                var subjectMail = $"نموذج جمع التحديات / الفرص – منصة ابتكار";
                                StringBuilder bodyMail = new StringBuilder();

                                bodyMail.Append("<html lang='ar'><body style='direction: rtl; font-family: Tahoma, Arial, sans-serif;'>");
                                bodyMail.Append("<h3>تم استلام طلب جديد جمع التحديات / الفرص – منصة ابتكار</h3>");
                                bodyMail.Append("<hr/>");

                                // البيانات الأساسية
                                bodyMail.Append($"<p><strong>الاسم:</strong> {txtName.Text}</p>");
                                bodyMail.Append($"<p><strong>البريد الإلكتروني:</strong> {txtEmail.Text}</p>");
                                bodyMail.Append($"<p><strong>رقم الجوال / التحويلة:</strong> {txtMobileNo.Text}</p>");


                                bodyMail.Append($"<p><strong>جهة العمل في الجامعة:</strong> {txtWorkplace.Text}</p>");
                                bodyMail.Append($"<p><strong>القسم/ الإدارة / الوحدة:</strong> {txtDepartment.Text}</p>");


                                // الجزء الأول: وصف التحدي أو الفرصة
                                bodyMail.Append("<h4>وصف التحدي أو الفرصة</h4>");
                                bodyMail.Append($"<p><strong>نوع المشاركة:</strong> {rbType.SelectedItem?.Text}</p>");
                                bodyMail.Append($"<p><strong>وصف التحدي أو الفرصة:</strong> {txtDescription.Text}</p>");
                                bodyMail.Append($"<p><strong> نطاق التحدي أو الفرصة:</strong> {rbImpactLevel.SelectedItem?.Text}</p>");
                                bodyMail.Append($"<p><strong> الدليل او المؤشر على وجود اثر سلبي لهذا التحدي او الفرصه:</strong> {txtImpactEvidence.Text}</p>");

                                // التأثيرات المحددة
                                 selectedImpacts = string.Join("; ", cbImpactIfIgnored.Items.Cast<ListItem>()
                                    .Where(i => i.Selected).Select(i => i.Text));
                                //if (!string.IsNullOrWhiteSpace(txtOtherImpact.Text))
                                //    selectedImpacts += $"; {txtOtherImpact.Text}";

                                bodyMail.Append($"<p><strong>ارتباط التحدي بأهداف الجامعة الاستراتيجية:</strong> {selectedImpacts}</p>");

                                // الجزء الثاني: الحلول والمقترحات
                                bodyMail.Append("<h4>الحلول والمقترحات</h4>");
                                bodyMail.Append($"<p><strong>متطلبات حل التحدي؟:</strong> {txtSolutionIdea.Text}</p>");
                                bodyMail.Append($"<p><strong>الشروط والمعايير التي يجب مراعاتها عند تصميم الحل المقترح:</strong> {txtConditions.Text}</p>");

                                //bodyMail.Append($"<p><strong>الاستعداد للمشاركة:</strong> {rbParticipation.SelectedItem?.Text}</p>");

                                // التقييم الذاتي (اختياري)
                                bodyMail.Append("<h4>التقييم الذاتي</h4>");
                                bodyMail.Append($"<p><strong>درجة التأثير:</strong> {rbImpactLevelSelf.SelectedItem?.Text}</p>");
                                bodyMail.Append($"<p><strong>قابلية التعميم:</strong> {rbApplicability.SelectedItem?.Text}</p>");

                                // مقترحات إضافية
                                bodyMail.Append("<h4>الأثر المتوقع</h4>");
                                bodyMail.Append($"<p>{selectedcbImpact}</p>");



                                //// مقترحات إضافية
                                //bodyMail.Append("<h4>مقترحات إضافية</h4>");
                                //bodyMail.Append($"<p>{txtAdditionalIdea.Text}</p>");

                                bodyMail.Append("<br/><p>مع الشكر،</p>");
                                bodyMail.Append("</body></html>");


                                EmailUtility.SendEmail(toMail, ccMail, subjectMail, bodyMail.ToString());

                            }


                        }

                    }

                }



            });

            
        }
    }
}
