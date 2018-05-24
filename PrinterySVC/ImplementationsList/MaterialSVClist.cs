using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrinteryModel;
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
            List<MaterialViewModel> result = new List<MaterialViewModel>();
            for (int i = 0; i < source.Materials.Count; ++i)
            {
                result.Add(new MaterialViewModel
                {
                    Number = source.Materials[i].Number,
                    MaterialName = source.Materials[i].MaterialName
                });
            }
            return result;
        }

        public MaterialViewModel GetElement(int number)
        {
            for (int i = 0; i < source.Materials.Count; ++i)
            {
                if (source.Materials[i].Number == number)
                {
                    return new MaterialViewModel
                    {
                        Number = source.Materials[i].Number,
                        MaterialName = source.Materials[i].MaterialName
                    };
                }
            }
            throw new Exception("Материал не найден");
        }

        public void AddElement(MaterialBindingModel model)
        {
            int maxNumber = 0;
            for (int i = 0; i < source.Materials.Count; ++i)
            {
                if (source.Materials[i].Number > maxNumber)
                {
                    maxNumber = source.Materials[i].Number;
                }
                if (source.Materials[i].MaterialName == model.MaterialName)
                {
                    throw new Exception("Уже есть материал с таким названием");
                }
            }
            source.Materials.Add(new Material
            {
                Number = maxNumber + 1,
                MaterialName = model.MaterialName
            });
        }

        public void UpElement(MaterialBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Materials.Count; ++i)
            {
                if (source.Materials[i].Number == model.Number)
                {
                    index = i;
                }
                if (source.Materials[i].MaterialName == model.MaterialName &&
                    source.Materials[i].Number != model.Number)
                {
                    throw new Exception("Уже есть материал с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Materials[index].MaterialName = model.MaterialName;
        }

        public void DelElement(int number)
        {
            for (int i = 0; i < source.Materials.Count; ++i)
            {
                if (source.Materials[i].Number == number)
                {
                    source.Materials.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Материал не найден");
        }
    }
}
