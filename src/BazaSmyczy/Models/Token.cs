using BazaSmyczy.Core.Services.TokensService;
using System;

namespace BazaSmyczy.Models
{
    public class Token
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationDate { get; set; }
        public TokenType Type { get; set; }
    }
}
