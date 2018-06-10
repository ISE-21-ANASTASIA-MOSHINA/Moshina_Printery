using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrinteryModel;

namespace PrinterySVC
{
    class SingletonDataList
    {
        private static SingletonDataList instance;

        public List<Booking> Bookings { get; set; }

        public List<Customer> Customers { get; set; }

        public List<Edition> Editions { get; set; }

        public List<EditionMaterial> EditionMaterials { get; set; }

        public List<Material> Materials { get; set; }

        public List<Rack> Racks { get; set; }

        public List<RackMaterial> RackMaterials { get; set; }

        public List<Typographer> Typographers { get; set; }

        private SingletonDataList()
        {
            Bookings = new List<Booking>();
            Customers = new List<Customer>();
            Editions = new List<Edition>();
            EditionMaterials = new List<EditionMaterial>();
            Materials = new List<Material>();
            Racks = new List<Rack>();
            RackMaterials = new List<RackMaterial>();
            Typographers = new List<Typographer>();
        }

        public static SingletonDataList GetInstance()
        {
            if (instance == null)
            {
                instance = new SingletonDataList();
            }

            return instance;
        }
    }
}

