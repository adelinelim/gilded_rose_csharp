using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject
{
    public class ShoppingCart
    {
        private readonly ICalculationRules _calculationRules;
        public ShoppingCart()
        {
            _calculationRules = new CalculationRules();
            Items = new List<Item>();
        }

        public List<Item> Items
        {
            get;
            set;
        }

        public string Run()
        {
            string result = "";

            foreach (var i in Items)
            {
                _calculationRules.UpdateItem(i);
                result = result + "\r\n" + "Name: " + i.Name + "\tSellIn: " + i.SellIn + "\tQuanlity: " + i.Quality;
            }
            result += "\r\n";
            return result;
        }
    }

    public static class CalculationHelper
    {
        public static readonly int MAX_QUALITY = 50;
        public static readonly int MIN_QUALITY = 0;

        public static void UpdateSellIn(Item item)
        {
            if (!IsLegendaryItem(item))
                item.SellIn -= 1;
        }

        public static bool IsLegendaryItem(Item item)
        {
            return item.Name.Equals("Sulfuras, Hand of Ragnaros");
        }

        public static bool IsExpired(Item item)
        {
            return item.SellIn <= 0;
        }
    }

    public interface ICalculationRules
    {
        void UpdateItem(Item item);
    }

    public class CalculationRules : ICalculationRules
    {
        private readonly List<ICalculateQualityStrategy> _qualityStrategies;


        public CalculationRules()
        {
            _qualityStrategies = new List<ICalculateQualityStrategy>();
            _qualityStrategies.Add(new CalculateAgedItemStrategy());
            _qualityStrategies.Add(new CalculateConjuredItemStrategy());
            _qualityStrategies.Add(new CalculateNormalItemStrategy());
            _qualityStrategies.Add(new CalculateSpecialAgeingItemStrategy());
            _qualityStrategies.Add(new CalculateLegendaryItemStrategy());
        }

        public void UpdateItem(Item item)
        {
            item.Quality = _qualityStrategies.First(r => r.IsMatch(item)).CalculateQuality(item);
            CalculationHelper.UpdateSellIn(item);
        }
    }

    public interface ICalculateQualityStrategy
    {
        bool IsMatch(Item item);
        int CalculateQuality(Item item);
    }

    public class CalculateAgedItemStrategy : ICalculateQualityStrategy
    {
        public bool IsMatch(Item item)
        {
            return item.Name.Equals("Aged Brie");
        }

        public int CalculateQuality(Item item)
        {
            int quality = item.Quality;

            if (CalculationHelper.IsExpired(item))
            {
                quality += 2;
            }
            else
            {
                quality += 1;
            }

            if (quality > CalculationHelper.MAX_QUALITY)
                quality = CalculationHelper.MAX_QUALITY;

            return quality;
        }
    }

    public class CalculateConjuredItemStrategy : ICalculateQualityStrategy
    {
        public bool IsMatch(Item item)
        {
            return item.Name.Equals("Conjured Mana Cake");
        }

        public int CalculateQuality(Item item)
        {
            int quality = item.Quality;

            if (CalculationHelper.IsExpired(item))
            {
                quality -= 4;
            }
            else
            {
                quality -= 2;
            }

            if (quality < CalculationHelper.MIN_QUALITY)
                quality = CalculationHelper.MIN_QUALITY;

            return quality;
        }
    }

    public class CalculateNormalItemStrategy : ICalculateQualityStrategy
    {
        public bool IsMatch(Item item)
        {
            return
                !CalculationHelper.IsLegendaryItem(item) && 
                !item.Name.Equals("Conjured Mana Cake") && 
                !item.Name.Equals("Aged Brie") && 
                !item.Name.Equals("Backstage passes to a TAFKAL80ETC concert");
        }

        public int CalculateQuality(Item item)
        {
            int quality = item.Quality;

            if (CalculationHelper.IsExpired(item))
            {
                quality -= 2;
            }
            else
            {
                quality -= 1;
            }

            if (quality < CalculationHelper.MIN_QUALITY)
                quality = CalculationHelper.MIN_QUALITY;

            return quality;
        }
    }

    public class CalculateSpecialAgeingItemStrategy : ICalculateQualityStrategy
    {
        public bool IsMatch(Item item)
        {
            return item.Name.Equals("Backstage passes to a TAFKAL80ETC concert");
        }

        public int CalculateQuality(Item item)
        {
            int quality = item.Quality;

            if (CalculationHelper.IsExpired(item))
            {
                quality = CalculationHelper.MIN_QUALITY;
            }
            else if (item.SellIn <= 5)
            {
                quality += 3;
            }
            else if (item.SellIn <= 10)
            {
                quality += 2;
            }
            else
            {
                quality += 1;
            }

            if (quality > CalculationHelper.MAX_QUALITY)
                quality = CalculationHelper.MAX_QUALITY;

            return quality;
        }
    }

    public class CalculateLegendaryItemStrategy : ICalculateQualityStrategy
    {
        public bool IsMatch(Item item)
        {
            return CalculationHelper.IsLegendaryItem(item);
        }

        public int CalculateQuality(Item item)
        {
            return item.Quality;
        }
    }

    //DO NOT MODIFY ITEM CLASS
    public class Item
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }
    }
    
}
