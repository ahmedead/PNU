<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEserviceSections.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList.ucEserviceSections" %>

<style>
    .pnu-eservice-card {
        border: 1px solid #e9ecef;
        border-radius: 12px;
        box-shadow: 0 4px 14px rgba(0, 0, 0, 0.04);
        transition: transform 0.2s ease, box-shadow 0.2s ease;
    }
    .pnu-eservice-card:hover {
        transform: translateY(-2px);
        box-shadow: 0 8px 22px rgba(0, 0, 0, 0.08);
    }
    .pnu-icon-box {
        width: 52px;
        height: 52px;
        background-color: #0b6637;
        color: #ffffff;
        border-radius: 10px;
        display: inline-flex;
        align-items: center;
        justify-content: center;
    }
    .pnu-btn-action {
        background-color: #0b6637;
        color: #ffffff !important;
        border: none;
        border-radius: 8px;
        padding: 12px 20px;
        font-weight: 600;
        font-size: 1rem;
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 10px;
        text-decoration: none;
        transition: background-color 0.2s ease, transform 0.15s ease;
    }
    .pnu-btn-action:hover {
        background-color: #074e29;
        color: #ffffff !important;
        transform: translateY(-1px);
    }
    .pnu-btn-action:active {
        transform: translateY(0);
    }
</style>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <section class="container py-5" aria-labelledby="electronic-services-title">
        <div class="row g-4">
            <!-- البطاقة الأولى: المستفيد الداخلي -->
            <div class="col-12 col-md-6 col-lg-6">
                <article class="card pnu-eservice-card h-100">
                    <div class="d-flex card-body flex-column gap-4 p-4">
                        <div class="pnu-icon-box">
                            <i class="hgi hgi-stroke <%= InternalIcon %> fs-3" aria-hidden="true"></i>
                        </div>
                        <div>
                            <h2 class="card-title fs-4 fw-bold mb-2"><%= InternalTitle %></h2>
                            <p class="card-text text-muted mb-0"><%= InternalDescription %></p>
                        </div>
                        <div class="d-flex flex-column gap-3 mt-auto pt-3">
                            <a class="pnu-btn-action" href="<%= InternalUniversityUrl %>" aria-label="<%= InternalTitle %>">
                                <span><%= InternalTitle %></span>
                                <i class="hgi hgi-stroke hgi-user-group fs-5" aria-hidden="true"></i>
                            </a>
                        </div>
                    </div>
                </article>
            </div>

            <!-- البطاقة الثانية: المستفيد الخارجي -->
            <div class="col-12 col-md-6 col-lg-6">
                <article class="card pnu-eservice-card h-100">
                    <div class="d-flex card-body flex-column gap-4 p-4">
                        <div class="pnu-icon-box">
                            <i class="hgi hgi-stroke <%= ExternalIcon %> fs-3" aria-hidden="true"></i>
                        </div>
                        <div>
                            <h2 class="card-title fs-4 fw-bold mb-2"><%= ExternalTitle %></h2>
                            <p class="card-text text-muted mb-0"><%= ExternalDescription %></p>
                        </div>
                        <div class="d-flex flex-column gap-3 mt-auto pt-3">
                            <a class="pnu-btn-action" href="<%= ExternalUrl %>" aria-label="<%= ExternalTitle %>">
                                <span><%= ExternalTitle %></span>
                                <i class="hgi hgi-stroke hgi-globe-02 fs-5" aria-hidden="true"></i>
                            </a>
                        </div>
                    </div>
                </article>
            </div>
        </div>
    </section>
</main>
