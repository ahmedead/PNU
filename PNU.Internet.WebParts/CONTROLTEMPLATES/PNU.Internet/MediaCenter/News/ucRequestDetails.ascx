<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucRequestDetails.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.ucRequestDetails" %>



<section class=" pt-5 mt-5 mb-4">
    <div class="container">
        <div class="row justify-content-center">
            <h2 id="hMsg" runat="server" style="color:red" class="d-none">ليس لديك صلاحية الوصول لهذه الشاشة</h2>

            <div class="col-lg-7 mb-4 mb-lg-5 news-form" id="dvForm" runat="server">
                <div class="card mb-4 p-5 shadow border-0 rounded-4 ">
                    <div class="row">
                       <div class="col-md-12">
                            <div class="mb-4">

                                <label>نشر علي الأخبار الرئيسية</label>
                                <ul class="list-group">
                                   <asp:RadioButtonList ID="opt_IsHome" runat="server" RepeatDirection="Horizontal">
                                          <asp:ListItem Text="نعم" Value="True" />
                                          <asp:ListItem Text="لا" Value="False"  />
                                    </asp:RadioButtonList>
                                
                                </ul>
                            </div>
                        </div>
                    
                        <div class="col-md-12">
                            <div class="mb-4">

                                <label>التاريخ</label>
                                <asp:TextBox ID="txtDate" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>


                            </div>

                        </div>
                        <div class="col-md-12">
                            <div class="mb-4">

                                <label>الجهة الطالبة</label>
                                <asp:Label ID="lblFacultyName" runat="server"></asp:Label>

                            </div>

                        </div>


                        <div class="col-md-12">
                            <div class=" mb-4">
                                <label>عنوان الخبر</label>
                                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class=" mb-4">
                                <label>عنوان الخبر انجليزي</label>
                                <asp:TextBox ID="txtTitleEn" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class=" mb-4">
                                <label>صورة الخبر</label>
                                <asp:Image ID="Image1" runat="server" CssClass="img-fluid" />

                            </div>
                        </div>


                        <div class="col-md-12 d-none">
                            <div class="mb-4">
                                <label>Summary</label>
                                <asp:TextBox ID="txtSummary" TextMode="MultiLine" runat="server" Rows="3" CssClass="form-control"
                                    Enabled="false">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-12 d-none">
                            <div class="mb-4">
                                <label>Summary English</label>
                                <asp:TextBox ID="txtSummaryEn" TextMode="MultiLine" runat="server" Rows="3" CssClass="form-control"
                                    Enabled="false">
                                </asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="mb-4">
                                <label>محتوى الخبر </label>
                                <asp:TextBox ID="txtDetails" TextMode="MultiLine" runat="server" Rows="30" CssClass="form-control"
                                    Enabled="false">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="mb-4">
                                <label>محتوى الخبرانجليزي</label>
                                <asp:TextBox ID="txtDetailsEn" TextMode="MultiLine" runat="server" Rows="30" CssClass="form-control"
                                    Enabled="false">
                                </asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="form-floating mb-4">
                                <asp:Label ID="Label7" runat="server" Text="Video URL"></asp:Label>
                                <asp:TextBox ID="txtVideoURL" runat="server" class="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-12" id="divAdditionalFileURL" runat="server" visible="false">
                            <div class=" mb-4 ">
                                <div class="input-group mb-4 ">
                                    <asp:Label ID="lblAdditionalFileURL" runat="server" Text="ملفات الجهة : "></asp:Label>
                                    <asp:HyperLink ID="AdditionalFileURLLink" runat="server" Target="_blank"></asp:HyperLink>
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
                <div class="row " id="dv_userAction" runat="server">
                        <div class="col-md-12">
                            <div class="mb-4 form-group">
                                <label>التعليق  </label>
                                <asp:TextBox ID="txtComments" Rows="3" TextMode="MultiLine" Height="300"  ValidationGroup="EditRequest" placeholder="كتابة التعليق" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator
                                    ID="rfvFaculties"
                                    runat="server"
                                    Enabled="False"
                                    ErrorMessage="التعليق مطلوب"
                                    ValidationGroup=""
                                    ControlToValidate="txtComments"
                                    CssClass="required"
                                    Style="color: red"
                                    Display="dynamic" />
                            </div>
                        </div>
                      
                    <div class="col-md-12" id="divSubmit" runat="server">
                            <div class="mb-4">
                                <div class="form-group">
                                    <%-- UseSubmitBehavior="false" : يمنع تنفيذ الزر تلقائياً عند الضغط على Enter في أي حقل بالصفحة (سبب انتقال الخبر للمرحلة التالية دون ضغط "موافقة") --%>
                                    <asp:Button ID="btnApprove" runat="server" Text="موافقة" CssClass="btn btn-primary mx-2" UseSubmitBehavior="false" OnClick="btnApprove_Click"
                                        OnClientClick="if (!pnuConfirmAction(this, 'هل أنت متأكد من الموافقة على الخبر والانتقال إلى المرحلة التالية؟')) return false;" />
                                    <asp:Button ID="btnReject" ValidationGroup="RejectRequest" runat="server" Text="رفض" CssClass="btn btn-danger mx-2" UseSubmitBehavior="false" OnClick="btnReject_Click"
                                        OnClientClick="if (!pnuConfirmAction(this, 'هل أنت متأكد من رفض الخبر؟')) return false;" />
                                    <asp:Button ID="brnEdit" runat="server" Text="تعديل" CssClass="btn alert-info" UseSubmitBehavior="false" OnClick="brnEdit_Click" />
                                    <asp:Button ID="btnAskForEdit" Visible="false" runat="server"  ValidationGroup="EditRequest" Text="طلب تعديل من مقدم الطلب" CssClass="btn alert-info" UseSubmitBehavior="false" OnClick="btnAskForEdit_Click"
                                        OnClientClick="if (!pnuConfirmAction(this, 'هل أنت متأكد من إرسال طلب تعديل إلى مقدم الطلب؟')) return false;" />


                                </div>
                            </div>
                        </div>

                        <%-- إرجاع الخبر إلى مرحلة سابقة --%>
                        <div class="col-md-12" id="divReturnStep" runat="server" visible="false">
                            <div class="mb-4 p-3 rounded-3" style="background-color: #FFFBEB; border: 1px solid #D97706;">
                                <label for="ddlReturnStep" class="form-label fw-bold">إرجاع الخبر إلى مرحلة</label>
                                <div class="d-flex flex-wrap gap-2 align-items-start">
                                    <asp:DropDownList ID="ddlReturnStep" runat="server" CssClass="form-select w-auto"></asp:DropDownList>
                                    <asp:Button ID="btnReturnToStep" runat="server" Text="إرجاع الخبر" CssClass="btn btn-warning" ValidationGroup="ReturnRequest"
                                        UseSubmitBehavior="false" OnClick="btnReturnToStep_Click"
                                        OnClientClick="if (!pnuConfirmReturn(this)) return false;" />
                                </div>
                                <asp:RequiredFieldValidator
                                    ID="rfvReturnStep"
                                    runat="server"
                                    Enabled="False"
                                    ErrorMessage="يرجى اختيار المرحلة المراد إرجاع الخبر إليها"
                                    ValidationGroup="ReturnRequest"
                                    ControlToValidate="ddlReturnStep"
                                    InitialValue=""
                                    CssClass="required"
                                    Style="color: red"
                                    Display="Dynamic" />
                                <small class="d-block mt-2 text-muted">سيتم إعادة الخبر إلى المرحلة المختارة مع إرسال إشعار بريدي بسبب الإرجاع (يُكتب في خانة التعليق).</small>
                            </div>
                        </div>

                    </div>

       
            </div>
        </div>

           

    </div>

      

</section>

<%-- نافذة تأكيد الإجراء (Bootstrap 5) --%>
<div class="modal fade" id="pnuConfirmModal" tabindex="-1" aria-labelledby="pnuConfirmModalTitle" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content rounded-4">
            <div class="modal-header">
                <h5 class="modal-title" id="pnuConfirmModalTitle">تأكيد الإجراء</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="إغلاق"></button>
            </div>
            <div class="modal-body">
                <p id="pnuConfirmModalMsg" class="mb-0"></p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">العودة للصفحة</button>
                <button type="button" id="pnuConfirmModalOk" class="btn btn-primary">تأكيد</button>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    (function () {
        var pnuPendingBtn = null;

        // يُستدعى من OnClientClick — يعرض نافذة التأكيد ويمنع الترحيل حتى يضغط المستخدم "تأكيد"
        window.pnuConfirmAction = function (btn, message) {
            if (btn.getAttribute('data-confirmed') === '1') {
                btn.removeAttribute('data-confirmed');
                return true; // تم التأكيد — أكمل الـ postback
            }
            var modalEl = document.getElementById('pnuConfirmModal');
            if (window.bootstrap && window.bootstrap.Modal && modalEl) {
                pnuPendingBtn = btn;
                document.getElementById('pnuConfirmModalMsg').textContent = message;
                window.bootstrap.Modal.getOrCreateInstance(modalEl).show();
            } else {
                // في حال عدم توفر Bootstrap JS نستخدم confirm الافتراضي
                if (window.confirm(message)) {
                    btn.setAttribute('data-confirmed', '1');
                    btn.click();
                }
            }
            return false;
        };

        // تأكيد الإرجاع مع اسم المرحلة المختارة
        window.pnuConfirmReturn = function (btn) {
            if (btn.getAttribute('data-confirmed') === '1') {
                btn.removeAttribute('data-confirmed');
                return true;
            }
            var ddl = document.getElementById('<%= ddlReturnStep.ClientID %>');
            if (!ddl || !ddl.value) {
                return true; // اترك التحقق (validator) في السيرفر يعرض رسالة اختيار المرحلة
            }
            var stepText = ddl.options[ddl.selectedIndex].text;
            return window.pnuConfirmAction(btn, 'هل أنت متأكد من إرجاع الخبر إلى مرحلة "' + stepText + '"؟');
        };

        var okBtn = document.getElementById('pnuConfirmModalOk');
        if (okBtn) {
            okBtn.addEventListener('click', function () {
                var modalEl = document.getElementById('pnuConfirmModal');
                if (window.bootstrap && window.bootstrap.Modal && modalEl) {
                    window.bootstrap.Modal.getOrCreateInstance(modalEl).hide();
                }
                if (pnuPendingBtn) {
                    pnuPendingBtn.setAttribute('data-confirmed', '1');
                    pnuPendingBtn.click();
                    pnuPendingBtn = null;
                }
            });
        }
    })();
</script>
