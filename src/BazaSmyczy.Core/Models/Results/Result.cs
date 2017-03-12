using System.Collections.Generic;
using System.Linq;

namespace BazaSmyczy.Core.Models.Results
{
    public class Result
    {
        public List<string> Errors { get; set; } = new List<string>();
        public bool IsError => Errors.Any();
    }
}
