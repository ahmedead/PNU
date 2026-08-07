<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Controls/Common/Captcha.ascx" TagPrefix="uc1" TagName="Captcha" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNoraNabtaker.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Forms.ucNoraNabtaker" %>



<style>
.col-md-12 {
    padding: 15px;
}
</style>
<div class="the-message p-2">
    <div class="mt-3 px-md-5 px-0">


        <asp:Label ID="lblSuccessMessage" runat="server" ForeColor="Green" Font-Size="Large" Visible="false" />
        <asp:Label ID="lblException" runat="server" ForeColor="Red" Font-Size="Large" Visible="false" />

        <asp:Panel ID="pnlData" runat="server">

            <section class=" pt-5 mt-5 mb-4">
                <div class="container">
                    <div class="row justify-content-center">


                        <p style="text-align: center;">
                            مبادرة لنورة نبتكر وُجدت لتكون بوابتك لطرح التحديات التي تواجه مجتمعنا الجامعي، لنحولّها معاً إلى فرص ابتكار تدم رؤية 2030.
                            <br>
                            <br>
                        </p>



                        <div class="col-lg-8 mb-4 mb-lg-5 news-form">
                            <div class="card mb-4 p-5 shadow border-0 rounded-4 ">
                                <div class="row">
                                    <div class="col-md-12 text-start">
                                        <h4 class="card-title mb-4 fw-bold">نموذج جمع التحديات / الفرص – منصة ابتكار     
                                        </h4>
                                    </div>





                                    <div class="form-horizontal">

                                        <!-- البيانات الأساسية -->
                                        <h3>البيانات الأساسية</h3>

                                        <!-- الاسم -->
                                        <div class="col-md-12">
                                            <label for="txtName">الاسم:</label>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                                ErrorMessage="الاسم مطلوب" ValidationGroup="AddRequest" CssClass="text-danger" Display="Dynamic" />
                                        </div>

                                        <div class="col-md-12">

                                            <label for="txtEmail">البريد الإلكتروني:</label>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>



                                        </div>

                                        <div class="col-md-12">

                                            <label for="txtMobileNo">رقم الجوال / التحويلة:</label>
                                            <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control"></asp:TextBox>


                                        </div>

                                        <!-- جهة العمل / الكلية -->
                                        <div class="col-md-12">
                                            <label for="txtWorkplace">جهة العمل في الجامعة:</label>
                                            <asp:TextBox ID="txtWorkplace" runat="server" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="rfvWorkplace" runat="server" ControlToValidate="txtWorkplace"
                                                ErrorMessage="جهة العمل في الجامعة مطلوبة" ValidationGroup="AddRequest" CssClass="text-danger" Display="Dynamic" />
                                        </div>

                                        <!-- القسم / الإدارة -->
                                        <div class="col-md-12">
                                            <label for="txtDepartment">القسم / الإدارة / الوحدة:</label>
                                            <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ControlToValidate="txtDepartment"
                                                ErrorMessage="القسم / الإدارة / الوحدة مطلوب" ValidationGroup="AddRequest" CssClass="text-danger" Display="Dynamic" />
                                        </div>

                                        <!-- الجزء الأول: وصف التحدي أو الفرصة -->
                                        <h3>الجزء الأول: وصف التحدي أو الفرصة</h3>

                                        <!-- نوع المشاركة -->
                                        <div class="col-md-12">
                                            <label>هل ما تود مشاركته هو:</label><br />
                                            <asp:RadioButtonList ID="rbType" runat="server" RepeatDirection="Vertical" ValidationGroup="AddRequest">
                                                <asp:ListItem Text="تحدٍ قائم" Value="تحدٍ قائم" />
                                                <asp:ListItem Text="فرصة قابلة للاستثمار" Value="فرصة قابلة للاستثمار" />
                                            </asp:RadioButtonList>
                                            <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="rbType" InitialValue=""
                                                ErrorMessage="يرجى اختيار نوع المشاركة" ValidationGroup="AddRequest" CssClass="text-danger" Display="Dynamic" />
                                        </div>

                                        <!-- شرح التحدي أو الفرصة -->
                                        <div class="col-md-12">
                                            <label for="txtDescription">اشرح التحدي أو الفرصة باختصار:</label><br />
                                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="4" MaxLength="300" />
                                            <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                                ErrorMessage="يرجى كتابة وصف للتحدي أو الفرصة" ValidationGroup="AddRequest" CssClass="text-danger" Display="Dynamic" />
                                        </div>

                                        <!-- نطاق التأثير -->
                                        <div class="col-md-12">
                                            <label>ماهو نطاق التحدي أو الفرصة؟</label><br />
                                            <asp:RadioButtonList ID="rbImpactLevel" runat="server" ValidationGroup="AddRequest">
                                                <asp:ListItem Text="على مستوى مجموعة أفراد" Value="على مستوى مجموعة أفراد" />
                                                <asp:ListItem Text="على مستوى القسم فقط" Value="على مستوى القسم فقط" />
                                                <asp:ListItem Text="على مستوى الإدارة / الكلية" Value="على مستوى الإدارة / الكلية" />
                                                <asp:ListItem Text="على مستوى مستشفى الملك عبدالله الجامعي" Value="على مستوى مستشفى الملك عبدالله الجامعي" />
                                                <asp:ListItem Text="على مستوى الجامعة ككل" Value="على مستوى الجامعة ككل" />
                                            </asp:RadioButtonList>
                                            <asp:RequiredFieldValidator ID="rfvImpactLevel" runat="server" ControlToValidate="rbImpactLevel"
                                                ErrorMessage="يرجى تحديد نطاق التأثير" ValidationGroup="AddRequest" CssClass="text-danger" Display="Dynamic" />
                                        </div>

                                        <!-- الدليل على التأثير -->
                                        <div class="col-md-12">
                                            <label for="txtImpactEvidence">ماهو الدليل او المؤشر على وجود اثر سلبي لهذا التحدي او الفرصة؟</label><br />
                                            <asp:TextBox ID="txtImpactEvidence" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="4" />
                                        </div>

                                        <%--<!-- أثر التحدي أو الفرصة -->
                                        <div class="col-md-12">
                                            <label>ما هو أثر التحدي أو الفرصة إذا تم تجاهله أو استثماره؟</label><br />
                                            <asp:CheckBoxList ID="cbImpactIfIgnored" runat="server" CssClass="form-check" RepeatDirection="Vertical">
                                                <asp:ListItem Text="يؤثر على جودة الأداء" />
                                                <asp:ListItem Text="يؤثر على رضا المستفيد" />
                                                <asp:ListItem Text="يؤثر على الكفاءة التشغيلية" />
                                                <asp:ListItem Text="يؤثر على فرص التحسين أو الابتكار" />
                                                <asp:ListItem Text="يؤثر على سمعة الجامعة أو مكانتها" />
                                                <asp:ListItem Text="غير ذلك" />
                                            </asp:CheckBoxList>
                                        </div>--%>

                                        <!-- أثر التحدي أو الفرصة -->
                                        <div class="col-md-12">
                                            <label>ارتباط التحدي بأهداف الجامعة الاستراتيجية ( اتاحة اكثر من خيار) </label>
                                            <br />
                                            <asp:CheckBoxList ID="cbImpact" runat="server" CssClass="form-check" RepeatDirection="Vertical">
                                                <asp:ListItem Text="1- كفاءات منافسة في الاقتصاد الوطني" />
                                                <asp:ListItem Text="2- برامج أكاديمية متجددة تستشرف المستقبل " />
                                                <asp:ListItem Text="3- ريادة المرأة في مسيرة التنمية الوطنية" />
                                                <asp:ListItem Text="4- منظومة البحث والابتكار وريادة الأعمال" />
                                                <asp:ListItem Text="5- قيادة التأثير المعرفي والمجتمعي" />
                                                <asp:ListItem Text="6- ممكنات مؤسسية تدعم التميز والاستدامة والاستقرار المالي" />
                                                <asp:ListItem Text="7- حياة جامعية داعمة للصحة والرفاهية" />
                                            </asp:CheckBoxList>
                                        </div>


                                        <%-- <!-- شرح "غير ذلك" -->
                                        <div class="col-md-12">
                                            <asp:TextBox ID="txtOtherImpact" runat="server" CssClass="form-control" Placeholder="يرجى التوضيح (غير ذلك)" />
                                        </div>--%>

                                        <!-- الجزء الثاني: الحلول والمقترحات -->
                                        <h3>الجزء الثاني: الحلول والمقترحات</h3>
                                        
                                        <!-- الفكرة المقترحة -->
                                        <div class="col-md-12">
                                            <label for="txtSolutionIdea">متطلبات حل التحدي؟.</label><br />
                                            <asp:TextBox ID="txtSolutionIdea" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="4" />
                                        </div>

                                        <div class="col-md-12">
                                            <label for="txtSolutionIdea">الشروط والمعايير التي يجب مراعاتها عند تصميم الحل المقترح</label><br />
                                            <asp:TextBox ID="txtConditions" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="4" />
                                        </div>


                                        <%--<!-- المشاركة في التنفيذ -->
                                        <div class="col-md-12">
                                            <label>هل لديك استعداد للمشاركة في تطوير أو تنفيذ الحل؟</label><br />
                                            <asp:RadioButtonList ID="rbParticipation" runat="server">
                                                <asp:ListItem Text="نعم" />
                                                <asp:ListItem Text="لا" />
                                                <asp:ListItem Text="حسب التفرغ / المهام" />
                                            </asp:RadioButtonList>
                                        </div>--%>

                                        <!-- الجزء الثالث: التقييم الذاتي -->
                                        <h3>الجزء الثالث: التقييم الذاتي (اختياري)</h3>

                                        <!-- درجة التأثير -->
                                        <div class="col-md-12">
                                            <label>برأيك، كم يمكن أن يكون لهذا المقترح من تأثير؟</label><br />
                                            <asp:RadioButtonList ID="rbImpactLevelSelf" runat="server">
                                                <asp:ListItem Text="منخفض (تحسين محلي بسيط)" />
                                                <asp:ListItem Text="متوسط (يُحدث فرقًا على مستوى إدارة أو كلية)" />
                                                <asp:ListItem Text="عالٍ (تحوّل ملموس على مستوى الجامعة)" />
                                            </asp:RadioButtonList>
                                        </div>

                                        <!-- قابلية التعميم -->
                                        <div class="col-md-12">
                                            <label>هل تعتقد أن هذا المقترح يمكن تطبيقه خارج نطاق الجهة التي تنتمي إليها؟</label><br />
                                            <asp:RadioButtonList ID="rbApplicability" runat="server">
                                                <asp:ListItem Text="نعم، ويمكن تعميمه" />
                                                <asp:ListItem Text="لا، الحل خاص" />
                                                <asp:ListItem Text="محتمل، حسب ظروف الجهة الأخرى" />
                                            </asp:RadioButtonList>
                                        </div>



                                        <!-- أثر التحدي أو الفرصة -->
                                        <div class="col-md-12">
                                            <label>الأثر المتوقع (اختر ما ينطبق- اتاحة اكثر من خيار)</label><br />
                                            <asp:CheckBoxList ID="cbImpactIfIgnored" runat="server" CssClass="form-check" RepeatDirection="Vertical">
                                                <asp:ListItem Text="تحسين جودة الحياة" />
                                                <asp:ListItem Text="رفع الكفاءة التشغيلية والإدارية" />
                                                <asp:ListItem Text="تمكين التحول الرقمي والذكاء الاصطناعي" />
                                                <asp:ListItem Text="تعزيز الصحة العامة وجودة الرعاية الصحية" />
                                                <asp:ListItem Text="تقليل الأثر البيئي وتحقيق الاستدامة" />
                                                <asp:ListItem Text="تعزيز التمكين الاقتصادي وريادة الأعمال" />
                                                <asp:ListItem Text="رفع مستوى التعليم والتدريب وبناء القدرات" />
                                                <asp:ListItem Text="تطوير البنية التحتية الذكية والمستدامة" />
                                                <asp:ListItem Text=" تحقيق وفر مالي أو زيادة العوائد الاقتصادية" />
                                                <asp:ListItem Text="دعم سمعة الجامعة وريادتها محليًا ودوليًا" />
                                                <asp:ListItem Text="تحقيق مستهدفات رؤية المملكة 2030" />
                                                <asp:ListItem Text="غير ذلك" />
                                            </asp:CheckBoxList>
                                        </div>



                                        <!-- شرح "غير ذلك" -->
                                        <div class="col-md-12">
                                            <asp:TextBox ID="txtOtherImpact" runat="server" CssClass="form-control" Placeholder="يرجى التوضيح (غير ذلك)" />
                                        </div>




                                        <div class="col-md-12">
                                            <div class="mb-4">

                                                <label>أدخل الرمز الظاهر في الصورة للتحقق</label><br />
                                                <uc1:Captcha runat="server" ValidationGroup="AddRequest" id="CaptchaControl" />
                                            </div>
                                        </div>
                                        <!-- زر الإرسال -->
                                        <div class="mt-3">
                                            <asp:Button ID="btnSubmit" runat="server"
                                                Text="إرسال"
                                                CssClass="btn btn-primary"
                                                ValidationGroup="AddRequest"
                                                OnClick="btnSubmit_Click" />
                                        </div>

                                    </div>

                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </section>

        </asp:Panel>







    </div>
</div>

