using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Error
    {
        public int Id { get; set; }

        [ForeignKey("ErrorType")]
        public int ErrorTypeId { get; set; }
        [ForeignKey("Car")]
        public int CarId { get; set; }

        public float Mileage { get; set; }
        public DateTime Date { get; set; }
        public bool IsFixed { get; set; }

        public ErrorType ErrorType { get; set; }
        public Car Car { get; set; }
    }
}