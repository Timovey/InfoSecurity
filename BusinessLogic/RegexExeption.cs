using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSecurity1.BusinessLogic
{
    public class RegexExeption : Exception
    {
        public RegexExeption(string message)  : base(message)
        {

        }
    }
}
