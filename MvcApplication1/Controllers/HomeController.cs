using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        Database2Entities db = new Database2Entities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchPageNoResult()
        {
            return View();
        }
        public ActionResult SearchPage()
        {
            string seatno = Request["search"];
            List<Reservation> res = db.Reservations.Where(x => x.seatno.Equals(seatno)).ToList();
            if (res.Count > 0)
            {
                ViewBag.list = res;
                return View();
            }
            else
            {
                return Redirect("SearchPageNoResult");
            }


        }
        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Profile()
        {
            return View();
        }
        public ActionResult UpdatePassword1()
        {
            return View();
        }

        public ActionResult ReserveSeat()
        {
            var res = db.Reservations.Where(x => x.seatStatus.Equals("available")).ToList();

            return View(res);
        }
        //public ActionResult CreateSeat()
        //{

        //    return View();
        //}
        public ActionResult CreateSeat(int Id)
        {

            // Session["seatnono"] = seatno;
            //string usern= Convert.ToString(Session["username"]);
            //var res = db.Reservations.Where(x => x.seatno.Equals(seatno) && x.username.Equals(usern)).ToList();

            Reservation r = db.Reservations.FirstOrDefault(x => x.Id.Equals(Id));
            Session["seatid"] = Id;
            Session["seatnono"] = r.seatno;
            return View(r);

        }
        public ActionResult UpdateSeat()
        {
            return View();

        }
        public ActionResult DelSeat()
        {
            return View();
        }

        public ActionResult AddSeat()
        {
            string username = Convert.ToString(Session["username"]);
            string id = Convert.ToString(Session["seatid"]);
            int id1 = int.Parse(id);
            string seatno = Convert.ToString(Session["seatnono"]);

            List<Reservation> resl = db.Reservations.Where(x => x.Id.Equals(id1)).ToList();
            if (resl.Count > 0)
            {
                Reservation r = resl.Find(x => x.Id.Equals(id1));
                //string stno = Request["seatno"];
                r.seatno = seatno;
                r.seatStatus = "reserve";
                r.username = username;
                r.flightno = "pk112";
                db.SaveChanges();
            }
            return Redirect("ViewSeat");
        }
        public JsonResult Checkzip(string zp)
        {
            if (zp.Length > 5)
            {
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ViewSeat()
        {
            string username = Convert.ToString(Session["username"]);
            List<Reservation> res = db.Reservations.Where(x => x.username.Equals(username) && x.seatStatus.Equals("reserve")).ToList();
            ViewBag.list = res;
            return View();
        }
        [HttpPost]
        public ActionResult SignInFunction()
        {
            string username = Request["username"];
            string pass = Request["password"];
            List<Table> tb = db.Tables.Where(x => x.username.Equals(username) && x.password.Equals(pass)).ToList();
            if (tb.Count > 0)
            {
                Table t = tb.Find(x => x.username.Equals(username) && x.password.Equals(pass));
                Session["error"] = "";
                Session["email"] = t.email;
                Session["username"] = username;
                return Redirect("Profile");

            }
            else
            {
                Session["error"] = "username and password does not match";
                return Redirect("Login");
            }

        }

        public JsonResult UpdateSeat3(string seat)
        {
            string username = Convert.ToString(Session["username"]);
            List<Reservation> res = db.Reservations.Where(x => x.username.Equals(username) && x.seatno.Equals(seat)).ToList();
            if (res.Count > 0)
            {
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
            else { return this.Json(false, JsonRequestBehavior.AllowGet); }


        }

        [HttpPost]
        public ActionResult deleteSeat()
        {
            string seatno = Request["seatno"];
            string username = Convert.ToString(Session["username"]);
            List<Reservation> res = db.Reservations.Where(x => x.username.Equals(username) && x.seatno.Equals(seatno)).ToList();

            Reservation r = new Reservation();
            r = res.Find(x => x.username.Equals(username));
            db.Reservations.Remove(r);
            db.SaveChanges();
            return Redirect("ViewSeat");

        }


        [HttpPost]
        public ActionResult ChangeSeat()
        {
            Reservation rsevation = new Reservation();
            string seatno = Request["seatn"];
            string seatst = Request["SeatStatus"];
            string username = Convert.ToString(Session["username"]);
            List<Reservation> resch = db.Reservations.Where(x => x.username.Equals(username) && x.seatno.Equals(seatno)).ToList();
            if (resch.Count > 0)
            {
                rsevation = resch.Find(x => x.username.Equals(username));
                rsevation.seatStatus = seatst;
            }
            db.SaveChanges();
            return Redirect("ViewSeat");

        }

        [HttpPost]
        public ActionResult ChangePass()
        {
            string ps = Request["ps"];
            string cp = Request["Cp"];
            string username = Convert.ToString(Session["username"]);
            List<Table> tb = db.Tables.Where(x => x.username.Equals(username)).ToList();
            if (tb.Count > 0)
            {
                Table t = tb.Find(x => x.username.Equals(username));
                t.password = ps;
                t.conPass = cp;
                Session["pass"] = "Password Chaged succesfully";
                db.SaveChanges();
            }
            else { Session["pass"] = "Password doesnot Chaged succesfully"; }
            return Redirect("Profile");
        }

        public JsonResult checkUsername(string username)
        {
            List<Table> tb = db.Tables.Where(x => x.username.Equals(username)).ToList();
            if (tb.Count > 0)
            {
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Pass(string pass)
        {
            string username = Convert.ToString(Session["username"]);
            List<Table> tb = db.Tables.Where(x => x.username.Equals(username)).ToList();
            if (tb.Count > 0)
            {
                Table t = tb.Find(x => x.username.Equals(username));

                if (pass == t.password)
                {
                    return this.Json(true, JsonRequestBehavior.AllowGet);
                }
                else { return this.Json(false, JsonRequestBehavior.AllowGet); }
            }
            else { return this.Json(true, JsonRequestBehavior.AllowGet); }
        }
        [HttpPost]
        public ActionResult SignUPFunc()
        {
            string fn = Request["fn"];
            string ln = Request["ln"];
            string email = Request["email"];
            string pass = Request["pass"];
            string conpass = Request["Conpass"];
            string zipcode = Request["zipCode"];
            Table t = new Table();
            t.email = email;
            t.username = fn;
            t.lname = ln;
            t.password = pass;
            t.conPass = conpass;
            t.zipCode = zipcode;
            Session["email"] = t.email;
            Session["username"] = t.username;
            db.Tables.Add(t);
            db.SaveChanges();
            return Redirect("Profile");
        }
    }
}
