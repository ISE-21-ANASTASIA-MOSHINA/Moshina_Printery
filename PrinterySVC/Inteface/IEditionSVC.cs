using PrinterySVC.Attributies;
using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System.Collections.Generic;

namespace PrinterySVC.Inteface
{
    [CustomInterface("Интерфейс для работы с изделиями")]

    public interface IEditionSVC
    {
        [CustomMethod("Метод получения списка изделий")]

        List<EditionViewModel> GetList();

        [CustomMethod("Метод получения изделия по id")]

        EditionViewModel GetElement(int number);

        [CustomMethod("Метод добавления изделия")]

        void AddElement(EditionBindingModel model);

        [CustomMethod("Метод изменения данных по изделию")]

        void UpElement(EditionBindingModel model);

        [CustomMethod("Метод удаления изделия")]

        void DelElement(int number);

    }
}
