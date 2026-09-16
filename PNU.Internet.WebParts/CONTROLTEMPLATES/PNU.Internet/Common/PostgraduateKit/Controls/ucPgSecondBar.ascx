<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgSecondBar.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgSecondBar" %>

<asp:PlaceHolder ID="phBar" runat="server">
<div id="secondBar" class="bg-light border-bottom icons pg-kit">
    <div class="container d-flex justify-content-between py-1 flex-wrap">

        <ul class="d-flex flex-wrap align-items-center list-unstyled gap-2 mb-0">
            <asp:Repeater ID="rptInfo" runat="server">
                <ItemTemplate>
                    <li>
                        <%# Eval("HasIcon").Equals(true) ? "<img src=\"" + Eval("IconUrl") + "\" alt=\"\" aria-hidden=\"true\" loading=\"lazy\" decoding=\"async\">" : "" %>
                        <%# Eval("IsLive").Equals(true)
                            ? "<span class=\"" + Eval("LiveClass") + "\"></span>"
                            : "<span>" + Eval("Text") + "</span>" %>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>

        <ul class="d-flex flex-wrap align-items-center list-unstyled gap-1 mb-0">
            <asp:Repeater ID="rptButtons" runat="server">
                <ItemTemplate>
                    <li>
                        <a href='<%# Eval("SafeLinkUrl") %>' title='<%# Eval("AriaLabel") %>' aria-label='<%# Eval("AriaLabel") %>'>
                            <%# Eval("HasIcon").Equals(true)
                                ? "<img src=\"" + Eval("IconUrl") + "\" alt=\"\" aria-hidden=\"true\" loading=\"lazy\" decoding=\"async\">"
                                : Eval("Text") %>
                        </a>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>

    </div>
</div>

<asp:PlaceHolder ID="phClock" runat="server">
<script type="text/javascript">
    (function () {
        var now = new Date();
        var dateOptions = { year: 'numeric', month: 'long', day: 'numeric', timeZone: 'Asia/Riyadh' };
        var timeOptions = { hour: 'numeric', minute: '2-digit', timeZone: 'Asia/Riyadh' };
        var locale = document.documentElement.lang && document.documentElement.lang.indexOf('en') === 0 ? 'en-GB' : 'ar-SA';

        document.querySelectorAll('.js-pg-date').forEach(function (el) {
            el.textContent = new Intl.DateTimeFormat(locale, dateOptions).format(now);
        });
        document.querySelectorAll('.js-pg-time').forEach(function (el) {
            el.textContent = new Intl.DateTimeFormat(locale, timeOptions).format(now);
        });
    })();
</script>
</asp:PlaceHolder>
</asp:PlaceHolder>
