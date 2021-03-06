﻿using BazaSmyczy.Core.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace BazaSmyczy.Core.Models
{
    public class Leash
    {
        public int ID { get; set; }

        public string ImageName { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public LeashSize Size { get; set; }

        [Required]
        public string Color { get; set; }

        public string Desc { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }
    }
}
