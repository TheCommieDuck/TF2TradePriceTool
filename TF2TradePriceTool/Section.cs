﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TF2TradePriceTool
{
    static class Extensions
    {
        public static void AddIfNotNull<T>(this List<T> list, T item)
        {
            if (item != null)
                list.Add(item);
        }

        public static void AddRangeIfNotNull<T>(this List<T> list, IEnumerable<T> items)
        {
            if (items != null)
                list.AddRange(items);
        }

        public static void AddCounted<T>(this Dictionary<T, int> dict, T item)
        {
            int currentCount;
            // We don't care about the return value here, as if it's false that'll
            // leave currentCount as 0, which is what we want
            dict.TryGetValue(item, out currentCount);
            dict[item] = currentCount + 1;
        }
    }

    abstract class Section
    {
        //needed sections


        public Dictionary<Item, int> Items { get; set; }

        public IOrderedEnumerable<Item> OrderedList { get; set; }

        public Section()
        {
            Items = new Dictionary<Item, int>();
        }

        public abstract void Sort();

        public abstract void Print(StreamWriter writer);

        public abstract bool TryAdd(Item item);

        public static void WriteTitle(StreamWriter writer, string title, string middle="Special")
        {
            writer.WriteLine("**"+title+"**\n");
            writer.WriteLine("Item|{0}|Price",middle);
            writer.WriteLine(":---|:---|:---");
        }
    }
}
