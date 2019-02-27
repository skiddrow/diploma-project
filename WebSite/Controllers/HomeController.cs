using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using WebSite.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private string UserId { get; set; }

        [AllowAnonymous]
        public ActionResult Index()
        {
            using (Context db = new Context())
            {
                //Car car = new Car { Brand = "Subaru", Model = "WRX STI", Year = 2018, EngineType = "Gas", EngineAmount = 2.5f, GearBoxType = "M", TankAmount = 60, IsCarHasDC = true };

                //db.Cars.Add(car);
                //db.SaveChanges();
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Contact()
        {
            ViewBag.Message = "Your contact page.";

            //HttpClient client = new HttpClient();
            //string uri = "http://localhost:8732/api/Server/GetCars/ " + User.Identity.GetUserId();
            //string res = await client.GetStringAsync(uri);

            //var car = JsonConvert.DeserializeObject(res, typeof(Car));

            HttpClient client = new HttpClient();
            string uri = "http://localhost:8732/api/Server/GetFuelingByCarId/" + "6";
            string res = await client.GetStringAsync(uri);

            var car = JsonConvert.DeserializeObject(res, typeof(IEnumerable<Fueling>));


            ViewBag.car = car;
            return View();
        }

        [AllowAnonymous]
        public ActionResult BuyDevice()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult DownloadApp()
        {
            return View();
        }

        public ActionResult MyRoom()
        {
            Car c;

            if (IsUserHasCar(out c))
            {
                ViewBag.IsUserHasCar = true;
                ViewBag.Car = c;
            }
            else
            {
                ViewBag.IsUserHasCar = false;
            }

            return View("SomeView");
        }

        //public ActionResult AddCar()
        //{
        //    return View();
        //}

        public ActionResult Mileage()
        {
            Car car;

            if (IsUserHasCar(out car))
            {
                var fuelings = GetFuelingWebApi().ToList();
                string[] fuelSpends = new string[fuelings.Count];
                string[] mileages = new string[fuelings.Count];

                fuelSpends[0] = "";
                mileages[0] = "";

                if (fuelings.Count > 1)
                {
                    for (int i = 1; i < fuelings.Count; i++)
                    {
                        double tempMileage = ((fuelings[i - 1].Value + fuelings[i - 1].TankValue) - fuelings[i].TankValue) / ((fuelings[i].Mileage - fuelings[i - 1].Mileage) / 100);
                        fuelSpends[i] = Math.Round(tempMileage, 2).ToString();
                        mileages[i] = (fuelings[i].Mileage - fuelings[i - 1].Mileage).ToString();
                    }
                }

                ViewBag.Fuelings = fuelings;
                ViewBag.FuelSpends = fuelSpends;
                ViewBag.Mileages = mileages;
            }

            return View("Mileage");
        }

        public ActionResult Oils()
        {
            Car car;

            if (IsUserHasCar(out car))
            {
                using (Context db = new Context())
                {
                    var oils = db.Liquids.Where(l => l.CarId == car.Id).ToList();
                    var oilTypes = db.LiquidTypes.ToList();

                    foreach (var i in oils)
                    {
                        i.LiquidType = oilTypes.Find(o => o.Id == i.LiquidTypeId);
                    }

                    ViewBag.Oils = oils;
                }
            }

            return View("Oils");
        }

        public ActionResult Errors()
        {
            Car car;

            if (IsUserHasCar(out car))
            {
                using (Context db = new Context())
                {
                    var errors = db.Errors.Where(e => e.CarId == car.Id).ToList();
                    var errorTypes = db.ErrorTypes.ToList();

                    foreach (var i in errors)
                    {
                        i.ErrorType = errorTypes.Find(et => et.Id == i.ErrorTypeId);
                    }

                    ViewBag.Errors = errors;
                }
            }

            return View();
        }

        public ActionResult FindVehicle()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddCar2()
        {
            Car c;
            bool res = IsUserHasCar(out c);

            if (res)
            {
                ViewBag.Message = "AlreadyHasCar";
                ViewBag.IsUserHasCar = false;
                return View("SomeView");
            }

            return View();
        }

        [HttpPost]
        public ActionResult AddCar2([Bind(Include = "Brand,Model,Year,TankAmount,IsCarHasDC")] Car Car, string EngineType, string EngineAmount, string GearBoxType)
        {
            Car.UserId = User.Identity.GetUserId();
            Car.EngineType = EngineType;
            Car.GearBoxType = GearBoxType;

            float t1 = Int32.Parse(EngineAmount.Substring(0, 1));
            float t2 = (Int32.Parse(EngineAmount.Substring(2, 1)) / 10f);
            Car.EngineAmount = t1 + t2;

            //if (ate.IsValid)
            //{
            using (Context db = new Context())
            {
                db.Cars.Add(Car);
                db.SaveChanges();
            }

            return RedirectToAction("MyRoom");
            //}

            //return View(Car);
        }

        [HttpPost]
        public ActionResult AddLiquid(string LiquidType, string Mileage, string LiquidAmount)
        {
            Car c;
            double mil, liqA;
            int liqT = 0;

            if (IsUserHasCar(out c))
            {
                ViewBag.IsUserHasCar = true;
                ViewBag.Car = c;
            }
            else
            {
                ViewBag.IsUserHasCar = false;
            }

            if (Double.TryParse(Mileage, out mil) && Double.TryParse(LiquidAmount, out liqA))
            {
                ViewBag.Message = "Liquid";

                if (LiquidType == "Oil")
                {
                    liqT = 1;
                }
                else if (LiquidType == "Antifreeze")
                {
                    liqT = 2;
                }
                else
                {
                    liqT = 3;
                }

                AddNewOiling(liqT, (float)liqA, (float)mil);
            }
            else
            {
                ViewBag.Message = "Float";
            }

            return View("SomeView");
        }

        [HttpPost]
        public ActionResult AddMileage(string Mileage, string FuelInTank, string FuelAmount, string Price)
        {
            Car c;
            double mil, price;
            int fuelT, fuelA;

            if (IsUserHasCar(out c))
            {
                ViewBag.IsUserHasCar = true;
                ViewBag.Car = c;
            }
            else
            {
                ViewBag.IsUserHasCar = false;
            }

            if (Double.TryParse(Mileage, out mil) && Int32.TryParse(FuelInTank, out fuelT) && Int32.TryParse(FuelAmount, out fuelA) && Double.TryParse(Price, out price))
            {
                ViewBag.Message = "Fuel";

                if (IsCorrectMileage((float)mil) && (fuelA + fuelT) <= c.TankAmount)
                {
                    using (Context db = new Context())
                    {
                        Fueling fuel = new Fueling { CarId = GetCarId(), Value = fuelA, TankValue = fuelT, Price = (float)price, Date = DateTime.Today.Date, Mileage = (float)mil };

                        db.Fuelings.Add(fuel);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ViewBag.Message = "IncorrectMileage";
                }

                if ((fuelA + fuelT) > c.TankAmount)
                {
                    ViewBag.Message = "IncorrectAmount";
                }
            }
            else
            {
                ViewBag.Message = "Float";
            }

            return View("SomeView");
        }

        [HttpPost]
        public ActionResult MilBetween(DateTime from, DateTime to)
        {
            var fuels = GetFuelingWebApi().ToList();

            if (fuels.Count == 0)
            {
                return RedirectToAction("Mileage");
            }

            var res = fuels.Where(f => f.Date >= from && f.Date <= to).ToList();

            string[] fuelSpends = new string[res.Count];
            string[] mileages = new string[res.Count];

            fuelSpends[0] = "";
            mileages[0] = "";

            if (res.Count > 1)
            {
                for (int i = 1; i < res.Count; i++)
                {
                    double tempMileage = ((res[i - 1].Value + res[i - 1].TankValue) - res[i].TankValue) / ((res[i].Mileage - res[i - 1].Mileage) / 100);
                    fuelSpends[i] = Math.Round(tempMileage, 2).ToString();
                    mileages[i] = (res[i].Mileage - res[i - 1].Mileage).ToString();
                }
            }

            ViewBag.Fuelings = res;
            ViewBag.FuelSpends = fuelSpends;
            ViewBag.Mileages = mileages;


            return View("Mileage");
        }

        public ActionResult EmulateIOT()
        {
            Car c;
            Random rnd = new Random((int)DateTime.Now.Millisecond);
            Random rnd2 = new Random(DateTime.Now.Millisecond);
            Random rnd3 = new Random(DateTime.Now.Millisecond);
            Random rnd4 = new Random(DateTime.Now.Millisecond);
            Random rnd5 = new Random(DateTime.Now.Millisecond);
            int randomValue = rnd.Next(1, 100);

            if (IsUserHasCar(out c))
            {
                var lastFueling = GetLastFueling(c.Id);
                var preLastFueling = GetPreLastFueling(c.Id);

                if (randomValue < 50)
                {
                    double averageSpending = ((preLastFueling.Value + preLastFueling.TankValue) - lastFueling.TankValue) / ((lastFueling.Mileage - preLastFueling.Mileage) / 100);
                    double maxMil = (lastFueling.TankValue + lastFueling.Value) / averageSpending * 100;
                    double minMil = maxMil / 5;
                    double mil = lastFueling.Mileage + rnd3.Next((int)minMil, (int)maxMil);
                    double fuelT = lastFueling.TankValue + lastFueling.Value - ((mil - lastFueling.Mileage) / averageSpending);
                    averageSpending += rnd4.Next(-2, 2);
                    double fuelA = rnd2.Next(1, c.TankAmount - (int)fuelT);
                    double price = (lastFueling.Price / lastFueling.Value) * fuelA + rnd4.Next(-1, 1);

                    using (Context db = new Context())
                    {
                        Fueling fuel = new Fueling { CarId = GetCarId(), Value = (float)Math.Round(fuelA, 1), TankValue = (float)Math.Round(fuelT, 1), Price = (float)Math.Round(price, 1), Date = DateTime.Today.Date, Mileage = (float)Math.Round(mil, 1) };

                        db.Fuelings.Add(fuel);
                        db.SaveChanges();
                    }
                }
                else if (randomValue >= 50)
                {
                    AddNewError();
                }
            }

            return RedirectToAction("MyRoom");
        }

        public bool IsUserHasCar(out Car Car)
        {
            this.UserId = User.Identity.GetUserId();

            var result = GetCarWebApi(UserId);

            if (result != null)
            {
                Car = result;
                return true;
            }
            else
            {
                Car = null;
                return false;
            }

        }

        #region WEBAPI
        public Car GetCarWebApi(string id)
        {
            HttpClient client = new HttpClient();
            string uri = "http://localhost:8732/api/Server/GetCars/" + User.Identity.GetUserId();
            string res = client.GetStringAsync(uri).Result;

            var car = JsonConvert.DeserializeObject(res, typeof(Car));

            return (Car)car;
        }

        public IEnumerable<Fueling> GetFuelingWebApi()
        {
            HttpClient client = new HttpClient();
            string uri = "http://localhost:8732/api/Server/GetFuelingByCarId/" + GetCarId().ToString();
            string res = client.GetStringAsync(uri).Result;

            var fuelings = JsonConvert.DeserializeObject(res, typeof(IEnumerable<Fueling>));

            return (IEnumerable<Fueling>)fuelings;
        }

        public IEnumerable<Liquid> GetLiquidsWebApi()
        {
            HttpClient client = new HttpClient();
            string uri = "http://localhost:8732/api/Server/GetOilsByCarId/" + GetCarId().ToString();
            string res = client.GetStringAsync(uri).Result;

            var liqs = JsonConvert.DeserializeObject(res, typeof(IEnumerable<Liquid>));

            return (IEnumerable<Liquid>)liqs;
        }

        public IEnumerable<LiquidType> GetLiquidTypesWebApi()
        {
            HttpClient client = new HttpClient();
            string uri = "http://localhost:8732/api/Server/GetOilsTypesByOilId/" + GetCarId().ToString();
            string res = client.GetStringAsync(uri).Result;

            var liqs = JsonConvert.DeserializeObject(res, typeof(IEnumerable<LiquidType>));

            return (IEnumerable<LiquidType>)liqs;
        }

        public IEnumerable<Error> GetErrorsWebApi()
        {
            HttpClient client = new HttpClient();
            string uri = "http://localhost:8732/api/Server/GetErrorsByCarId/" + GetCarId().ToString();
            string res = client.GetStringAsync(uri).Result;

            var liqs = JsonConvert.DeserializeObject(res, typeof(IEnumerable<Error>));

            return (IEnumerable<Error>)liqs;
        }

        public IEnumerable<ErrorType> GetErrorTypesWebApi()
        {
            HttpClient client = new HttpClient();
            string uri = "http://localhost:8732/api/Server/GetErrorTypes/" + GetCarId().ToString();
            string res = client.GetStringAsync(uri).Result;

            var liqs = JsonConvert.DeserializeObject(res, typeof(IEnumerable<ErrorType>));

            return (IEnumerable<ErrorType>)liqs;
        }
        #endregion

        public int GetCarId()
        {
            this.UserId = User.Identity.GetUserId();

            var result = GetCarWebApi(UserId);

            if (result != null)
            {
                return result.Id;
            }
            else
            {
                return -1;
            }
        }

        public Fueling GetLastFueling(int carId)
        {
            return GetFuelingWebApi().ToList().FindLast(f => f.CarId == carId);
        }

        public Fueling GetPreLastFueling(int carId)
        {
            var res = GetFuelingWebApi().ToList().FindAll(f => f.CarId == carId);
            return res[res.Count - 2];
        }

        public bool AddNewOiling(int liquidTypeId, float liquidAmount, float Mileage)
        {
            try
            {
                using (Context db = new Context())
                {
                    Liquid liq = new Liquid { CarId = GetCarId(), LiquidTypeId = liquidTypeId, LiquidAmount = liquidAmount, Date = DateTime.Today.Date, Mileage = Mileage };

                    db.Liquids.Add(liq);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddNewMileage()
        {
            return false;
        }

        public bool AddNewError()
        {
            try
            {
                Random rnd = new Random(DateTime.Now.Millisecond);
                Random rnd2 = new Random(DateTime.Now.Millisecond);

                var carId = GetCarId();
                var milF = GetFuelingWebApi().ToList();
                var milO = GetLiquidsWebApi().ToList();
                var milFres = milF.Count == 0 ? 0 : milF.Last(f => f.CarId == carId).Mileage;
                var milOres = milO.Count == 0 ? 0 : milO.Last(l => l.CarId == carId).Mileage;
                var errorTypes = GetErrorTypesWebApi().ToList();
                int errorNumber = rnd.Next(0, errorTypes.Count - 1);

                var mil = milFres + milOres;
                mil += rnd2.Next(40, 1000);

                using (Context db = new Context())
                {
                    Error liq = new Error { CarId = carId, Date = DateTime.Today.Date, Mileage = mil, IsFixed = false, ErrorTypeId = errorNumber };

                    db.Errors.Add(liq);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsCorrectMileage(float mileage)
        {
            var carId = GetCarId();

            try
            {

                var milF = GetFuelingWebApi().ToList();
                var milO = GetLiquidsWebApi().ToList();

                if (milF.Count == 0 && milO.Count == 0 && mileage > 0)
                {
                    return true;
                }

                var milFres = milF.Count == 0 ? 0 : milF.Last(f => f.CarId == carId).Mileage;
                var milOres = milO.Count == 0 ? 0 : milO.Last(l => l.CarId == carId).Mileage;
                //var mil = milFres.Mileage > milOres.Mileage ? milFres.Mileage : milOres.Mileage;

                if (milFres > mileage || milOres > mileage)
                {
                    return false;
                }

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

//public bool AddNewError()
//       {
//           try
//           {
//               Random rnd = new Random(DateTime.Now.Millisecond);
//               Random rnd2 = new Random(DateTime.Now.Millisecond);

//               using (Context db = new Context())
//               {
//                   var carId = GetCarId();
//                   var errorTypes = db.ErrorTypes.ToList();
//                   int errorNumber = rnd.Next(0, errorTypes.Count - 1);
//                   var milF = db.Fuelings.ToList();
//                   var milFres = milF.Last(f => f.CarId == carId);
//                   var milO = db.Liquids.ToList();
//                   var milOres = milO.Last(l => l.CarId == carId);
//                   var mil = milFres.Mileage > milOres.Mileage ? milFres.Mileage : milOres.Mileage;
//                   mil += rnd2.Next(40, 1000);

//                   var milF = GetFuelingWebApi().ToList();
//                   var milO = GetLiquidsWebApi().ToList();

//                   if (milF.Count == 0 && milO.Count == 0 && mileage > 0)
//                   {
//                       return true;
//                   }

//                   var milFres = milF.Count == 0 ? 0 : milF.Last(f => f.CarId == carId).Mileage;
//                   var milOres = milO.Count == 0 ? 0 : milO.Last(l => l.CarId == carId).Mileage;

//                   Error liq = new Error { CarId = carId, Date = DateTime.Today.Date, Mileage = mil, IsFixed = false, ErrorTypeId = errorNumber };

//                   db.Errors.Add(liq);
//                   db.SaveChanges();
//               }

//               return true;
//           }
//           catch (Exception)
//           {
//               return false;
//           }
//       }