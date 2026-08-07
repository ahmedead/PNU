<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDgaSurveyFeedback.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.ucDgaSurveyFeedback" %>



<dga-survey-feedback>
    <div class="border-top border-2 border-primary py-3 py-lg-4">
        <div class="container">
            <div class="d-flex flex-column gap-4">
                <div class="d-flex flex-column gap-3 flex-lg-row align-items-lg-center justify-content-lg-between gap-lg-1">
                    <div class="d-flex align-items-center gap-3">
                        <p class="mb-0"><asp:Literal ID="litWasHelpful" runat="server"></asp:Literal></p>
                        <button id="btnYes" runat="server" type="button" class="btn btn-primary dga-survey-answer" data-helpful="1"
                            data-bs-toggle="collapse" data-bs-target="#surveyFormCollapse"
                            aria-controls="surveyFormCollapse" aria-expanded="false"></button>
                        <button id="btnNo" runat="server" type="button" class="btn btn-primary px-3 dga-survey-answer" data-helpful="0"
                            data-bs-toggle="collapse" data-bs-target="#surveyFormCollapse"
                            aria-controls="surveyFormCollapse" aria-expanded="false"></button>
                    </div>
                    <p class="mb-0"><asp:Literal ID="litStatsLine" runat="server"></asp:Literal></p>
                </div>

                <asp:Panel ID="pnlThanks" runat="server" Visible="false" CssClass="alert alert-success mb-0">
                    <asp:Literal ID="litThankYou" runat="server"></asp:Literal>
                </asp:Panel>

                <div id="surveyFormCollapse" class="dga-form collapse mt-4">
                    <asp:HiddenField ID="hfIsHelpful" runat="server" Value="" />
                    <div class="d-flex flex-column gap-4">
                        <div class="d-flex flex-column gap-3 flex-lg-row justify-content-lg-between gap-lg-1">
                            <div>
                                <p class="fw-semibold"><asp:Literal ID="litTellUsWhy" runat="server"></asp:Literal></p>
                                <div>
                                    <div class="form-check mb-2">
                                        <asp:CheckBox ID="chkRelevant" runat="server" />
                                    </div>
                                    <div class="form-check mb-2">
                                        <asp:CheckBox ID="chkWellWritten" runat="server" />
                                    </div>
                                    <div class="form-check mb-2">
                                        <asp:CheckBox ID="chkLayout" runat="server" />
                                    </div>
                                    <div class="form-check mb-2">
                                        <asp:CheckBox ID="chkOther" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="lblSurveyComment" runat="server" AssociatedControlID="txtSurveyComment" CssClass="form-label"></asp:Label>
                                <div class="form-control-container">
                                    <asp:TextBox ID="txtSurveyComment" runat="server" TextMode="MultiLine" Rows="4"
                                        MaxLength="500" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="d-flex gap-3 flex-lg-row">
                            <p class="mb-0"><asp:Literal ID="litIAm" runat="server"></asp:Literal><span class="text-danger mx-1">*</span></p>
                            <div class="form-check mb-2">
                                <asp:RadioButton ID="rdMale" runat="server" GroupName="gender" />
                            </div>
                            <div class="form-check mb-2">
                                <asp:RadioButton ID="rdFemale" runat="server" GroupName="gender" />
                            </div>
                            <div class="form-check mb-2">
                                <asp:RadioButton ID="rdNoSay" runat="server" GroupName="gender" />
                            </div>
                        </div>
                        <div class="d-flex gap-3 justify-content-end">
                            <button id="btnCancelSurvey" runat="server" type="button" class="btn btn-outline-secondary"
                                data-bs-toggle="collapse" data-bs-target="#surveyFormCollapse"
                                aria-controls="surveyFormCollapse"></button>
                            <asp:Button ID="btnSubmitSurvey" runat="server" CssClass="btn btn-primary"
                                OnClick="btnSubmitSurvey_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</dga-survey-feedback>

<script type="text/javascript">
    (function () {
        var hidden = document.getElementById('<%= hfIsHelpful.ClientID %>');
        document.querySelectorAll('.dga-survey-answer').forEach(function (btn) {
            btn.addEventListener('click', function () {
                if (hidden) hidden.value = btn.getAttribute('data-helpful');
                var collapseEl = document.getElementById('surveyFormCollapse');
                if (collapseEl && window.bootstrap) {
                    bootstrap.Collapse.getOrCreateInstance(collapseEl).show();
                }
            });
        });
    })();
</script>
