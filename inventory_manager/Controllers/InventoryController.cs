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
    [Route("api/[controller]")]
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
            using (var reader = new StreamReader("items.txt"))

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
        // Get the max price for each item
        [HttpGet("max")]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetMax()
        {
            
            //For each unique item, find the highest price and its associated ID
            var query = from ranked in (from items in _context.InventoryItems
                    orderby items.Name
                    select new 
                    {
                        items.Name,
                        items.Cost,
                        items.ID,
                        // To find the most expensive and remove duplicates, we rank by cost (and then ID)
                        Rank = (from inner in _context.InventoryItems
                            where inner.Name == items.Name && (
                                (inner.ID != items.ID && inner.Cost > items.Cost) //Different IDs -> compare costs
                                || (inner.Cost == items.Cost && inner.ID < items.ID) //Different costs -> use IDs to pick just one
                            )
                            select inner
                        ).Count() + 1   //Counting occurrences as a way to rank them
                    })
                    where ranked.Rank == 1 //Only retrieve the highest-rank item for each unique name
                    select new Inventory {
                        Name = ranked.Name,
                        Cost = ranked.Cost,
                        ID = ranked.ID
                    };


            return await query.ToListAsync();
        }

        // Requirement 3
        // Take as an input an item name and returns the max price for it
        [HttpGet("max/{name}")]
        public async Task<ActionResult<Inventory>> GetByName(string name)
        {
            try { 
                //Search for the name in the database, order results by price, and then take the first result
                var inventory = await _context.InventoryItems
                    .Where(item => item.Name.ToLower().Equals(name.ToLower()))
                    .OrderByDescending(item => item.Cost)
                    .FirstOrDefaultAsync();


                // Give back a 404 message if the item isn't in database
                if (inventory == null)
                {
                    return NotFound();
                }

                return inventory;
            }
            catch
            {
                // Give back a 404 message if the item isn't in database
                return NotFound();
            }
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
