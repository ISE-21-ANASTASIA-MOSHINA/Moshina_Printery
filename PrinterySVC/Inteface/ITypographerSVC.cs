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
    [CustomInterface("Интерфейс для работы с работниками")]
    public interface ITypographerSVC
    {
        [CustomMethod("Метод получения списка работников")]
        List<TypographerViewModel> GetList();

        [CustomMethod("Метод получения списка работников")]
        TypographerViewModel GetElement(int number);

        [CustomMethod("Метод добавления работника")]
        void AddElement(TypographerBildingModel model);

        [CustomMethod("Метод изменения данных по работнику")]
        void UpElement(TypographerBildingModel model);

        [CustomMethod("Метод удаления работника")]
        void DelElement(int number);
    }
}