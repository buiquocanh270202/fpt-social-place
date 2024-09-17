using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
    public class StringHelper
    {
        public  string[] SplitString(string stringRoot)
        {
            var array = stringRoot.Split(" ");
            return array;
        }
    }
}
