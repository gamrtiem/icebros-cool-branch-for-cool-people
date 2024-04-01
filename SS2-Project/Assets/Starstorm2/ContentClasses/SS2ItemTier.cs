﻿using MSU;
using R2API.ScriptableObjects;
using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SS2
{
    public abstract class SS2ItemTier : IItemTierContentPiece
    {
        public abstract NullableRef<SerializableColorCatalogEntry> ColorIndex { get; }
        public abstract NullableRef<SerializableColorCatalogEntry> DarkColorIndex { get; }
        public abstract GameObject PickupDisplayVFX { get; }
        public List<ItemIndex> ItemsWithThisTier { get; set; }
        public List<PickupIndex> AvailableTierDropList { get; set; }
        ItemTierDef IContentPiece<ItemTierDef>.Asset { get; }
        public abstract ItemTierDef ItemTierDef { get; }

        public abstract void Initialize();
        public abstract bool IsAvailable();
        public abstract IEnumerator LoadContentAsync();
    }
}