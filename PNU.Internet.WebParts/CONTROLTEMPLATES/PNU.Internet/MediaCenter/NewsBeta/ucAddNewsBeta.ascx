<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAddNewsBeta.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.NewsBeta.ucAddNewsBeta" %>



<section class=" pt-5 mt-5 mb-4">
    <div class="container" >
        <div class="row justify-content-center">
             <h2 id="hMsg" runat="server" style="color:red" class="d-none">ليس لديك صلاحية الوصول لهذه الشاشة</h2>
            <h2 id="hSuccess" runat="server" style="color:green" class="d-none">تم الحفظ بنجاح</h2>
            <div class="col-lg-7 mb-4 mb-lg-5 news-form" id="dvForm" runat="server">
               
                <div class="card mb-4 p-5 shadow border-0 rounded-4 ">
                    <div class="row">
                        <div class="col-md-12 text-start">
                            <h4 class="card-title mb-4 fw-bold">نموذج اضافة خبر </h4>
                        </div>
                        <div class="col-md-12" id="dvIsHome" runat="server" visible="false">
                            <div class="mb-4">

                                <label>نشر علي الأخبار الرئيسية</label>
                                <ul class="list-group">
                                  
                                    <asp:RadioButtonList ID="opt_IsHome" runat="server" RepeatDirection="Horizontal">
                                          <asp:ListItem Text="نعم" Value="True" />
                                          <asp:ListItem Text="لا" Value="False"  Selected="True"/>
                                    </asp:RadioButtonList>
                                
                                </ul>
                            </div>
                        </div>
                       
                      
                        <div class="col-md-12" id="dvDept" runat="server" visible="false">
                            <div class="mb-4">

                                <label>الجهة الطالبة للخبر</label>
                                <ul class="list-group">
                                    <asp:DropDownList ID="ddlFaculties" CssClass="form-select"   runat="server" DataValueField="TitleEn"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvFaculties" runat="server" Enabled="False" ErrorMessage="required" ValidationGroup="group1"
                                        ControlToValidate="ddlFaculties" InitialValue="0" CssClass="required" Style="color: red" Display="dynamic" />

                                </ul>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="mb-4">

                                <label>التاريخ</label>
                                
                                <SharePoint:DateTimeControl ID="publishingDate" runat="server" CssClassTextBox="form-control" IsRequiredField="true" />
                            </div>

                        </div>
                       
                        <div class="col-md-12">
                            <div class=" mb-4 ">
                                <label>صورة الخبر</label>

                                <div class="input-group mb-4 ">

                                    <asp:FileUpload ID="fileUPload" runat="server" class="form-control h-100 m-0" />
                                    <asp:RequiredFieldValidator ID="rfvNewsImage" runat="server" Enabled="false"
                                        Text="required" ErrorMessage="<%$ Resources:PNU-WF,res_required %>"
                                        ControlToValidate="fileUPload" ValidationGroup="group1" Display="Dynamic" Style="color: red" />
                                    <asp:RegularExpressionValidator ID="regNewsImage" runat="server" SetFocusOnError="true"
                                        ControlToValidate="fileUPload" Display="Dynamic" ValidationExpression="(.*).(.webp|.WEBP)$"
                                        ErrorMessage="required"
                                        ValidationGroup="group1" Style="color: red" />
                                </div>
                                <%--<div class="input-group mb-4 ">
                                    <input type="file" class="form-control h-100 m-0" runat="server" id="inputGroupFile02"  accept="image/*" required>
                                    <label class="input-group-text" for="inputGroupFile02">Upload</label>
                                </div>--%>
                            </div>

                        </div>

                        <div class="col-md-12">
                            <div class="form-floating mb-4">
                                <asp:Label ID="Label1" runat="server" Text="عنوان الخبر باللغة العربية"></asp:Label>
                                <asp:TextBox ID="txtTitle" runat="server" class="form-control" placeholder=" Media Title Arabic"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rvfTitle" runat="server" Enabled="true" ErrorMessage="required" ValidationGroup="group1" 
                                    Style="color: red" ControlToValidate="txtTitle" CssClass="required" Display="dynamic" />
                                <label for="floatingInputValue"></label>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="form-floating mb-4">
                                <asp:Label ID="Label2" runat="server" Text="عنوان الخبر باللغة الإنجليزية (اختياري)"></asp:Label>
                                <asp:TextBox ID="txtTitleEn" runat="server" class="form-control" placeholder=" Media Title English"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Enabled="false" ErrorMessage="required" ValidationGroup="group1" 
                                    Style="color: red" ControlToValidate="txtTitleEn" CssClass="required" Display="dynamic" />
                                <label for="floatingInputValue"></label>
                            </div>
                        </div>

                        <div class="col-md-12 d-none">

                            <div class="form-floating mb-4">
                                <asp:Label ID="Label3" runat="server" Text="ملخص الخبر باللغة العربية"></asp:Label>
                                <asp:TextBox ID="txtSummary" placeholder=" Media Summary Arabic" TextMode="MultiLine" runat="server" Rows="3" class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rvfSummary" runat="server" Enabled="false" ErrorMessage="required" ValidationGroup="group1" 
                                    ControlToValidate="txtSummary" CssClass="required" Style="color: red" Display="dynamic" />

                                <label for="floatingTextarea2"></label>

                            </div>
                        </div>

                        <div class="col-md-12 d-none">

                            <div class="form-floating mb-4">
                                <asp:Label ID="Label4" runat="server" Text="ملخص الخبر باللغة الإنجليزية"></asp:Label>
                                <asp:TextBox ID="txtSummaryEn" placeholder=" Media Summary English" TextMode="MultiLine" runat="server" Rows="3" class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Enabled="false" ErrorMessage="required" ValidationGroup="group1" 
                                    ControlToValidate="txtSummaryEn" CssClass="required" Style="color: red" Display="dynamic" />

                                <label for="txtSummaryEn"></label>

                            </div>
                        </div>
                        <div class="col-md-12" id="dvContentAr" runat="server">

                            <div class="form-floating mb-4">
                                <asp:Label ID="Label5" runat="server" Text="محتوي الخبر باللغة العربية"></asp:Label>
                                <asp:TextBox ID="txtDetails" placeholder=" Media Content Arabic" TextMode="MultiLine" Height="300" runat="server" Rows="40" class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rvfDetails" runat="server" Enabled="true" ErrorMessage="required" ValidationGroup="group1" 
                                    ControlToValidate="txtDetails" CssClass="required" Style="color: red" Display="dynamic" />
                                <label for="floatingTextarea2"></label>
                            </div>
                        </div>

                        <div class="col-md-12" id="dvContentEn" runat="server">
                            <div class="form-floating mb-4">
                            <asp:Label ID="Label6" runat="server" Text="محتوي الخبر باللغة الإنجليزية (اختياري)"></asp:Label>

                                <asp:TextBox ID="txtDetailsEn" placeholder=" Media Content  English" TextMode="MultiLine" Height="300" runat="server" Rows="40" class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Enabled="false" ErrorMessage="required" ValidationGroup="group1"  
                                    ControlToValidate="txtDetailsEn" CssClass="required" Style="color: red" Display="dynamic" />
                                <label for="floatingTextarea2"></label>
                            </div>
                        </div>


                        <div class="col-md-12">
                            <div class="form-floating mb-4">
                                <asp:Label ID="Label7" runat="server" Text="رابط الفيديو"></asp:Label>
                                <asp:TextBox ID="txtVideoURL" runat="server" class="form-control" ></asp:TextBox>
                            </div>
                        </div>

                                                <%--<div class="col-md-12">
                            <div class=" mb-4 ">
                                <label>الفيديو</label>

                                <div class="input-group mb-4 ">

                                    <asp:FileUpload ID="VideofileUPload" runat="server" class="form-control h-100 m-0" />

                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" SetFocusOnError="true"
                                        ControlToValidate="VideofileUPload" Display="Dynamic" ValidationExpression="(.*).(.mp4|.MP4)$"
                                        ErrorMessage="required"
                                        ValidationGroup="group1" Style="color: red" />
                                </div>

                            </div>

                        </div>--%>

                        <div id="divConfirmation" runat="server" >
                            <asp:CheckBox ID="chkConfirmation" runat="server" Text="تأكيد وإقرار من الجهة بصحة المعلومات وأنها تحت مسؤوليتهم" CssClass="AcceptedAgreement"/>

                            <h5 id="hConfirmation" runat="server" style="color:red" class="d-none">يجب تأكيد الإقرار قبل المتابعة</h5>

                            

                            <br />
                            <div class="col-md-12">
                                <div class=" mb-4 ">
                                    <label>إرفاق ملفات لإدارة الإعلام بهدف الاطلاع والتأكد من صحة المعلومات</label>

                                    <div class="input-group mb-4 ">
                                        <asp:FileUpload ID="fileAdditionalUpload" runat="server" />
                                    </div>
                                    <label>يرجي وضع الملفات جميعها في ملف واحد مضغوط</label>
                                </div>
                                <h2 id="h1" runat="server" style="color:red" class="d-none">يجب تأكيد الإقرار قبل المتابعة</h2>

                            </div>
                        </div>
                        <div class="col-md-12 align-items-stretch text-center">
                            <asp:Button ID="btnSubmit" runat="server" Text="تقديم الطلب" ValidationGroup="group1" CssClass="btn btn-lg btn-primary  px-5  me-3  flex-fill w-100" OnClick="btnSubmit_Click" />

                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

</section>

