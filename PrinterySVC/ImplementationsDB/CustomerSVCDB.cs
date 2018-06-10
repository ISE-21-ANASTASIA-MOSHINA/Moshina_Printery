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
    public class CustomerSVCDB : ICustomerSVC
    {
        private AbstractDbContext context;

        public CustomerSVCDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<CustomerVievModel> GetList()
        {
            List<CustomerVievModel> result = context.Customers
                .Select(rec => new CustomerVievModel
                {
                    Number = rec.Number,
                    CustomerFIO = rec.CustomerFIO
                })
                .ToList();
            return result;
        }

        public CustomerVievModel GetElement(int id)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                return new CustomerVievModel
                {
                    Number = element.Number,
                    CustomerFIO = element.CustomerFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(CustomerBindingModel model)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.CustomerFIO == model.CustomerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            context.Customers.Add(new Customer
            {
                CustomerFIO = model.CustomerFIO
            });
            context.SaveChanges();
        }

        public void UpElement(CustomerBindingModel model)
        {
            Customer element = context.Customers.FirstOrDefault(rec =>
                                    rec.CustomerFIO == model.CustomerFIO && rec.Number != model.Number);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = context.Customers.FirstOrDefault(rec => rec.Number == model.Number);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.CustomerFIO = model.CustomerFIO;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                context.Customers.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
