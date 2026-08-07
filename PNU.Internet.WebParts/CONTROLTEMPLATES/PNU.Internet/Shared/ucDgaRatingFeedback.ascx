<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDgaRatingFeedback.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.ucDgaRatingFeedback" %>



<dga-rating-feedback>
    <div class="border-top border-2 border-primary py-3 py-lg-4">
        <div class="container">
            <div class="d-flex flex-column gap-4">
                <div class="d-flex flex-column align-items-start align-items-lg-center gap-3 flex-lg-row justify-content-lg-between">
                    <div class="d-flex flex-column gap-3 flex-lg-row align-items-lg-center">
                        <p class="mb-0"><asp:Literal ID="litAvgLine" runat="server"></asp:Literal></p>
                        <div class="d-flex gap-2 align-items-center">
                            <div class="d-flex gap-1">
                                <asp:Literal ID="litAvgStars" runat="server"></asp:Literal>
                            </div>
                            <span class="small text-muted"><asp:Literal ID="litCountLine" runat="server"></asp:Literal></span>
                        </div>
                    </div>
                    <div class="d-flex justify-content-center">
                        <button id="rateServiceBtn" runat="server" type="button" class="btn btn-primary collapsed"
                            data-bs-toggle="collapse" data-bs-target="#ratingFormCollapse"
                            aria-controls="ratingFormCollapse" aria-expanded="false"></button>
                    </div>
                </div>

                <asp:Panel ID="pnlThanks" runat="server" Visible="false" CssClass="alert alert-success mb-0">
                    <asp:Literal ID="litThankYou" runat="server"></asp:Literal>
                </asp:Panel>

                <div id="ratingFormCollapse" class="dga-form collapse mt-4">
                    <div class="d-flex flex-column justify-content-between gap-4">
                        <div>
                            <h6><asp:Literal ID="litTellUs" runat="server"></asp:Literal></h6>
                            <p class="mb-0 small"><asp:Literal ID="litPrivacy" runat="server"></asp:Literal></p>
                        </div>
                        <div class="d-flex flex-column flex-lg-row justify-content-between gap-4">
                            <div>
                                <h6><asp:Literal ID="litHowRate" runat="server"></asp:Literal></h6>
                                <p class="mb-0 small"><asp:Literal ID="litRateScale" runat="server"></asp:Literal></p>
                                <div class="d-flex gap-1 dga-rating-stars-container mt-1" id="dgaRatingStars">
                                    <asp:Literal ID="litFormStars" runat="server"></asp:Literal>
                                </div>
                                <asp:HiddenField ID="hfRatingValue" runat="server" Value="0" />
                            </div>
                            <div>
                                <asp:Label ID="lblRatingComment" runat="server" AssociatedControlID="txtRatingComment" CssClass="form-label"></asp:Label>
                                <div class="form-control-container">
                                    <asp:TextBox ID="txtRatingComment" runat="server" TextMode="MultiLine" Rows="4"
                                        MaxLength="500" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="d-flex gap-3 justify-content-end">
                            <button id="btnCancelRating" runat="server" type="button" class="btn btn-outline-secondary"
                                data-bs-toggle="collapse" data-bs-target="#ratingFormCollapse"
                                aria-controls="ratingFormCollapse"></button>
                            <asp:Button ID="btnSubmitRating" runat="server" CssClass="btn btn-primary"
                                OnClick="btnSubmitRating_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</dga-rating-feedback>

<script type="text/javascript">
    (function () {
        var container = document.getElementById('dgaRatingStars');
        if (!container) return;
        var hidden = document.getElementById('<%= hfRatingValue.ClientID %>');
        var submitBtn = document.getElementById('<%= btnSubmitRating.ClientID %>');
        var stars = container.querySelectorAll('.dga-rating-icon');
        stars.forEach(function (star) {
            star.addEventListener('click', function (ev) {
                ev.preventDefault();
                var val = parseInt(star.getAttribute('data-value'), 10);
                hidden.value = val;
                stars.forEach(function (s) {
                    s.classList.toggle('selected', parseInt(s.getAttribute('data-value'), 10) <= val);
                });
                if (submitBtn) submitBtn.removeAttribute('disabled');
            });
        });
    })();
</script>
