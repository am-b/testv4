﻿#region Includes
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Testv3.Models;
using PagedList;
using System.Threading.Tasks;
using System.IO;
using ExcelDataReader;



#endregion Includes

namespace Testv3.Controllers
{
    
    public class AdminController : DefaultController
    {
        private Testv3Entities db = new Testv3Entities();
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }




        // GET: /Admin/
        [Authorize(Roles = "Administrator,Counselor")]
        #region public ActionResult Index(string searchStringUserNameOrEmail)
        public ActionResult Index(string searchStringUserNameOrEmail, string currentFilter, int? page)
        {
            GetCurrentUserInViewBag();
            try
            {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;

                if (searchStringUserNameOrEmail != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringUserNameOrEmail = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringUserNameOrEmail = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringUserNameOrEmail;

                List<ExpandedUserDTO> col_UserDTO = new List<ExpandedUserDTO>();
                int intSkip = (intPage - 1) * intPageSize;

                intTotalPageCount = UserManager.Users
                    .Where(x => x.UserName.Contains(searchStringUserNameOrEmail))
                    .Count();

                var result = UserManager.Users
                    .Where(x => x.UserName.Contains(searchStringUserNameOrEmail))
                    .OrderBy(x => x.UserName)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                foreach (var item in result)
                {
                    ExpandedUserDTO objUserDTO = new ExpandedUserDTO();

                    objUserDTO.UserName = item.UserName;
                    objUserDTO.Email = item.Email;
                    objUserDTO.LockoutEndDateUtc = item.LockoutEndDateUtc;

                    //Update your DTO here so that it store other attributes as well. For eg
                    /*
                     * objUserDTO.FirstName = item.firstname;
                     * objUserDTO.LastName = item.lastname;
                     * etc etc
                     */

                    col_UserDTO.Add(objUserDTO);
                }

                // Set the number of pages
                var _UserDTOAsIPagedList =
                    new StaticPagedList<ExpandedUserDTO>
                    (
                        col_UserDTO, intPage, intPageSize, intTotalPageCount
                        );

                return View(_UserDTOAsIPagedList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<ExpandedUserDTO> col_UserDTO = new List<ExpandedUserDTO>();

                return View(col_UserDTO.ToPagedList(1, 25));
            }
        }
        #endregion

        // Users *****************************

        // GET: /Admin/Edit/Create 
        [Authorize(Roles = "Administrator")]
        #region public ActionResult Create()
        public ActionResult Create()
        {
            GetCurrentUserInViewBag();
            ExpandedUserDTO objExpandedUserDTO = new ExpandedUserDTO();
            ViewBag.Roles = GetAllRolesAsSelectList();

            var college = GetAllCollege();
            objExpandedUserDTO.Programm = GetSelectListItems(college);


            return View(objExpandedUserDTO);
        }
        #endregion

        // PUT: /Admin/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region 
        public async Task<ActionResult> Create(ExpandedUserDTO paramExpandedUserDTO)
        {
            GetCurrentUserInViewBag();
            try
            {
                if (paramExpandedUserDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var college = GetAllCollege();
                paramExpandedUserDTO.Programm = GetSelectListItems(college);


                var Email = paramExpandedUserDTO.Email.Trim();
                var UserName = paramExpandedUserDTO.Email.Trim();
                var Password = paramExpandedUserDTO.Password.Trim();

                var LastName = paramExpandedUserDTO.StudentLastName.Trim();
                var FirstName = paramExpandedUserDTO.StudentFirstName.Trim();
                var MiddleName = paramExpandedUserDTO.StudentMiddleName.Trim();


                var Program = paramExpandedUserDTO.Program.Trim();
                var YrLevel = paramExpandedUserDTO.YearLevel;
                var StudentID = paramExpandedUserDTO.StudentID;
                //var IsActive = paramExpandedUserDTO.IsActive.Trim();


                if (Email == "")
                {
                    throw new Exception("No Email");
                }
                if (Password == "")
                {
                    throw new Exception("No Password");
                }
                if (LastName == "")
                {
                    throw new Exception("No LastName");
                }
                if (FirstName == "")
                {
                    throw new Exception("No FirstName");
                }
                if (MiddleName == "")
                {
                    throw new Exception("No MiddleName");
                }
                if (Program == "")
                {
                    throw new Exception("No Program");
                }
                if (YrLevel == null)
                {
                    throw new Exception("No YrLevel");
                }
                if (StudentID == "")
                {
                    throw new Exception("No StudentID");
                }


                // UserName is LowerCase of the Email
                UserName = Email.ToLower();

                // Create user
                var objNewAdminUser = new ApplicationUser { UserName = UserName, Email = Email };
                var AdminUserCreateResult = UserManager.Create(objNewAdminUser, Password);

                if (AdminUserCreateResult.Succeeded == true)
                {
                    string strNewRole = Convert.ToString(Request.Form["Roles"]);

                    if (strNewRole != "0")
                    {
                        // Put user in role
                        UserManager.AddToRole(objNewAdminUser.Id, strNewRole);

                        //Put user in student table
                        if (strNewRole == "Student")
                        {

                            var newid = db.Students.FirstOrDefault(d => d.UserID == objNewAdminUser.Id);
                            if (newid == null)
                            {
                                newid = db.Students.Create();
                                newid.UserID = objNewAdminUser.Id;
                                newid.StudentEmail = objNewAdminUser.Email;

                                newid.StudentLastName = LastName;
                                newid.StudentFirstName = FirstName;
                                newid.StudentMiddleName = MiddleName;
                                newid.StudentID = StudentID;
                                newid.Program = Program;
                                newid.YearLevel = YrLevel;

                                db.Students.Add(newid);

                                db.SaveChanges();
                            }
                        }

                        //Put user in counselor table
                        if (strNewRole == "Counselor")
                        {

                            var newid = db.Counsellor.FirstOrDefault(d => d.UserID == objNewAdminUser.Id);
                            if (newid == null)
                            {
                                newid = db.Counsellor.Create();
                                newid.UserID = objNewAdminUser.Id;
                                newid.CounsellorEmail = objNewAdminUser.Email;
                                db.Counsellor.Add(newid);
                                db.SaveChanges();
                            }
                        }

                    }

                    
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(objNewAdminUser.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = objNewAdminUser.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(objNewAdminUser.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return Redirect("~/Admin");
                }
                else
                {
                    ViewBag.Roles = GetAllRolesAsSelectList();

                    ModelState.AddModelError(string.Empty, "Error: User " + Email + " already exists!");
                    return View(paramExpandedUserDTO);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Roles = GetAllRolesAsSelectList();

                ExpandedUserDTO objExpandedUserDTO = new ExpandedUserDTO();
                var college = GetAllCollege();
                objExpandedUserDTO.Programm = GetSelectListItems(college);

                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View(paramExpandedUserDTO);
            }
        }
        #endregion


        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link:
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Admin", new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID, subject, "Please confirm your account by <a href=\"" + callbackUrl + "\">clicking here</a>");


            return callbackUrl;
        }


        //// GET: /Admin/Edit/TestUser 
        [Authorize(Roles = "Administrator")]
        public ActionResult EditUser(string UserName)
        {
            GetCurrentUserInViewBag();
            ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);
            ApplicationUser appUser = new ApplicationUser();
            appUser = UserManager.FindByEmail(UserName);
            ExpandedUserDTO u = new ExpandedUserDTO();

            u.UserName = appUser.Email;
            u.Email = appUser.Email;

            return View(u);
        }

        // PUT: /Admin/EditUser
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(ExpandedUserDTO model)
        {
            GetCurrentUserInViewBag();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(store);
            var u = manager.FindByEmail(model.Email);


            u.UserName = model.Email;
            u.Email = model.Email;

            await manager.UpdateAsync(u);
            var ctx = store.Context;
            ctx.SaveChanges();
            TempData["Message"] = "User: " +u.UserName + ", successfully updated!";

            return RedirectToAction("Index");
        }
        



        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }



        // DELETE: /Admin/DeleteUser
        [Authorize(Roles = "Administrator")]
        #region public ActionResult DeleteUser(string UserName)
        public ActionResult DeleteUser(string UserName)
        {
            GetCurrentUserInViewBag();
            try
            {
                if (UserName == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (UserName.ToLower() == this.User.Identity.Name.ToLower())
                {
                    ModelState.AddModelError(
                        string.Empty, "Error: Cannot delete the current user");

                    return View("EditUser");
                }

                ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);

                if (objExpandedUserDTO == null)
                {
                    return HttpNotFound();
                }
                else
                {

                    DeleteUser(objExpandedUserDTO);
                }

                return Redirect("~/Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditUser", GetUser(UserName));
            }
        }
        #endregion

        // GET: /Admin/EditRoles/TestUser 
        [Authorize(Roles = "Administrator")]
        #region ActionResult EditRoles(string UserName)
        public ActionResult EditRoles(string UserName)
        {
            GetCurrentUserInViewBag();
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserName = UserName.ToLower();

            // Check that we have an actual user
            ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);

            if (objExpandedUserDTO == null)
            {
                return HttpNotFound();
            }

            UserAndRolesDTO objUserAndRolesDTO =
                GetUserAndRoles(UserName);

            return View(objUserAndRolesDTO);
        }
        #endregion

        // PUT: /Admin/EditRoles/TestUser 
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region public ActionResult EditRoles(UserAndRolesDTO paramUserAndRolesDTO)
        public ActionResult EditRoles(UserAndRolesDTO paramUserAndRolesDTO)
        {
            GetCurrentUserInViewBag();
            try
            {
                if (paramUserAndRolesDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                string UserName = paramUserAndRolesDTO.UserName;
                string strNewRole = Convert.ToString(Request.Form["AddRole"]);

                if (strNewRole != "No Roles Found")
                {
                    // Go get the User
                    ApplicationUser user = UserManager.FindByName(UserName);

                    // Put user in role
                    UserManager.AddToRole(user.Id, strNewRole);
                }

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                UserAndRolesDTO objUserAndRolesDTO =
                    GetUserAndRoles(UserName);

                return View(objUserAndRolesDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditRoles");
            }
        }
        #endregion

        // DELETE: /Admin/DeleteRole?UserName="TestUser&RoleName=Administrator
        [Authorize(Roles = "Administrator")]
        #region public ActionResult DeleteRole(string UserName, string RoleName)
        public ActionResult DeleteRole(string UserName, string RoleName)
        {
            GetCurrentUserInViewBag();
            try
            {
                if ((UserName == null) || (RoleName == null))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                UserName = UserName.ToLower();

                // Check that we have an actual user
                ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);

                if (objExpandedUserDTO == null)
                {
                    return HttpNotFound();
                }

                if (UserName.ToLower() ==
                    this.User.Identity.Name.ToLower() && RoleName == "Administrator")
                {
                    ModelState.AddModelError(string.Empty,
                        "Error: Cannot delete Administrator Role for the current user");
                }

                // Go get the User
                ApplicationUser user = UserManager.FindByName(UserName);
                // Remove User from role
                UserManager.RemoveFromRoles(user.Id, RoleName);
                UserManager.Update(user);

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                return RedirectToAction("EditRoles", new { UserName = UserName });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                UserAndRolesDTO objUserAndRolesDTO =
                    GetUserAndRoles(UserName);

                return View("EditRoles", objUserAndRolesDTO);
            }
        }
        #endregion

        // Roles *****************************

        // GET: /Admin/ViewAllRoles
        [Authorize(Roles = "Administrator")]
        #region public ActionResult ViewAllRoles()
        public ActionResult ViewAllRoles()
        {
            GetCurrentUserInViewBag();
            var roleManager =
                new RoleManager<IdentityRole>
                (
                    new RoleStore<IdentityRole>(new ApplicationDbContext())
                    );

            List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles
                                        select new RoleDTO
                                        {
                                            Id = objRole.Id,
                                            RoleName = objRole.Name
                                        }).ToList();

            return View(colRoleDTO);
        }
        #endregion

        // GET: /Admin/AddRole
        [Authorize(Roles = "Administrator")]
        #region public ActionResult AddRole()
        public ActionResult AddRole()
        {
            GetCurrentUserInViewBag();
            RoleDTO objRoleDTO = new RoleDTO();

            return View(objRoleDTO);
        }
        #endregion

        // PUT: /Admin/AddRole
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region public ActionResult AddRole(RoleDTO paramRoleDTO)
        public ActionResult AddRole(RoleDTO paramRoleDTO)
        {
            GetCurrentUserInViewBag();
            try
            {
                if (paramRoleDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var RoleName = paramRoleDTO.RoleName.Trim();

                if (RoleName == "")
                {
                    throw new Exception("No RoleName");
                }

                // Create Role
                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext())
                        );

                if (!roleManager.RoleExists(RoleName))
                {
                    roleManager.Create(new IdentityRole(RoleName));
                }

                return Redirect("~/Admin/ViewAllRoles");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("AddRole");
            }
        }
        #endregion

        // DELETE: /Admin/DeleteUserRole?RoleName=TestRole
        [Authorize(Roles = "Administrator")]
        #region public ActionResult DeleteUserRole(string RoleName)
        public ActionResult DeleteUserRole(string RoleName)
        {
            GetCurrentUserInViewBag();
            try
            {
                if (RoleName == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (RoleName.ToLower() == "administrator")
                {
                    throw new Exception(String.Format("Cannot delete {0} Role.", RoleName));
                }

                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext()));

                var UsersInRole = roleManager.FindByName(RoleName).Users.Count();
                if (UsersInRole > 0)
                {
                    throw new Exception(
                        String.Format(
                            "Canot delete {0} Role because it still has users.",
                            RoleName)
                            );
                }

                var objRoleToDelete = (from objRole in roleManager.Roles
                                       where objRole.Name == RoleName
                                       select objRole).FirstOrDefault();
                if (objRoleToDelete != null)
                {
                    roleManager.Delete(objRoleToDelete);
                }
                else
                {
                    throw new Exception(
                        String.Format(
                            "Canot delete {0} Role does not exist.",
                            RoleName)
                            );
                }

                List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles
                                            select new RoleDTO
                                            {
                                                Id = objRole.Id,
                                                RoleName = objRole.Name
                                            }).ToList();

                return View("ViewAllRoles", colRoleDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);

                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext()));

                List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles
                                            select new RoleDTO
                                            {
                                                Id = objRole.Id,
                                                RoleName = objRole.Name
                                            }).ToList();

                return View("ViewAllRoles", colRoleDTO);
            }
        }
        #endregion


        [Authorize(Roles = "Administrator")]
        public ActionResult CreateMultiple()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMultiple(HttpPostedFileBase uploadfile)
        {
            if (ModelState.IsValid)
            {
                if (uploadfile != null && uploadfile.ContentLength > 0)
                {
                    //ExcelDataReader works on binary excel file
                    Stream stream = uploadfile.InputStream;
                    //We need to written the Interface.
                    IExcelDataReader reader = null;
                    if (uploadfile.FileName.EndsWith(".xls"))
                    {
                        //reads the excel file with .xls extension
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (uploadfile.FileName.EndsWith(".xlsx"))
                    {
                        //reads excel file with .xlsx extension
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        //Shows error if uploaded file is not Excel file
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View();
                    }

                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            //First row iss treated as the header
                            UseHeaderRow = true
                        }
                    });

                    var dataTable = result.Tables[0];
                    List<string> EmailList = new List<string>();

                    //Read each row one by one
                    for (var i = 0; i < dataTable.Rows.Count; i++)
                    {
                        var FName = dataTable.Rows[i][0].ToString().Trim(); //First Name
                         var MName = dataTable.Rows[i][1].ToString().Trim(); //Middle Name
                        var LName = dataTable.Rows[i][2].ToString().Trim(); //Last Name
                        var StudNum = dataTable.Rows[i][3].ToString().Trim(); //Student Number
                        var Program = dataTable.Rows[i][4].ToString().Trim(); //Program
                        var YearLvl = dataTable.Rows[i][5].ToString().Trim(); //Year Level
                        var Email = dataTable.Rows[i][6].ToString().Trim(); //Email & Username
                        var isActive = dataTable.Rows[i][7].ToString().Trim(); //Active Bool
                        var Role = dataTable.Rows[i][8].ToString().Trim(); //Role

                        //Add each student's email to the email list
                        EmailList.Add(Email);

                        //Call the user making method here
                        AddUser(FName, MName, LName, StudNum, Program, YearLvl, Email, isActive, Role);
                    }

                    //Once each student is registered, send them an email
                    //This launches the SendEmail in a separate thread in the background
                    var tEmail = new Thread(() => SendEmail(EmailList));
                    tEmail.Start();

                    reader.Close();
                    //Sending result data to View
                    return View(result.Tables[0]);
                }
            }
            else
            {
                ModelState.AddModelError("File", "Please upload your file");
            }
            return View();
        }


        [Authorize(Roles = "Administrator")]
        public ActionResult UpdateMultiple()
        {
            return View();
        }


        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMultiple(HttpPostedFileBase uploadfile)
        {
            if (ModelState.IsValid)
            {
                if (uploadfile != null && uploadfile.ContentLength > 0)
                {
                    //ExcelDataReader works on binary excel file
                    Stream stream = uploadfile.InputStream;
                    //We need to written the Interface.
                    IExcelDataReader reader = null;
                    if (uploadfile.FileName.EndsWith(".xls"))
                    {
                        //reads the excel file with .xls extension
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (uploadfile.FileName.EndsWith(".xlsx"))
                    {
                        //reads excel file with .xlsx extension
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        //Shows error if uploaded file is not Excel file
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View();
                    }

                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            //First row is treated as the header
                            UseHeaderRow = true
                        }
                    });

                    var dataTable = result.Tables[0];

                    List<Student> StudentList = new List<Student>();

                    //Read each row one by one
                    for (var i = 0; i < dataTable.Rows.Count; i++)
                    {
                        var studNum = dataTable.Rows[i][0].ToString().Trim(); //Student Number
                        var program = dataTable.Rows[i][1].ToString().Trim(); //Program
                        var yearLvl = dataTable.Rows[i][2].ToString().Trim(); //Year Level

                        var updateStudent = new Student
                        {
                            StudentID = studNum,
                            Program = program,
                            YearLevel = Int32.Parse(yearLvl)
                        };

                        //Check if the student exists
                        var studentInDb = db.Students.FirstOrDefault(s => s.StudentID == updateStudent.StudentID);
                        if (studentInDb != null)
                        {
                            //update student data here
                            studentInDb.Program = updateStudent.Program;
                            studentInDb.YearLevel = updateStudent.YearLevel;
                            studentInDb.IsActive = true;
                            db.SaveChanges();

                            //add this student to the list of currently updated students
                            StudentList.Add(updateStudent);
                        }
                        else
                            Response.Redirect("/Home/Error?Error=Error updating student. Student # " + updateStudent.StudentID + " was not found in the database.");
                    }

                    //Get all the students who are active except for the currently updated students
                    List<string> StudentIdList = StudentList.Select(q => q.StudentID).ToList();
                    var activeStudents = db.Students.Where(s => s.IsActive == true && !StudentIdList.Contains(s.StudentID)).ToList();

                    //set isActive for all these students to false
                    foreach (var student in activeStudents)
                    {
                        student.IsActive = false;
                        db.SaveChanges();
                    }


                    reader.Close();
                    //Sending result data to View
                    return View(result.Tables[0]);
                }
            }
            else
            {
                ModelState.AddModelError("File", "Please upload your file");
            }
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ViewStudents()
        {
            var studentList = db.Students.ToList();
            List<ViewStudentDTO> DTOList = new List<ViewStudentDTO>();
            foreach (var student in studentList)
            {
                var DTOStudent = new ViewStudentDTO
                {
                    StudentFirstName = student.StudentFirstName,
                    StudentMiddleName = student.StudentMiddleName,
                    StudentLastName = student.StudentLastName,
                    StudentID = student.StudentID,
                    Program = student.Program,
                    YearLevel = student.YearLevel,
                    IsActive = student.IsActive,
                    Email = student.StudentEmail
                };
                DTOList.Add(DTOStudent);
            }
            return View(DTOList);
        }

        private void AddUser(string FName, string MName, string LName, string StudNum, string _Program, string YearLvl, string _Email, string isActive, string Role)
        {
            var user = new ApplicationUser
            {
                UserName = _Email, //to log in with email
                Email = _Email
            };

            string userPWD = LName + "&" + StudNum;

            var TempStudent = db.Students.FirstOrDefault(x => x.StudentID == StudNum);

            //To make sure that Student number is unique (no duplicate entries/students)
            if (TempStudent == null)
            {
                var UserResult = UserManager.Create(user, userPWD);
                if (UserResult.Succeeded)
                {
                    if (!RoleManager.RoleExists(Role))
                    {
                        var NewRole = new IdentityRole(Role);
                        RoleManager.Create(NewRole);
                    }

                    var RoleResult = UserManager.AddToRole(user.Id, Role);

                    if (RoleResult.Succeeded)
                    {
                        if (Role == "Student")
                        {
                            var student = new Student
                            {
                                StudentFirstName = FName,
                                StudentMiddleName = MName,
                                StudentLastName = LName,
                                StudentID = StudNum,
                                Program = _Program,
                                YearLevel = Int32.Parse(YearLvl),
                                StudentEmail = _Email,
                                IsActive = Boolean.Parse(isActive),
                                UserID = user.Id
                            };

                            db.Students.Add(student);
                            db.SaveChanges();
                        }
                    }
                    else
                        Response.Redirect("/Home/Error?error=Could not add user to the role.");
                }
                else
                    Response.Redirect("/Home/Error?error=Could not create user.");
            }
        }

        private void SendEmail(List<String> StudentsEmails)
        {
            new EmailProviderUtility().SendNotificationEmail(StudentsEmails);
        }

        private IEnumerable<string> GetAllCollege()
        {
            return new List<string>
            {
                "Psychology",
                "Nursing",
                "Rad Tech",
            };
        }



        #region public ApplicationRoleManager RoleManager
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion

        #region private List<SelectListItem> GetAllRolesAsSelectList()
        private List<SelectListItem> GetAllRolesAsSelectList()
        {
            List<SelectListItem> SelectRoleListItems =
                new List<SelectListItem>();

            var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var colRoleSelectList = roleManager.Roles.OrderBy(x => x.Name).ToList();

            SelectRoleListItems.Add(
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                });

            foreach (var item in colRoleSelectList)
            {
                SelectRoleListItems.Add(
                    new SelectListItem
                    {
                        Text = item.Name.ToString(),
                        Value = item.Name.ToString()
                    });
            }

            return SelectRoleListItems;
        }
        #endregion

        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            var selectList = new List<SelectListItem>();

            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
        }

        #region private ExpandedUserDTO GetUser(string paramUserName)
        private ExpandedUserDTO GetUser(string paramUserName)
        {
            ExpandedUserDTO objExpandedUserDTO = new ExpandedUserDTO();
            Response.Write(paramUserName);
            Thread.Sleep(3000);
            var result = UserManager.FindByName(paramUserName);
            //var result = UserManager.FindByEmail(paramUserName);

            // If we could not find the user, throw an exception
            if (result == null) throw new Exception("Could not find the User");

            objExpandedUserDTO.UserName = result.UserName;
            objExpandedUserDTO.Email = result.Email;
            objExpandedUserDTO.LockoutEndDateUtc = result.LockoutEndDateUtc;
            objExpandedUserDTO.AccessFailedCount = result.AccessFailedCount;
            objExpandedUserDTO.PhoneNumber = result.PhoneNumber;

            return objExpandedUserDTO;
        }
        #endregion

        #region private ExpandedUserDTO UpdateDTOUser(ExpandedUserDTO objExpandedUserDTO)
        private ExpandedUserDTO UpdateDTOUser(ExpandedUserDTO paramExpandedUserDTO)
        {
            //string UserName = (paramExpandedUserDTO.UserName);
            string UserName = (paramExpandedUserDTO.Email); 
            ApplicationUser result = UserManager.FindByName(UserName);
            //ApplicationUser result = UserManager.FindByName(paramExpandedUserDTO.UserName);

            // If we could not find the user, throw an exception
            if (result == null)
            {
                throw new Exception("Could not find the User");
            }

            result.Email = paramExpandedUserDTO.Email;

            // Lets check if the account needs to be unlocked
            if (UserManager.IsLockedOut(result.Id))
            {
                // Unlock user
                UserManager.ResetAccessFailedCountAsync(result.Id);
            }

            UserManager.Update(result);

            // Was a password sent across?
            if (!string.IsNullOrEmpty(paramExpandedUserDTO.Password))
            {
                // Remove current password
                var removePassword = UserManager.RemovePassword(result.Id);
                if (removePassword.Succeeded)
                {
                    // Add new password
                    var AddPassword =
                        UserManager.AddPassword(result.Id,paramExpandedUserDTO.Password);

                    if (AddPassword.Errors.Count() > 0)
                    {
                        throw new Exception(AddPassword.Errors.FirstOrDefault());
                    }
                }
            }

            return paramExpandedUserDTO;
        }
        #endregion

        #region private void DeleteUser(ExpandedUserDTO paramExpandedUserDTO)
        private void DeleteUser(ExpandedUserDTO paramExpandedUserDTO)
        {
            ApplicationUser user = 
                UserManager.FindByName(paramExpandedUserDTO.UserName);

            // If we could not find the user, throw an exception
            if (user == null)
            {
                throw new Exception("Could not find the User");
            }

            UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());
            UserManager.Update(user);
            UserManager.Delete(user);
        }
        #endregion

        #region private UserAndRolesDTO GetUserAndRoles(string UserName)
        private UserAndRolesDTO GetUserAndRoles(string UserName)
        {
            // Go get the User
            ApplicationUser user = UserManager.FindByName(UserName);

            List<UserRoleDTO> colUserRoleDTO =
                (from objRole in UserManager.GetRoles(user.Id)
                 select new UserRoleDTO
                 {
                     RoleName = objRole,
                     UserName = UserName
                 }).ToList();

            if (colUserRoleDTO.Count() == 0)
            {
                colUserRoleDTO.Add(new UserRoleDTO { RoleName = "No Roles Found" });
            }

            ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

            // Create UserRolesAndPermissionsDTO
            UserAndRolesDTO objUserAndRolesDTO =
                new UserAndRolesDTO();
            objUserAndRolesDTO.UserName = UserName;
            objUserAndRolesDTO.colUserRoleDTO = colUserRoleDTO;
            return objUserAndRolesDTO;
        }
        #endregion

        #region private List<string> RolesUserIsNotIn(string UserName)
        private List<string> RolesUserIsNotIn(string UserName)
        {
            // Get roles the user is not in
            var colAllRoles = RoleManager.Roles.Select(x => x.Name).ToList();

            // Go get the roles for an individual
            ApplicationUser user = UserManager.FindByName(UserName);

            // If we could not find the user, throw an exception
            if (user == null)
            {
                throw new Exception("Could not find the User");
            }

            var colRolesForUser = UserManager.GetRoles(user.Id).ToList();
            var colRolesUserInNotIn = (from objRole in colAllRoles
                                       where !colRolesForUser.Contains(objRole)
                                       select objRole).ToList();

            if (colRolesUserInNotIn.Count() == 0)
            {
                colRolesUserInNotIn.Add("No Roles Found");
            }

            return colRolesUserInNotIn;
        }
        #endregion
    }
}