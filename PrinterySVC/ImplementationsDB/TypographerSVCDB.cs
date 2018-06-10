using AbstractPrinteryModel;
using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.ImplementationsDB
{
    public class TypographerSVCDB : ITypographerSVC
    {
        private AbstractDbContext context;

        public TypographerSVCDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<TypographerViewModel> GetList()
        {
            List<TypographerViewModel> result = context.Typographers
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
            Typographer element = context.Typographers.FirstOrDefault(rec => rec.Number == id);
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
            Typographer element = context.Typographers.FirstOrDefault(rec => rec.TypographerFIO == model.TypographerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            context.Typographers.Add(new Typographer
            {
                TypographerFIO = model.TypographerFIO
            });
            context.SaveChanges();
        }

        public void UpElement(TypographerBildingModel model)
        {
            Typographer element = context.Typographers.FirstOrDefault(rec =>
                                        rec.TypographerFIO == model.TypographerFIO && rec.Number != model.Number);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = context.Typographers.FirstOrDefault(rec => rec.Number == model.Number);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.TypographerFIO = model.TypographerFIO;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Typographer element = context.Typographers.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                context.Typographers.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

    }
}
