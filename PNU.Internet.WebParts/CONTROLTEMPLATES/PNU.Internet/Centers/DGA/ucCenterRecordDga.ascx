<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCenterRecordDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA.ucCenterRecordDga" %>

<section id="center-smoc-record" class="pnu-section-anchor mb-5">
    <h2 class="mb-3">
        <asp:Literal ID="litSmocTitle" runat="server" />
    </h2>
    <div class="accordion accordion-flush" id="center-smoc-record-accordion">
        <asp:Repeater ID="rptSmocRecord" runat="server">
            <ItemTemplate>
                <div class="accordion-item">
                    <h3 class="accordion-header" id='<%# "center-smoc-heading-" + Container.ItemIndex %>'>
                        <button class='<%# "accordion-button" + (Container.ItemIndex == 0 ? "" : " collapsed") %>' 
                                type="button" data-bs-toggle="collapse"
                                data-bs-target='<%# "#center-smoc-collapse-" + Container.ItemIndex %>' 
                                aria-expanded='<%# Container.ItemIndex == 0 ? "true" : "false" %>' 
                                aria-controls='<%# "center-smoc-collapse-" + Container.ItemIndex %>'>
                            <%# Eval("Title") %>
                        </button>
                    </h3>
                    <div id='<%# "center-smoc-collapse-" + Container.ItemIndex %>' 
                         class='<%# "accordion-collapse collapse" + (Container.ItemIndex == 0 ? " show" : "") %>'
                         aria-labelledby='<%# "center-smoc-heading-" + Container.ItemIndex %>' 
                         data-bs-parent="#center-smoc-record-accordion">
                        <div class="accordion-body">
                            <div><%# Eval("HtmlContent") %></div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</section>
