using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public partial class ucInternalNafathEservices : ucEserviceBrow
    {
        public ucInternalNafathEservices()
        {
            this.ExcludeVisitor = false;
            this.ShowVisitorOnly = false;
            this.NafathLogin = true;
        }

        protected override void OnInit(EventArgs e)
        {
            this.ExcludeVisitor = false;
            this.ShowVisitorOnly = false;
            this.NafathLogin = true;
            base.OnInit(e);
        }
    }
}

