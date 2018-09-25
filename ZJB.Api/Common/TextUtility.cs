using System.Linq;
using System.Text.RegularExpressions;

namespace ZJB.Api.Common
{
    public class TextUtility
    {

        public static double GetRelateLevel(string segment, string text)
        {
            return GetSimilarLevel(segment, text, 2, 1, 1.0 * segment.Length / text.Length);
        }

        public static double GetSimilarLevel(string str1, string str2)
        {
            return GetSimilarLevel(str1, str2, 1, 1, 1);
        }

        private static double GetSimilarLevel(string str1, string str2, double kq, double kr, double ks)
        {
            int maxHtmlTagLength = (str1.Length > str2.Length) ? str2.Length/5 : str1.Length/5;
            Regex htmlRegex = new Regex(string.Format(@"\<.{{1,{0}}}?\>", maxHtmlTagLength > 1 ? maxHtmlTagLength : 1));
            str1 = htmlRegex.Replace(str1.Trim(), string.Empty);
            str2 = htmlRegex.Replace(str2.Trim(), string.Empty);

            var arr1 = str1.ToCharArray();
            var arr2 = str2.ToCharArray();

            int q = arr1.Intersect(arr2).Count();
            int r = arr1.Distinct().Count() - q;
            int s = arr2.Distinct().Count() - q;
            return kq * q / (kq * q + kr * r + ks * s);
        }
    }
}