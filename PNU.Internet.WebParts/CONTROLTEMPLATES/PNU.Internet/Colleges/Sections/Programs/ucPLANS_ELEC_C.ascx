<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPLANS_ELEC_C.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.Programs.ucPLANS_ELEC_C" %>



<div class="accordion-card">
    <asp:Repeater ID="masterRepeaterU" runat="server">
        <ItemTemplate>
            <div class="accordion accordion-flush " id="accordionFlushExample<%# Eval("LevelCode") %>">

                <div class="accordion-item  my-2 border-0">
                    <h2 class="accordion-header" id="flush-headingOne">
                        <button class="accordion-button collapsed border" type="button"
                            data-bs-toggle="collapse" data-bs-target="#flush-collapse<%# Eval("LevelCode") %>" aria-expanded="false"
                            aria-controls="flush-collapse<%# Eval("LevelCode") %>">
                            <%# Eval("LevelDesc") %>
                        </button>
                    </h2>

                    <div id="flush-collapse<%# Eval("LevelCode") %>" class="accordion-collapse collapse "
                        aria-labelledby="flush-headingOne" data-bs-parent="#accordionFlushExample<%# Eval("LevelCode") %>">
                        <div class="accordion-body p-0">


                            <table class="table rounded-bottom-4 fs-5 text-nowrap mb-0  table-striped-secondary overflow-hidden">
                                <thead>
                                    <tr class="my-2 align-middle">
                                        <th class="text-center"><asp:Literal runat="server" Text="<%$ Resources: PNUres, SCRATTR_SUBJ_CODE %>" /> </th>
                                        <th class="text-start"><asp:Literal runat="server" Text="<%$ Resources: PNUres, COURSE_TITLE %>" /></th>
                                        <th class="text-center"><asp:Literal runat="server" Text="<%$ Resources: PNUres, CREDIT %>" /></th>
                                       
                                    </tr>
                                </thead>

                                <tbody>
                                    <asp:Repeater ID="rptCoursesU" runat="server" DataSource='<%# Eval("StudyPlanC") %>'>
                                        <ItemTemplate>
                                            <tr>
                                                <td class=" text-primary text-center">
                                                    <button class="btn btn-link text-primary fs-5 p-0 text-decoration-none" type="button"
                                                        data-bs-toggle="collapse" data-bs-target="#collapseWidthExample0"
                                                        aria-expanded="false" aria-controls="   ">
                                                        <%# Eval("SCRATTR_SUBJ_CODE") %> <%# Eval("SCRATTR_CRSE_NUMB") %>
                                                    </button>
                                                </td>
                                                <td class="text-start"><%# Eval("COURSE_TITLE") %></td>
                                               <td class="text-center"><%# Eval("CREDIT") %></td>
                                                
                                            </tr>
                                            <tr>
                                            </tr>
                                            
                                        </ItemTemplate>
                                    </asp:Repeater>


                                </tbody>

                            </table>


                        </div>
                    </div>

                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>




</div>
