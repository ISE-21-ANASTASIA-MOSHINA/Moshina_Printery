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

    [CustomInterface("Интерфейс для работы с компонентами")]
    public interface IMaterialSVC
    {
        [CustomMethod("Метод получения списка компонент")]
        List<MaterialViewModel> GetList();

        [CustomMethod("Метод получения компонента по id")]
        MaterialViewModel GetElement(int number);

        [CustomMethod("Метод добавления компонента")]
        void AddElement(MaterialBindingModel model);

        [CustomMethod("Метод изменения данных по компоненту")]
        void UpElement(MaterialBindingModel model);

        [CustomMethod("Метод удаления компонента")]
        void DelElement(int number);

    }
}
