<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucListEditor.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Admin.ucListEditor" %>



<%-- Generic add/edit/delete editor for one content list. Hidden entirely unless
     the caller passed all security checks (see ucListEditor.ascx.cs). --%>
<asp:PlaceHolder ID="phEditor" runat="server">
    <div class="card mb-4">
        <div class="card-body">

            <asp:HiddenField ID="hfMode" runat="server" />
            <asp:HiddenField ID="hfItemId" runat="server" />

            <div class="d-flex justify-content-between align-items-center mb-3">
                <h3 class="h5 mb-0"><asp:Literal ID="ltrListTitle" runat="server" /></h3>
                <span class="badge badge-info">عدد السجلات: <asp:Literal ID="ltrCount" runat="server" /></span>
            </div>

            <asp:PlaceHolder ID="phMessage" runat="server" Visible="false">
                <asp:Panel ID="pnlMessage" runat="server">
                    <asp:Literal ID="ltrMessage" runat="server" />
                </asp:Panel>
            </asp:PlaceHolder>

            <%-- ======================= GRID ======================= --%>
            <asp:PlaceHolder ID="phGrid" runat="server">

                <asp:LinkButton ID="lnkAdd" runat="server" CssClass="btn btn-primary mb-3"
                    OnClick="lnkAdd_Click">
                    <i class="hgi hgi-stroke hgi-add-01 fs-5" aria-hidden="true"></i>
                    إضافة سجل جديد
                </asp:LinkButton>

                <asp:PlaceHolder ID="phEmpty" runat="server" Visible="false">
                    <p class="text-body-secondary mb-0">لا توجد بيانات مضافة في هذه القائمة حتى الآن.</p>
                </asp:PlaceHolder>

                <asp:PlaceHolder ID="phTable" runat="server" Visible="false">
                    <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
                        <table class="table table-striped mb-0 align-middle">
                            <thead>
                                <tr>
                                    <asp:Repeater ID="rptHeaders" runat="server">
                                        <ItemTemplate><th scope="col"><%# Container.DataItem %></th></ItemTemplate>
                                    </asp:Repeater>
                                    <th scope="col">إجراءات</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptRows" runat="server"
                                    OnItemDataBound="rptRows_ItemDataBound"
                                    OnItemCommand="rptRows_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <asp:Repeater ID="rptCells" runat="server">
                                                <ItemTemplate><td><%# Container.DataItem %></td></ItemTemplate>
                                            </asp:Repeater>
                                            <td class="text-nowrap">
                                                <asp:LinkButton runat="server" CssClass="btn btn-secondary btn-sm"
                                                    CommandName="EditItem" CommandArgument='<%# Eval("Id") %>'>
                                                    تعديل
                                                </asp:LinkButton>
                                                <asp:LinkButton runat="server" CssClass="btn btn-danger btn-sm"
                                                    CommandName="DeleteItem" CommandArgument='<%# Eval("Id") %>'
                                                    OnClientClick="return confirm('هل أنتِ متأكدة من حذف هذا السجل؟ لا يمكن التراجع عن الحذف.');">
                                                    حذف
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </asp:PlaceHolder>
            </asp:PlaceHolder>

            <%-- ======================= FORM ======================= --%>
            <asp:PlaceHolder ID="phForm" runat="server" Visible="false">
                <h4 class="h6 mb-3"><asp:Literal ID="ltrFormMode" runat="server" /></h4>

                <asp:Repeater ID="rptFields" runat="server" OnItemDataBound="rptFields_ItemDataBound">
                    <ItemTemplate>
                        <div class="mb-3">
                            <asp:HiddenField ID="hfFieldName" runat="server" Value='<%# Eval("Name") %>' />
                            <label class="form-label"><%# Eval("Label") %></label>

                            <%-- TextMode/Rows are set in rptFields_ItemDataBound.
                                 Databinding an enum property such as TextMode emits a
                                 direct cast, so a string expression here fails to
                                 compile with CS0030. --%>
                            <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"
                                Text='<%# Eval("Value") %>' />

                            <asp:Panel runat="server" Visible='<%# Eval("HasHint") %>'>
                                <div class="form-text"><%# Eval("Hint") %></div>
                            </asp:Panel>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <div class="d-flex gap-2 mt-4">
                    <asp:LinkButton ID="lnkSave" runat="server" CssClass="btn btn-primary" OnClick="lnkSave_Click">
                        حفظ
                    </asp:LinkButton>
                    <asp:LinkButton ID="lnkCancel" runat="server" CssClass="btn btn-secondary" OnClick="lnkCancel_Click">
                        إلغاء
                    </asp:LinkButton>
                </div>
            </asp:PlaceHolder>

        </div>
    </div>
</asp:PlaceHolder>
