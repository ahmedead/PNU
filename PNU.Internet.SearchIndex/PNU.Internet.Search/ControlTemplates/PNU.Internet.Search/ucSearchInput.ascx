<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="ucSearchInput.ascx.cs"
    Inherits="PNU.Internet.Search.ControlTemplates.PNU.Internet.Search.ucSearchInput" %>

<div class="dga-search-wrapper">
    <div class="dga-search-input">
        <span class="dga-search-icon">
            <i class="fa fa-search" aria-hidden="true"></i>
        </span>
        <asp:TextBox runat="server" ID="txtKeyword"
                     CssClass="dga-search-textbox"
                     placeholder="ابحث في موقع جامعة الأميرة نورة..." />
        <asp:Button runat="server" ID="btnSearch"
                    CssClass="dga-search-button"
                    Text="بحث"
                    OnClick="btnSearch_Click" />
    </div>
</div>

<style>
.dga-search-wrapper { width:100%; max-width:720px; margin:0 auto; }
.dga-search-input {
    display:flex; align-items:stretch;
    border:1px solid #d0d5dd; border-radius:8px;
    background:#fff; overflow:hidden;
}
.dga-search-icon {
    display:flex; align-items:center; justify-content:center;
    padding:0 14px; color:#667085;
}
.dga-search-textbox {
    flex:1; border:0; padding:12px 8px;
    font-size:15px; outline:none; direction:rtl;
}
.dga-search-button {
    border:0; padding:0 28px;
    background:#005C5C; color:#fff;
    font-weight:600; cursor:pointer;
}
.dga-search-button:hover { background:#00484c; }
</style>
