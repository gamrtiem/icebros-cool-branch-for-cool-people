﻿using R2API;
using RoR2;
namespace SS2
{
    //TODO: Create proper difficulty module home-made
    /*public static class Cyclone
    {
        public static R2API.ScriptableObjects.SerializableDifficultyDef CycloneDef { get; private set; }
        public static DifficultyIndex CycloneIndex { get => CycloneDef.DifficultyIndex; }

        private static int defMonsterCap;
        private static RuleChoiceDef rcd;

        internal static void Init()
        {
            CycloneDef = SS2Assets.LoadAsset<R2API.ScriptableObjects.SerializableDifficultyDef>("Cyclone", SS2Bundle.Base);
            DifficultyAPI.AddDifficulty(CycloneDef);
            Run.onRunDestroyGlobal += Run_onRunDestroyGlobal;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
        }

        private static void Run_onRunStartGlobal(Run run)
        {
            defMonsterCap = TeamCatalog.GetTeamDef(TeamIndex.Monster).softCharacterLimit;
        }

        private static void Run_onRunDestroyGlobal(Run run)
        {
            TeamCatalog.GetTeamDef(TeamIndex.Monster).softCharacterLimit = defMonsterCap;
            TeamCatalog.GetTeamDef(TeamIndex.Void).softCharacterLimit = defMonsterCap;
            TeamCatalog.GetTeamDef(TeamIndex.Lunar).softCharacterLimit = defMonsterCap;
        }
    }*/
}
