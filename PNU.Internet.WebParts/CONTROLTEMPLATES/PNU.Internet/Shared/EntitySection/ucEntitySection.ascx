<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEntitySection.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.EntitySection.ucEntitySection" %>

<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<asp:Panel ID="pnlSection" runat="server">
    <section id="deanship-beneficiary-tracks" class="pnu-section-anchor mb-5">
        <h2 class="mb-3">
            <%= SPContext.Current.ListItem["Title"] %>
        </h2>
        <div class="accordion accordion-flush" id="deanship-beneficiary-tracks-accordion">
            <asp:Repeater ID="rptTracks" runat="server" OnItemDataBound="rptTracks_ItemDataBound">
                <ItemTemplate>
                    <div class="accordion-item">
                        <h3 class="accordion-header" id='<%# "deanship-beneficiary-tracks-accordion-heading-" + Eval("Index") %>'>
                            <button class='<%# Convert.ToBoolean(Eval("IsExpanded")) ? "accordion-button" : "accordion-button collapsed" %>'
                                    type="button"
                                    data-bs-toggle="collapse"
                                    data-bs-target='<%# "#deanship-beneficiary-tracks-accordion-collapse-" + Eval("Index") %>'
                                    aria-expanded='<%# Convert.ToBoolean(Eval("IsExpanded")) ? "true" : "false" %>'
                                    aria-controls='<%# "deanship-beneficiary-tracks-accordion-collapse-" + Eval("Index") %>'>
                                <%# Eval("Title") %>
                            </button>
                        </h3>
                        <div class='<%# Convert.ToBoolean(Eval("IsExpanded")) ? "accordion-collapse collapse show" : "accordion-collapse collapse" %>'
                             id='<%# "deanship-beneficiary-tracks-accordion-collapse-" + Eval("Index") %>'
                             aria-labelledby='<%# "deanship-beneficiary-tracks-accordion-heading-" + Eval("Index") %>'
                             data-bs-parent="#deanship-beneficiary-tracks-accordion">
                            <div class="accordion-body">
                                <asp:PlaceHolder ID="phDescription" runat="server" Visible='<%# !string.IsNullOrEmpty(Eval("Description") as string) %>'>
                                    <p><%# Eval("Description") %></p>
                                </asp:PlaceHolder>

                                <asp:PlaceHolder ID="phBullets" runat="server">
                                    <asp:Literal ID="litListOpenTag" runat="server" />
                                    <asp:Repeater ID="rptBullets" runat="server">
                                        <ItemTemplate>
                                            <li><%# Eval("Text") %></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:Literal ID="litListCloseTag" runat="server" />
                                </asp:PlaceHolder>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </section>
</asp:Panel>
