<%@ Assembly Name="EserviceList, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7abd535f11cceb57" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchListUserControl.ascx.cs" Inherits="EserviceList.SearchList.SearchListUserControl" %>
<asp:GridView ID="grdcrud" runat="server" AutoGenerateColumns="false">
    
        <Columns>
            <asp:TemplateField HeaderText="ID" Visible="true" HeaderStyle-HorizontalAlign="right" >
                <EditItemTemplate>
                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                </EditItemTemplate>
  <ItemTemplate>
                    <asp:Label ID="lblNId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                </ItemTemplate>

<HeaderStyle HorizontalAlign="right"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Service Name"  HeaderStyle-HorizontalAlign="center" HeaderStyle-Wrap="true">

               
                
                <ItemTemplate>
                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("ARServiceName") %>'></asp:Label>
                </ItemTemplate>

<HeaderStyle HorizontalAlign="right"></HeaderStyle>
            </asp:TemplateField>
                
                
    </Columns>

</asp:GridView>
