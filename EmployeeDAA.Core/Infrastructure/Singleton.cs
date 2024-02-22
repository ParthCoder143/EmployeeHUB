using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Core.Infrastructure
{
    public class Singleton<T>
    {
        private static T instance;  

        public static T Instance
        {
            get => instance;
            set => instance = value;
        }
    }
}
