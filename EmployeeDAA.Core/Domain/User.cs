using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Core.Domain
{
    public class User : BaseEntity, ISoftDeletedEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        [NoColumnMap]
        public RoleTypes RoleType { get; set; }
        [NoColumnMap]
        public string Role { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public bool IsActive { get; set; }


        public bool IsDeleted { get; set; }
        public int? UserTypeId { get; set; }

        public string Photo { get; set; }

        public string CameraUrl { get; set; }

        public string UserToken { get; set; }


    }
}
