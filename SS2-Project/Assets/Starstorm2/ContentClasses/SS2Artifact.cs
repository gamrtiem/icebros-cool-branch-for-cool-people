﻿using MSU;
using R2API.ScriptableObjects;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS2
{
    public abstract class SS2Artifact : IArtifactContentPiece
    {
        public abstract NullableRef<ArtifactCode> ArtifactCode { get; }
        ArtifactDef IContentPiece<ArtifactDef>.Asset => ArtifactDef;
        public abstract ArtifactDef ArtifactDef { get; }
        public abstract void Initialize();
        public abstract bool IsAvailable();
        public abstract IEnumerator LoadContentAsync();
        public abstract void OnArtifactDisabled();
        public abstract void OnArtifactEnabled();
    }
}