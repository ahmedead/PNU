<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPgStyles.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls.ucPgStyles" %>

<asp:PlaceHolder ID="phStyles" runat="server">

<% if (!IsFarmAccount) { %>
<script type="text/javascript">
    (function () {
        function isFarmOrSystemAccount() {
            if (typeof _spPageContextInfo !== 'undefined') {
                if (_spPageContextInfo.userId === 1073741823) return true;
                var sysKey = (_spPageContextInfo.systemUserKey || '').toLowerCase();
                if (sysKey.indexOf('system') !== -1 || sysKey.indexOf('farm') !== -1 || sysKey.indexOf('spsvc') !== -1 || sysKey.indexOf('sp_farm') !== -1) return true;
                var login = (_spPageContextInfo.userLoginName || '').toLowerCase();
                if (login === 'sharepoint\\system' || login.indexOf('\\system') !== -1 || login.indexOf('farm') !== -1) return true;
                var disp = (_spPageContextInfo.userDisplayName || '').toLowerCase();
                if (disp.indexOf('system') !== -1 || disp.indexOf('farm') !== -1 || disp.indexOf('النظام') !== -1) return true;
            }
            return false;
        }
        if (isFarmOrSystemAccount()) return;

        function purgeCoreCss() {
            if (isFarmOrSystemAccount()) return;
            var links = document.querySelectorAll('link[href*="corev15"], link[href*="corev14"]');
            for (var i = 0; i < links.length; i++) {
                links[i].disabled = true;
                if (links[i].parentNode) {
                    links[i].parentNode.removeChild(links[i]);
                }
            }
            if (document.styleSheets) {
                for (var j = 0; j < document.styleSheets.length; j++) {
                    try {
                        var sheet = document.styleSheets[j];
                        if (sheet && sheet.href && (sheet.href.indexOf('corev15') !== -1 || sheet.href.indexOf('corev14') !== -1)) {
                            sheet.disabled = true;
                        }
                    } catch (e) {}
                }
            }
        }
        purgeCoreCss();
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', purgeCoreCss);
        }
        window.addEventListener('load', purgeCoreCss);
    })();
</script>
<% } else { %>
<script type="text/javascript">
    (function () {
        var hasCore = document.querySelector('link[href*="corev15"]');
        if (!hasCore) {
            var lcid = (typeof _spPageContextInfo !== 'undefined' && _spPageContextInfo.currentLanguage) ? _spPageContextInfo.currentLanguage : '1025';
            var link = document.createElement('link');
            link.rel = 'stylesheet';
            link.type = 'text/css';
            link.href = '/_layouts/15/' + lcid + '/styles/Themable/corev15.css';
            document.head.insertBefore(link, document.head.firstChild);
        }
    })();
</script>
<% } %>
</asp:PlaceHolder>
