using StudentJobApplication.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace StudentJobApplication.Controllers
{
    public class StudentController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public ActionResult Index()
        {
            List<StudentApplication> students = new List<StudentApplication>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_ManageStudentApplication", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Action", "SELECT_ALL");
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    students.Add(new StudentApplication
                    {
                        StudentID = Convert.ToInt32(rdr["StudentID"]),
                        FirstName = rdr["FirstName"].ToString(),
                        LastName = rdr["LastName"].ToString(),
                        Email = rdr["Email"].ToString(),
                        Phone = rdr["Phone"].ToString(),
                        Photo = rdr["Photo"] as byte[],
                        Resume = rdr["Resume"] as byte[]
                    });
                }
            }
            return View(students);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(StudentApplication student, HttpPostedFileBase photoFile, HttpPostedFileBase resumeFile)
        {
            if (ModelState.IsValid)
            {
                if (photoFile != null && photoFile.ContentType.Contains("image"))
                {
                    using (var binaryReader = new BinaryReader(photoFile.InputStream))
                    {
                        student.Photo = binaryReader.ReadBytes(photoFile.ContentLength);
                    }
                }
                if (resumeFile != null && resumeFile.ContentType.Contains("pdf"))
                {
                    using (var binaryReader = new BinaryReader(resumeFile.InputStream))
                    {
                        student.Resume = binaryReader.ReadBytes(resumeFile.ContentLength);
                    }
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_ManageStudentApplication", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", student.LastName);
                    cmd.Parameters.AddWithValue("@Email", student.Email);
                    cmd.Parameters.AddWithValue("@Phone", student.Phone);
                    cmd.Parameters.AddWithValue("@Photo", student.Photo);
                    cmd.Parameters.AddWithValue("@Resume", student.Resume);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            StudentApplication student = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_ManageStudentApplication", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Action", "SELECT");
                cmd.Parameters.AddWithValue("@StudentID", id);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    student = new StudentApplication
                    {
                        StudentID = Convert.ToInt32(rdr["StudentID"]),
                        FirstName = rdr["FirstName"].ToString(),
                        LastName = rdr["LastName"].ToString(),
                        Email = rdr["Email"].ToString(),
                        Phone = rdr["Phone"].ToString(),
                        Photo = rdr["Photo"] as byte[],
                        Resume = rdr["Resume"] as byte[]
                    };
                }
            }
            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(StudentApplication student, HttpPostedFileBase photoFile, HttpPostedFileBase resumeFile)
        {
            if (ModelState.IsValid)
            {
                if (photoFile != null && photoFile.ContentType.Contains("image"))
                {
                    using (var binaryReader = new BinaryReader(photoFile.InputStream))
                    {
                        student.Photo = binaryReader.ReadBytes(photoFile.ContentLength);
                    }
                }
                if (resumeFile != null && resumeFile.ContentType.Contains("pdf"))
                {
                    using (var binaryReader = new BinaryReader(resumeFile.InputStream))
                    {
                        student.Resume = binaryReader.ReadBytes(resumeFile.ContentLength);
                    }
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_ManageStudentApplication", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@StudentID", student.StudentID);
                    cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", student.LastName);
                    cmd.Parameters.AddWithValue("@Email", student.Email);
                    cmd.Parameters.AddWithValue("@Phone", student.Phone);
                    cmd.Parameters.AddWithValue("@Photo", student.Photo);
                    cmd.Parameters.AddWithValue("@Resume", student.Resume);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_ManageStudentApplication", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@StudentID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}