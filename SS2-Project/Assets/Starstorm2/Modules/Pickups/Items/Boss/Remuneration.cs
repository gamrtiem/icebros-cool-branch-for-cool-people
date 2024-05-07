﻿using MSU;
using RoR2;
using RoR2.ContentManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SS2.Items
{
    public sealed class Remuneration : SS2Item
    {
        public override SS2AssetRequest<ItemAssetCollection> AssetRequest<ItemAssetCollection>()
        {
            return SS2Assets.LoadAssetAsync<ItemAssetCollection>("acRemuneration", SS2Bundle.Items);
        }
        public static GameObject remunerationControllerPrefab;

        public override void OnAssetCollectionLoaded(AssetCollection assetCollection)
        {
            remunerationControllerPrefab = assetCollection.FindAsset<GameObject>("RemunerationController");
        }
        public override void Initialize()
        {
            On.RoR2.PickupDisplay.RebuildModel += EnableVoidParticles;
        }

        public override bool IsAvailable(ContentPack contentPack)
        {
            return contentPack.survivorDefs.Find("survivorNemMerc");
        }

        // this works for all sibylline items but i dont know where to put general hooks like that
        // also MSU is apparently supposed to be able to do this. can remove this whenever thats fixed

        //That was orb, someone please ask him what he meant by this, he never told me what he was trying to do :,) -N
        private void EnableVoidParticles(On.RoR2.PickupDisplay.orig_RebuildModel orig, PickupDisplay self)
        {
            orig(self);
            PickupDef pickupDef = PickupCatalog.GetPickupDef(self.pickupIndex);
            ItemIndex itemIndex = (pickupDef != null) ? pickupDef.itemIndex : ItemIndex.None;
            if (itemIndex != ItemIndex.None && ItemCatalog.GetItemDef(itemIndex).tier == SS2Content.ItemTierDefs.Sibylline.tier)
            {
                if (self.voidParticleEffect)
                {
                    self.voidParticleEffect.SetActive(true);
                }
            }
        }

        public sealed class RemunerationBehavior : BaseItemMasterBehaviour
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => SS2Content.Items.Remuneration;

            private void Awake()
            {
                base.Awake();
                Stage.onServerStageBegin += TrySpawnShop;
            }

            private void OnDestroy()
            {
                Stage.onServerStageBegin -= TrySpawnShop;
            }

            private void TrySpawnShop(Stage stage)
            {
                if (stage.sceneDef && stage.sceneDef.sceneType == SceneType.Stage)
                    base.GetComponent<CharacterMaster>().onBodyStart += SpawnPortalOnBody;
            }

            // should only happen the first time a master spawns each stage
            private void SpawnPortalOnBody(CharacterBody body)
            {
                GameObject controller = GameObject.Instantiate(remunerationControllerPrefab, body.coreTransform.position, body.coreTransform.rotation);
                controller.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(body.gameObject);

                body.master.onBodyStart -= SpawnPortalOnBody;

            }
        }
    }
}
