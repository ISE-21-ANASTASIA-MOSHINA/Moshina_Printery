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
    [CustomInterface("Интерфейс для работы с клиентами")]
    public interface ICustomerSVC
    {
        [CustomMethod("Метод получения списка клиентов")]
        List<CustomerVievModel> GetList();

        [CustomMethod("Метод получения клиента по id")]
        CustomerVievModel GetElement(int number);

        [CustomMethod("Метод добавления клиента")]
        void AddElement(CustomerBindingModel model);

        [CustomMethod("Метод изменения данных по клиенту")]
        void UpElement(CustomerBindingModel model);

        [CustomMethod("Метод удаления клиента")]
        void DelElement(int number);
    }
}
