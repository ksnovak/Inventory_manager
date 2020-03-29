using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace inventory_manager.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        [HttpGet]
        public Inventory Get()
        {
            return new Inventory
            {
                ID = 123,
                Name = "Important Item",
                Cost = 999

            };
        }

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

        // Take as an input an item name and returns the max price for it
        [HttpGet("item/{name}")]
        public Inventory GetByName(string name)
        {
            return new Inventory
            {
                ID = 867,
                Name = name,
                Cost = 5309
            };
        }

        /**
         * OPTIONAL tasks below
         * 
         */

        // Add an item
        [HttpPost("item/add")]
        public Inventory AddItem(Inventory item)
        {
            return new Inventory
            {
                ID = 1,
                Name = "wowzers, successfully added",
                Cost = 0
            };
        }

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
