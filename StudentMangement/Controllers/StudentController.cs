using Grpc.Core;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudentMangement.Models;
using StudentMangement.Repository.Abstract;
using System.Diagnostics;

namespace StudentMangement.Controllers
{
    public class StudentController : Controller
    {
        private readonly Db _db;
        private IWebHostEnvironment environment;
        public readonly IFileService _fileService;

        public StudentController (Db db, IWebHostEnvironment env, IFileService fs)
        {
            _db = db;
            this.environment = env;
            this._fileService = fs;
        }


        [HttpGet]
        public  IActionResult RegisterStudent()
        {
            return View();
        }


        [HttpPost] 
        public IActionResult RegisterStudent( StudentRequest student)
        {
            try
            {
                var fileResult = _fileService.SaveImage(student.ImageFile);

                if (fileResult.Item1 == 1)
                {
                    var studentDB = new Student();
                    studentDB.FirstName = student.FirstName;
                    studentDB.MiddleName = student.MiddleName;
                    studentDB.LastName = student.LastName;
                    studentDB.Dirth = student.Dirth;
                    studentDB.Image = fileResult.Item2;

                    _db.Students.Add(studentDB);
                    _db.SaveChanges();

                    return RedirectToAction("Students");
                }

                return View();  
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public  IActionResult Students()
        {
            var students = _db.Students.ToList();
            return View(students);
        }

        public IActionResult Delete(int id)
        {
            var student = _db.Students.Find(id);
            if (student  != null)
            {
                var deleteImage = _fileService.DeleteImage(student.Image);
                _db.Students.Remove(student);   
                _db.SaveChanges();
                return RedirectToAction("Students");
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditStudent(int id)
        {
            var student = _db.Students.Find(id);
            if (student != null) { 
                var studentRespon = new StudentResponse();
                studentRespon.Id = id;
                studentRespon.Dirth = student.Dirth;
                studentRespon.LastName = student.LastName;
                studentRespon.FirstName = student.FirstName;
                studentRespon.MiddleName = student.MiddleName;
                //studentRespon.ImageFile = Server( _fileService.GetImage(student.Image));
                return View(studentRespon);
            }
            return RedirectToAction("Students");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
