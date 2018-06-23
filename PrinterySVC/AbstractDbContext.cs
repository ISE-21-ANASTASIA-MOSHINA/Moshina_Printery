using AbstractPrinteryModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace PrinterySVC
{
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

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (Exception)
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Deleted:
                            entry.Reload();
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                    }
                }
                throw;
            }
        }
    }
}
