using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Data.AccessLink.WebAPI.Utils.ReferenceData
{
    // Helpter class string util
    public static class StringUtil
    {
        // this method will expect string
        public static String AppendString(this string str, string str1)
        {
            if (str != null)
            {
                str.Replace(str1, String.Empty);
            }
            return str.ToString();
        }
    }
}
