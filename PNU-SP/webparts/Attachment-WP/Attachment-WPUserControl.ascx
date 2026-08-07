<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Attachment-WPUserControl.ascx.cs" Inherits="PNU_SP.webparts.Attachment_WP.Attachment_WPUserControl" %>

<div class="row">
   
       <asp:Repeater ID="rep" runat="server"  OnItemCommand="rep_ItemCommand" >
           <ItemTemplate>
                <div class="col">
                    
                    <asp:Label ID="Label1" runat="server" Text='<%#Eval("ItemId")%>'></asp:Label>
                </div>
                <div class="col">
                    
                    <asp:Label ID="lbl" runat="server" Text='<%#Eval("Title")%>'></asp:Label>
                </div>
              
                 <div class="col">
                    <asp:HyperLink ID="HL"  runat="server" Target="_blank" NavigateUrl='<%#Eval("FileUrl")%>' Text="openFile"></asp:HyperLink>
                </div>

                <div class="col">
                    
                     <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("ItemId") %>'
             CommandName="download">Download</asp:LinkButton>
                </div>
           </ItemTemplate>
       </asp:Repeater>
   
   
</div>