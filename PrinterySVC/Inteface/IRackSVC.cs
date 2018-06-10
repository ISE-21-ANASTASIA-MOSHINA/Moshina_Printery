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
    [CustomInterface("Интерфейс для работы со складами")]
    public interface IRackSVC
    {
        [CustomMethod("Метод получения списка складов")]
        List<RackViewModel> GetList();

        [CustomMethod("Метод получения склада по id")]
        RackViewModel GetElement(int number);

        [CustomMethod("Метод добавления склада")]
        void AddElement(RackBindingModel model);

        [CustomMethod("Метод изменения данных по складу")]
        void UpElement(RackBindingModel model);

        [CustomMethod("Метод удаления склада")]
        void DelElement(int number);
    }
}
