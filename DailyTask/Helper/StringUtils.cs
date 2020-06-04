using System;
using System.Collections.Generic;
using System.Text;

namespace DailyTask.Helper
{
    public static class StringUtils
    {
        /// <summary>
        /// string.contains is case sensitive. this extention method, make string.contains not case sensitive
        /// </summary>
        /// <param name="source"></param>
        /// <param name="strToCheck"></param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase(this string source, string strToCheck)
        {
            return source.IndexOf(strToCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
