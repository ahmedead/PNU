<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFAQ.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.ucFAQ" %>


<main id="main-content" class="dga-main-body" tabindex="-1">
    <section class="py-5" data-aos="fade-up" aria-labelledby="reasons-section-title">
        <div class="container">

            <%-- ========== TABS HEADER ========== --%>
            <ul class="nav nav-tabs nav-underline nav-flush w-100 mb-4" id="faq-tabs" role="tablist">
                <asp:Repeater ID="rptCategoriesTabs" runat="server">
                    <ItemTemplate>
                        <li class="nav-item" role="presentation">
                            <button class='<%# GetTabButtonClass((int)Container.ItemIndex) %>'
                                id='<%# "faq-tabs-tab-" + Container.ItemIndex %>'
                                data-bs-toggle="tab"
                                data-bs-target='<%# "#faq-tabs-pane-" + Container.ItemIndex %>'
                                type="button"
                                role="tab"
                                aria-controls='<%# "faq-tabs-pane-" + Container.ItemIndex %>'
                                aria-selected='<%# Container.ItemIndex == 0 ? "true" : "false" %>'>
                                <i class='<%# "hgi hgi-stroke " + Eval("IconClass") + " fs-5 fw-light" %>' aria-hidden="true"></i>
                                <span><%# Eval("Title") %></span>
                            </button>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>

            <%-- ========== TABS CONTENT ========== --%>
            <div class="tab-content pt-2">
                <asp:Repeater ID="rptCategoriesPanes" runat="server" OnItemDataBound="rptCategoriesPanes_ItemDataBound">
                    <ItemTemplate>
                        <div class='<%# GetTabPaneClass((int)Container.ItemIndex) %>'
                             id='<%# "faq-tabs-pane-" + Container.ItemIndex %>'
                             role="tabpanel"
                             aria-labelledby='<%# "faq-tabs-tab-" + Container.ItemIndex %>'>

                            <div class="accordion accordion-flush" id='<%# "faqAccordion_" + Eval("ID") %>'>
                                <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="accordion-item">
                                            <p class="accordion-header" id='<%# "faqHeading_" + Eval("ID") %>'>
                                                <button aria-controls='<%# "faqCollapse_" + Eval("ID") %>'
                                                    aria-expanded="false"
                                                    class="accordion-button collapsed"
                                                    data-bs-target='<%# "#faqCollapse_" + Eval("ID") %>'
                                                    data-bs-toggle="collapse"
                                                    type="button">
                                                    <%# Eval("QuestionTextAr") %>
                                                </button>
                                            </p>
                                            <div class="accordion-collapse collapse"
                                                 id='<%# "faqCollapse_" + Eval("ID") %>'
                                                 aria-labelledby='<%# "faqHeading_" + Eval("ID") %>'>
                                                <div class="accordion-body">
                                                    <%-- Answer items are rendered in code-behind into this literal,
                                                         grouping consecutive bullets into a single <ul>. --%>
                                                    <asp:Literal ID="litAnswer" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </div>
    </section>
</main>
