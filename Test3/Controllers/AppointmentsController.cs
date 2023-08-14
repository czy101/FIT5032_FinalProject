using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Test3.Models;
using System.Diagnostics;
using System.Net.Mail;

namespace Test3.Controllers
{
    public class AppointmentsController : Controller
    {
        private Entities db = new Entities();

        // GET: Appointments
        public ActionResult Index()
        {
            return View(db.Appointments.ToList());
        }
        // GET: Doctor Appointments index
        public ActionResult Doctor_Index(String id)
        {
            //var appointments = db.Appointments.Where(a => a.Doctor_id == Doctor_id);
            //if (Doctor_id == null)
            //{
            //     Doctor_id = db.Appointments.Select(a => a.Doctor_id).FirstOrDefault();
            //}
            //System.Diagnostics.Debug.WriteLine("Doctor ID: " + Doctor_id);
            //string doctorId = id.ToString();
            var appointments = db.Appointments.Where(a => a.Doctor_id == id);

            System.Diagnostics.Debug.WriteLine("Appointments count: " + appointments.Count());
            foreach (var appointment in appointments)
            {
                System.Diagnostics.Debug.WriteLine("Appointment ID: " + appointment.Appointment_id);
                System.Diagnostics.Debug.WriteLine("Doctor ID: " + appointment.Doctor_id);
            }
            return View(appointments.ToList());
        }
        // GET: Appointments/Patient_Index
        public ActionResult Patient_Index(String id)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var appointments = db.Appointments.Where(a => a.Patient_id == id).ToList();
            var doctors = userManager.Users.Where(u => u.Person_role == "Doctor").ToList();
            var model = appointments.Select(a => new AppointmentViewModel
            {
                Appointment_id = a.Appointment_id,
                Patient_id = a.Patient_id,
                Doctor_id = a.Doctor_id,
                Doctor_name = doctors.FirstOrDefault(d => d.Id == a.Doctor_id)?.Person_name,
                Doctor_email = doctors.FirstOrDefault(d => d.Id == a.Doctor_id)?.Email,
                Date = DateTime.Parse(a.Date),
                Status = a.Status
            });
            return View(model);
        }

        // GET: Appointments/Show_data_table
        public ActionResult Show_data_table(String id)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var appointments = db.Appointments.Where(a => a.Patient_id == id).ToList();
            var doctors = userManager.Users.Where(u => u.Person_role == "Doctor").ToList();
            var model = appointments.Select(a => new AppointmentViewModel
            {
                Appointment_id = a.Appointment_id,
                Patient_id = a.Patient_id,
                Doctor_id = a.Doctor_id,
                Doctor_name = doctors.FirstOrDefault(d => d.Id == a.Doctor_id)?.Person_name,
                Doctor_email = doctors.FirstOrDefault(d => d.Id == a.Doctor_id)?.Email,
                Date = DateTime.Parse(a.Date),
                Status = a.Status
            });
            return View(model);
        }


        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Appointments/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性。有关
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Appointment_id,Patient_id,Doctor_id,Patient_email,Date,Status")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appointment);
        }


        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性。有关
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Appointment_id,Patient_id,Doctor_id,Patient_email,Date,Status")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Appointments/Doctor_Accept_Finish/5
        public ActionResult Doctor_Accept_Finish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Doctor_Accept_Finish/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Doctor_Accept_Finish(int id, [Bind(Include = "Appointment_id,Status")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                var appointmentToUpdate = db.Appointments.Find(id);
                if (appointmentToUpdate == null)
                {
                    return HttpNotFound();
                }
                appointmentToUpdate.Status = appointment.Status;
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }

                //return RedirectToAction("Index");
                // 重定向到带有用户ID参数的Doctor_Index操作
                Debug.WriteLine("返回的页面医生ID", appointmentToUpdate.Doctor_id);
                return RedirectToAction("Doctor_Index", new { id = appointmentToUpdate.Doctor_id });

            }
            return View(appointment);
        }
        // GET: Appointments/Doctor_Create
        public ActionResult Doctor_Create()
        {
            // 获取当前登录用户
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            // 获取所有病人的电子邮件地址
            var patients = userManager.Users.Where(u => u.Person_role == "Patient").ToList();
            foreach (var patient in patients)
            {
                System.Diagnostics.Debug.WriteLine("Patient ID: " + patient.Id + ", Email: " + patient.Email);
            }
            ViewBag.PatientEmails = new SelectList(patients, "Id", "Email");

            // 创建预约对象并设置医生ID
            var appointment = new Appointment { Doctor_id = user.Id };
            return View(appointment);
        }

        // POST: Appointments/Doctor_Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Doctor_Create([Bind(Include = "Appointment_id,Patient_id,Doctor_id,Patient_email,Date,Status")] Appointment appointment)
        {
            // 打印提交的表单数据
            System.Diagnostics.Debug.WriteLine("Patient ID: " + appointment.Patient_id);
            System.Diagnostics.Debug.WriteLine("Doctor ID: " + appointment.Doctor_id);
            System.Diagnostics.Debug.WriteLine("Date: " + appointment.Date);
            System.Diagnostics.Debug.WriteLine("Status: " + appointment.Status);
            // 检查预约时间是否冲突
            var conflictAppointment = db.Appointments.FirstOrDefault(a => a.Patient_id == appointment.Patient_id && a.Date == appointment.Date && a.Status == appointment.Status);
            if (conflictAppointment != null)
            {
                // 如果存在冲突的预约，则显示错误消息
                ModelState.AddModelError("", "预约时间冲突！");
            }


            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                try
                {
                    // 保存更改
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // 遍历验证错误
                    foreach (var errors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in errors.ValidationErrors)
                        {
                            // 获取错误消息
                            string errorMessage = validationError.ErrorMessage;
                            System.Diagnostics.Debug.WriteLine(errorMessage);
                        }
                    }
                }

                // 重定向到带有用户ID参数的Doctor_Index操作
                return RedirectToAction("Doctor_Index", new { id = appointment.Doctor_id });
            }

            return View(appointment);
        }
        // GET: Appointments/Patient_Create
        public ActionResult Patient_Create()
        {
            // 获取当前登录用户
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            // 获取所有部门
            var departments = userManager.Users.Where(u => u.Person_role == "Doctor").Select(u => u.Department).Distinct().ToList();
            foreach (var department in departments)
            {
                Debug.WriteLine("部门:" + department);
            }
            ViewBag.Departments = new SelectList(departments);

            // 获取所有医生
            var doctors = userManager.Users.Where(u => u.Person_role == "Doctor").ToList();
            foreach (var doctor in doctors)
            {
                Debug.WriteLine("所有医生：" + doctor.Person_name);
            }
            ViewBag.Doctors = doctors;

            // 创建预约对象并设置病人ID和电子邮件地址
            var appointment = new Appointment { Patient_id = user.Id, Patient_email = user.Email };
            return View(appointment);
        }

        // POST: Appointments/Patient_Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Patient_Create([Bind(Include = "Appointment_id,Patient_id,Doctor_id,Patient_email,Date,Status")] Appointment appointment)
        {
            // 设置预约状态为"pending acceptance"
            appointment.Status = "pending acceptance";


            // 检查预约是否与数据库中现有的预约冲突
            /*var conflict = db.Appointments.Any(a => a.Patient_id == appointment.Patient_id && a.Doctor_id == appointment.Doctor_id && a.Date == appointment.Date);
            if (conflict)
            {
                // 如果存在冲突，则添加模型错误并返回视图
                ModelState.AddModelError("", "时间冲突，请重新预定。");
                return View(appointment);
            } */

            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                try
                {
                    // 保存更改
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // 遍历验证错误
                    foreach (var errors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in errors.ValidationErrors)
                        {
                            // 获取错误消息
                            string errorMessage = validationError.ErrorMessage;
                            System.Diagnostics.Debug.WriteLine(errorMessage);
                        }
                    }
                }

                // 重定向到带有用户ID参数的Patient_Index操作
                return RedirectToAction("Patient_Index", new { id = appointment.Patient_id });
            }

            return View(appointment);
        }

        // GET: Appointments/Patient_Delete/5
        public ActionResult Patient_Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            // 检查当前用户是否有权删除此预约
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());
            if (appointment.Patient_id != user.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(appointment);
        }

        // POST: Appointments/Patient_Delete/5
        [HttpPost, ActionName("Patient_Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Patient_DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);

            // 检查当前用户是否有权删除此预约
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());
            if (appointment.Patient_id != user.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            // 检查预约状态是否为"pending acceptance"
            if (appointment.Status != "pending acceptance")
            {
                ViewBag.ErrorMessage = "无法删除此预约，因为它的状态不是'pending acceptance'。";
                return View(appointment);
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();
            return RedirectToAction("Patient_Index", new { id = appointment.Patient_id });
        }

        // GET: Appointments/SendFeedback
        //  [HttpGet, ActionName("SendFeedback")]
        public ActionResult SendFeedbackGet()
        {

            return View();
        }
        // POST: Appointments/SendFeedback
        //发送邮件
        [HttpPost]
        public ActionResult SendFeedback()
        {
            var appointment = new Appointment
            {


                Patient_id = Request.Form["StudentID"],
                Doctor_id = Request.Form["EngineerID"],
                Date = Request.Form["AppointmentDate"]
            };

            var vx = Request.Files["Attachment"].ContentLength;

            // Store the attachment in local storage.
            var Str1 = Request.Files[0].FileName.Split('.');
            var FileType = Str1[Str1.Length - 1];
            var FilePath =
                Server.MapPath("~/Uploads/") +
                string.Format(@"{0}", Guid.NewGuid()) +
                "." + FileType;
            Request.Files[0].SaveAs(FilePath);

            if (ModelState.IsValid)
            {
                // Add the appointment into the database.
                //db.Appointments.Add(appointment);
                //db.SaveChanges();

                // Send confirmation email.
                var mail = new MailMessage();
                mail.To.Add(new MailAddress(Request.Form["EmailAddress"]));
                mail.From = new MailAddress("czytalent@outlook.com");

                mail.Subject = "Appointment Conformation";
                mail.Body =
                    "Your feedback!\n" +
                    "Patient id: " + Request.Form["StudentID"] + "\n" +
                    "Doctor id: " + Request.Form["EngineerID"] + "\n" +
                    "Date: " + Request.Form["AppointmentDate"];
                mail.IsBodyHtml = false;

                var attachment = new System.Net.Mail.Attachment(FilePath);
                mail.Attachments.Add(attachment);

                var smtp = new SmtpClient();
                smtp.Host = "smtp.office365.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential
                    ("czytalent@outlook.com", "Czy74110");
                // 增加超时时间
                smtp.Timeout = 60000; // 60秒
                smtp.Send(mail);



                return View(appointment); ;
            }

            return null;
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
