namespace RdxCopy.CopyManager
{
    public static class IDictionaryExtensions
    {
        public static void MergeDictionaries<T1, T2>(this IDictionary<T1, List<T2>> d1, IDictionary<T1, List<T2>> d2)
        {
            foreach (var kvp2 in d2)
            {
                // If the dictionary already contains the key then merge them
                if (d1.ContainsKey(kvp2.Key))
                {
                    d1[kvp2.Key].AddRange(kvp2.Value);
                    continue;
                }
                d1.Add(kvp2);
            }
        }
    }
}
