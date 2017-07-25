namespace RM.Data.AccessLink.WebAPI.Utils.ReferenceData
{
    // Helpter class string util
    public static class StringUtilHelper
    {
        // this method will expect string
        public static string AppendString(this string str, string str1)
        {
            if (str != null)
            {
                str.Replace(str1, string.Empty);
            }

            return str.ToString();
        }
    }
}
