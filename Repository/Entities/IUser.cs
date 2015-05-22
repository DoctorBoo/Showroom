using System;

namespace Repository.Contexts
{
    public interface IUser
    {
        DateTime CreateDate { get; set; }
        string Email { get; set; }
        bool IsActive { get; set; }
        string LoginName { get; set; }
        string ModifiedBy { get; set; }
        DateTime ModifyDate { get; set; }
        string PassWord { get; set; }
        int RoleId { get; set; }
        int UserLoginID { get; set; }
    }
}