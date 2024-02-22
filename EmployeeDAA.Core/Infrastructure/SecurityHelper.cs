using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Core.Infrastructure
{
    public class SecurityHelper
    {
        public const string OrderEncryptionKey = "testtesttestteeertyutyujtyuhtyhg";
        public const string EnDeKey = "daaadmincomnetindiadevelopmented";
        public const string UserEnKey = "userinsertupdatedatafordaasystem";
        public string Secret { get; set; }
    }
}
