<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDestSections.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.GeneralDest.ucDestSections" %>
<section id="agency-departments" class="pnu-section-anchor mb-5">
    <div class="accordion accordion-flush" id="agency-departments-accordion">
        <asp:Repeater ID="rptItems" runat="server"
            OnItemDataBound="rptItems_ItemDataBound">
            <ItemTemplate>
                <div class="accordion-item">
                    <h3 class="accordion-header"
                        id='agency-department-heading-<%# Container.ItemIndex + 1 %>'>
                        <button
                            class='accordion-button <%# Container.ItemIndex == 0 ? "" : "collapsed" %>'
                            type="button"
                            data-bs-toggle="collapse"
                            data-bs-target='#agency-department-collapse-<%# Container.ItemIndex + 1 %>'
                            aria-expanded='<%# Container.ItemIndex == 0 ? "true" : "false" %>'
                            aria-controls='agency-department-collapse-<%# Container.ItemIndex + 1 %>'>
                            <%# Eval("Title") %>
                        </button>
                    </h3>
                    <div id='agency-department-collapse-<%# Container.ItemIndex + 1 %>'
                         class='accordion-collapse collapse <%# Container.ItemIndex == 0 ? "show" : "" %>'
                         aria-labelledby='agency-department-heading-<%# Container.ItemIndex + 1 %>'
                         data-bs-parent="#agency-departments-accordion">
                        <div class="accordion-body">
                            <asp:Repeater ID="rptSections" runat="server">
                                <ItemTemplate>
                                    <h4 runat="server"
                                        class="h5 mb-3"
                                        visible='<%# !string.IsNullOrWhiteSpace(Convert.ToString(Eval("Title"))) %>'>
                                        <%# Eval("Title") %>
                                    </h4>
                                    <%# CleanRichText(Convert.ToString(Eval("Content"))) %>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</section>