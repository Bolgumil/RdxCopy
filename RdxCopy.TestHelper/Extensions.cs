namespace RdxCopy.TestHelper
{
    public static class Extensions
    {
        public static string[] ToStringArray(this object[] objArr)
        {
            return objArr.Select(o => o.ToString()).ToArray();
        }
    }
}