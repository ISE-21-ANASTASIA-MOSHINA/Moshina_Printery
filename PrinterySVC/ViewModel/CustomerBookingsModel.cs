using System.Runtime.Serialization;

namespace PrinterySVC.ViewModel
{
    [DataContract]
    public class CustomerBookingsModel
    {
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string DateCreate { get; set; }
        [DataMember]
        public string EditionName { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
        [DataMember]
        public string Status { get; set; }
    }
}
