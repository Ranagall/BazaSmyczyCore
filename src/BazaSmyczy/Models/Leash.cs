using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BazaSmyczy.Models
{
    public class Leash
    {
        public Leash() { }

        public int ID { get; set; }
        public string ImageName { get; set; }
        public string Text { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Desc { get; set; }
    }
}
