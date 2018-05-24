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
            List<BookingViewModel> result = new List<BookingViewModel>();
            for (int i = 0; i < source.Bookings.Count; ++i)
            {
                string customerFIO = string.Empty;
                for (int j = 0; j < source.Customers.Count; ++j)
                {
                    if (source.Customers[j].Number == source.Bookings[i].CustomerNumber)
                    {
                        customerFIO = source.Customers[j].CustomerFIO;
                        break;
                    }
                }
                string editionName = string.Empty;
                for (int j = 0; j < source.Editions.Count; ++j)
                {
                    if (source.Editions[j].Number == source.Bookings[i].EditionNumber)
                    {
                        editionName = source.Editions[j].EditionName;
                        break;
                    }
                }
                string typographerFIO = string.Empty;
                if (source.Bookings[i].TypographerNumber.HasValue)
                {
                    for (int j = 0; j < source.Typographers.Count; ++j)
                    {
                        if (source.Typographers[j].Number == source.Bookings[i].TypographerNumber.Value)
                        {
                            typographerFIO = source.Typographers[j].TypographerFIO;
                            break;
                        }
                    }
                }
                result.Add(new BookingViewModel
                {
                    Number = source.Bookings[i].Number,
                    CustomerNumber = source.Bookings[i].CustomerNumber,
                    CustomerFIO = customerFIO,
                    EditionNumber = source.Bookings[i].EditionNumber,
                    EditionName = editionName,
                    TypographerNumber = source.Bookings[i].TypographerNumber,
                    TypographerName = typographerFIO,
                    Count = source.Bookings[i].Count,
                    Sum = source.Bookings[i].Sum,
                    DateCreate = source.Bookings[i].DateCreate.ToLongDateString(),
                    DateTypograph = source.Bookings[i].DateTypographer?.ToLongDateString(),
                    Status = source.Bookings[i].Status.ToString()
                });
            }
            return result;
        }

        public void CreateBooking(BookingBindingModel model)
        {
            int maxNumber = 0;
            for (int i = 0; i < source.Bookings.Count; ++i)
            {
                if (source.Bookings[i].Number > maxNumber)
                {
                    maxNumber = source.Customers[i].Number;
                }
            }
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
            int index = -1;
            for (int i = 0; i < source.Bookings.Count; ++i)
            {
                if (source.Bookings[i].Number == model.Number)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            for (int i = 0; i < source.EditionMaterials.Count; ++i)
            {
                if (source.EditionMaterials[i].EditionNamber == source.Bookings[index].EditionNumber)
                {
                    int countOnStocks = 0;
                    for (int j = 0; j < source.RackMaterials.Count; ++j)
                    {
                        if (source.RackMaterials[j].MaterialNamber == source.EditionMaterials[i].MaterialNamber)
                        {
                            countOnStocks += source.RackMaterials[j].Count;
                        }
                    }
                    if (countOnStocks < source.EditionMaterials[i].Count * source.Bookings[index].Count)
                    {
                        for (int j = 0; j < source.Materials.Count; ++j)
                        {
                            if (source.Materials[j].Number == source.EditionMaterials[i].MaterialNamber)
                            {
                                throw new Exception("Не достаточно компонента " + source.Materials[j].MaterialName +
                                    " требуется " + source.EditionMaterials[i].Count * source.Bookings[index].Count + ", в наличии " + countOnStocks);
                            }
                        }
                    }
                }
            }
            // списываем
            for (int i = 0; i < source.EditionMaterials.Count; ++i)
            {
                if (source.EditionMaterials[i].EditionNamber == source.Bookings[index].EditionNumber)
                {
                    int countOnStocks = source.EditionMaterials[i].Count * source.Bookings[index].Count;
                    for (int j = 0; j < source. RackMaterials.Count; ++j)
                    {
                        if (source.RackMaterials[j].MaterialNamber == source.EditionMaterials[i].MaterialNamber)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (source.RackMaterials[j].Count >= countOnStocks)
                            {
                                source.RackMaterials[j].Count -= countOnStocks;
                                break;
                            }
                            else
                            {
                                countOnStocks -= source.RackMaterials[j].Count;
                                source.RackMaterials[j].Count = 0;
                            }
                        }
                    }
                }
            }
            source.Bookings[index].TypographerNumber = model.TypographerNumber;
            source.Bookings[index].DateTypographer = DateTime.Now;
            source.Bookings[index].Status = BookingStatus.Выполняется;
        }

        public void FinishBooking(int number)
        {
            int index = -1;
            for (int i = 0; i < source.Bookings.Count; ++i)
            {
                if (source.Customers[i].Number == number)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Bookings[index].Status = BookingStatus.Готов;
        }

        public void PayBooking(int number)
        {
            int index = -1;
            for (int i = 0; i < source.Bookings.Count; ++i)
            {
                if (source.Customers[i].Number== number)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Bookings[index].Status = BookingStatus.Оплачен;
        }

        public void PutMaterialOnRack(RackMaterialBindingModel model)
        {
            int maxNumber= 0;
            for (int i = 0; i < source.RackMaterials.Count; ++i)
            {
                if (source.RackMaterials[i].RackNamber == model.RackNamber &&
                    source.RackMaterials[i].RackNamber == model.MaterialNamber)
                {
                    source.RackMaterials[i].Count += model.Count;
                    return;
                }
                if (source.RackMaterials[i].Namber > maxNumber)
                {
                    maxNumber = source.RackMaterials[i].Namber;
                }
            }
            source.RackMaterials.Add(new RackMaterial
            {
                Namber = ++maxNumber,
                RackNamber = model.RackNamber,
                MaterialNamber = model.MaterialNamber,
                Count = model.Count
            });
        }

    }
}
