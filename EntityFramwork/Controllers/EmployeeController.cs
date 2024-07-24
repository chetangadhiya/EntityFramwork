using EntityFramwork.Auth;
using EntityFramwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EntityFramwork.Controllers
{
    [Authorization("Admin,HR")]
    [LogActionFilter]
    public class EmployeeController : Controller
    {
        private Demo1Entities db = new Demo1Entities();


        // GET: Employee
        public ActionResult Index()
        {
            
            var list = db.Employees.ToList();

            //EF
            List<Detail> abc = list.Where(x=>x.Age>28).Select(x => new Detail() {FirstName = x.FirstName, LastName = x.LastName, Age=(int)x.Age }).Distinct().ToList();

            //LINQ
            var empName = (from emp in db.Employees.Where(x=>x.Age<28).ToList()
                           select emp.FirstName).Distinct().ToList();

            var LinqEmpData = (from mulEmp in db.Employees.ToList()
                               where mulEmp.Age <28
                               select new Detail()
                               {
                                   FirstName = mulEmp.FirstName,
                                   LastName = mulEmp.LastName,
                                   Age = (int)mulEmp.Age
                               }).ToList();


            List<object> objData = new List<object>() { "Chetan", "Gadhiya", 24, 100000, };
            var intData = objData.OfType<int>().ToList();

            var EfExcep = LinqEmpData.Except(abc).ToList();

            var linqExcep = (from linq1 in LinqEmpData
                             select linq1).Except(abc).ToList();

            var EfInter = LinqEmpData.Intersect(abc).ToList();

            var LinqConcat = (from co1 in LinqEmpData
                              select co1).Concat(abc).ToList();

            var LinqUnion = (from uni1 in LinqEmpData
                             select uni1).Union(abc).ToList();


            var LinqEnumerable = (from Enu in db.Employees.ToList()
                                  select new Detail
                                  {
                                      FirstName = Enu.FirstName,
                                      Age = (int)Enu.Age

                                  }).ToList().AsEnumerable();

            var EFIQueryable = db.Employees.AsQueryable().ToList();

            var EFGroup = db.Employees.GroupBy(x => x.Department).ToList();

            var EFfilter = EFGroup.Where(x => x.Key == "Sortation");

            var LqData = from LinqEmp in db.Employees.ToList()
                         group LinqEmp by LinqEmp.Department into DepKey
                         select new Detail
                         {
                             key = DepKey.Key,
                             FirstName = DepKey.FirstOrDefault().FirstName,
                             LastName = DepKey.FirstOrDefault().LastName,
                             Age = (int)DepKey.FirstOrDefault().Age,
                             Employees = DepKey.OrderBy(x=>x.Gender).ToList()

                         };

            var MulGroup = db.Employees.ToList().GroupBy(x => (x.Gender, x.Department))
                            .Where(x=>x.Key.Gender.Equals("F"))
                            .Select(y => new
                            {
                                Gender = y.Key.Gender,
                                Department = y.Key.Department,
                                Employees = y.OrderBy(z => z.FirstName)
                            });

            int F_Count = MulGroup.Where(x => x.Department.Equals("HR")).FirstOrDefault().Employees.Count();


            //JOINS

            var LeftJoin = from salesLJ in db.Sales.ToList()
                           join EmployeeLJ in db.Employees.ToList()
                           on salesLJ.SaleID equals EmployeeLJ.EmployeeId
                           into employeDetail
                           from empDetailData in employeDetail.DefaultIfEmpty()
                           select new { salesLJ, empDetailData };

            var EF_InnerJoin = db.Sales.ToList()
                            .Join(db.Employees.ToList(),
                            sale => sale.SaleID,
                            emp => emp.EmployeeId,
                            (sale, emp) => new
                            {
                                sale,
                                emp

                            }).ToList();

            var LQ_Mul_innerJoin = from stu in db.Students.ToList()
                               join course in db.Courses.ToList()
                               on stu.CourseId equals course.CourseId
                               join teacher in db.Teachers.ToList()
                               on stu.TeacherId equals teacher.TeacherId
                               select new
                               {
                                   stu
                                   //course,
                                   //teacher
                               };

            var EF_Mul_innerJoin = db.Students.ToList()
                                    .Join(db.Courses.ToList(), S => S.StudentId, c => c.CourseId, (S, c) => new { S, c })
                                    .Join(db.Teachers.ToList(), S => S.S.TeacherId, t => t.TeacherId, (S, t) => new { S, t })
                                    .Select(x => new
                                    {
                                        stu = x.S.S,
                                        cou = x.S.c,
                                       //teaName = x.
                                    });


            var groupJoin = (from stu in db.Students.ToList()
                             join co in db.Courses.ToList()
                             on stu.CourseId equals co.CourseId
                             select new
                             {
                                 stu.StudentId,
                                 stu.S_Name,
                                 co.C_Name
                             }).ToList();

            var EFGroupjoin = db.Students.ToList()
                              .GroupJoin(db.Courses.ToList()
                              , stu => stu.CourseId
                              , cou => cou.CourseId
                              , (stu, cou) => new { 
                                  stu.StudentId,
                                  stu.S_Name,
                                  cou.FirstOrDefault().CourseId,
                                  cou.FirstOrDefault().C_Name}).ToList();








            return View(LinqEmpData);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(employee);
        }

        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var data = db.Employees.Find(id);
            if(data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(employee);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

                var D_data = db.Employees.Find(id);
                db.Employees.Remove(D_data);
                db.SaveChanges();
                return RedirectToAction("Index");
            
        }
    }
}