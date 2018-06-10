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
    public class CustomerSVClist: ICustomerSVC
    {
        private SingletonDataList source;

        public CustomerSVClist()
        {
            source = SingletonDataList.GetInstance();
        }

        public List<CustomerVievModel> GetList()
        {
            }
            throw new Exception("Элемент не найден");
        }

        public void UpElement(CustomerBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Customers.Count; ++i)
            {
                if (source.Customers[i].Number == model.Number)
                {
                    index = i;
                }
                if (source.Customers[i].CustomerFIO == model.CustomerFIO &&
                    source.Customers[i].Number != model.Number)
                {
                    throw new Exception("Уже есть клиент с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Customers[index].CustomerFIO = model.CustomerFIO;
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
