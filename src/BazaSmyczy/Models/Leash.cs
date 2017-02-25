using BazaSmyczy.Core.Utils;

namespace BazaSmyczy.Models
{
    public class Leash
    {
        public Leash() { }

        public int ID { get; set; }
        public string ImageName { get; set; }
        public string Text { get; set; }
        public LeashSize Size { get; set; }
        public string Color { get; set; }
        public string Desc { get; set; }
    }
}
