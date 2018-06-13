using System.Runtime.Serialization;

namespace PrinterySVC.ViewModel
{
    [DataContract]
    public class BookingViewModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int CustomerNumber { get; set; }
        [DataMember]
        public string CustomerFIO { get; set; }
        [DataMember]
        public int EditionNumber { get; set; }
        [DataMember]
        public string EditionName { get; set; }
        [DataMember]
        public int? TypographerNumber { get; set; }
        [DataMember]
        public string TypographerName { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string DateCreate { get; set; }
        [DataMember]
        public string DateTypograph{ get; set; }
    }
}
