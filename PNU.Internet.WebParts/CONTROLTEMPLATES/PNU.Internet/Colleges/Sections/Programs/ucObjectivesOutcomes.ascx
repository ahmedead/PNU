<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucObjectivesOutcomes.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.Programs.ucObjectivesOutcomes" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<asp:Repeater ID="rptMaster" runat="server">
    <ItemTemplate>
        <div class='<%# Container.ItemIndex == 0 ? "tab-pane fade active show" : "tab-pane fade" %>' 
             id='<%# Container.ItemIndex == 0 ? "academic-program-v2-objectives-pane" : (Container.ItemIndex == 1 ? "academic-program-v2-outcomes-pane" : "academic-program-v2-graduate-pane") %>' 
             role="tabpanel" 
             aria-labelledby='<%# Container.ItemIndex == 0 ? "academic-program-v2-objectives-tab" : (Container.ItemIndex == 1 ? "academic-program-v2-outcomes-tab" : "academic-program-v2-graduate-tab") %>' 
             tabindex="0">
            <ol class="mb-0 d-flex flex-column gap-2">
                <asp:Repeater ID="rptObjectivesOutcomes" runat="server" DataSource='<%# Eval("ObjectivesOutcomes") %>'>
                    <ItemTemplate>
                        <li>
                            <%# (Eval("Numbering") != null && !String.IsNullOrEmpty(Eval("Numbering").ToString()) && !Char.IsDigit(Eval("Numbering").ToString()[0])) ? Eval("Numbering") + " — " : "" %><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ol>
        </div>
    </ItemTemplate>
</asp:Repeater>
