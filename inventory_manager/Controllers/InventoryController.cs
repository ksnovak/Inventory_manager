using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inventory_manager;
using inventory_manager.Models;
using CsvHelper;
using System.Globalization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace inventory_manager.Controllers
{
    

    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {

        private readonly InventoryContext _context;

        public InventoryController(InventoryContext context)
        {
            _context = context;

            // If there are no records stored yet, read from the input file to initialize them
            if (!_context.InventoryItems.Any())
            {
                this.InitializeInventory();
            }        
        }

        // Initialize the database with the input file
        public async void InitializeInventory()
        {

            //Read from the file, into a stream
            using (var reader = new StreamReader("./items.txt"))

            //Parse the stream as a CSV set of data
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;

                //Read each line of the Csv file (skipping header row)
                while (csv.Read())
                {
                    Inventory item = csv.GetRecord<Inventory>();
                    await this.AddItem(item);

                }
            }
        }

        [HttpGet] // Get all inventory items
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventoryItems()
        {
            return await _context.InventoryItems.ToListAsync();            
        }


        // Requirement 2
        // Return a list of max prices of items, grouped by item.name
        [HttpGet("max")]
        public Inventory GetMax()
        {
            return new Inventory
            {
                ID = 999,
                Name = "Biggest item!",
                Cost = 1
            };
        }

        // Requirement 3
        // Take as an input an item name and returns the max price for it
        [HttpGet("item/{name}")]
        public async Task<ActionResult<Inventory>> GetByName(string name)
        {
            //var inventory = await _context.InventoryItems.FindAsync(name);
            var inventory = await _context.InventoryItems.FirstAsync<Inventory>();

            if (inventory == null)
            {
                return NotFound();
            }

            return inventory;
        }


        // Get an item by specified ID
        [HttpGet("id/{id}")]
        public async Task<ActionResult<Inventory>> GetItem(int id)
        {
            var items = await _context.InventoryItems.FindAsync(id);
            if (items == null)
            {
                return NotFound();
            }

            return items;
        }


        /**
         * OPTIONAL tasks below
         * 
         */

        // Requirement 7
        // Add an item
        [HttpPost("add")]
        public async Task<ActionResult<Inventory>> AddItem(Inventory item)
        {
            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.ID }, item);
        }

        // Requirement 7
        //Update an item
        [HttpPost("item/{id}")]
        public Inventory UpdateItem(int id, Inventory item)
        {
            return new Inventory
            {
                ID = id,
                Name = item.Name,
                Cost = 5959
            };
        }

        // Requirement 7
        // Delete an item
        [HttpDelete("item/{id}")]
        public Inventory DeleteItem(int id)
        {
            return new Inventory
            {
                ID = 0,
                Name = "",
                Cost = 0
            };
        }
    }
}
