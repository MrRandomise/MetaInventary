using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using InventoryCore;
using CharacterCore;

namespace UniTest
{
    [TestFixture]
    public class EquipmentUniTests
    {
        private Character _character;
        private Inventory _inventory;
        private Equipment _equipment;

        private const string GoldBoots = "goldBoots";
        private const string AirBots = "airBots";
        private const string StrongSword = "strongSword";
        private const string MetallShield = "metallShield";
        private const string BloodMail = "bloodMail";
        private const string Helmet = "helmet";

        private const string SpeedStat = "speed";
        private const string DamageStat = "damage";
        private const string HealthStat = "health";

        [SetUp]
        public void Init()
        {
            _character = new Character(
                new KeyValuePair<string, int>(DamageStat, 11),
                new KeyValuePair<string, int>(HealthStat, 12),
                new KeyValuePair<string, int>(SpeedStat, 5));
            _inventory = new Inventory();
            _equipment = new Equipment();
            CreateItemsAndAddToInventory();
        }

        private void CreateItemsAndAddToInventory()
        {
            var goldBoots = new Item(GoldBoots, ItemFlags.EQUIPPABLE | ItemFlags.EFFECTIBLE,
                new Stats(SpeedStat, 10),
                new EquipmentTypeComponent(EquipmentType.LEGS)
            );

            var airBots = new Item(AirBots, ItemFlags.EQUIPPABLE | ItemFlags.EFFECTIBLE,
                new Stats(HealthStat, 10),
                new Stats(SpeedStat, 10),
                new EquipmentTypeComponent(EquipmentType.LEGS)
            );

            var strongSword = new Item(StrongSword, ItemFlags.EQUIPPABLE | ItemFlags.EFFECTIBLE,
                new Stats(DamageStat, 15),
                new EquipmentTypeComponent(EquipmentType.RIGHT_HAND)
            );

            var metallShield = new Item(MetallShield, ItemFlags.EQUIPPABLE | ItemFlags.EFFECTIBLE,
                new Stats(HealthStat, 15),
                new EquipmentTypeComponent(EquipmentType.LEFT_HAND)
            );

            var bloodMail = new Item(BloodMail, ItemFlags.EQUIPPABLE | ItemFlags.EFFECTIBLE,
                new Stats(HealthStat, 16),
                new EquipmentTypeComponent(EquipmentType.BODY)
            );

            var helmet = new Item(Helmet, ItemFlags.EQUIPPABLE | ItemFlags.EFFECTIBLE,
                new Stats(HealthStat, 12),
                new EquipmentTypeComponent(EquipmentType.HEAD)
            );

            _inventory.AddItem(goldBoots);
            _inventory.AddItem(airBots);
            _inventory.AddItem(strongSword);
            _inventory.AddItem(metallShield);
            _inventory.AddItem(bloodMail);
            _inventory.AddItem(helmet);
        }
        
        
        [TestCase(GoldBoots, EquipmentType.LEGS)]
        [TestCase(StrongSword, EquipmentType.RIGHT_HAND)]
        [TestCase(MetallShield, EquipmentType.LEFT_HAND)]
        [TestCase(BloodMail, EquipmentType.BODY)]
        [TestCase(Helmet, EquipmentType.HEAD)]
        [Test]
        public void EquipItem(string itemName, EquipmentType type)
        {
            _inventory.FindItem(itemName, out var item);
            _equipment.EquipItem(item, _character);
            Assert.AreEqual(true, _equipment.HasItem(type));
        }
        
        [TestCase(GoldBoots, EquipmentType.LEGS)]
        [TestCase(StrongSword, EquipmentType.RIGHT_HAND)]
        [TestCase(MetallShield, EquipmentType.LEFT_HAND)]
        [TestCase(BloodMail, EquipmentType.BODY)]
        [TestCase(Helmet, EquipmentType.HEAD)]
        [Test]
        public void UnequipItem(string itemName, EquipmentType type)
        {
            _inventory.FindItem(itemName, out var item);
            _equipment.EquipItem(item, _character);
            _equipment.UnequipItem(item, _character);
            Assert.AreEqual(false, _equipment.HasItem(type));
        }

        [TestCase(GoldBoots)]
        [TestCase(StrongSword)]
        [TestCase(MetallShield)]
        [TestCase(BloodMail)]
        [TestCase(Helmet)]
        [Test]
        public void WhenEquip_CheckStats(string itemName)
        {
            var stats= _character.GetStats();
            var cachedStats = new KeyValuePair<string,int>[stats.Length];
            stats.CopyTo(cachedStats,0);
            
            _inventory.FindItem(itemName, out var item);
            var itemStatsArray = item.GetComponents<Stats>();

            _equipment.EquipItem(item, _character);
            var currentStats= _character.GetStats();
            
            for (var i = 0; i < stats.Length; i++)
            {
                var itemStat = itemStatsArray.FirstOrDefault(t => t.Name == stats[i].Key);
                if (itemStat == null)
                    continue;
                Assert.AreEqual( currentStats[i].Value-cachedStats[i].Value, itemStat.Value);
            }
        }
        
        [TestCase(GoldBoots)]
        [TestCase(StrongSword)]
        [TestCase(MetallShield)]
        [TestCase(BloodMail)]
        [TestCase(Helmet)]
        [Test]
        public void WhenUnequip_CheckStats(string itemName)
        {
            var stats= _character.GetStats();
            var cachedStats = new KeyValuePair<string,int>[stats.Length];
            stats.CopyTo(cachedStats,0);
            
            _inventory.FindItem(itemName, out var item);

            _equipment.EquipItem(item, _character);
            _equipment.UnequipItem(item, _character);

            var currentStats = _character.GetStats();
            
            for (var i = 0; i < stats.Length; i++)
            {
                Assert.AreEqual( currentStats[i].Value, cachedStats[i].Value);
            }
        }
        
        [Test]
        public void SwapItems()
        {
            _inventory.FindItem(GoldBoots, out var item);

            _equipment.EquipItem(item, _character);
            Assert.AreEqual(true, _equipment.HasItem(EquipmentType.LEGS));

            _inventory.FindItem(AirBots, out item);

            _equipment.EquipItem(item, _character);
            Assert.AreEqual(true, _equipment.HasItem(EquipmentType.LEGS));
        }

        [Test]
        public void EquipTwice()
        {
            _inventory.FindItem(GoldBoots, out var item);

            _equipment.EquipItem(item, _character);
            _equipment.EquipItem(item, _character);
            Assert.AreEqual(true, _equipment.HasItem(EquipmentType.LEGS));
        }

        [Test]
        public void UnequipTwice()
        {
            _inventory.FindItem(GoldBoots, out var item);

            _equipment.EquipItem(item, _character);

            _equipment.UnequipItem(item, _character);
            _equipment.UnequipItem(item, _character);
            Assert.AreEqual(false, _equipment.HasItem(EquipmentType.LEGS));
        }
        
        [Test]
        public void UnequipEmpty()
        {
            _inventory.FindItem(GoldBoots, out var item);
            _equipment.UnequipItem(item, _character);
            Assert.AreEqual(false, _equipment.HasItem(EquipmentType.LEGS));
        }
    }
}