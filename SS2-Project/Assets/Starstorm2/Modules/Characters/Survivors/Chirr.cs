﻿using RoR2;
using UnityEngine;
using RoR2.Skills;
using System.Runtime.CompilerServices;
using UnityEngine.AddressableAssets;
using System;
using MSU;
using System.Collections;
using MSU.Config;
using System.Collections.Generic;
using Assets.Starstorm2;
using static R2API.DamageAPI;
using R2API;
using RoR2.ContentManagement;

namespace SS2.Survivors
{
    public sealed class Chirr : SS2Survivor, IContentPackModifier
    {
        public override SurvivorDef SurvivorDef => _survivorDef;
        private SurvivorDef _survivorDef;
        public override NullableRef<GameObject> MasterPrefab => _monsterMaster;
        private GameObject _monsterMaster;
        public override GameObject CharacterPrefab => _prefab;
        private GameObject _prefab;

        public static Vector3 chirristmasPos = new Vector3(-6.8455f, -7.0516f, 57.0163f);
        public static Vector3 chirristmasRot = new Vector3(0, 178.3926f, 0);

        private static GameObject chirristmas;

        [RiskOfOptionsConfigureField(SS2Config.ID_SURVIVOR, ConfigDescOverride = "Can Weightless Frame be activated by toggling.")]
        public static bool toggleHover = false;
        
        public static float confuseDuration = 4f;
        public static ModdedDamageType confuseDamageType;
        public static float _confuseSlowAmount = 0.5f;
        public static float _confuseAttackSpeedSlowAmount = 0.0f;
        private BuffDef _confuseBuffDef;

        private BuffDef _convertBuffDef;
        private float _convertDotDamageCoefficient;
        public static DotController.DotIndex ConvertDotIndex { get; private set; }

        private BuffDef _buffChirrFriend;
        private Material _matFriendOverlay;

        private static float _grabFriendAttackBoost = 1f;
        private BuffDef _buffGrabFriend;

        private static float _percentHealthRegen = 0.05f;
        private BuffDef _buffChirrRegen;

        public override void Initialize()
        {
            if (SS2Main.ScepterInstalled)
            {
                ScepterCompat();
            }

            if (SS2Main.ChristmasTime)
            {
                On.RoR2.UI.MainMenu.BaseMainMenuScreen.OnEnter += HiChirrHiiiiii;
            }

            Stage.onStageStartGlobal += FixGoolakeRaycasts;

            ModifyPrefab();
            R2API.RecalculateStatsAPI.GetStatCoefficients += ModifyStats;

            RegisterConfuseOnHit();
            RegisterConvert();
            BuffOverlays.AddBuffOverlay(_buffChirrFriend, _matFriendOverlay);
        }

        private void HiChirrHiiiiii(On.RoR2.UI.MainMenu.BaseMainMenuScreen.orig_OnEnter orig, RoR2.UI.MainMenu.BaseMainMenuScreen self, RoR2.UI.MainMenu.MainMenuController mainMenuController)
        {
            orig(self, mainMenuController);
            if (chirristmas) return;
            chirristmas = GameObject.Instantiate(SS2Assets.LoadAsset<GameObject>("ChirrDisplay", SS2Bundle.Chirr), chirristmasPos, Quaternion.Euler(chirristmasRot));
            chirristmas.transform.localScale = Vector3.one * 2.4f;
        }

        //Disables CookForFasterSimulation on the terrain in goolake, since it fucks up world raycasts
        // only when chirr is in the stage cuz idk how badly it affects performance
        private void FixGoolakeRaycasts(Stage stage)
        {
            BodyIndex chirr = _prefab.GetComponent<CharacterBody>().bodyIndex;
            if (stage.sceneDef == SceneCatalog.GetSceneDefFromSceneName("goolake"))
            {
                foreach (PlayerCharacterMasterController pcmc in PlayerCharacterMasterController.instances)
                {
                    if (pcmc.master.bodyPrefab.GetComponent<CharacterBody>().bodyIndex == chirr)
                    {
                        GameObject terrain = GameObject.Find("HOLDER: GameplaySpace/Terrain");
                        if (terrain)
                        {
                            SS2Log.Warning("Player Chirr found. Disabling terrain mesh optimization on goolake to avoid gameplay bugs.");
                            terrain.GetComponent<MeshCollider>().cookingOptions &= ~MeshColliderCookingOptions.CookForFasterSimulation;
                            break;
                        }
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public void ScepterCompat()
        {
            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(SS2Assets.LoadAsset<SkillDef>("BefriendScepter", SS2Bundle.Chirr), "ChirrBody", SkillSlot.Special, 0);
        }

        private void ModifyStats(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            int confuseCount = sender.GetBuffCount(_confuseBuffDef);
            int grabFriendCount = sender.GetBuffCount(_buffGrabFriend);
            bool hasRegenBuff = sender.HasBuff(_buffChirrRegen);

            args.moveSpeedMultAdd -= _confuseSlowAmount * confuseCount;
            args.attackSpeedMultAdd -= _confuseAttackSpeedSlowAmount * confuseCount;
            args.baseAttackSpeedAdd += _grabFriendAttackBoost;

            if(hasRegenBuff)
            {
                args.baseRegenAdd += sender.maxHealth * _percentHealthRegen;
            }
        }

        public override bool IsAvailable(ContentPack contentPack)
        {
            return true;
        }

        public override IEnumerator LoadContentAsync()
        {
            //N: Thank fuck the "ParallelAsseTLoadCoroutineHelper" isnt real anymore

            /*
             * GameObject - "ChirrBody" - Chirr
             * GameObject - "ChirrMonsterMaster" - Chirr
             * SurvivorDef - "Chirr" - Chirr
             * BuffDef - "BuffChirrConfuse" - Chirr
             * BuffDef - "BuffChirrConvert" - Chirr
             * Material - "matFriendOverlay" - Chirr
             * BuffDef - "BuffChirrGrabFriend" - Chirr
             * BuffDef - "BuffChirrRegen" - Chirr
             */
            yield break;
        }

        private void ModifyPrefab()
        {
            var cb = _prefab.GetComponent<CharacterBody>();

            // would be cool to have something unique for her
            // someone mentioned her "hatching" from a tree i think
            cb.preferredPodPrefab = Resources.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod");

            cb._defaultCrosshairPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/StandardCrosshair.prefab").WaitForCompletion();
            cb.GetComponent<ModelLocator>().modelTransform.GetComponent<FootstepHandler>().footstepDustPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/GenericFootstepDust.prefab").WaitForCompletion();
        }
    
        private void RegisterConfuseOnHit()
        {
            confuseDamageType = R2API.DamageAPI.ReserveDamageType();
            GlobalEventManager.onServerDamageDealt += ApplyConfuse;
        }

        private void RegisterConvert()
        {
            ConvertDotIndex = DotAPI.RegisterDotDef(0.33f, _convertDotDamageCoefficient, DamageColorIndex.Poison, _convertBuffDef);
        }
        private void ApplyConfuse(DamageReport obj)
        {
            var victimBody = obj.victimBody;
            var damageInfo = obj.damageInfo;
            if (DamageAPI.HasModdedDamageType(damageInfo, confuseDamageType))
            {
                victimBody.AddTimedBuffAuthority(SS2Content.Buffs.BuffChirrConfuse.buffIndex, confuseDuration);
            }
        }

        public void ModifyContentPack(ContentPack contentPack)
        {
            contentPack.buffDefs.Add(new BuffDef[]
            {
                _confuseBuffDef,
                _convertBuffDef,
                _buffChirrFriend,
                _buffGrabFriend,
            });
        }
    }
}
