using PersonalNews.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalNews.Controllers
{
    public class NewsController : Controller
    {
        userDBContext db = new userDBContext();
        static int now = 1;
        static int z = 0;

        //文章返回和分页
        public ActionResult getDivideNews(int? lanmuid, int? pageno)
        {
            if (pageno == null)
                pageno = 1;
            if (lanmuid == null)
                lanmuid = 1;
            int prepage = (int)pageno - 1;
            int nextpage = (int)pageno + 1;
            int pagecount = db.Articles.Where(o=>o.CategoryId==lanmuid).Count() / 6 + 1;

            //边界判断
            if (prepage < 1) prepage = 1;
            if (nextpage > pagecount) nextpage = pagecount;
            string pagebtn = "";
            pagebtn = "<button onclick='getDivideNews(" + lanmuid+",1)'>首页</button> &nbsp; &nbsp; &nbsp; &nbsp; ";
            pagebtn += "<button onclick='getDivideNews(" + lanmuid+"," + prepage + ")'>上一页</button>&nbsp;&nbsp;&nbsp;&nbsp;";
            pagebtn += pageno + "&nbsp;&nbsp;&nbsp;&nbsp;";
            pagebtn += "<button onclick='getDivideNews(" + lanmuid + "," + nextpage + ")'>下一页</button>&nbsp;&nbsp;&nbsp;&nbsp;";
            pagebtn += "<button onclick='getDivideNews(" + lanmuid + "," + pagecount + ")'>末页</button>&nbsp;&nbsp;&nbsp;&nbsp;";
            string s = "";

            foreach (Article a in db.Articles.Where(o => o.CategoryId == lanmuid).OrderBy(o => o.ArticleId).Skip(((int)pageno - 1) * 6).Take(6))
            {
                s += "<li id='news1'><a href='/News/details/" + a.ArticleId + "'>" + a.Intro + "</a></li>";
            }
            s += pagebtn;
            return Content(s);
        }

        // GET: News
        public ActionResult Index(int? pageno)
        {
            if (pageno == null)
                pageno = 1;
            int prepage = (int)pageno - 1;
            int nextpage = (int)pageno + 1;
            int pagecount = db.Articles.Count() / 6 + 1;
            //边界判断
            if (prepage < 1) prepage = 1;
            if (nextpage > pagecount) nextpage = pagecount;

            ViewBag.page = "<a href='/news/index?pageno=1'>首页</ a > &nbsp; &nbsp; &nbsp; &nbsp; ";
            ViewBag.page += "<a href='/news/index?pageno=" +prepage + "'>上一页</a>&nbsp;&nbsp;&nbsp;&nbsp;";
            ViewBag.page += pageno +"</a>&nbsp;&nbsp;&nbsp;&nbsp;";
            ViewBag.page += "<a href='/news/index?pageno=" +nextpage + "'>下一页</a>&nbsp;&nbsp;&nbsp;&nbsp;";
            ViewBag.page += "<a href='/news/index?pageno=" +pagecount + "'>末页</a>&nbsp;&nbsp;&nbsp;&nbsp;";
            var q = db.Articles.OrderBy(o =>o.ArticleId).Skip(((int)pageno - 1) * 6).Take(6);
            return View("index", q);
        }
        public ActionResult Index1(int? pageno, int? kind)
        {
            if (pageno == null)
                pageno = 1;
            int prepage = (int)pageno - 1;
            int nextpage = (int)pageno + 1;
            if (kind == null)
            {
                kind = 0;
            }
            z= (int)kind;
            var news = from m in db.Articles select m;
            if (kind != 0)
            {
                news = news.Where(s => s.CategoryId.Equals(z));
            }
            int pagecount = news.Count() / 6 + 1;
            if (prepage < 1) prepage = 1;
            if (nextpage >= pagecount) nextpage = pagecount;
            ViewBag.page = "<ahref = '/news/index1?pageno=1&&kind=" + kind + "'> 首页</ a > &nbsp; &nbsp; &nbsp; &nbsp; ";
            ViewBag.page += "<a href='/news/index1?pageno=" +prepage + "&&kind=" + kind + "'>上一页</ a > &nbsp; &nbsp; &nbsp; &nbsp; ";
            ViewBag.page += pageno +"</a>&nbsp;&nbsp;&nbsp;&nbsp;";
            ViewBag.page += "<a href='/news/index1?pageno=" +nextpage + "&&kind=" + kind + "'>下一页</ a > &nbsp; &nbsp; &nbsp; &nbsp; ";
            ViewBag.page += "<a href='/news/index1?pageno=" +pagecount + "&&kind=" + kind + "'>末页</a > &nbsp; &nbsp; &nbsp; &nbsp; ";
            now = (int)pageno;
            return View();
        }

        public ActionResult Index2()
        {
            var news = from m in db.Articles
                       select m;
            if (z != 0)
            {
                news = news.Where(s => s.CategoryId.Equals(z));
            }
            var q = news.OrderBy(o => o.ArticleId).Skip(((int)now- 1) * 6).Take(6);
            return Json(q, JsonRequestBehavior.AllowGet);
            // return Json(db.Articles,JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index3(int? id)
        {
            Article article = db.Articles.Find(id); //找到
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article); //放到视图里访问
        }

        public ActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(db.Categorys,"CategoryId", "CategoryName");
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Article a)
        {
            if (ModelState.IsValid) //如果数据符合要求（ 按照规则校验）
            {  db.Articles.Add(a);
                db.SaveChanges();
            }
            return RedirectToAction("index");
        }

        public ActionResult Edit(int id)
        {
            Article a = db.Articles.SingleOrDefault(m =>
            m.ArticleId == id);
            ViewData["CategoryId"] = new SelectList(db.Categorys,"CategoryId", "CategoryName", a.CategoryId);
            return View(a);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Article article) //修改的时候提交的数据时一个类
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;//修改
                db.SaveChanges(); //保存
                return RedirectToAction("index");
            }
            return View("index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        [HttpPost]
        public ActionResult Delete(int id) //删除的时候提交的数据只是id 和修改不一样
        {
            Article article = db.Articles.Find(id); //找到
            db.Articles.Remove(article);//删除
            db.SaveChanges();//保存
        return RedirectToAction("index");
        }
        public ActionResult Details(int? id) //删除的时候提交的数据只是id 和修改不一样
        {
            Article article = db.Articles.Find(id); //找到
            if (article == null)
            {
                    return HttpNotFound();
            }
            return View(article); //放到视图里访问
        }
        
    }
}