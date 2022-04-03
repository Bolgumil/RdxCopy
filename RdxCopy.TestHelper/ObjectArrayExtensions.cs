namespace RdxCopy.TestHelper
{
    public static class ObjectArrayExtensions
    {
        public static string[] ToStringArray(this object[] objArr)
        {
            return objArr.Select(o => o.ToString()).ToArray();
        }
    }
}