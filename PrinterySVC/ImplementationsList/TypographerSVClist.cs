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
            List<TypographerViewModel> result = source.Typographers
                .Select(rec => new TypographerViewModel
                {
                    Number = rec.Number,
                    TypographerFIO = rec.TypographerFIO
                })
                .ToList();
            return result;
        }

        public TypographerViewModel GetElement(int id)
        {
            Typographer element = source.Typographers.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                return new TypographerViewModel
                {
                    Number = element.Number,
                    TypographerFIO = element.TypographerFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(TypographerBildingModel model)
        {
            Typographer element = source.Typographers.FirstOrDefault(rec => rec.TypographerFIO == model.TypographerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            int maxNumber = source.Typographers.Count > 0 ? source.Typographers.Max(rec => rec.Number) : 0;
            source.Typographers.Add(new Typographer
            {
                Number = maxNumber + 1,
                TypographerFIO = model.TypographerFIO
            });
        }

        public void UpElement(TypographerBildingModel model)
        {
            Typographer element = source.Typographers.FirstOrDefault(rec =>
                                        rec.TypographerFIO == model.TypographerFIO && rec.Number != model.Number);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = source.Typographers.FirstOrDefault(rec => rec.Number == model.Number);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.TypographerFIO = model.TypographerFIO;
        }

        public void DelElement(int id)
        {
            Typographer element = source.Typographers.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                source.Typographers.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
