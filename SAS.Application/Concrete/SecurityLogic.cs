using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SAS.Application.Interface;
using SAS.Domain.DataContext;
using SAS.Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAS.Application.Concrete
{
    public class SecurityLogic : ISecurityLogic
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private PacSimsIdentityDbContext _securityDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private List<ControllerInfo> _Controllers;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        public SecurityLogic(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor, PacSimsIdentityDbContext securityDbContext, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _securityDbContext = securityDbContext;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }
        public string GetCurrentUserId()
        {
            return _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
        }

        public string GetCurrentUserName()
        {
            return _userManager.GetUserName(_httpContextAccessor.HttpContext.User);
        }

        public async Task<string> GetUserRole()
        {
            var roleName = await (from role in _securityDbContext.Roles
                                  join UR in _securityDbContext.UserRoles on role.Id equals UR.RoleId
                                  where UR.UserId == GetCurrentUserId()
                                  select role.Name).FirstOrDefaultAsync();
            return roleName;
        }

        public bool IsHasThePermission(string controllerName, string actionName)
        {
            var actionId = $":{controllerName}:{actionName}";
            var status = false;
            var userId = GetCurrentUserId();
            var menuList = (
                from mm in _securityDbContext.MenuMaster
                join mtr in _securityDbContext.MenuToRole on mm.Id equals mtr.MenuMasterId
                join userRole in _securityDbContext.UserRoles on mtr.UserRoleId equals userRole.RoleId
                where userRole.UserId == userId
                select mm
           );

            foreach (var menu in menuList)
            {
                if (menu == null)
                    continue;
                var savedActionId = ":" + menu.ControllerName + ":" + menu.ActionName;

                if (savedActionId.ToUpper() == actionId.ToUpper())
                {
                    status = true;
                    return status;
                }
            }
            return status;
        }
        public bool IsHasThePermission(string controllerName)
        {

            var status = false;
            var userId = GetCurrentUserId();
            var menuList = (
                from mm in _securityDbContext.MenuMaster
                join mtr in _securityDbContext.MenuToRole on mm.Id equals mtr.MenuMasterId
                join userRole in _securityDbContext.UserRoles on mtr.UserRoleId equals userRole.RoleId
                where userRole.UserId == userId && mm.ControllerName == controllerName

                select mm
           );



            if (menuList.Any())
            {
                status = true;
                return status;

            }
            else
            {
                status = false;
            }
            return status;
        }

        #region Manage Roles 
        public SelectList GetRoles(string selectedValue)
        {
            SelectList listUserRoles = new SelectList(_securityDbContext.Roles, "Id", "Name", selectedValue);
            return listUserRoles;

        }

        public SelectList GetAllRoles()
        {
            SelectList listUserRoles = new SelectList(_securityDbContext.Roles, "Id", "Name");
            return listUserRoles;

        }

        public async Task<IdentityRole> GetRoleById(string roleId)
        {
            var role = await _roleManager.Roles.Where(x => x.Id == roleId).ToListAsync();
            return role[0];
        }

        public async Task<IdentityRole> GetRoleByName(string name)
        {
            var role = await _roleManager.Roles.Where(x => x.Name == name).FirstOrDefaultAsync();
            return role;
        }

        public async Task AddUsersToRole(string roleId, string emailId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            //var roleName = _securityDbContext.checkRolesById(roleId);
            var user = await _userManager.FindByEmailAsync(emailId);
            await _userManager.AddToRoleAsync(user, role.Name);
        }
        public async Task<int> DeleteRole(IdentityRole userRole)
        {
            _securityDbContext.Roles.Remove(userRole);
            return (await _securityDbContext.SaveChangesAsync());
        }

        #endregion

        #region Manage Menu 
        public IQueryable<MenuMaster> MenuMaster => _securityDbContext.MenuMaster;

        public async Task<MenuMaster> GetMenuById(int? id)
        {
            MenuMaster menuMaster = await _securityDbContext.MenuMaster.FindAsync(id);
            return menuMaster;
        }

        public async Task<int> AddMenus(string menuItem)
        {
            dynamic data = JsonConvert.DeserializeObject(menuItem);
            foreach (dynamic item in data)
            {
                if (item != null)
                {
                    dynamic actionList = item.actionList;
                    foreach (var action in actionList)
                    {
                        MenuMaster menu = new MenuMaster();
                        menu.ControllerName = item.controllerName;
                        if (action != null)
                        {
                            menu.ActionName = action.actionName;
                            menu.ActionId = action.actionId;
                            menu.ActionDisplayName = action.displayName;
                            menu.CreatedDate = DateTime.Now;
                            _securityDbContext.MenuMaster.Add(menu);

                        }
                    }
                }
            }
            await _securityDbContext.SaveChangesAsync();
            return (await _securityDbContext.SaveChangesAsync());
        }

        public async Task<int> DeleteMenuById(int? id)
        {
            var menu = _securityDbContext.MenuMaster.Find(id);
            _securityDbContext.MenuMaster.Remove(menu);
            return (await _securityDbContext.SaveChangesAsync());
        }

        public IEnumerable<ControllerInfo> GetControllers()
        {
            _Controllers = new List<ControllerInfo>();
            List<IGrouping<string, ControllerActionDescriptor>> items = _actionDescriptorCollectionProvider
                .ActionDescriptors.Items
                .Where(descriptor => descriptor.GetType() == typeof(ControllerActionDescriptor))
                .Select(descriptor => (ControllerActionDescriptor)descriptor)
                .GroupBy(descriptor => descriptor.ControllerTypeInfo.FullName)
                .ToList();

            foreach (IGrouping<string, ControllerActionDescriptor> actionDescriptors in items)
            {
                if (!actionDescriptors.Any())
                {
                    continue;
                }

                ControllerActionDescriptor actionDescriptor = actionDescriptors.First();
                TypeInfo controllerTypeInfo = actionDescriptor.ControllerTypeInfo;
                ControllerInfo currentController = new ControllerInfo
                {
                    AreaName = controllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue,
                    DisplayName = controllerTypeInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName,
                    Name = actionDescriptor.ControllerName,
                };

                List<ActionInfo> actions = new List<ActionInfo>();
                foreach (ControllerActionDescriptor descriptor in actionDescriptors.GroupBy(a => a.ActionName).Select(g => g.First()))
                {
                    var menuMasterCount = _securityDbContext.MenuMaster.Where(x => x.ActionName == descriptor.ActionName && x.ControllerName == currentController.Name).Count();
                    if (menuMasterCount == 0)
                    {
                        MethodInfo methodInfo = descriptor.MethodInfo;
                        if (IsProtectedAction(controllerTypeInfo, methodInfo))
                        {
                            actions.Add(new ActionInfo
                            {
                                //ActionId = descriptor.Id,
                                ControllerId = currentController.Id,
                                Name = descriptor.ActionName,
                                DisplayName = methodInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName,
                            });
                        }
                    }

                }

                if (actions.Any())
                {
                    currentController.Actions = actions;
                    _Controllers.Add(currentController);
                }
            }

            return _Controllers;
        }

        private static bool IsProtectedAction(MemberInfo controllerTypeInfo, MemberInfo actionMethodInfo)
        {
            if (actionMethodInfo.GetCustomAttribute<AllowAnonymousAttribute>(true) != null)
            {
                return false;
            }

            if (controllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>(true) != null)
            {
                return true;
            }

            if (actionMethodInfo.GetCustomAttribute<AuthorizeAttribute>(true) != null)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Manage Menu To Role
        public async Task<MenuToRole> GetMenuToRoleById(int? id)
        {
            var menuToRole = await _securityDbContext.MenuToRole.Include(x => x.MenuMaster).Where(x => x.MenuToRoleId == id).FirstOrDefaultAsync();
            return menuToRole;
        }

        public async Task<int> DeleteMenuToRoleById(int? id)
        {
            var menuToRole = _securityDbContext.MenuToRole.Find(id);
            _securityDbContext.MenuToRole.Remove(menuToRole);
            return (await _securityDbContext.SaveChangesAsync());
        }
        public IQueryable<MenuToRole> GetMenuByRoleId(string roleId)
        {
            var menuToRole = _securityDbContext.MenuToRole.Where(x => x.UserRoleId == roleId).Include(x => x.MenuMaster);
            return menuToRole;
        }

        public IEnumerable<ControllerInfo> GetUnAssignedMenuByRoleId(string roleId)
        {
            var assignedMenus = from mm in _securityDbContext.MenuMaster
                                join mto in _securityDbContext.MenuToRole on mm.Id equals mto.MenuMasterId
                                where mto.UserRoleId == roleId
                                select mm;
            var menuMaster = _securityDbContext.MenuMaster;
            var unAssignedMenuList = menuMaster.Except(assignedMenus);



            var group = (from ml in unAssignedMenuList
                         group ml by ml.ControllerName into controller
                         select controller);

            List<ControllerInfo> Controllers = new List<ControllerInfo>();
            foreach (var item in group)
            {
                ControllerInfo Controller = new ControllerInfo();
                Controller.DisplayName = item.Key;
                Controller.Name = item.Key;
                List<ActionInfo> actions = new List<ActionInfo>();
                foreach (var con in item)
                {
                    ActionInfo action = new ActionInfo();
                    action.ControllerId = con.ControllerName;
                    action.Name = con.ActionName;
                    action.DisplayName = con.ActionDisplayName;
                    action.ActionId = con.Id;
                    actions.Add(action);
                }
                Controller.Actions = actions;
                Controllers.Add(Controller);
            }
            return Controllers;
        }

        public async Task<string> AssignMenuToRole(string assignMenuList)
        {
            dynamic data = JsonConvert.DeserializeObject(assignMenuList);
            dynamic roleId = data.roleId;
            dynamic MenuList = data.assignMenuList;
            foreach (var item in MenuList)
            {
                MenuToRole menuToRole = new MenuToRole();
                menuToRole.UserRoleId = roleId;
                if (item != null)
                {
                    menuToRole.MenuMasterId = Convert.ToInt32(item.menuId);
                    _securityDbContext.MenuToRole.Add(menuToRole);
                }
            }
            await _securityDbContext.SaveChangesAsync();
            return (roleId);
        }

        #endregion
    }
}
