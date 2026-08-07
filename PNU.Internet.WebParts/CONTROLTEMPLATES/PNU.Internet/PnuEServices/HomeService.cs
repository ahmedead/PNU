using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PnuEServices
{
    public class HomeService
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string  Desc { get; set; }
        public List<Beneficiary> Beneficiaries { get; set; }
        public string ImageUrl { get; set; }
    }
}
