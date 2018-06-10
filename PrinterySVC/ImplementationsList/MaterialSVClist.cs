using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractPrinteryModel;
using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;

namespace PrinterySVC.ImplementationsList
{
    public class MaterialSVClist : IMaterialSVC
    {
        private SingletonDataList source;

        public MaterialSVClist()
        {
            source = SingletonDataList.GetInstance();
        }

        public List<MaterialViewModel> GetList()
        {
            List<MaterialViewModel> result = source.Materials
                .Select(rec => new MaterialViewModel
                {
                    Number = rec.Number,
                    MaterialName = rec.MaterialName
                })
                .ToList();
            return result;
        }

        public MaterialViewModel GetElement(int id)
        {
            Material element = source.Materials.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                return new MaterialViewModel
                {
                    Number = element.Number,
                    MaterialName = element.MaterialName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(MaterialBindingModel model)
        {
            Material element = source.Materials.FirstOrDefault(rec => rec.MaterialName == model.MaterialName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            int maxNumber = source.Materials.Count > 0 ? source.Materials.Max(rec => rec.Number) : 0;
            source.Materials.Add(new Material
            {
                Number = maxNumber + 1,
                MaterialName = model.MaterialName
            });
        }

        public void UpElement(MaterialBindingModel model)
        {
            Material element = source.Materials.FirstOrDefault(rec =>
                                        rec.MaterialName == model.MaterialName && rec.Number != model.Number);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = source.Materials.FirstOrDefault(rec => rec.Number == model.Number);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.MaterialName = model.MaterialName;
        }

        public void DelElement(int id)
        {
            Material element = source.Materials.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                source.Materials.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
