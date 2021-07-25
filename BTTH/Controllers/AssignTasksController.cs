using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTTH.Models;
using PagedList;

namespace BTTH.Controllers
{
    public class AssignTasksController : Controller
    {
        private Model_Project_Context db = new Model_Project_Context();

        // GET: AssignTasks
        public ActionResult Index(string searchString, string Dep,string com, string Date, int? Page_No)
        {
            var ass = from a in db.AssignTasks
                      join cl in db.Clients
                      on a.ClientId equals cl.ClientId
                      join e in db.Employees
                      on a.EmployeeId equals e.EmployeeId
                      join p in db.Projects
                      on a.ProjectId equals p.ProjectId
                      select a;
            if (!String.IsNullOrEmpty(searchString))
                ass = ass.Where(cl => cl.Client.ClientName.Contains(searchString) || cl.Employee.EmployeeName.Contains(searchString) || cl.Project.ProjectName.Contains(searchString));

            var DepartmentList = new List<String>();
            var DepartmentSql = from e in db.Employees
                                orderby e.EmployeeId
                                select e.EmployeeDepartment;
            DepartmentList.AddRange(DepartmentSql.Distinct());
            ViewBag.Dep = new SelectList(DepartmentSql);
            if (!String.IsNullOrEmpty(Dep)) {
                ass = ass.Where(e => e.Employee.EmployeeDepartment.Contains(Dep));
            }


            var CompanyList = new List<String>();
            var Comsql = from cl in db.Clients
                         orderby cl.ClientId
                         select cl.ClientCompany;
            CompanyList.AddRange(Comsql.Distinct());
            ViewBag.com = new SelectList(Comsql);
            if (!String.IsNullOrEmpty(com))
                ass = ass.Where(cl => cl.Client.ClientCompany.Contains(com));

            var datelist = new List<String>()
            {
                "Đã hoàn thành", "Đang thực hiện"
            };
            ViewBag.Date = new SelectList(datelist);
            if (!String.IsNullOrEmpty(Date) && Date.Equals("Đã hoàn thành")) {
                ass = ass.Where(p => p.Project.ProjectEnd != null && p.Project.ProjectEnd  < DateTime.Now);
            }
            else if(!String.IsNullOrEmpty(Date) && Date.Equals("Đang thực hiện")){
                ass = ass.Where(p => p.Project.ProjectEnd == null || p.Project.ProjectEnd > DateTime.Now);
            }

            int PagedSize = 2;
            int NoOfPaged = (Page_No ?? 1);

            //Lưu ý: ToPagedList chỉ hoạt động khi "ass" được sắp xếp theo 1 trình tư nào đấy
            ass = ass.OrderBy(a => a.AssignTaskId);
            //var assignTasks = db.AssignTasks.Include(a => a.Client).Include(a => a.Employee).Include(a => a.Project);
            return View(ass.ToPagedList(NoOfPaged,PagedSize));
        }

        // GET: AssignTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignTask assignTask = db.AssignTasks.Find(id);
            if (assignTask == null)
            {
                return HttpNotFound();
            }
            return View(assignTask);
        }

        // GET: AssignTasks/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "ClientName");
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "EmployeeName");
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName");
            return View();
        }

        // POST: AssignTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssignTaskId,EmployeeId,ClientId,ProjectId,Task,Note")] AssignTask assignTask)
        {
            if (ModelState.IsValid)
            {
                db.AssignTasks.Add(assignTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "ClientName", assignTask.ClientId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "EmployeeName", assignTask.EmployeeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName", assignTask.ProjectId);
            return View(assignTask);
        }

        // GET: AssignTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignTask assignTask = db.AssignTasks.Find(id);
            if (assignTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "ClientName", assignTask.ClientId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "EmployeeName", assignTask.EmployeeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName", assignTask.ProjectId);
            return View(assignTask);
        }

        // POST: AssignTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssignTaskId,EmployeeId,ClientId,ProjectId,Task,Note")] AssignTask assignTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "ClientName", assignTask.ClientId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "EmployeeName", assignTask.EmployeeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName", assignTask.ProjectId);
            return View(assignTask);
        }

        // GET: AssignTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignTask assignTask = db.AssignTasks.Find(id);
            if (assignTask == null)
            {
                return HttpNotFound();
            }
            return View(assignTask);
        }

        // POST: AssignTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssignTask assignTask = db.AssignTasks.Find(id);
            db.AssignTasks.Remove(assignTask);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
