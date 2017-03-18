using System.ComponentModel.DataAnnotations;

namespace BazaSmyczy.Core.Utils
{
    public enum LeashSize
    {
        [Display(Name = "Very Small")]
        VerySmall = 5,
        Small = 10,
        Medium = 20,
        Large = 30
    }
}
