﻿using MSU;
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
    public abstract class SS2Item : IItemContentPiece
    {
        public abstract NullableRef<GameObject> ItemDisplayPrefab { get; }
        ItemDef IContentPiece<ItemDef>.Asset => ItemDef;
        public abstract ItemDef ItemDef { get; }

        public abstract void Initialize();
        public abstract bool IsAvailable();
        public abstract IEnumerator LoadContentAsync();
    }
}