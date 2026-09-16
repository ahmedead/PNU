<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgStyles.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgStyles" %>

<asp:PlaceHolder ID="phStyles" runat="server">
<style type="text/css">
/* Postgraduate academic kit - styles the base DGA stylesheet does not provide. */
.pg-kit .fs-64 { font-size: 64px; }
.pg-kit .fs-64.mb-0 { margin-bottom: 2rem !important; }

.pg-kit .custom-title {
    background: url("/Style Library/PostgraduateKit/public/images/icons/title-arrow.png") no-repeat right;
    padding-inline: 4rem;
}
.pg-kit .custom-paragraph { padding-inline: 4rem; }

.pg-kit .fw-custom-bold { font-weight: bold; }
.pg-kit .fs-custom-4 { font-size: 1.25rem !important; }
.pg-kit .fw-600 { font-weight: 600; }

.pg-kit .border-dark-blue { border-inline-start-color: #194185 !important; }
.pg-kit .border-light-blue { border-inline-start-color: #D1E9FF !important; }
.pg-kit .border-custom-10 { border-width: 8px !important; }

.pg-kit .step-icon img { width: 50px; height: 50px; }
.pg-kit .step-icon::after {
    content: "";
    position: absolute;
    height: 60%;
    width: 1px;
    background: var(--dga-gray-400, #98A2B3);
    bottom: -50%;
}
.pg-kit .step:last-child .step-icon::after { display: none; }

.pg-kit .faq-icon img { width: 62px; height: 62px; }

.pg-kit .custom-link-collapse { position: relative; }
.pg-kit .custom-link-collapse::after {
    content: "";
    position: absolute;
    inset-inline-end: 0;
    top: 3px;
    background: url("/Style Library/PostgraduateKit/public/images/icons/accordion-arrow.png") no-repeat center;
    width: 34px;
    height: 21px;
    transition: transform .3s ease-in-out;
}
.pg-kit .custom-link-collapse[aria-expanded="true"]::after { transform: rotateX(180deg) rotateY(180deg); }

.pg-kit .list-group-item-primary { background-color: #D1E9FF !important; }

.pg-kit .bg-custom-primary {
    background-color: #E7F3FF;
    color: #384250;
    padding: 0.625rem 1.5rem;
    border: 0;
}

@media (max-width: 767.98px) {
    .pg-kit .fs-64 { font-size: 2.25rem; }
    .pg-kit .custom-title,
    .pg-kit .custom-paragraph { padding-inline: 0; background-image: none; }
}
</style>
</asp:PlaceHolder>
