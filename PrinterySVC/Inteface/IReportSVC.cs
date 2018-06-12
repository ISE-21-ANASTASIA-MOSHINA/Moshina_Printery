using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System.Collections.Generic;

namespace PrinterySVC.Inteface
{
    public interface IReportSVC
    {
        void SaveProductPrice(ReportBindingModel model);

        List<RacksLoadViewModel> GetRacksLoad();

        void SaveRacksLoad(ReportBindingModel model);

        List<CustomerBookingsModel> GetCustomerOrders(ReportBindingModel model);

        void SaveCustomerOrders(ReportBindingModel model);
    }
}
