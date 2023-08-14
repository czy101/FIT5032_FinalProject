using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Test3.Models;

using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;

namespace Test3.Controllers
{
    public class FeedbacksController : Controller
    {
        private FeedbackModel db = new FeedbackModel();

        // GET: Feedbacks
        public ActionResult Index()
        {
            return View(db.Feedbacks.ToList());
        }

        // GET: Feedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: Feedbacks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Feedbacks/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性。有关
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Feedback_id,Appointment_id,Feedback_Date,Feedback_Content,Doctor_id,Patient_id,Feedback_File_Path")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性。有关
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Feedback_id,Appointment_id,Feedback_Date,Feedback_Content,Doctor_id,Patient_id,Feedback_File_Path")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult FeedbacksByDoctor()
        {
            // 从数据库中检索数据
            var data = db.Feedbacks
                .GroupBy(x => x.Doctor_id)
                .Select(x => new FeedbackData { DoctorId = x.Key, Count = x.Count() })
                .ToList();

            // 将数据传递给视图
            return View(data);
        }

        public ActionResult ExportCSVData()
        {
            // 从数据库中检索数据
            var data = db.Feedbacks.ToList();

            // 创建一个StringBuilder对象，用于存储CSV数据
            var csv = new StringBuilder();

            // 添加CSV文件的标题行
            csv.AppendLine("Feedback_id,Appointment_id,Feedback_Date,Feedback_Content,Doctor_id,Patient_id,Feedback_File_Path");

            // 添加CSV文件的数据行
            foreach (var item in data)
            {
                csv.AppendLine($"{item.Feedback_id},{item.Appointment_id},{item.Feedback_Date},{item.Feedback_Content},{item.Doctor_id},{item.Patient_id},{item.Feedback_File_Path}");
            }

            // 获取当前日期
            var currentDate = DateTime.Now.ToString("yyyyMMdd");

            // 将CSV数据返回给客户端
            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"{currentDate}csvdata.csv");
        }


        public ActionResult ExportJSONData()
        {
            // 从数据库中检索数据
            var data = db.Feedbacks.ToList();

            // 将数据序列化为JSON字符串
            var json = JsonConvert.SerializeObject(data);

            // 获取当前日期
            var currentDate = DateTime.Now.ToString("yyyyMMdd");

            // 将JSON数据返回给客户端
            return File(Encoding.UTF8.GetBytes(json), "application/json", $"{currentDate}jsondata.json");
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
