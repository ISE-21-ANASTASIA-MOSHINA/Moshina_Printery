using PrinteryModel;
using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.ImplementationsList
{
    public class TypographerSVClist : ITypographerSVC
    {
        private SingletonDataList source;

        public TypographerSVClist()
        {
            source = SingletonDataList.GetInstance();
        }

        public List<TypographerViewModel> GetList()
        {
            List<TypographerViewModel> result = new List<TypographerViewModel>();
            for (int i = 0; i < source.Typographers.Count; ++i)
            {
                result.Add(new TypographerViewModel
                {
                    Number = source.Typographers[i].Number,
                    TypographerFIO = source.Typographers[i].TypographerFIO
                });
            }
            return result;
        }

        public TypographerViewModel GetElement(int number)
        {
            for (int i = 0; i < source.Typographers.Count; ++i)
            {
                if (source.Typographers[i].Number == number)
                {
                    return new TypographerViewModel
                    {
                        Number = source.Typographers[i].Number,
                        TypographerFIO = source.Typographers[i].TypographerFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(TypographerBildingModel model)
        {
            int maxNumber = 0;
            for (int i = 0; i < source.Typographers.Count; ++i)
            {
                if (source.Typographers[i].Number > maxNumber)
                {
                    maxNumber = source.Typographers[i].Number;
                }
                if (source.Typographers[i].TypographerFIO == model.TypographerFIO)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            source.Typographers.Add(new Typographer
            {
                Number = maxNumber + 1,
                TypographerFIO = model.TypographerFIO
            });
        }

        public void UpElement(TypographerBildingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Typographers.Count; ++i)
            {
                if (source.Typographers[i].Number == model.Number)
                {
                    index = i;
                }
                if (source.Typographers[i].TypographerFIO == model.TypographerFIO &&
                    source.Typographers[i].Number != model.Number)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Typographers[index].TypographerFIO = model.TypographerFIO;
        }

        public void DelElement(int number)
        {
            for (int i = 0; i < source.Typographers.Count; ++i)
            {
                if (source.Typographers[i].Number == number)
                {
                    source.Typographers.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
