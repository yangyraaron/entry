using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entry.Reports.Model
{
    public class WarehouseModel
    {
        public IList<Warehouse> GetWarehouses()
        {
            return new[] { new Warehouse { Name = "AA", Division="DD", Description = "AAAAAAAAAAAAAAAA" } ,
                            new Warehouse{ Name = "BB", Division="DD",Description="BBBBBBBBBBBBBBBB"},
            new Warehouse{ Name = "CC", Division="gs",Description="CCCCCCCCCCCCCC"},
            new Warehouse{ Name = "DD",Division="gs", Description="DDDDDDDDDDDDDD"},
            new Warehouse{ Name = "ef",Division="gs", Description="effff"},
            new Warehouse{ Name = "ab",Division="gs", Description="ab"},
            new Warehouse{ Name = "cd",Division="xx", Description="cd"},
            new Warehouse{ Name = "bx",Division="xx", Description="bx"},
            new Warehouse{ Name = "EE", Division="gs",Description="EEEEEEEEEEEEEE"}};
        }

    }

    public class Warehouse
    {
        public string Name { get; set; }

        public string Division { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return string.Format("{0}-{1}", Name, Description);
        }
    }
}
