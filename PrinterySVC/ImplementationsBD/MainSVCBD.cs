using PrinteryModel;
using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Data.Entity;


namespace PrinterySVC.ImplementationsBD
{
    public class MainSVCBD : IMainSVC
    {

        private AbstractDbContext context;

        public MainSVCBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<BookingViewModel> GetList()
        {
            List<BookingViewModel> result = context.Bookings
                .Select(rec => new BookingViewModel
                {
                    Number = rec.Number,
                    CustomerNumber = rec.CustomerNumber,
                    EditionNumber = rec.EditionNumber,
                    TypographerNumber = rec.TypographerNumber,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                    DateTypograph = rec.DateTypographer == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateTypographer.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateTypographer.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateTypographer.Value),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = rec.Customer.CustomerFIO,
                    EditionName = rec.Edition.EditionName,
                    TypographerName = rec.Typographer.TypographerFIO
                })
                .ToList();
            return result;
        }

        public void CreateBooking(BookingBindingModel model)
        {
            context.Bookings.Add(new Booking
            {
                CustomerNumber = model.CustomerNumber,
                EditionNumber = model.EditionNumber,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = BookingStatus.Принят
            });
            context.SaveChanges();
        }

        public void TakeBookingInWork(BookingBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    Booking element = context.Bookings.FirstOrDefault(rec => rec.Number == model.Number);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var editionMaterials = context.EditionMaterials
                                                .Include(rec => rec.Material)
                                                .Where(rec => rec.EditionNumber == element.EditionNumber);
                    // списываем
                    foreach (var editionMaterial in editionMaterials)
                    {
                        int countOnRacks = editionMaterial.Count * element.Count;
                        var rackMaterials = context.RackMaterials
                                                    .Where(rec => rec.MaterialNumber == editionMaterial.MaterialNumber);
                        foreach (var rackMaterial in rackMaterials)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (rackMaterial.Count >= countOnRacks)
                            {
                                rackMaterial.Count -= countOnRacks;
                                countOnRacks = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnRacks -= rackMaterial.Count;
                                rackMaterial.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnRacks > 0)
                        {
                            throw new Exception("Не достаточно ингредиента " +
                                editionMaterial.Material.MaterialName + " требуется " +
                                editionMaterial.Count + ", не хватает " + countOnRacks);
                        }
                    }
                    element.TypographerNumber = model.TypographerNumber;
                    element.DateTypographer = DateTime.Now;
                    element.Status = BookingStatus.Выполняется;
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void FinishBooking(int id)
        {
            Booking element = context.Bookings.FirstOrDefault(rec => rec.Number == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = BookingStatus.Готов;
            context.SaveChanges();
        }

        public void PayBooking(int id)
        {
            Booking element = context.Bookings.FirstOrDefault(rec => rec.Number == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = BookingStatus.Оплачен;
            context.SaveChanges();
        }

        public void PutMaterialOnRack(RackMaterialBindingModel model)
        {
            RackMaterial element = context.RackMaterials
                                                .FirstOrDefault(rec => rec.RackNumber == model.RackNamber &&
                                                                    rec.MaterialNumber == model.MaterialNamber);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context.RackMaterials.Add(new RackMaterial
                {
                    RackNumber = model.RackNamber,
                    MaterialNumber = model.MaterialNamber,
                    Count = model.Count
                });
            }
            context.SaveChanges();
        }

    }
}
