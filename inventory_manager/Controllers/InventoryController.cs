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
        // GET: /<controller>/
        // public IActionResult Index()
        // {
            // return "123123";
        // }


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
    }
}
