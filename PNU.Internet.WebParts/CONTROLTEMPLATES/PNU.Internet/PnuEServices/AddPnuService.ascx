<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddPnuService.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PnuEServices.AddPnuService" %>


<section class=" pt-5 mt-5 mb-4">
    <div class="container">
        <div class="row">
            <div class="col-lg-12">
                <div id="Msg" runat="server">
                </div>
            </div>
        </div>
        <div class="row justify-content-center">


            <div class="col-lg-6 mb-4 mb-lg-1 news-form">
                AR Details
                <div class="card mb-2 p-4 shadow border-0 rounded-4 ">
                    <div class="row">
                        <div class="col-md-12 text-start">
                            <h5 class="card-title mb-4 fw-bold">
                                <asp:Literal ID="lit_formTitle" runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceBasicInfo%>"></asp:Literal>
                            </h5>
                        </div>



                        <div class="col-md-12">
                            <div class="form-floating mb-4">
                                <asp:TextBox ID="txtServiceTitle" placeholder="" runat="server" class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rvfTitle" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group1" Style="color: red" ControlToValidate="txtServiceTitle" CssClass="required" Display="dynamic" />
                                <label for="floatingTextarea2">
                                    <asp:Literal ID="lit_Servicetitle" runat="server" Text="<%$Resources:PnuInternetResources, res_lblServiceTitle%>"></asp:Literal></label>


                            </div>
                        </div>


                        <div class="col-md-12">

                            <div class="form-floating mb-4">
                                <asp:TextBox ID="txtServiceDEsc" placeholder="" runat="server" TextMode="MultiLine" Rows="3" class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group1" Style="color: red" ControlToValidate="txtServiceDEsc" CssClass="required" Display="dynamic" />
                                <label for="floatingTextarea2">
                                    <asp:Literal ID="littxtServiceDEsc_ServiceDesc" runat="server" Text="<%$Resources:PnuInternetResources, res_lblServiceDesc%>"></asp:Literal></label>


                            </div>
                        </div>



                        <div class="col-md-12">
                            <div class="mb-4">

                                <label for="floatingTextarea2">
                                    <asp:Literal ID="lit_Benef" runat="server" Text="<%$Resources:PnuInternetResources, res_lblBeneficiery%>"></asp:Literal></label>

                                <asp:CheckBoxList ID="chk_Benef" runat="server"></asp:CheckBoxList>
                                <asp:CustomValidator ID="rfvchkBenif" runat="server" Enabled="true" ClientValidationFunction="ValidateBenif" 
                                    Text="<%$Resources:PnuInternetResources,res_RequiredField %>" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" 
                                    ValidationGroup="group1" Display="Dynamic" Style="color: red" />
                            </div>
                        </div>


                        <div class="col-md-12">
                            <div class="mb-4">

                                <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:PnuInternetResources, res_lblServiceCategory%>"></asp:Literal>
                                <asp:DropDownList ID="ddl_Category" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvCategory" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group1" ControlToValidate="ddl_Category" InitialValue="0" CssClass="required" Style="color: red" Display="dynamic" />

                            </div>
                        </div>

                    </div>

                </div>
                <div class="card mb-2 p-4 shadow border-0 rounded-4 ">
                    <div class="row">
                        <div class="col-md-12 text-start">
                            <h5 class="card-title mb-4 fw-bold">
                                <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_AddServiceCond%>"></asp:Literal>
                            </h5>
                        </div>


                        <div class="col-md-12">

                            <div class="row g-1 align-items-baseline">
                                <div class="col-md-1">
                                    <div class=" mb-4">
                                        <label for="floatingTextarea2">
                                            <asp:Literal ID="lit_sort" runat="server" Text="<%$Resources:PnuInternetResources, res_Sort%>"></asp:Literal></label>
                                        <asp:TextBox ID="txtCondOrder" placeholder="" runat="server" TextMode="Number" class="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group2" Style="color: red" ControlToValidate="txtCondOrder" CssClass="required" Display="dynamic" />
                                    </div>
                                </div>

                                <div class="col-md-10">

                                    <div class="mb-4">
                                        <label for="floatingTextarea2">
                                            <asp:Literal ID="lit_Cond" runat="server" Text="<%$Resources:PnuInternetResources, res_lblCond%>"></asp:Literal></label>
                                        <asp:TextBox ID="txt_Cond" runat="server" class="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group2" Style="color: red" ControlToValidate="txt_Cond" CssClass="required" Display="dynamic" />

                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:Button ID="btn_AddCond" class="btn btn-sm btn-primary" runat="server" Text="<%$Resources:PnuInternetResources, res_lblAddButtonTitle%>" ValidationGroup="group2" OnClick="btn_AddCond_Click" />
                                </div>
                            </div>
                            <div class="table-responsive">


                                <asp:Repeater ID="rep_cond" runat="server" OnItemCommand="rep_cond_ItemCommand">
                                    <HeaderTemplate>

                                        <table class="table table-striped-secondary table-borderless fs-5 text-nowrap">
                                            <thead>
                                                <th>
                                                    <asp:Literal ID="lit_rptitle" runat="server" Text="<%$Resources:PnuInternetResources, res_lblCondList%>"></asp:Literal></th>

                                                <th></th>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Eval("Title") %></td>
                                            <td>
                                                <asp:Button ID="btn_CondDel" runat="server" class="btn btn-sm btn-danger" Text="<%$Resources:PnuInternetResources, res_delete%>" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
                                            </td>

                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                                </table>
                                           
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="card mb-2 p-4 shadow border-0 rounded-4 ">
                    <div class="row">
                        <div class="col-md-12 text-start">
                            <h5 class="card-title mb-4 fw-bold">
                                <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_AddServiceDoc%>"></asp:Literal>
                            </h5>
                        </div>


                        <div class="col-md-12">

                            <div class="row g-1 align-items-baseline">
                                <div class="col-md-1">
                                    <div class=" mb-4">
                                        <asp:TextBox ID="txt_docOrder" placeholder="" runat="server" TextMode="Number" class="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group4" Style="color: red" ControlToValidate="txt_docOrder" CssClass="required" Display="dynamic" />
                                        <label for="floatingTextarea2">
                                            <asp:Literal ID="lit_docORder" runat="server" Text="<%$Resources:PnuInternetResources, res_Sort%>"></asp:Literal></label>
                                    </div>
                                </div>

                                <div class="col-md-10">

                                    <div class="mb-4">
                                        <asp:TextBox ID="txt_DocTitle" runat="server" class="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group4" Style="color: red" ControlToValidate="txt_DocTitle" CssClass="required" Display="dynamic" />
                                        <label for="floatingTextarea2">
                                            <asp:Literal ID="lit_DocTitle" runat="server" Text="<%$Resources:PnuInternetResources, res_lblDocTitle%>"></asp:Literal></label>
                                    </div>
                                </div>
                                <div class="col-md-1">

                                    <asp:Button ID="btn_AddDoc" class="btn btn-sm btn-primary" runat="server" Text="<%$Resources:PnuInternetResources, res_lblAddButtonTitle%>" ValidationGroup="group4" OnClick="btn_AddDoc_Click" />
                                </div>
                            </div>
                            <div class="table-responsive">
                                <asp:Repeater ID="rep_Documents" runat="server" OnItemCommand="rep_Documents_ItemCommand">
                                    <HeaderTemplate>

                                        <table class="table table-striped-secondary table-borderless fs-5 text-nowrap">
                                            <thead>
                                                <th>
                                                    <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_lblDocTitle%>"></asp:Literal></th>

                                                <th></th>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Eval("Title") %></td>
                                            <td>
                                                <asp:Button ID="btn_DocDel" runat="server" class="btn btn-sm btn-danger" Text="<%$Resources:PnuInternetResources, res_delete%>" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
                                            </td>

                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                                </table>
                                          
                                          
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="card mb-2 p-4 shadow border-0 rounded-4 ">
                    <div class="row">
                        <div class="col-md-12 text-start">
                            <h5 class="card-title mb-4 fw-bold">
                                <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_AddServiceSchedule%>"></asp:Literal>

                            </h5>
                        </div>


                        <div class="col-md-12">

                            <div class="row g-1 align-items-baseline">
                                <div class="form-floating mb-4">

                                    <asp:TextBox ID="txt_procedure" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group6" Style="color: red" ControlToValidate="txt_procedure" CssClass="required" Display="dynamic" />
                                    <label for="floatingTextarea2">
                                        <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceProcedure%>"></asp:Literal></label>

                                </div>
                                <div class="form-floating mb-4">

                                    <SharePoint:DateTimeControl ID="ProcedureDate" runat="server" CssClassTextBox="form-control" IsRequiredField="false" />


                                    <label for="floatingTextarea2">
                                        <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceDate%>"></asp:Literal></label>

                                </div>
                                <div class="form-floating mb-4">

                                    <asp:DropDownList ID="ddl_day" runat="server">
                                        <asp:ListItem Text="--اختر--"  Value="0"></asp:ListItem>
                                        <asp:ListItem Text="السبت" Value="السبت"></asp:ListItem>
                                        <asp:ListItem Text="الأحد" Value="الأحد"></asp:ListItem>
                                        <asp:ListItem Text="الأثنين" Value="الأثنين"></asp:ListItem>
                                        <asp:ListItem Text="الثلاثاء" Value="الثلاثاء"></asp:ListItem>
                                        <asp:ListItem Text="الأربعاء" Value="الأربعاء"></asp:ListItem>
                                        <asp:ListItem Text="الخميس" Value="الخميس"></asp:ListItem>
                                        <asp:ListItem Text="الجمعة" Value="الجمعة"></asp:ListItem>

                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group6" ControlToValidate="ddl_day" InitialValue="0" CssClass="required" Style="color: red" Display="dynamic" />

                                    <label for="floatingTextarea2">
                                        <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceDay%>"></asp:Literal></label>

                                </div>
                                <div class="col-md-1">

                                    <asp:Button ID="btn_AddProcedure" class="btn btn-sm btn-primary" runat="server" Text="<%$Resources:PnuInternetResources, res_lblAddButtonTitle%>" ValidationGroup="group6" OnClick="btn_AddProcedure_Click" />
                                </div>
                            </div>
                            <div class="table-responsive">
                                <asp:Repeater ID="rep_Schedule" runat="server" OnItemCommand="rep_Schedule_ItemCommand">
                                    <HeaderTemplate>

                                        <table class="table table-striped-secondary table-borderless fs-5 text-nowrap">
                                            <thead>
                                                <th>
                                                    <asp:Literal ID="lit_rpScheduletitle" runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceProcedure%>"></asp:Literal></th>
                                                <th>
                                                    <asp:Literal ID="lit_rpDay" runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceDay%>"></asp:Literal></th>
                                                <th>
                                                    <asp:Literal ID="lit_prDate" runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceDate%>"></asp:Literal></th>

                                                <th></th>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Eval("Title") %></td>
                                            <td>
                                                <%# Eval("Day") %></td>
                                            <td>
                                                <%# Eval("Date") %></td>

                                            <td>
                                                <asp:Button ID="btn_ScheduleDel" runat="server" class="btn btn-sm btn-danger" Text="<%$Resources:PnuInternetResources, res_delete%>" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
                                            </td>

                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                                </table>
                                      
                                          
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-lg-6 mb-4 mb-lg-1 news-form text-end" dir="ltr">
                En Details

            <div class="card mb-2 p-4 shadow border-0 rounded-4 end-text">
                <div class="row">
                    <div class="col-md-12 text-start">
                        <h5 class="card-title mb-4 fw-bold">
                            <asp:Literal runat="server" Text="Basic Information"></asp:Literal>
                        </h5>
                    </div>



                    <div class="col-md-12">
                        <div class="form-floating mb-4">
                            <asp:TextBox ID="txt_ServiceTitleEn" placeholder="" runat="server" class="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group1" Style="color: red" ControlToValidate="txt_ServiceTitleEn" CssClass="required" Display="dynamic" />
                            <label for="floatingTextarea2">
                                <asp:Literal runat="server" Text="service title"></asp:Literal></label>


                        </div>
                    </div>


                    <div class="col-md-12">

                        <div class="form-floating mb-4">
                            <asp:TextBox ID="txtServiceDescEn" placeholder="" runat="server" TextMode="MultiLine" Rows="3" class="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group1" Style="color: red" ControlToValidate="txtServiceDescEn" CssClass="required" Display="dynamic" />
                            <label for="floatingTextarea2">
                                <asp:Literal runat="server" Text="service description"></asp:Literal></label>


                        </div>
                    </div>

                    <div class="col-md-12">
                        <div class="mb-4">

                            <label for="floatingTextarea2">
                                <asp:Literal runat="server" Text="Beneficiaries"></asp:Literal></label>

                            <asp:CheckBoxList ID="chkBenfEn" runat="server"></asp:CheckBoxList>
                            <asp:CustomValidator ID="rvfBenfEn" runat="server" Enabled="true" ClientValidationFunction="ValidateBenifEn"  
                                Text="<%$Resources:PnuInternetResources,res_RequiredField %>" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" 
                                ValidationGroup="group1" Display="Dynamic" Style="color: red" />
                        </div>
                    </div>


                    <div class="col-md-12">
                        <div class="mb-4">



                            <label for="floatingTextarea2">
                                <asp:Literal runat="server" Text="Categories"></asp:Literal></label>
                            <asp:DropDownList ID="ddlCategoryEn" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group1" ControlToValidate="ddlCategoryEn" InitialValue="0" CssClass="required" Style="color: red" Display="dynamic" />

                        </div>
                    </div>

                </div>

            </div>
                <div class="card mb-2 p-4 shadow border-0 rounded-4 end-text">
                    <div class="row">
                        <div class="col-md-12 text-start">
                            <asp:Literal runat="server" Text="Add service Conditions"></asp:Literal>

                        </div>


                        <div class="col-md-12">

                            <div class="row g-1 align-items-baseline">
                                <div class="col-md-1">
                                    <div class=" mb-4">
                                        <label for="floatingTextarea2">
                                            <asp:Literal runat="server" Text="Order"></asp:Literal></label>
                                        <asp:TextBox ID="txtConOrderEn" placeholder="" runat="server" TextMode="Number" class="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group3" Style="color: red" ControlToValidate="txtConOrderEn" CssClass="required" Display="dynamic" />
                                    </div>
                                </div>

                                <div class="col-md-10">

                                    <div class="mb-4">
                                        <label for="floatingTextarea2">
                                            <asp:Literal runat="server" Text="condition"></asp:Literal></label>
                                        <asp:TextBox ID="txtCondEn" runat="server" class="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group3" Style="color: red" ControlToValidate="txtCondEn" CssClass="required" Display="dynamic" />

                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:Button ID="btnAddCondEn" class="btn btn-sm btn-primary" runat="server" Text="Add" ValidationGroup="group3" OnClick="btnAddCondEn_Click" />
                                </div>
                            </div>
                            <div class="table-responsive">


                                <asp:Repeater ID="rep_CondEn" runat="server" OnItemCommand="rep_CondEn_ItemCommand">
                                    <HeaderTemplate>

                                        <table class="table table-striped-secondary table-borderless fs-5 text-nowrap">
                                            <thead>
                                                <th>
                                                    <asp:Literal runat="server" Text="Conditions"></asp:Literal></th>

                                                <th></th>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Eval("Title") %></td>
                                            <td>
                                                <asp:Button ID="btn_CondDel" runat="server" class="btn btn-sm btn-danger" Text="delete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
                                            </td>

                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                                </table>
                                           
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="card mb-2 p-4 shadow border-0 rounded-4 end-text">
                    <div class="row">
                        <div class="col-md-12 text-start">
                            <h5 class="card-title mb-4 fw-bold">
                                <asp:Literal runat="server" Text="Add Service documents"></asp:Literal>
                            </h5>
                        </div>


                        <div class="col-md-12">

                            <div class="row g-1 align-items-baseline">
                                <div class="col-md-1">
                                    <div class=" mb-4">
                                        <asp:TextBox ID="txtDocOrderEn" placeholder="" runat="server" TextMode="Number" class="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group5" Style="color: red" ControlToValidate="txtDocOrderEn" CssClass="required" Display="dynamic" />
                                        <label for="floatingTextarea2">
                                            <asp:Literal runat="server" Text="Order"></asp:Literal></label>
                                    </div>
                                </div>

                                <div class="col-md-10">

                                    <div class="mb-4">
                                        <asp:TextBox ID="txtDoctitleEn" runat="server" class="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group5" Style="color: red" ControlToValidate="txtDoctitleEn" CssClass="required" Display="dynamic" />
                                        <label for="floatingTextarea2">
                                            <asp:Literal runat="server" Text="document"></asp:Literal></label>
                                    </div>
                                </div>
                                <div class="col-md-1">

                                    <asp:Button ID="btnAddDocEn" class="btn btn-sm btn-primary" runat="server" Text="Add" ValidationGroup="group5" OnClick="btnAddDocEn_Click" />
                                </div>
                            </div>
                            <div class="table-responsive">
                                <asp:Repeater ID="rep_DocumentsEn" runat="server" OnItemCommand="rep_DocumentsEn_ItemCommand">
                                    <HeaderTemplate>

                                        <table class="table table-striped-secondary table-borderless fs-5 text-nowrap">
                                            <thead>
                                                <th>
                                                    <asp:Literal runat="server" Text="Documents"></asp:Literal></th>

                                                <th></th>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Eval("Title") %></td>
                                            <td>
                                                <asp:Button ID="btn_DocDel" runat="server" class="btn btn-sm btn-danger" Text="delete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
                                            </td>

                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                                </table>
                                          
                                          
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="card mb-2 p-4 shadow border-0 rounded-4 end-text">
                    <div class="row">
                        <div class="col-md-12 text-start">
                            <h5 class="card-title mb-4 fw-bold">
                                <asp:Literal runat="server" Text="Add service Schedule"></asp:Literal>

                            </h5>
                        </div>


                        <div class="col-md-12">

                            <div class="row g-1 align-items-baseline">
                                <div class="form-floating mb-4">

                                    <asp:TextBox ID="txtProcEn" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group7" Style="color: red" ControlToValidate="txtProcEn" CssClass="required" Display="dynamic" />
                                    <label for="floatingTextarea2">
                                        <asp:Literal runat="server" Text="procedure"></asp:Literal></label>

                                </div>
                                <div class="form-floating mb-4">

                                    <SharePoint:DateTimeControl ID="ProcedureDateEn" runat="server" CssClassTextBox="form-control" IsRequiredField="False" />


                                    <label for="floatingTextarea2">
                                        <asp:Literal runat="server" Text="date"></asp:Literal></label>

                                </div>
                                <div class="form-floating mb-4">

                                    <asp:DropDownList ID="ddl_dayEn" runat="server">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Saturday" Value="Saturday"></asp:ListItem>
                                        <asp:ListItem Text="Sunday" Value="Sunday"></asp:ListItem>
                                        <asp:ListItem Text="Monday" Value="Monday"></asp:ListItem>
                                        <asp:ListItem Text="Tuesday" Value="Tuesday"></asp:ListItem>
                                        <asp:ListItem Text="Wednesday" Value="Wednesday"></asp:ListItem>
                                        <asp:ListItem Text="Thursday" Value="Thursday"></asp:ListItem>
                                        <asp:ListItem Text="Friday" Value="Friday"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group7" ControlToValidate="ddl_dayEn" InitialValue="0" CssClass="required" Style="color: red" Display="dynamic" />

                                    <label for="floatingTextarea2">
                                        <asp:Literal runat="server" Text="days"></asp:Literal></label>

                                </div>
                                <div class="col-md-1">

                                    <asp:Button ID="btnAddScheduleEn" class="btn btn-sm btn-primary" runat="server" Text="Add" ValidationGroup="group7" OnClick="btnAddScheduleEn_Click" />
                                </div>
                            </div>
                            <div class="table-responsive">
                                <asp:Repeater ID="rep_ScheduleEn" runat="server" OnItemCommand="rep_ScheduleEn_ItemCommand">
                                    <HeaderTemplate>

                                        <table class="table table-striped-secondary table-borderless fs-5 text-nowrap">
                                            <thead>
                                                <th>
                                                    <asp:Literal runat="server" Text="Procedure"></asp:Literal></th>
                                                <th>
                                                    <asp:Literal runat="server" Text="Day"></asp:Literal></th>
                                                <th>
                                                    <asp:Literal runat="server" Text="Date"></asp:Literal></th>

                                                <th></th>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Eval("Title") %></td>
                                            <td>
                                                <%# Eval("Day") %></td>
                                            <td>
                                                <%# Eval("Date") %></td>

                                            <td>
                                                <asp:Button ID="btn_ScheduleDel" runat="server" class="btn btn-sm btn-danger" Text="delete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
                                            </td>

                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                                </table>
                                      
                                          
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>

                    </div>
                </div>


            </div>
            <!-- -->

            <div class="col-lg-12 mb-4 mb-lg-1 news-form">
                <div class="card mb-2 p-4 shadow border-0 rounded-4 ">
                    <div class="row">
                        <div class="col-md-12 text-start">
                            <h5 class="card-title mb-4 fw-bold">
                                <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceStatus%>"></asp:Literal>
                            </h5>
                        </div>
                        <div class="col-md-12">
                            <label for="floatingTextarea2">
                                <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_lblIsActive%>"></asp:Literal></label>

                            <asp:RadioButtonList ID="rdb_IsActive" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="<%$Resources:PnuInternetResources, res_YesValue%>" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:PnuInternetResources, res_NoValue%>" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>

                        <div class="col-md-12">
                            <div class="mb-4">
                                <label for="floatingTextarea2">
                                    <asp:Literal ID="lit_IsHome" runat="server" Text="<%$Resources:PnuInternetResources, res_lblIsHome%>"></asp:Literal></label>
                                <asp:RadioButtonList ID="rdb_IsHome" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="<%$Resources:PnuInternetResources, res_YesValue%>" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:PnuInternetResources, res_NoValue%>" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="mb-4">
                                <label for="floatingTextarea2">
                                    <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceSort%>"></asp:Literal></label>
                                <asp:TextBox ID="txtServiceOrder" runat="server" TextMode="Number"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rvfServiceorder" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group1" Style="color: red" ControlToValidate="txtServiceOrder" CssClass="required" Display="dynamic" />
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="form-floating mb-4">
                                <label for="floatingTextarea2">
                                    <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_ServiceImage%>"></asp:Literal></label>

                                <div class="input-group mb-4 ">

                                    <asp:FileUpload ID="fileUPload" runat="server" class="form-control h-100 m-0" />
                                    <asp:RequiredFieldValidator ID="rfvNewsImage" runat="server" Enabled="true"
                                        Text="<%$Resources:PnuInternetResources,res_RequiredField %>" ErrorMessage="<%$ Resources:PnuInternetResources,res_RequiredField %>"
                                        ControlToValidate="fileUPload" ValidationGroup="group1" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ID="regNewsImage" runat="server" SetFocusOnError="true"
                                        ControlToValidate="fileUPload" Display="Dynamic" ValidationExpression="(.*).(.jpg|.JPG|.JPEG|.jpeg|.PNG|.png|.webp|.WEBP)$"
                                        ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>"
                                        ValidationGroup="group1" Style="color: red" />

                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="form-floating mb-4">
                                <asp:TextBox ID="txtServiceUrl" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Enabled="true" ErrorMessage="<%$Resources:PnuInternetResources,res_RequiredField %>" ValidationGroup="group1" Style="color: red" ControlToValidate="txtServiceUrl" CssClass="required" Display="dynamic" />
                                <label for="floatingTextarea2">
                                    <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_lblServiceUrl%>"></asp:Literal></label>


                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <!-- -->
            <div class="col-md-12 align-items-stretch text-center my-5">

                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" class="btn btn-lg btn-primary  px-5  me-3  flex-fill " ValidationGroup="group1" Text="<%$Resources:PnuInternetResources, res_Save%>" />
            </div>
        </div>
    </div>


</section>


<script>
    function ValidateBenif(source, args) {
        var chkListModules = document.getElementById('<%= chk_Benef.ClientID %>');
        var chkListinputs = chkListModules.getElementsByTagName("input");
        for (var i = 0; i < chkListinputs.length; i++) {
            if (chkListinputs[i].checked) {
                args.IsValid = true;
                return;
            }
        }
        args.IsValid = false;
    }

    function ValidateBenifEn(source, args) {
        var chkListModules = document.getElementById('<%= chkBenfEn.ClientID %>');
        var chkListinputs = chkListModules.getElementsByTagName("input");
        for (var i = 0; i < chkListinputs.length; i++) {
            if (chkListinputs[i].checked) {
                args.IsValid = true;
                return;
            }
        }
        args.IsValid = false;
    }


</script>
