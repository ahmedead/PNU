using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public partial class ucInternalNafathEservices : ucEserviceBrow
    {
        // Show only items with TargetGroup = Visitor
        public ucInternalNafathEservices()
        {
            this.ExcludeVisitor = false;
            this.ShowVisitorOnly = true;
            this.NafathLogin = false;
        }

        protected override void OnInit(EventArgs e)
        {
            this.ExcludeVisitor = false;
            this.ShowVisitorOnly = true;
            this.NafathLogin = false;
            base.OnInit(e);
        }
    }
}
