﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TF2TradePriceTool
{
    class StrangeWeaponSection : Section
    {
        public override void Sort()
        {
            OrderedList = Items.Keys.OrderBy(t => t.Name).ThenBy(t => t.PaintName).ThenBy(t => t[Item.StrangePart1]).ThenBy(t => t[Item.StrangePart2]).ThenBy(
                t => t[Item.StrangePart3]);
        }

        public override void Print(System.IO.StreamWriter writer)
        {
            Section.WriteTitle(writer, "Strange Weapons", "Parts");
            int cnt = 0;
            foreach (Item i in OrderedList)
            {
                double percent = Math.Round(((double)cnt) * 100 / ((double)Items.Keys.Count));
                Console.WriteLine("Progress: Item {0} of {1} (" + percent + "%)", cnt + 1, Items.Keys.Count);
                List<String> attribs = new List<string>();
                if(i.StrangeParts != null)
                    attribs.AddRangeIfNotNull(i.StrangeParts.Select(t => TF2PricerMain.Schema.StrangePartNames[Convert.ToInt32(t)]));
                if (i.IsGifted)
                    attribs.Add("Gifted");
                //pretty print the item
                String item = TF2PricerMain.FormatItem(i, true, Items[i], attribs.ToArray());
                Price[] parts = new Price[3] { null, null, null };
                if (i.StrangeParts != null)
                {
                    for (int partCount = 0; partCount < 3; ++partCount)
                    {
                        parts[partCount] = TF2PricerMain.PriceSchema.GetPartPrice(i[Item.StrangePart1 + partCount]);
                    }
                }
                Price p = TF2PricerMain.PriceSchema.GetPrice(i);
                //so write the item, then follow up with the bp.tf prices
                Console.WriteLine(item + "\n");
                if (i.StrangeParts != null)
                {
                    Console.WriteLine("Original: " + p.ToString());
                    int partNo = 0;
                    foreach (String part in i.StrangeParts)
                    {
                        Console.WriteLine(TF2PricerMain.Schema.StrangePartNames[Convert.ToInt32(part)] + ": " + parts[partNo].ToString());
                        p += parts[partNo];
                        ++partNo;
                    }
                }
                Console.WriteLine("Price: " + p.ToString());
                TF2PricerMain.GetInputPrice(item, writer, p.LowPrice, p.HighPrice);
                cnt++;
            }
        }

        public override bool TryAdd(Item item)
        {
            if (item.Quality == Quality.Strange && item.Type == ItemType.Weapon)
            {
                Items.AddCounted(item);
                return true;
            }
            else
                return false;
        }
    }
}
