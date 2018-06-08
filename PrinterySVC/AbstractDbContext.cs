using PrinteryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC
{
    [Table("AbstractDatabase")]
    public class AbstractDbContext : DbContext
    {
        public AbstractDbContext()
        {
            //настройки конфигурации для entity
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Material> Materials { get; set; }

        public virtual DbSet<Typographer> Typographers { get; set; }

        public virtual DbSet<Booking> Bookings { get; set; }

        public virtual DbSet<Edition> Editions { get; set; }

        public virtual DbSet<EditionMaterial> EditionMaterials { get; set; }

        public virtual DbSet<Rack> Racks { get; set; }

        public virtual DbSet<RackMaterial> RackMaterials { get; set; }
    }
}
