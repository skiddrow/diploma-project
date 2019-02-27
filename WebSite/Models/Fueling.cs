using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSite.Models
{
    public class Fueling
    {
        public int Id { get; set; }
        [ForeignKey("Car")]
        public int CarId { get; set; }

        public float Mileage { get; set; }
        public float Value { get; set; }
        public float TankValue { get; set; }
        public float Price { get; set; }
        public DateTime Date { get; set; }
        public Car Car { get; set; }
    }
}