﻿using RoR2;
using RoR2.Items;
using UnityEngine;
using UnityEngine.Networking;
namespace SS2.Items
{
    //N: Nebby here, just stating that the code related to Relic of Extinction should be treated as a cancerous tumour, it was coded by anreol, and just like anything he does for stuff outside his own projects, never finished it.
    //I'll get to finish it eventually with a full rewrite, he can kiss my ass.
    /*public sealed class RelicOfExtinction : SS2Item
    {
        public override ItemDef ItemDef { get; } = SS2Assets.LoadAsset<ItemDef>("RelicOfExtinction", SS2Bundle.Indev);

        public sealed class Behavior : BaseItemBodyBehavior
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => SS2Content.Items.RelicOfExtinction;

            private GameObject prolapsedInstance;
            public bool shouldFollow = true;
            public void Start()
            {
                if (!NetworkServer.active)
                {
                    return;
                }
                if (!prolapsedInstance)
                {
                    prolapsedInstance = UnityEngine.Object.Instantiate(SS2Assets.LoadAsset<GameObject>("ExtinctionHole", SS2Bundle.Indev), body.corePosition, Quaternion.identity);
                    prolapsedInstance.GetComponent<GenericOwnership>().ownerObject = body.gameObject;
                    prolapsedInstance.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(body.gameObject);
                }
            }

            //Should kill itself once it loses the owner
            /*public void OnDestroy()
            {
                if (prolapsed)
                {
                    UnityEngine.Object.Destroy(prolapsed);
                }
            }
        }
    }*/
}
