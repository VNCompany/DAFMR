using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbm_lib.components
{
    public static class MMB
    {
        public static string JoinStr<T>(this IEnumerable<T> ts, object separator)
        {
            StringBuilder joined = new StringBuilder();
            T[] arr = ts.ToArray();
            for(int i = 0; i < arr.Length; i++)
            {
                if (i != arr.Length - 1)
                    joined.Append(arr[i].ToString() + separator.ToString());
                else
                    joined.Append(arr[i].ToString());
            }
            return joined.ToString();
        }
    }
}
