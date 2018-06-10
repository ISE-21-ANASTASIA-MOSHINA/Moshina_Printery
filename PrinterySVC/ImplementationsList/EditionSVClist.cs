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
                    Cost = rec.CostEdition,
                    EditionMaterials = source.EditionMaterials
                            .Where(recPC => recPC.EditionNamber == rec.Number)
                            .Select(recPC => new EditionMaterialViewModel
                            {
                                Number = recPC.Number,
                                EditionNamber = recPC.EditionNamber,
                                MaterialNamber = recPC.MaterialNamber,
                                MaterialName = source.Materials
                                    .FirstOrDefault(recC => recC.Number == recPC.MaterialNamber)?.MaterialName,
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
                    Cost = element.CostEdition,
                    EditionMaterials = source.EditionMaterials
                            .Where(recPC => recPC.EditionNamber == element.Number)
                            .Select(recPC => new EditionMaterialViewModel
                            {
                                Number = recPC.Number,
                                EditionNamber = recPC.EditionNamber,
                                MaterialNamber = recPC.MaterialNamber,
                                MaterialName = source.Materials
                                        .FirstOrDefault(recC => recC.Number == recPC.MaterialNamber)?.MaterialName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
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
                CostEdition = model.Coast
            });
            // компоненты для изделия
            int maxPCNumber = source.EditionMaterials.Count > 0 ?
                                    source.EditionMaterials.Max(rec => rec.Number) : 0;
            // убираем дубли по компонентам
            var groupMaterials = model.EditionMaterials
                                        .GroupBy(rec => rec.MaterialNamber)
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
                    EditionNamber = maxNumber + 1,
                    MaterialNamber = groupMaterial.MaterialNumber,
                    Count = groupMaterial.Count
                });
            }
        }

        public void UpElement(EditionBindingModel model)
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
            element.CostEdition = model.Coast;

            int maxPCNumber = source.EditionMaterials.Count > 0 ? source.EditionMaterials.Max(rec => rec.Number) : 0;
            // обновляем существуюущие компоненты
            var compNumbers = model.EditionMaterials.Select(rec => rec.MaterialNamber).Distinct();
            var updateMaterials = source.EditionMaterials
                                            .Where(rec => rec.EditionNamber == model.Number &&
                                           compNumbers.Contains(rec.MaterialNamber));
            foreach (var updateMaterial in updateMaterials)
            {
                updateMaterial.Count = model.EditionMaterials
                                                .FirstOrDefault(rec => rec.Number == updateMaterial.Number).Count;
            }
            source.EditionMaterials.RemoveAll(rec => rec.EditionNamber == model.Number &&
                                       !compNumbers.Contains(rec.MaterialNamber));
            // новые записи
            var groupMaterials = model.EditionMaterials
                                        .Where(rec => rec.Number == 0)
                                        .GroupBy(rec => rec.MaterialNamber)
                                        .Select(rec => new
                                        {
                                            MaterialNumber = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            foreach (var groupMaterial in groupMaterials)
            {
                EditionMaterial elementPC = source.EditionMaterials
                                        .FirstOrDefault(rec => rec.EditionNamber == model.Number &&
                                                        rec.MaterialNamber == groupMaterial.MaterialNumber);
                if (elementPC != null)
                {
                    elementPC.Count += groupMaterial.Count;
                }
                else
                {
                    source.EditionMaterials.Add(new EditionMaterial
                    {
                        Number = ++maxPCNumber,
                        EditionNamber = model.Number,
                        MaterialNamber = groupMaterial.MaterialNumber,
                        Count = groupMaterial.Count
                    });
                }
            }
        }

        public void DelElement(int id)
        {
            Edition element = source.Editions.FirstOrDefault(rec => rec.Number == id);
            if (element != null)
            {
                // удаяем записи по компонентам при удалении изделия
                source.EditionMaterials.RemoveAll(rec => rec.EditionNamber == id);
                source.Editions.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
