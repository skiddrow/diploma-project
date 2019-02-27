using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSite.Models
{
    public class Liquid
    {
        public int Id { get; set; }
        [ForeignKey("Car")]
        public int CarId { get; set; }

        [ForeignKey("LiquidType")]
        public int LiquidTypeId { get; set; }

        public float Mileage { get; set; }
        public float LiquidAmount { get; set; }
        public DateTime Date { get; set; }

        public Car Car { get; set; }
        public LiquidType LiquidType { get; set; }
    }
}