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
    public class CustomerSVClist: ICustomerSVC
    {
        private SingletonDataList source;

        public CustomerSVClist()
        {
            source = SingletonDataList.GetInstance();
        }

        public List<CustomerVievModel> GetList()
        {
            List<CustomerVievModel> result = source.Customers
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
            Customer element = source.Customers.FirstOrDefault(rec => rec.Number == id);
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
            Customer element = source.Customers.FirstOrDefault(rec => rec.CustomerFIO == model.CustomerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            int maxNumber = source.Customers.Count > 0 ? source.Customers.Max(rec => rec.Number) : 0;
            source.Customers.Add(new Customer
            {
                Number = maxNumber + 1,
                CustomerFIO = model.CustomerFIO
            });
        }

        public void UpElement(CustomerBindingModel model)
        {
            Customer element = source.Customers.FirstOrDefault(rec =>
                                    rec.CustomerFIO == model.CustomerFIO && rec.Number != model.Number);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = source.Customers.FirstOrDefault(rec => rec.Number == model.Number);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.CustomerFIO = model.CustomerFIO;
        }

        public void DelElement(int id)
        {
            Customer element = source.Customers.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                source.Customers.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
