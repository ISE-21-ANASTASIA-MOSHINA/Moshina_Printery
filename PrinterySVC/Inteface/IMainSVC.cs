using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrinterySVC.Attributies;
using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;

namespace PrinterySVC.Inteface
{
    [CustomInterface("Интерфейс для работы с заказами")]

    public interface IMainSVC
    {
        [CustomMethod("Метод получения списка заказов")]

        List<BookingViewModel> GetList();

        [CustomMethod("Метод создания заказа")]

        void CreateBooking(BookingBindingModel model);

        [CustomMethod("Метод передачи заказа в работу")]

        void TakeBookingInWork(BookingBindingModel model);

        [CustomMethod("Метод передачи заказа на оплату")]
        void FinishBooking(int number);

        [CustomMethod("Метод фиксирования оплаты по заказу")]
        void PayBooking(int number);

        [CustomMethod("Метод пополнения компонент на складе")]
        void PutMaterialOnRack(RackMaterialBindingModel model);
    }
}