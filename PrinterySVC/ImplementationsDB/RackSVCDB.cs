﻿using PrinteryModel;
using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrinterySVC.ImplementationsDB
{
    public class RackSVCDB : IRackSVC
    {
        private AbstractDbContext context;

        public RackSVCDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<RackViewModel> GetList()
        {
            List<RackViewModel> result = context.Racks
                .Select(rec => new RackViewModel
                {
                    Number = rec.Number,
                    RackName = rec.RackName,
                    RackMaterials = context.RackMaterials
                            .Where(recPC => recPC.RackNumber == rec.Number)
                            .Select(recPC => new RackMaterialViewModel
                            {
                                Number = recPC.Number,
                                RackNumber = recPC.RackNumber,
                                MaterialNumber = recPC.MaterialNumber,
                                MaterialName = recPC.Material.MaterialName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public RackViewModel GetElement(int id)
        {
            Rack element = context.Racks.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                return new RackViewModel
                {
                    Number = element.Number,
                    RackName = element.RackName,
                    RackMaterials = context.RackMaterials
                            .Where(recPC => recPC.RackNumber == element.Number)
                            .Select(recPC => new RackMaterialViewModel
                            {
                                Number = recPC.Number,
                                RackNumber = recPC.RackNumber,
                                MaterialNumber = recPC.MaterialNumber,
                                MaterialName = recPC.Material.MaterialName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(RackBindingModel model)
        {
            Rack element = context.Racks.FirstOrDefault(rec => rec.RackName == model.RackName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            context.Racks.Add(new Rack
            {
                RackName = model.RackName
            });
            context.SaveChanges();
        }

        public void UpdElement(RackBindingModel model)
        {
            Rack element = context.Racks.FirstOrDefault(rec =>
                                        rec.RackName == model.RackName && rec.Number != model.Number);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = context.Racks.FirstOrDefault(rec => rec.Number == model.Number);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.RackName = model.RackName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Rack element = context.Racks.FirstOrDefault(rec => rec.Number == id);
                    if (element != null)
                    {
                        // при удалении удаляем все записи о компонентах на удаляемом складе
                        context.RackMaterials.RemoveRange(
                                            context.RackMaterials.Where(rec => rec.RackNumber == id));
                        context.Racks.Remove(element);
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
