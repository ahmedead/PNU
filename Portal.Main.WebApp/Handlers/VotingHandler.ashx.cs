using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portal.Main.Helper;
using Newtonsoft.Json;
using System.Data;
using Microsoft.SharePoint;
using Newtonsoft.Json.Converters;
using System.Globalization;
using System.Web.Caching;
using Microsoft.SharePoint.Publishing;
using System.Text;

namespace Portal.Main.WebApp.Handlers
{
    /// <summary>
    /// Summary description for VotingHandler
    /// </summary>
    public class VotingHandler : IHttpHandler
    {

        #region Private Fields

        private const string XML_START_TAG = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";

        private HttpContext currContext;

        #endregion

        #region Public Methods

        public void ProcessRequest(HttpContext context)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion



    }
}