using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebSite.Models;

namespace WebSite.Controllers
{
    //Фрагмент коду з серверу, що отримує інформацію за бази даних
    public class ServerController : ApiController
    {
        Context db = new Context();

        public IHttpActionResult GetCars(string id)
        {
            return Json<Car>(db.Cars.SingleOrDefault(c => c.UserId == id));
        }

        public IHttpActionResult GetFuelingByCarId(int id)
        {
            return Json<IEnumerable<Fueling>>(db.Fuelings.Where(f => f.CarId == id));
        }

        public IHttpActionResult GetErrorsByCarId(int id)
        {
            return Json<IEnumerable<Error>>(db.Errors.Where(e => e.CarId == id));
        }

        public IHttpActionResult GetErrorTypes()
        {
            return Json<IEnumerable<ErrorType>>(db.ErrorTypes);
        }

        public IHttpActionResult GetOilsByCarId(int id)
        {
            return Json<IEnumerable<Liquid>>(db.Liquids.Where(l => l.CarId == id));
        }

        public IHttpActionResult GetOilsTypesByOilId(int id)
        {
            return Json<IEnumerable<LiquidType>>(db.LiquidTypes.Where(lt => lt.Id == id));
        }
    }
}
