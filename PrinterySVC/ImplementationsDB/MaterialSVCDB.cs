﻿using AbstractPrinteryModel;
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
    public class MaterialSVCDB : IMaterialSVC
    {
        private AbstractDbContext context;

        public MaterialSVCDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<MaterialViewModel> GetList()
        {
            List<MaterialViewModel> result = context.Materials
                .Select(rec => new MaterialViewModel
                {
                    Number = rec.Number,
                    MaterialName = rec.MaterialName
                })
                .ToList();
            return result;
        }

        public MaterialViewModel GetElement(int id)
        {
            Material element = context.Materials.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                return new MaterialViewModel
                {
                    Number = element.Number,
                    MaterialName = element.MaterialName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(MaterialBindingModel model)
        {
            Material element = context.Materials.FirstOrDefault(rec => rec.MaterialName == model.MaterialName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            context.Materials.Add(new Material
            {
                MaterialName = model.MaterialName
            });
            context.SaveChanges();
        }

        public void UpElement(MaterialBindingModel model)
        {
            Material element = context.Materials.FirstOrDefault(rec =>
                                        rec.MaterialName == model.MaterialName && rec.Number != model.Number);
            if (element != null)
            {
                throw new Exception("Уже есть материал с таким названием");
            }
            element = context.Materials.FirstOrDefault(rec => rec.Number == model.Number);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.MaterialName = model.MaterialName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Material element = context.Materials.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                context.Materials.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
