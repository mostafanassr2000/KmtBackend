using System.Collections.Generic;

namespace KmtBackend.DAL.Constants
{
    /// <summary>
    /// Static class that defines all application permissions
    /// </summary>
    public static class Permissions
    {
        #region User Permissions
        
        /// <summary>
        /// Permission to view all users
        /// </summary>
        public const string ViewUsers = "users.view";
        
        /// <summary>
        /// Permission to create new users
        /// </summary>
        public const string CreateUsers = "users.create";
        
        /// <summary>
        /// Permission to update existing users
        /// </summary>
        public const string UpdateUsers = "users.update";
        
        /// <summary>
        /// Permission to delete users
        /// </summary>
        public const string DeleteUsers = "users.delete";
        
        #endregion
        
        #region Department Permissions
        
        /// <summary>
        /// Permission to view all departments
        /// </summary>
        public const string ViewDepartments = "departments.view";
        
        /// <summary>
        /// Permission to create new departments
        /// </summary>
        public const string CreateDepartments = "departments.create";
        
        /// <summary>
        /// Permission to update existing departments
        /// </summary>
        public const string UpdateDepartments = "departments.update";
        
        /// <summary>
        /// Permission to delete departments
        /// </summary>
        public const string DeleteDepartments = "departments.delete";
        
        #endregion
        
        #region Role Permissions
        
        /// <summary>
        /// Permission to view all roles
        /// </summary>
        public const string ViewRoles = "roles.view";
        
        /// <summary>
        /// Permission to create new roles
        /// </summary>
        public const string CreateRoles = "roles.create";
        
        /// <summary>
        /// Permission to update existing roles
        /// </summary>
        public const string UpdateRoles = "roles.update";
        
        /// <summary>
        /// Permission to delete roles
        /// </summary>
        public const string DeleteRoles = "roles.delete";
        
        /// <summary>
        /// Permission to assign roles to users
        /// </summary>
        public const string AssignRoles = "roles.assign";
        
        #endregion
        
        #region Permission Management
        
        /// <summary>
        /// Permission to view all available permissions
        /// </summary>
        public const string ViewPermissions = "permissions.view";
        
        /// <summary>
        /// Permission to assign permissions to roles
        /// </summary>
        public const string AssignPermissions = "permissions.assign";
        
        #endregion

        /// <summary>
        /// Gets all defined permissions as a list of strings
        /// </summary>
        /// <returns>List of all permission codes</returns>
        public static IEnumerable<string> GetAllPermissions()
        {
            // Return all permission constants
            return new List<string>
            {
                // User permissions
                ViewUsers,
                CreateUsers,
                UpdateUsers,
                DeleteUsers,
                
                // Department permissions
                ViewDepartments,
                CreateDepartments,
                UpdateDepartments,
                DeleteDepartments,
                
                // Role permissions
                ViewRoles,
                CreateRoles,
                UpdateRoles,
                DeleteRoles,
                AssignRoles,
                
                // Permission management
                ViewPermissions,
                AssignPermissions
            };
        }
        
        /// <summary>
        /// Gets all permissions with their descriptions
        /// </summary>
        /// <returns>Dictionary of permission codes and descriptions</returns>
        public static Dictionary<string, (string Description, string DescriptionAr)> GetAllPermissionsWithDescriptions()
        {
            // Create dictionary with code as key and description tuple as value
            return new Dictionary<string, (string, string)>
            {
                // User permissions
                [ViewUsers] = ("View all users", "عرض جميع المستخدمين"),
                [CreateUsers] = ("Create new users", "إنشاء مستخدمين جدد"),
                [UpdateUsers] = ("Update existing users", "تحديث المستخدمين الحاليين"),
                [DeleteUsers] = ("Delete users", "حذف المستخدمين"),
                
                // Department permissions
                [ViewDepartments] = ("View all departments", "عرض جميع الأقسام"),
                [CreateDepartments] = ("Create new departments", "إنشاء أقسام جديدة"),
                [UpdateDepartments] = ("Update existing departments", "تحديث الأقسام الحالية"),
                [DeleteDepartments] = ("Delete departments", "حذف الأقسام"),
                
                // Role permissions
                [ViewRoles] = ("View all roles", "عرض جميع الأدوار"),
                [CreateRoles] = ("Create new roles", "إنشاء أدوار جديدة"),
                [UpdateRoles] = ("Update existing roles", "تحديث الأدوار الحالية"),
                [DeleteRoles] = ("Delete roles", "حذف الأدوار"),
                [AssignRoles] = ("Assign roles to users", "تعيين الأدوار للمستخدمين"),
                
                // Permission management
                [ViewPermissions] = ("View all permissions", "عرض جميع الصلاحيات"),
                [AssignPermissions] = ("Assign permissions to roles", "تعيين الصلاحيات للأدوار")
            };
        }
    }
}