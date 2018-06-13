using System.Runtime.Serialization;

namespace PrinterySVC.BindingModel
{
    [DataContract]
    public class BookingBindingModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int CustomerNumber { get; set; }
        [DataMember]
        public int EditionNumber { get; set; }
        [DataMember]
        public int? TypographerNumber { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
    }
}
