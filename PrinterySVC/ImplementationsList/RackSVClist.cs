using AbstractPrinteryModel;
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
    public class RackSVClist : IRackSVC
    {
        private SingletonDataList source;

        public RackSVClist()
        {
            source = SingletonDataList.GetInstance();
        }

        public List<RackViewModel> GetList()
        {
            List<RackViewModel> result = source.Racks
                .Select(rec => new RackViewModel
                {
                    Number = rec.Number,
                    RackName = rec.RackName,
                    RackMaterial = source.RackMaterials
                            .Where(recPC => recPC.RackNamber == rec.Number)
                            .Select(recPC => new RackMaterialViewModel
                            {
                                Namber = recPC.Namber,
                                RackNamber = recPC.RackNamber,
                                MaterialNameber = recPC.MaterialNamber,
                                MaterialName = source.Materials
                                    .FirstOrDefault(recC => recC.Number == recPC.MaterialNamber)?.MaterialName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public RackViewModel GetElement(int id)
        {
            Rack element = source.Racks.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                return new RackViewModel
                {
                    Number = element.Number,
                    RackName = element.RackName,
                    RackMaterial = source.RackMaterials
                            .Where(recPC => recPC.RackNamber == element.Number)
                            .Select(recPC => new RackMaterialViewModel
                            {
                                Namber = recPC.Namber,
                                RackNamber = recPC.RackNamber,
                                MaterialNameber = recPC.MaterialNamber,
                                MaterialName = source.Materials
                                    .FirstOrDefault(recC => recC.Number == recPC.MaterialNamber)?.MaterialName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(RackBindingModel model)
        {
            Rack element = source.Racks.FirstOrDefault(rec => rec.RackName == model.RackName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            int maxNumber = source.Racks.Count > 0 ? source.Racks.Max(rec => rec.Number) : 0;
            source.Racks.Add(new Rack
            {
                Number = maxNumber + 1,
                RackName = model.RackName
            });
        }

        public void UpElement(RackBindingModel model)
        {
            Rack element = source.Racks.FirstOrDefault(rec =>
                                        rec.RackName == model.RackName && rec.Number != model.Number);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = source.Racks.FirstOrDefault(rec => rec.Number == model.Number);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.RackName = model.RackName;
        }

        public void DelElement(int id)
        {
            Rack element = source.Racks.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                // при удалении удаляем все записи о компонентах на удаляемом складе
                source.RackMaterials.RemoveAll(rec => rec.RackNamber == id);
                source.Racks.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
