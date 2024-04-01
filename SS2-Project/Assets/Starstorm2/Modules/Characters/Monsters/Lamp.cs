﻿using UnityEngine;
namespace SS2.Monsters
{
    public sealed class Lamp : SS2Monster
    {
        public override GameObject BodyPrefab { get; } = SS2Assets.LoadAsset<GameObject>("LampBody", SS2Bundle.Monsters);
        public override GameObject MasterPrefab { get; } = SS2Assets.LoadAsset<GameObject>("LampMaster", SS2Bundle.Monsters);

        private MSMonsterDirectorCard defaultCard = SS2Assets.LoadAsset<MSMonsterDirectorCard>("msmdcLamp", SS2Bundle.Monsters);

        public override void Initialize()
        {
            base.Initialize();
            MonsterDirectorCards.Add(defaultCard);
            //Addressables.LoadAssetAsync<DccsPool>()
        }
    }
}
