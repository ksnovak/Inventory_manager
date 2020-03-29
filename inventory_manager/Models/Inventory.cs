using CsvHelper.Configuration.Attributes;
using System;

namespace inventory_manager
{
    public class Inventory
    {

        public int ID { get; set; }

        [Name("ITEM Name")]
        public string Name { get; set; }

        [Name("COST")]
        public int Cost { get; set; }
    }
}
 