﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TF2TradePriceTool
{
    class UniqueWeaponSection : Section
    {
        public override void Sort()
        {
            OrderedList = Items.Keys.OrderBy(t => t.Name).ThenBy(t => t.IsCraftable);
        }

        public override void Print(System.IO.StreamWriter writer)
        {
            Section.WriteTitle(writer, "Unique Weapons");
            int cnt = 0;
            foreach (Item i in OrderedList)
            {
                double percent = Math.Round(((double)cnt) * 100 / ((double)Items.Keys.Count));
                Console.WriteLine("Progress: Item {0} of {1} (" + percent + "%)", cnt + 1, Items.Keys.Count);
                List<String> attribs = new List<string>();
                if (i.IsGifted)
                    attribs.Add("Gifted");
                if (!i.IsCraftable)
                    attribs.Add("Uncraftable");
                if (!i.IsTradable && i.Quality != Quality.Unusual)
                    continue; //nobody cares about UT weapons
                //pretty print the item
                String item = TF2PricerMain.FormatItem(i, false, Items[i], attribs.ToArray());
                Price p = TF2PricerMain.PriceSchema.GetPrice(i);
                //so write the item, then follow up with the bp.tf prices
                Console.WriteLine(item + "\n");
                Console.WriteLine("Price: " + p.ToString());
                TF2PricerMain.GetInputPrice(item, writer, p.LowPrice, p.HighPrice);
                cnt++;
            }
        }

        public override bool TryAdd(Item item)
        {
            if ((item.Quality == Quality.Unique || item.Quality == Quality.Unusual) && item.Type == ItemType.Weapon)
            {
                Items.AddCounted(item);
                return true;
            }
            else
                return false;
        }
    }
}
