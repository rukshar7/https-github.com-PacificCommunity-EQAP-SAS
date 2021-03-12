using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using SAS.Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAS.Application.Interface
{
    public interface ISecurityLogic
    {
        string GetCurrentUserId();
        string GetCurrentUserName();
        Task<string> GetUserRole();
        bool IsHasThePermission(string controllerName, string actionName);
        bool IsHasThePermission(string controllerName);
        SelectList GetRoles(string selectedValue);
        SelectList GetAllRoles();
        Task<IdentityRole> GetRoleById(string roleId);
        Task<IdentityRole> GetRoleByName(string name);
        Task AddUsersToRole(string roleId, string emailId);
        IEnumerable<ControllerInfo> GetControllers();
        IQueryable<MenuMaster> MenuMaster { get; }
        Task<MenuMaster> GetMenuById(int? id);
        Task<int> DeleteMenuById(int? id);
        Task<MenuToRole> GetMenuToRoleById(int? id);
        IQueryable<MenuToRole> GetMenuByRoleId(string roleId);
        Task<int> AddMenus(string menuItem);
        IEnumerable<ControllerInfo> GetUnAssignedMenuByRoleId(string roleId);
        Task<int> DeleteRole(IdentityRole userRole);
        Task<string> AssignMenuToRole(string assignMenuList);
        Task<int> DeleteMenuToRoleById(int? id);

    }
}
