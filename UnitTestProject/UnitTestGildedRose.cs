using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace UnitTestProject
{
    [TestClass]
    public class ShoppingCartTest
    {
        private const string Aged = "Aged Brie";
        private const string Elixir = "Elixir of the Mongoose";
        private const string Sulfuras = "Sulfuras, Hand of Ragnaros";
        private const string Backstage = "Backstage passes to a TAFKAL80ETC concert";
        private const string Conjured = "Conjured Mana Cake";

        [TestMethod]
        public void LegendaryItem_BeforeSellDate_QualityAndSellIn_RemainUnchanged()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Sulfuras, 10, 80);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(80, cart.Items.First(x => x.Name == Sulfuras).Quality);
            Assert.AreEqual(10, cart.Items.First(x => x.Name == Sulfuras).SellIn);
        }

        [TestMethod]
        public void LegendaryItem_OnSellDate_QualityAndSellIn_RemainUnchanged()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Sulfuras, 0, 80);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(80, cart.Items.First(x => x.Name == Sulfuras).Quality);
            Assert.AreEqual(0, cart.Items.First(x => x.Name == Sulfuras).SellIn);
        }

        [TestMethod]
        public void LegendaryItem_AfterSellDate_QualityAndSellIn_RemainUnchanged()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Sulfuras, -1, 80);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(80, cart.Items.First(x => x.Name == Sulfuras).Quality);
            Assert.AreEqual(-1, cart.Items.First(x => x.Name == Sulfuras).SellIn);
        }

        [TestMethod]
        public void NormalItem_BeforeSellDate_Quality_ReducedByOne()
        {
            var cart = new ShoppingCart();
            
            var mockedItem = GetMockedItem(Elixir, 5, 7);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(6, cart.Items.First(x => x.Name == Elixir).Quality);
            Assert.AreEqual(4, cart.Items.First(x => x.Name == Elixir).SellIn);
        }

        [TestMethod]
        public void NormalItem_OnSellDate_Quality_ReduceByTwo()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Elixir, 0, 7);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(5, cart.Items.First(x => x.Name == Elixir).Quality);
            Assert.AreEqual(-1, cart.Items.First(x => x.Name == Elixir).SellIn);
        }

        [TestMethod]
        public void NormalItem_AfterSellDate_Quality_ReducedByTwo()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Elixir, -1, 7);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(5, cart.Items.First(x => x.Name == Elixir).Quality);
            Assert.AreEqual(-2, cart.Items.First(x => x.Name == Elixir).SellIn);
        }

        [TestMethod]
        public void NormalItem_WithZeroQuality_ReturnMinQuality()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Elixir, 2, 0);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(0, cart.Items.First(x => x.Name == Elixir).Quality);
            Assert.AreEqual(1, cart.Items.First(x => x.Name == Elixir).SellIn);
        }

        [TestMethod]
        public void AgedItem_BeforeSellDate_Quality_IncreasedByOne()
        {
            var cart = new ShoppingCart();

            var mockedItem = GetMockedItem(Aged, 2, 0);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(1, cart.Items.First(x => x.Name == Aged).Quality);
            Assert.AreEqual(1, cart.Items.First(x => x.Name == Aged).SellIn);
        }

        [TestMethod]
        public void AgedItem_OnSellDate_Quality_IncreasedByTwo()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Aged, 0, 0);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(2, cart.Items.First(x => x.Name == Aged).Quality);
            Assert.AreEqual(-1, cart.Items.First(x => x.Name == Aged).SellIn);
        }

        [TestMethod]
        public void AgedItem_AfterSellDate_Quality_RemainUnchanged()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Aged, -1, 0);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(2, cart.Items.First(x => x.Name == Aged).Quality);
            Assert.AreEqual(-2, cart.Items.First(x => x.Name == Aged).SellIn);
        }

        [TestMethod]
        public void AgedItem_WithMaxQuality_ReturnMaxQuality()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Aged, 10, 50);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(50, cart.Items.First(x => x.Name == Aged).Quality);
            Assert.AreEqual(9, cart.Items.First(x => x.Name == Aged).SellIn);
        }

        [TestMethod]
        public void BackstageItem_On11SellDate_Quality_IncreasedByOne()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Backstage, 11, 20);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(21, cart.Items.First(x => x.Name == Backstage).Quality);
            Assert.AreEqual(10, cart.Items.First(x => x.Name == Backstage).SellIn);
        }

        [TestMethod]
        public void BackstageItem_On10SellDate_Quality_IncreasedByTwo()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Backstage, 10, 20);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(22, cart.Items.First(x => x.Name == Backstage).Quality);
            Assert.AreEqual(9, cart.Items.First(x => x.Name == Backstage).SellIn);
        }

        [TestMethod]
        public void BackstageItem_On6SellDate_Quality_IncreasedByTwo()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Backstage, 6, 20);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(22, cart.Items.First(x => x.Name == Backstage).Quality);
            Assert.AreEqual(5, cart.Items.First(x => x.Name == Backstage).SellIn);
        }

        [TestMethod]
        public void BackstageItem_On5SellDate_Quality_IncreasedByTwo()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Backstage, 5, 20);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(23, cart.Items.First(x => x.Name == Backstage).Quality);
            Assert.AreEqual(4, cart.Items.First(x => x.Name == Backstage).SellIn);
        }

        [TestMethod]
        public void BackstageItem_OnSellDate_Quality_DroppedToZero()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Backstage, 0, 20);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(0, cart.Items.First(x => x.Name == Backstage).Quality);
            Assert.AreEqual(-1, cart.Items.First(x => x.Name == Backstage).SellIn);
        }

        [TestMethod]
        public void BackstageItem_AfterSellDate_Quality_DroppedToZero()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Backstage, -1, 20);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(0, cart.Items.First(x => x.Name == Backstage).Quality);
            Assert.AreEqual(-2, cart.Items.First(x => x.Name == Backstage).SellIn);
        }

        [TestMethod]
        public void BackstageItem_WithMaxQuality_ReturnMaxQuality()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Backstage, 15, 50);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(50, cart.Items.First(x => x.Name == Backstage).Quality);
            Assert.AreEqual(14, cart.Items.First(x => x.Name == Backstage).SellIn);
        }

        [TestMethod]
        public void ConjuredItem_BeforeSellDate_Quality_IncreasedByOne()
        {
            var cart = new ShoppingCart();

            var mockedItem = GetMockedItem(Conjured, 3, 6);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(4, cart.Items.First(x => x.Name == Conjured).Quality);
            Assert.AreEqual(2, cart.Items.First(x => x.Name == Conjured).SellIn);
        }

        [TestMethod]
        public void ConjuredItem_OnSellDate_Quality_IncreasedByTwo()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Conjured, 0, 6);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(2, cart.Items.First(x => x.Name == Conjured).Quality);
            Assert.AreEqual(-1, cart.Items.First(x => x.Name == Conjured).SellIn);
        }

        [TestMethod]
        public void ConjuredItem_AfterSellDate_Quality_RemainUnchanged()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Conjured, -1, 6);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(2, cart.Items.First(x => x.Name == Conjured).Quality);
            Assert.AreEqual(-2, cart.Items.First(x => x.Name == Conjured).SellIn);
        }

        [TestMethod]
        public void ConjuredItem_WithZeroQuality_ReturnMinQuality()
        {
            var cart = new ShoppingCart();
            var mockedItem = GetMockedItem(Conjured, 3, 0);
            cart.Items.Add(mockedItem);
            cart.Run();

            Assert.AreEqual(0, cart.Items.First(x => x.Name == Conjured).Quality);
            Assert.AreEqual(2, cart.Items.First(x => x.Name == Conjured).SellIn);
        }

        private Item GetMockedItem(string itemName, int sellIn, int quality)
        {
            var items = MockRepository.GenerateStub<Item>();

            items.Name = itemName;
            items.Quality = quality;
            items.SellIn = sellIn;

            return items;
        }
    }
}
