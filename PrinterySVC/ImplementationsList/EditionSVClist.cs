using PrinteryModel;
using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrinterySVC.ImplementationsList
{
    public class EditionSVClist : IEditionSVC
    {
        private SingletonDataList source;

        public EditionSVClist()
        {
            source = SingletonDataList.GetInstance();
        }

        public List<EditionViewModel> GetList()
        {
            List<EditionViewModel> result = source.Editions
                .Select(rec => new EditionViewModel
                {
                    Number = rec.Number,
                    EditionName = rec.EditionName,
                    Coast = rec.Coast,
                    EditionMaterials = source.EditionMaterials
                            .Where(recPC => recPC.EditionNumber == rec.Number)
                            .Select(recPC => new EditionMaterialViewModel
                            {
                                Number = recPC.Number,
                                EditionNumber = recPC.EditionNumber,
                                MaterialNumber = recPC.MaterialNumber,
                                MaterialName = source.Materials
                                    .FirstOrDefault(recC => recC.Number == recPC.MaterialNumber)?.MaterialName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public EditionViewModel GetElement(int id)
        {
            Edition element = source.Editions.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                return new EditionViewModel
                {
                    Number = element.Number,
                    EditionName = element.EditionName,
                    Coast = element.Coast,
                    EditionMaterials = source.EditionMaterials
                            .Where(recPC => recPC.EditionNumber == element.Number)
                            .Select(recPC => new EditionMaterialViewModel
                            {
                                Number = recPC.Number,
                                EditionNumber = recPC.EditionNumber,
                                MaterialNumber = recPC.MaterialNumber,
                                MaterialName = source.Materials
                                        .FirstOrDefault(recC => recC.Number == recPC.MaterialNumber)?.MaterialName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

      

     

        public void DelElement(int id)
        {
            Edition element = source.Editions.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                // удаяем записи по компонентам при удалении изделия
                source.EditionMaterials.RemoveAll(rec => rec.EditionNumber == id);
                source.Editions.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public void AddElement(EditionBindingModel model)
        {
            Edition element = source.Editions.FirstOrDefault(rec => rec.EditionName == model.EditionName);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            int maxNumber = source.Editions.Count > 0 ? source.Editions.Max(rec => rec.Number) : 0;
            source.Editions.Add(new Edition
            {
                Number = maxNumber + 1,
                EditionName = model.EditionName,
                Coast = model.Coast
            });
            // компоненты для изделия
            int maxPCNumber = source.EditionMaterials.Count > 0 ?
                                    source.EditionMaterials.Max(rec => rec.Number) : 0;
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
                source.EditionMaterials.Add(new EditionMaterial
                {
                    Number = ++maxPCNumber,
                    EditionNumber = maxNumber + 1,
                    MaterialNumber = groupMaterial.MaterialNumber,
                    Count = groupMaterial.Count
                });
            }
        }

        public void UpdElement(EditionBindingModel model)
        {
            Edition element = source.Editions.FirstOrDefault(rec =>
                                        rec.EditionName == model.EditionName && rec.Number != model.Number);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            element = source.Editions.FirstOrDefault(rec => rec.Number == model.Number);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.EditionName = model.EditionName;
            element.Coast = model.Coast;

            int maxPCNumber = source.EditionMaterials.Count > 0 ? source.EditionMaterials.Max(rec => rec.Number) : 0;
            // обновляем существуюущие компоненты
            var compNumbers = model.EditionMaterials.Select(rec => rec.MaterialNumber).Distinct();
            var updateMaterials = source.EditionMaterials
                                            .Where(rec => rec.EditionNumber == model.Number &&
                                           compNumbers.Contains(rec.MaterialNumber));
            foreach (var updateMaterial in updateMaterials)
            {
                updateMaterial.Count = model.EditionMaterials
                                                .FirstOrDefault(rec => rec.Number == updateMaterial.Number).Count;
            }
            source.EditionMaterials.RemoveAll(rec => rec.EditionNumber == model.Number &&
                                       !compNumbers.Contains(rec.MaterialNumber));
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
                EditionMaterial elementPC = source.EditionMaterials
                                        .FirstOrDefault(rec => rec.EditionNumber == model.Number &&
                                                        rec.MaterialNumber == groupMaterial.MaterialNumber);
                if (elementPC != null)
                {
                    elementPC.Count += groupMaterial.Count;
                }
                else
                {
                    source.EditionMaterials.Add(new EditionMaterial
                    {
                        Number = ++maxPCNumber,
                        EditionNumber = model.Number,
                        MaterialNumber = groupMaterial.MaterialNumber,
                        Count = groupMaterial.Count
                    });
                }
            }
        }
    }
}
