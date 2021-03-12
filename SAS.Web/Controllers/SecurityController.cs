using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAS.Application.Interface;
using SAS.Domain.Entities.Security;

namespace SAS.Web
{
    [DisplayName("Security")]
    public class SecurityController : Controller
    {
        #region Constructor

        public readonly ISecurityLogic _securityLogic;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public SecurityController(ISecurityLogic securityLogic, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {

            _securityLogic = securityLogic;
            _roleManager = roleManager;
            _userManager = userManager;

        }

        #endregion

        public IActionResult Index()
        {
            return View();

        }

        #region Manage Roles


        [DisplayName("Roles")]
        public IActionResult ManageRoles()
        {
            return View();
        }

        [DisplayName("Get Roles")]
        public JsonResult GetRoles()
        {
            var rolesdata = _roleManager.Roles.ToList();
            return Json(new { data = rolesdata });
        }

        [HttpPost]
        [DisplayName("Save Roles")]
        public async Task<IActionResult> SaveRoles()
        {
            string roleName = Request.Form["rolename"];
            string roleId = Request.Form["roleId"];

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                if (roleId.Trim() != "")
                {
                    var role = await _roleManager.FindByIdAsync(roleId.Trim());
                    role.Name = roleName;
                    await _roleManager.UpdateAsync(role);
                    TempData["success"] = "Role Updated Successfully!";
                }
                else
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                    TempData["success"] = "Role Created Successfully!";
                }
            }
            else
            {
                TempData["error"] = "Role already exists!";
            }

            //return RedirectToAction("Roles");
            return View("ManageRoles");
        }

        [DisplayName("Delete Role")]
        public async Task<IActionResult> DeleteRole(string id)
        {

            try
            {
                var role = await _roleManager.FindByIdAsync(id.Trim());
                _securityLogic.DeleteRole(role).Wait();
                TempData["success"] = "Role Deleted Successfully!";

            }
            catch (Exception e)
            {
                TempData["error"] = "An error occured processing your request";

            }

            return RedirectToAction("ManageRoles");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole()
        {
            string userId = Request.Form["ruserId"];
            string roleName = Request.Form["roleName"];

            if (userId != "")
            {
                var user = await _userManager.FindByIdAsync(userId);
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count() > 0)
                    await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                await _userManager.AddToRoleAsync(user, roleName);
            }
            TempData["success"] = "Role Changed Successfully!";
            return RedirectToAction("ManageUsers");
        }

        #endregion

        #region Manage Users 

        [DisplayName("Manage Users")]
        public IActionResult ManageUsers()
        {
            ViewData["roles"] = _securityLogic.GetAllRoles();
            return View();
        }

        [DisplayName("Get All Users")]
        public async Task<JsonResult> GetAllUsers()
        {
            List<UserRole> userRolelist = new List<UserRole>();

            foreach (var user in _userManager.Users.ToList())
            {
                userRolelist.Add(new UserRole()
                {
                    Name = user.FirstName + " " + user.LastName,
                    UserEmail = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user),
                    UserId = user.Id,
                    LockoutEnd = user.LockoutEnd,
                    LockoutEnabled = user.LockoutEnabled,
                });
            }

            return Json(new { data = userRolelist });
        }

        [DisplayName("Lock Un Lock")]
        public async Task<IActionResult> LockUnLock(string id)
        {
            var lockuser = await _userManager.FindByIdAsync(id);

            if (lockuser.LockoutEnd == null)
            {
                lockuser.LockoutEnd = DateTime.UtcNow.AddYears(50);
                await _userManager.UpdateAsync(lockuser);
                TempData["success"] = "User Locked Successfully!";
            }
            else
            {
                lockuser.LockoutEnd = null;
                await _userManager.UpdateAsync(lockuser);
                TempData["success"] = "User Unlocked Successfully!";
            }
            return RedirectToAction("ManageUsers");
        }

        [DisplayName("Delete User")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var rolesForUser = await _userManager.GetRolesAsync(user);
            foreach (var item in rolesForUser.ToList())
            {
                var result = await _userManager.RemoveFromRoleAsync(user, item);
            }

            try
            {
                await _userManager.DeleteAsync(user);
                TempData["success"] = "User Deleted Successfully!";
            }
            catch
            {

                TempData["error"] = "An error occured processing your request";
            }


            return RedirectToAction("ManageUsers");
        }

        [DisplayName("Admin Change Password")]
        public ActionResult AdminChangePassword()
        {
            return View();
        }

        [HttpPost]
        [DisplayName("Change User Password")]
        public async Task<IActionResult> ChangeUserPassword()
        {
            string userId = Request.Form["userId"];
            //string oldPassword = Request.Form["oldpassword"];
            string newPassword = Request.Form["newpassword"];
            string errdescription = "";

            if (userId != "")
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                    var changePasswordResult = await _userManager.AddPasswordAsync(user, newPassword);
                    //var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

                    if (changePasswordResult.Succeeded)
                    {
                        TempData["success"] = "Password Changed Successfully!";
                    }
                    else
                    {
                        foreach (var error in changePasswordResult.Errors)
                        {
                            errdescription += error.Description + "<br/>";
                        }
                        TempData["error"] = errdescription;
                        return View("AdminChangePassword");
                    }
                }
            }

            return RedirectToAction("ManageUsers");
        }

        #endregion

        #region Manage Menu

        [DisplayName("Create Menus")]
        public ActionResult CreateMenus()
        {
            ViewData["roles"] = _securityLogic.GetAllRoles();
            return View(_securityLogic.GetControllers());
        }

        [HttpPost]
        [DisplayName("Create Menus")]
        public async Task<IActionResult> CreateMenus(string menuItem)
        {
            if (menuItem == "[]")
            {
                return Ok("False");
            }

            try
            {
                await _securityLogic.AddMenus(menuItem);
            }
            catch (Exception e)
            {
                return Ok(e);
            }

            return Ok("True");
        }

        [DisplayName("Delete")]
        public async Task<IActionResult> DeleteMenu(int? id)
        {
            try
            {
                await _securityLogic.DeleteMenuById(id);
                TempData["success"] = "Menu Successfully Deleted";
            }
            catch
            {
                TempData["error"] = "An error occured processing your request";
            }

            return RedirectToAction("Menu");
        }

        //[HttpPost, ActionName("DeleteMenu")]
        //[DisplayName("Delete Menu")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteMenu(int id)
        //{
        //    await _securityLogic.DeleteMenuById(id);
        //    TempData["success"] = "Successfully Deleted";
        //    return RedirectToAction("Index", "Role");
        //}

        [DisplayName("Index")]
        public async Task<IActionResult> Menu()
        {
            return View();
        }

        [DisplayName("Data Listing Role")]
        public JsonResult DataListingRole()
        {
            var roledata = _securityLogic.MenuMaster.ToList();
            return Json(new { data = roledata });
        }
        #endregion

        #region  Role Assignment to Menu 

        // [Route("Security/MenuToRole/{roleId}")]
        [DisplayName("MenuToRole")]
        public IActionResult MenuToRole(string id)
        {
            ViewData["roles"] = _securityLogic.GetRoles(id);
            return View(_securityLogic.GetMenuByRoleId(id));
        }

        // [Route("Security/AssignMenuToRole/{roleId}")]
        [DisplayName("Assign Menu To Role")]
        public IActionResult AssignMenuToRole(string id)
        {

            ViewData["roles"] = _securityLogic.GetRoles(id);
            return View(_securityLogic.GetUnAssignedMenuByRoleId(id));
        }

        [HttpPost]
        [DisplayName("Menu Assignment to Role")]
        public async Task<IActionResult> MenuAssignmentToRole(string assignMenuList)
        {
            if (assignMenuList == null)
                return Ok(false);

            var roleId = "";
            try
            {
                roleId = await _securityLogic.AssignMenuToRole(assignMenuList);
            }
            catch (Exception e)
            {
                return Ok(e);
            }

            return Ok(true);
        }

        [DisplayName("Delete Menu To Role Assignment")]
        public async Task<IActionResult> DeleteMenuToRoleAssignment(int? id)
        {
            try
            {
                _securityLogic.DeleteMenuToRoleById(id);
                TempData["success"] = "Successfully Deleted";
            }
            catch
            {
                TempData["error"] = "An error occured processing your request";

            }
            return RedirectToAction("MenuToRole/null", "Security");

        }

        //[HttpPost, ActionName("DeleteMenuToRoleAssignment")]
        //[DisplayName("Delete Menu To Role Assignment")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteMenuToRoleAssignment(int id)
        //{
        //    _securityLogic.DeleteMenuToRoleById(id);
        //    TempData[""] = "Successfully Deleted";
        //    return RedirectToAction("MenuToRole/null", "Security");
        //}


        #endregion

    }
}
