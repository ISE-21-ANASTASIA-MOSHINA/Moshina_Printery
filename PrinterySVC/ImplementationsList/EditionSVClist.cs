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
    public class EditionSVClist : IEditionSVC
    {
        private SingletonDataList source;

        public EditionSVClist()
        {
            source = SingletonDataList.GetInstance();
        }

        public List<EditionViewModel> GetList()
        {
            List<EditionViewModel> result = new List<EditionViewModel>();
            for (int i = 0; i < source.Editions.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<EditionMaterialViewModel> editionMaterial = new List<EditionMaterialViewModel>();
                for (int j = 0; j < source.EditionMaterials.Count; ++j)
                {
                    if (source.EditionMaterials[j].EditionNamber == source.Editions[i].Number)
                    {
                        string materialName = string.Empty;
                        for (int k = 0; k < source.Editions.Count; ++k)
                        {
                            if (source.EditionMaterials[j].EditionNamber == source.Editions[k].Number)
                            {
                                materialName = source.Editions[k].EditionName;
                                break;
                            }
                        }
                        editionMaterial.Add(new EditionMaterialViewModel
                        {
                            Number = source.EditionMaterials[j].Number,
                            EditionNamber = source.EditionMaterials[j].EditionNamber,
                            MaterialNamber = source.EditionMaterials[j].MaterialNamber,
                            MaterialName = materialName,
                            Count = source.EditionMaterials[j].Count
                        });
                    }
                }
                result.Add(new EditionViewModel
                {
                    Number = source.Editions[i].Number,
                    EditionName = source.Editions[i].EditionName,
                    Cost = source.Editions[i].CostEdition,
                    EditionMaterials = editionMaterial
                });
            }
            return result;
        }

        public EditionViewModel GetElement(int number)
        {
            for (int i = 0; i < source.Editions.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<EditionMaterialViewModel> editionMaterial = new List<EditionMaterialViewModel>();
                for (int j = 0; j < source.EditionMaterials.Count; ++j)
                {
                    if (source.EditionMaterials[j].EditionNamber == source.Editions[i].Number)
                    {
                        string materialName = string.Empty;
                        for (int k = 0; k < source.Editions.Count; ++k)
                        {
                            if (source.EditionMaterials[j].MaterialNamber == source.Materials[k].Number)
                            {
                                materialName = source.Materials[k].MaterialName;
                                break;
                            }
                        }
                        editionMaterial.Add(new EditionMaterialViewModel
                        {
                            Number = source.EditionMaterials[j].Number,
                            EditionNamber = source.EditionMaterials[j].EditionNamber,
                            MaterialNamber = source.EditionMaterials[j].MaterialNamber,
                            MaterialName = materialName,
                            Count = source.EditionMaterials[j].Count
                        });
                    }
                }
                if (source.Editions[i].Number == number)
                {
                    return new EditionViewModel
                    {
                        Number = source.Editions[i].Number,
                        EditionName = source.Editions[i].EditionName,
                        Cost = source.Editions[i].CostEdition,
                        EditionMaterials = editionMaterial
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }


        public void AddElement(EdiitionBindingModel model)
        {
            int maxNumber = 0;
            for (int i = 0; i < source.Editions.Count; ++i)
            {
                if (source.Editions[i].Number > maxNumber)
                {
                    maxNumber = source.Editions[i].Number;
                }
                if (source.Editions[i].EditionName == model.EditionName)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            source.Editions.Add(new Edition
            {
                Number = maxNumber + 1,
                EditionName = model.EditionName,
                CostEdition = model.Coast
            });
            // компоненты для изделия
            int maxPCId = 0;
            for (int i = 0; i < source.EditionMaterials.Count; ++i)
            {
                if (source.EditionMaterials[i].Number > maxPCId)
                {
                    maxPCId = source.EditionMaterials[i].Number;
                }
            }
            // убираем дубли по компонентам
            for (int i = 0; i < model.EditionMaterials.Count; ++i)
            {
                for (int j = 1; j < model.EditionMaterials.Count; ++j)
                {
                    if (model.EditionMaterials[i].MaterialNamber ==
                        model.EditionMaterials[j].MaterialNamber)
                    {
                        model.EditionMaterials[i].Count +=
                            model.EditionMaterials[j].Count;
                        model.EditionMaterials.RemoveAt(j--);
                    }

                }
            }
                // добавляем компоненты
                for (int i = 0; i < model.EditionMaterials.Count; ++i)
                {
                    source.EditionMaterials.Add(new EditionMaterial
                    {
                        Number = ++maxPCId,
                        EditionNamber = maxNumber + 1,
                        MaterialNamber = model.EditionMaterials[i].MaterialNamber,
                        Count = model.EditionMaterials[i].Count
                    });
                }
            }
        
        public void UpElement(EdiitionBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Editions.Count; ++i)
            {
                if (source.Editions[i].Number == model.Number)
                {
                    index = i;
                }
                if (source.Editions[i].EditionName == model.EditionName &&
                    source.Editions[i].Number != model.Number)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Editions[index].EditionName = model.EditionName;
            source.Editions[index].CostEdition = model.Coast;
            int maxPCId = 0;
            for (int i = 0; i < source.EditionMaterials.Count; ++i)
            {
                if (source.EditionMaterials[i].Number > maxPCId)
                {
                    maxPCId = source.EditionMaterials[i].Number;
                }
            }
            // обновляем существуюущие компоненты
            for (int i = 0; i < source.EditionMaterials.Count; ++i)
            {
                if (source.EditionMaterials[i].EditionNamber == model.Number)
                {
                    bool flag = true;
                    for (int j = 0; j < model.EditionMaterials.Count; ++j)
                    {
                        // если встретили, то изменяем количество
                        if (source.EditionMaterials[i].Number== model.EditionMaterials[j].Number)
                        {
                            source.EditionMaterials[i].Count = model.EditionMaterials[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    // если не встретили, то удаляем
                    if (flag)
                    {
                        source.EditionMaterials.RemoveAt(i--);
                    }
                }
            }
            // новые записи
            for (int i = 0; i < model.EditionMaterials.Count; ++i)
            {
                if (model.EditionMaterials[i].Number == 0)
                {
                    // ищем дубли
                    for (int j = 0; j < source.EditionMaterials.Count; ++j)
                    {
                        if (source.EditionMaterials[j].EditionNamber == model.Number &&
                            source.EditionMaterials[j].MaterialNamber == model.EditionMaterials[i].MaterialNamber  ) 
                        {
                            source.EditionMaterials[j].Count += model.EditionMaterials[i].Count;
                            model.EditionMaterials[i].Number= source.EditionMaterials[j].Number;
                            break;
                        }
                    }
                    // если не нашли дубли, то новая запись
                    if (model.EditionMaterials[i].Number == 0)
                    {
                        source.EditionMaterials.Add(new EditionMaterial
                        {
                            Number= ++maxPCId,
                            EditionNamber = model.Number,
                            MaterialNamber = model.EditionMaterials[i].MaterialNamber,
                            Count = model.EditionMaterials[i].Count
                        });
                    }
                }
            }
        }

        public void DelElement(int id)
        {
            // удаяем записи по компонентам при удалении изделия
            for (int i = 0; i < source.EditionMaterials.Count; ++i)
            {
                if (source.EditionMaterials[i].EditionNamber == id)
                {
                    source.EditionMaterials.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Editions.Count; ++i)
            {
                if (source.Editions[i].Number == id)
                {
                    source.Editions.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
