using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PnuEServices
{

   public  class ServiceDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ImageUrl { get; set; }
        public List<Beneficiary> Beneficiaries { get; set; }
        public List<Condition> Conditions { get; set; }
        public List<Document> Documents { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
    [Serializable]
    public class Condition
    {
        public int Id { get; set; }
        public string Title  { get; set; }
        public int Order { get; set; }
    }
    [Serializable]
    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
    }
    [Serializable]
    public class Schedule
    {
        public int Id { get; set; }
        public string  Title { get; set; }
        public string Day { get; set; }
        public string  Date { get; set; }
        public int Order { get; set; }
    }
}
