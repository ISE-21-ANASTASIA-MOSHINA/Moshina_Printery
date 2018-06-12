using PrinteryModel;
using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrinterySVC.ImplementationsDB
{
    public class EditionSVCDB : IEditionSVC
    {
        private AbstractDbContext context;

        public EditionSVCDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<EditionViewModel> GetList()
        {
            List<EditionViewModel> result = context.Editions
                .Select(rec => new EditionViewModel
                {
                    Number = rec.Number,
                    EditionName = rec.EditionName,
                    Coast = rec.Coast,
                    EditionMaterials = context.EditionMaterials
                            .Where(recPC => recPC.EditionNumber == rec.Number)
                            .Select(recPC => new EditionMaterialViewModel
                            {
                                Number = recPC.Number,
                                EditionNumber = recPC.EditionNumber,
                                MaterialNumber = recPC.MaterialNumber,
                                MaterialName = recPC.Material.MaterialName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public EditionViewModel GetElement(int id)
        {
            Edition element = context.Editions.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                return new EditionViewModel
                {
                    Number = element.Number,
                    EditionName = element.EditionName,
                    Coast = element.Coast,
                    EditionMaterials = context.EditionMaterials
                            .Where(recPC => recPC.EditionNumber == element.Number)
                            .Select(recPC => new EditionMaterialViewModel
                            {
                                Number = recPC.Number,
                                EditionNumber = recPC.EditionNumber,
                                MaterialNumber = recPC.MaterialNumber,
                                MaterialName = recPC.Material.MaterialName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(EditionBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Edition element = context.Editions.FirstOrDefault(rec => rec.EditionName == model.EditionName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new Edition
                    {
                        EditionName = model.EditionName,
                        Coast = model.Coast
                    };
                    context.Editions.Add(element);
                    context.SaveChanges();
                    // убираем дубли по компонентам
                    var groupMaterials = model.EditionMaterials
                                                .GroupBy(rec => rec.MaterialNumber)
                                                .Select(rec => new
                                                {
                                                    MaterialNumber = rec.Key,
                                                    Count = rec.Sum(r => r.Count)
                                                });
                    // добавляем компоненты
                    foreach (var groupMaterial in groupMaterials)
                    {
                        context.EditionMaterials.Add(new EditionMaterial
                        {
                            EditionNumber = element.Number,
                            MaterialNumber = groupMaterial.MaterialNumber,
                            Count = groupMaterial.Count
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdElement(EditionBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Edition element = context.Editions.FirstOrDefault(rec =>
                                        rec.EditionName == model.EditionName && rec.Number != model.Number);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context.Editions.FirstOrDefault(rec => rec.Number == model.Number);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.EditionName = model.EditionName;
                    element.Coast = model.Coast;
                    context.SaveChanges();

                    // обновляем существуюущие компоненты
                    var compNumbers = model.EditionMaterials.Select(rec => rec.MaterialNumber).Distinct();
                    var updateMaterials = context.EditionMaterials
                                                    .Where(rec => rec.EditionNumber == model.Number &&
                                                        compNumbers.Contains(rec.MaterialNumber));
                    foreach (var updateMaterial in updateMaterials)
                    {
                        updateMaterial.Count = model.EditionMaterials
                                                        .FirstOrDefault(rec => rec.Number == updateMaterial.Number).Count;
                    }
                    context.SaveChanges();
                    context.EditionMaterials.RemoveRange(
                                        context.EditionMaterials.Where(rec => rec.EditionNumber == model.Number &&
                                                                            !compNumbers.Contains(rec.MaterialNumber)));
                    context.SaveChanges();
                    // новые записи
                    var groupMaterials = model.EditionMaterials
                                                .Where(rec => rec.Number == 0)
                                                .GroupBy(rec => rec.MaterialNumber)
                                                .Select(rec => new
                                                {
                                                    MaterialNumber = rec.Key,
                                                    Count = rec.Sum(r => r.Count)
                                                });
                    foreach (var groupMaterial in groupMaterials)
                    {
                        EditionMaterial elementPC = context.EditionMaterials
                                                .FirstOrDefault(rec => rec.EditionNumber == model.Number &&
                                                                rec.MaterialNumber == groupMaterial.MaterialNumber);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupMaterial.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.EditionMaterials.Add(new EditionMaterial
                            {
                                EditionNumber = model.Number,
                                MaterialNumber = groupMaterial.MaterialNumber,
                                Count = groupMaterial.Count
                            });
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Edition element = context.Editions.FirstOrDefault(rec => rec.Number == id);
                    if (element != null)
                    {
                        // удаяем записи по компонентам при удалении изделия
                        context.EditionMaterials.RemoveRange(
                                            context.EditionMaterials.Where(rec => rec.EditionNumber == id));
                        context.Editions.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }




    }
}
