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
    public class RackSVClist : IRackSVC
    {
        private SingletonDataList source;

        public RackSVClist()
        {
            source = SingletonDataList.GetInstance();
        }

        public List<RackViewModel> GetList()
        {
            List<RackViewModel> result = new List<RackViewModel>();
            for (int i = 0; i < source.Racks.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<RackMaterialViewModel> RackMaterial = new List<RackMaterialViewModel>();
                for (int j = 0; j < source.RackMaterials.Count; ++j)
                {
                    if (source.RackMaterials[j].RackNamber == source.Racks[i].Number)
                    {
                        string materialName = string.Empty;
                        for (int k = 0; k < source.Materials.Count; ++k)
                        {
                            if (source.EditionMaterials[j].MaterialNamber == source.Materials[k].Number)
                            {
                                materialName = source.Materials[k].MaterialName;
                                break;
                            }
                        }
                        RackMaterial.Add(new RackMaterialViewModel
                        {
                            Namber = source.RackMaterials[j].Namber,
                            RackNamber = source.RackMaterials[j].RackNamber,
                            MaterialNameber = source.RackMaterials[j].MaterialNamber,
                            MaterialName = materialName,
                            Count = source.RackMaterials[j].Count
                        });
                    }
                }
                result.Add(new RackViewModel
                {
                    Number = source.Racks[i].Number,
                    RackName = source.Racks[i].RackName,
                    RackMaterial = RackMaterial
                });
            }
            return result;
        }

        public RackViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Racks.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<RackMaterialViewModel> RackMaterial = new List <RackMaterialViewModel>();
                for (int j = 0; j < source.RackMaterials.Count; ++j)
                {
                    if (source.RackMaterials[j].RackNamber == source.Racks[i].Number)
                    {
                        string materialName = string.Empty;
                        for (int k = 0; k < source.Materials.Count; ++k)
                        {
                            if (source.EditionMaterials[j].MaterialNamber == source.Materials[k].Number)
                            {
                                materialName = source.Materials[k].MaterialName;
                                break;
                            }
                        }
                        RackMaterial.Add(new RackMaterialViewModel
                        {
                            Namber = source.RackMaterials[j].Namber,
                            RackNamber = source.RackMaterials[j].RackNamber,
                            MaterialNameber = source.RackMaterials[j].MaterialNamber,
                            MaterialName = materialName,
                            Count = source.RackMaterials[j].Count
                        });
                    }
                }
                if (source.Racks[i].Number == id)
                {
                    return new RackViewModel
                    {
                        Number = source.Racks[i].Number,
                        RackName = source.Racks[i].RackName,
                        RackMaterial = RackMaterial
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(RackBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Racks.Count; ++i)
            {
                if (source.Racks[i].Number > maxId)
                {
                    maxId = source.Racks[i].Number;
                }
                if (source.Racks[i].RackName == model.RackName)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            source.Racks.Add(new Rack
            {
                Number= maxId + 1,
                RackName = model.RackName
            });
        }

        public void UpElement(RackBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Racks.Count; ++i)
            {
                if (source.Racks[i].Number == model.Number)
                {
                    index = i;
                }
                if (source.Racks[i].RackName == model.RackName &&
                    source.Racks[i].Number != model.Number)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Racks[index].RackName = model.RackName;
        }

        public void DelElement(int id)
        {
            // при удалении удаляем все записи о компонентах на удаляемом складе
            for (int i = 0; i < source.RackMaterials.Count; ++i)
            {
                if (source.RackMaterials[i].RackNamber == id)
                {
                    source.RackMaterials.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Racks.Count; ++i)
            {
                if (source.Racks[i].Number== id)
                {
                    source.Racks.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
