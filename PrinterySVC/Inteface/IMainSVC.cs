using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;

namespace PrinterySVC.Inteface
{
    public interface IMainSVC
    {
        List<BookingViewModel> GetList();
        void CreateBooking(BookingBindingModel model);
        void TakeBookingInWork(BookingBindingModel model);
        void FinishBooking(int number);
        void PayBooking(int number);
        void PutMaterialOnRack(RackMaterialBindingModel model);
    }
}
