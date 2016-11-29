using System;
using System.Collections.Generic;

namespace AvalonAssetsTests
{
    internal static class Utils
    {
        public static IList<int> UniqueNumberList(this Random rand, int count = 10)
        {
            var nodes = new List<int>();
            for (var i = 0; i < count; i++)
            {
                var num = rand.Next();
                while (nodes.Contains(num))
                    num = rand.Next();
                nodes.Add(num);
            }
            return nodes;
        }

    }
}