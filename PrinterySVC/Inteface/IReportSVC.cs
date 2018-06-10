using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.Inteface
{
    public interface IReportSVC
    {
        void SaveProductPrice(ReportBindingModel model);

        List<RacksLoadViewModel> GetRacksLoad();

        void SaveRacksLoad(ReportBindingModel model);

        List<CustomerBookcingsModel> GetCustomerOrders(ReportBindingModel model);

        void SaveCustomerOrders(ReportBindingModel model);
    }
}
