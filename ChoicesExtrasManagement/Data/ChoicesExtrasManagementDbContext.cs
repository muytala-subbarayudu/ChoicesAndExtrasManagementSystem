using ChoicesExtrasManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ChoicesExtrasManagement.Data
{
    public class ChoicesExtrasManagementDbContext : IdentityDbContext<AppUser>
    {
        public ChoicesExtrasManagementDbContext(DbContextOptions<ChoicesExtrasManagementDbContext> options) : base(options)
        {
        
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<PlotType> PlotTypes { get; set; }
        public DbSet<Plot> Plots { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<PlotTypeRoomMapping> PlotTypeRoomMappings { get; set; }
        public DbSet<Catalogue> Catalogues { get; set; }
        public DbSet<SubCatalogue> SubCatalogues { get; set; }
        public DbSet<CatalogueRoomPlotTypeMapping> CatalogueRoomPlotTypeMappings { get; set; }
        public DbSet<SavedChoice> SavedChoices { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<SavedExtra> SavedExtras { get; set; }     
        public DbSet<Notification> Notifications { get; set; }
    }
}
