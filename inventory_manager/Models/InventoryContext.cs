using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace inventory_manager.Models
{
    public class InventoryContext : DbContext
    {
        public InventoryContext (DbContextOptions<InventoryContext> options) : base (options)
        {

        }

        public DbSet<Inventory> InventoryItems { get; set; }
    }
}
