using PersonalNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalNews.Controllers
{
    public class lanmuController : Controller
    {
        userDBContext db = new userDBContext();
        //
        // GET: lanmu

       public ActionResult Index()
        {
            return View("index");
        }
        //文章分页
        public ActionResult GetDivideList()
        {
            string s = "";
            foreach (Category c in db.Categorys)
            {
                //onclick="getnews(1)"
                s += "<li id='lanmu1' onclick='getDivideNews(" + c.CategoryId + ",1)' > " + c.CategoryName + "</li>";
            }
            return Content(s);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category c)
        {
            db.Categorys.Add(c);
            db.SaveChanges();
            return View();
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Category ca = db.Categorys.Find(id);
            if (ca == null)
            {
                return HttpNotFound();
            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Category ca = db.Categorys.Find(id);
            db.Categorys.Remove(ca);
            db.SaveChanges();
            return View();
        }
        
        public ActionResult Details(int? id) //删除的时候提交的数据只是id 和修改不一样
        {
            Category category = db.Categorys.Find(id); //找到
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category); //放到视图里访问
        }

    }
}