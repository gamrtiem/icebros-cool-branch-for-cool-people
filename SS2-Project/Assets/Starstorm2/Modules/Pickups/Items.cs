﻿using BepInEx;
using R2API.ScriptableObjects;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using RiskOfOptions.OptionConfigs;

using MSU;
namespace SS2.Modules
{
    public sealed class Items : ItemModuleBase
    {
        public static Items Instance { get; private set; }

        public BaseUnityPlugin MainClass => SS2Main.instance;
        public override R2APISerializableContentPack SerializableContentPack => SS2Content.Instance.SerializableContentPack;

        public static ConfigurableBool EnableItems = SS2Config.MakeConfigurableBool(true, b =>
        {
            b.Section = "Enable All Items";
            b.Key = "Enable All Items";
            b.Description = "Enables Starstorm 2's items. Set to false to disable all items";
            b.ConfigFile = SS2Config.ConfigItem;
            b.CheckBoxConfig = new CheckBoxConfig
            {
                restartRequired = true,
            };
        }).DoConfigure();

        private static IEnumerable<SS2Item> items;
        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            SS2Log.Info($"Initializing Items...");
            items = GetItemBases();
        }

        [SystemInitializer(typeof(ItemTierCatalog))]  // done for custom tiered items that doesn't work with deprecatedtier. -P
        public static void TierInit()
        {
            SS2Log.Info($"Post-Initializing Items...");
            items.ToList().ForEach(i => { if (i.ItemDef.deprecatedTier == ItemTier.NoTier) i.ItemDef.tier = ItemTier.NoTier; });
        }

        protected override IEnumerable<SS2Item> GetItemBases()
        {
            List<SS2Item> list = base.GetItemBases().ToList();
            list.ForEach(item => AddItem(item));
            list.ForEach(CheckEnabledStatus);
            return list;
        }

        public void CheckEnabledStatus(SS2Item item)
        {
            if (item.ItemDef.deprecatedTier != ItemTier.NoTier || item.ItemDef.tier == ItemTier.AssignedAtRuntime) //fix for sybl
            {
                string niceName = MSUtil.NicifyString(item.GetType().Name);
                var cfg = SS2Config.MakeConfigurableBool(true, b =>
                {
                    b.Section = niceName;
                    b.Key = "Enabled";
                    b.Description = "Should this item be enabled";
                    b.ConfigFile = SS2Config.ConfigItem;
                    b.CheckBoxConfig = new CheckBoxConfig
                    {
                        restartRequired = true,
                        checkIfDisabled = () => !EnableItems,
                    };
                }).DoConfigure();
                //SS2Log.Info("EnabledItems checking " + item.ItemDef.nameToken );
                if (!EnableItems || !cfg)
                {
                    //SS2Log.Info("Disabling " + niceName);
                    item.ItemDef.deprecatedTier = ItemTier.NoTier;
                }
            }
        }

    }
}
