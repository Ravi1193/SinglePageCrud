using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SinglePageApplication.Controllers
{
    
    public class HomeController : Controller
    {
        // GET: Home
        DataDataContext db = new DataDataContext();
        public ActionResult Index()
        {
            try
            {
                ViewBag.user = db.users.Where(x => x.hide == "N").ToList();
                return View();
            }catch(Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return View("Error");
            }

        }
        [HttpPost]
        public ActionResult addUser(Homeviewmodel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Hide = "N";
                    var saveUsers = saveUser(model);

                    if (saveUsers)
                    {
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        ModelState.AddModelError("", "Something went Wrong");
                    }
                }
            }catch(Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.user = db.users.Where(x => x.hide == "N").ToList();
            return View("Index");
            
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                ViewBag.user = db.users.Where(x => x.hide == "N").ToList();
                var model = getUseById(id);
                return View("Index", model);
            }catch(Exception ex)
            {
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult Edit(Homeviewmodel model)
        {
             ViewBag.user = db.users.Where(x => x.hide == "N").ToList();
            if (ModelState.IsValid)
            {
                var editUser = editUsers(model);
                if (editUser)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Something Went Wrong");
                    
                }

            }
            return View("Index");
        }

        public ActionResult Delete(Homeviewmodel model)
        {
            try
            {
                ViewBag.user = db.users.Where(x => x.hide == "N").ToList();
                model.Hide = "Y";
                var dUsers = deleteUsers(model);
                if(dUsers)
                {
                    return RedirectToAction("Index");
                }

            } catch(Exception ex)
            {

            }
            return View("Index");
        }
        public bool saveUser(Homeviewmodel model)
        {
            user users = new user();
            users.firstname = model.Firstname;
            users.lastname = model.Lastname;
            users.username = model.Username;
            users.email = model.Email;
            users.hide = model.Hide;
            db.users.InsertOnSubmit(users);
            db.SubmitChanges();
            return true;
        }

       public Homeviewmodel getUseById(int id)
        {
            return db.users.Where(x => x.id == id).Select(x => new Homeviewmodel
            {
               Id =x.id,
               Firstname=x.firstname,
               Lastname=x.lastname,
               Username=x.username,
               Email=x.email
            }).FirstOrDefault();
        }

        public bool editUsers(Homeviewmodel model)
        {
            var user = db.users.Where(x => x.id == model.Id).FirstOrDefault();
            user.firstname = model.Firstname;
            user.lastname = model.Lastname;
            user.username = model.Username;
            user.email = model.Email;
            db.SubmitChanges();
            return true;
        }

        public bool deleteUsers(Homeviewmodel model)
        {
            var deleteUser = db.users.Where(x => x.id == model.Id).FirstOrDefault();
            deleteUser.hide = model.Hide;
            db.SubmitChanges();
            return true;
        }
    }
    public class Homeviewmodel
    {
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }

        public string Hide{ get; set; }
    }

   
}

