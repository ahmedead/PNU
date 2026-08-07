<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SEO.ascx.cs" Inherits="PNU.Internet.Web.Controls.Common.SEO" %>

<meta property="og:type" content="website" />
<meta property="og:url" content='<%= HttpContext.Current.Request.Url.AbsoluteUri %>' />
<meta property="og:title" content='<%=Microsoft.SharePoint.SPContext.Current.ListItem["Title"]%>' />
<meta property="og:description" content='<%= Microsoft.SharePoint.SPContext.Current.ListItem["Comments"] != null ? Microsoft.SharePoint.SPContext.Current.ListItem["Comments"] : Microsoft.SharePoint.SPContext.Current.Web.Title %>' />
<meta property="og:image" content='<%=Microsoft.SharePoint.SPContext.Current.Site.Url + (Microsoft.SharePoint.SPContext.Current.ListItem["PublishingRollupImage"] != null ? ((Microsoft.SharePoint.Publishing.Fields.ImageFieldValue)Microsoft.SharePoint.SPContext.Current.ListItem["PublishingRollupImage"]).ImageUrl : "/Style%20Library/Portal/images/logo.png")  %>'  />

<meta name="twitter:card" content="summary" />
<meta name="twitter:site" content="@PNU_sa"  />


<SharePoint:LanguageSpecificContent runat="server" Languages="1025">
	<ContentTemplate>
		<link rel="alternate" href='<%=  HttpContext.Current.Request.Url.AbsoluteUri %>' hreflang='ar-SA' />
		<link rel="alternate" href='<%=  HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path).Replace("/ar/","/en/") %>' hreflang='en-US' />
	</ContentTemplate>
</SharePoint:LanguageSpecificContent>
<SharePoint:LanguageSpecificContent runat="server" Languages="1033">
	<ContentTemplate>
		<link rel="alternate" href='<%=  HttpContext.Current.Request.Url.AbsoluteUri %>' hreflang='en-US' />
		<link rel="alternate" href='<%=  HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path).Replace("/en/","/ar/") %>' hreflang='ar-SA' />
	</ContentTemplate>
 </SharePoint:LanguageSpecificContent>