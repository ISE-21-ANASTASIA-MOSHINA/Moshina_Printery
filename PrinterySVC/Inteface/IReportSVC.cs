using PrinterySVC.Attributies;
using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.Inteface
{

    [CustomInterface("Интерфейс для работы с отчетами")]
    public interface IReportSVC
    {
        [CustomMethod("Метод сохранения списка изделий в doc-файл")]
        void SaveProductPrice(ReportBindingModel model);

        [CustomMethod("Метод получения списка складов с количество компонент на них")]
        List<RacksLoadViewModel> GetRacksLoad();

        [CustomMethod("Метод сохранения списка списка складов с количество компонент на них в xls-файл")]
        void SaveRacksLoad(ReportBindingModel model);

        [CustomMethod("Метод получения списка заказов клиентов")]
        List<CustomerBookcingsModel> GetCustomerOrders(ReportBindingModel model);

        [CustomMethod("Метод сохранения списка заказов клиентов в pdf-файл")]
        void SaveCustomerOrders(ReportBindingModel model);
    }
}
