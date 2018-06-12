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
    public class MainSVClist : IMainSVC
    {
        private SingletonDataList source;

        public MainSVClist()
        {
            source = SingletonDataList.GetInstance();
        }

        public List<BookingViewModel> GetList()
        {
            List<BookingViewModel> result = source.Bookings
                .Select(rec => new BookingViewModel
                {
                    Number = rec.Number,
                    CustomerNumber = rec.CustomerNumber,
                    EditionNumber = rec.EditionNumber,
                    TypographerNumber = rec.TypographerNumber,
                    DateCreate = rec.DateCreate.ToLongDateString(),
                    DateTypograph = rec.DateTypographer?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = source.Customers
                                    .FirstOrDefault(recC => recC.Number == rec.CustomerNumber)?.CustomerFIO,
                    EditionName = source.Editions
                                    .FirstOrDefault(recP => recP.Number == rec.EditionNumber)?.EditionName,
                    TypographerName = source.Typographers
                                    .FirstOrDefault(recI => recI.Number == rec.TypographerNumber)?.TypographerFIO
                })
                .ToList();
            return result;
        }

        public void CreateBooking(BookingBindingModel model)
        {
            int maxNumber = source.Bookings.Count > 0 ? source.Bookings.Max(rec => rec.Number) : 0;
            source.Bookings.Add(new Booking
            {
                Number = maxNumber + 1,
                CustomerNumber = model.CustomerNumber,
                EditionNumber = model.EditionNumber,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = BookingStatus.Принят
            });
        }

        public void TakeBookingInWork(BookingBindingModel model)
        {
            Booking element = source.Bookings.FirstOrDefault(rec => rec.Number == model.Number);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            var productMaterials = source.EditionMaterials.Where(rec => rec.EditionNumber == element.EditionNumber);
            foreach (var productMaterial in productMaterials)
            {
                int countOnRacks = source.RackMaterials
                                            .Where(rec => rec.MaterialNumber == productMaterial.MaterialNumber)
                                            .Sum(rec => rec.Count);
                if (countOnRacks < productMaterial.Count * element.Count)
                {
                    var componentName = source.Materials
                                    .FirstOrDefault(rec => rec.Number == productMaterial.MaterialNumber);
                    throw new Exception("Не достаточно компонента " + componentName?.MaterialName +
                        " требуется " + productMaterial.Count + ", в наличии " + countOnRacks);
                }
            }
            // списываем
            foreach (var productMaterial in productMaterials)
            {
                int countOnRacks = productMaterial.Count * element.Count;
                var stockMaterials = source.RackMaterials
                                            .Where(rec => rec.MaterialNumber == productMaterial.MaterialNumber);
                foreach (var stockMaterial in stockMaterials)
                {
                    // компонентов на одном слкаде может не хватать
                    if (stockMaterial.Count >= countOnRacks)
                    {
                        stockMaterial.Count -= countOnRacks;
                        break;
                    }
                    else
                    {
                        countOnRacks -= stockMaterial.Count;
                        stockMaterial.Count = 0;
                    }
                }
            }
            element.TypographerNumber = model.TypographerNumber;
            element.DateTypographer = DateTime.Now;
            element.Status = BookingStatus.Выполняется;
        }

        public void FinishBooking(int id)
        {
            Booking element = source.Bookings.FirstOrDefault(rec => rec.Number == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = BookingStatus.Готов;
        }

        public void PayBooking(int id)
        {
            Booking element = source.Bookings.FirstOrDefault(rec => rec.Number == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = BookingStatus.Оплачен;
        }

        public void PutMaterialOnRack(RackMaterialBindingModel model)
        {
            RackMaterial element = source.RackMaterials
                                                .FirstOrDefault(rec => rec.RackNumber == model.RackNumber &&
                                                                    rec.MaterialNumber == model.MaterialNumber);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxNumber = source.RackMaterials.Count > 0 ? source.RackMaterials.Max(rec => rec.Number) : 0;
                source.RackMaterials.Add(new RackMaterial
                {
                    Number = ++maxNumber,
                    RackNumber = model.RackNumber,
                    MaterialNumber = model.MaterialNumber,
                    Count = model.Count
                });
            }
        }

    }
}
