﻿using R2API;
using RoR2;

using MSU;
namespace SS2.Buffs
{
    /*public sealed class Overstress : BuffBase
    {
        public override BuffDef BuffDef { get; } = SS2Assets.LoadAsset<BuffDef>("bdOverstress", SS2Bundle.Indev);

        public sealed class Behavior : BaseBuffBodyBehavior, IBodyStatArgModifier
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => SS2Content.Buffs.bdOverstress;
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.armorAdd -= 20f;
                args.moveSpeedReductionMultAdd += 0.3f;
                args.damageMultAdd -= 0.5f;
            }

            private void OnDestroy()
            {
                body.RecalculateStats();
            }
        }
    }*/
}
