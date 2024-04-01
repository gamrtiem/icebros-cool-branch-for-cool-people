﻿using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using RiskOfOptions.OptionConfigs;

using MSU;
namespace SS2.Modules
{
    public sealed class Interactables : InteractableModuleBase
    {
        public static Interactables Instance { get; private set; }

        public override R2APISerializableContentPack SerializableContentPack { get; } = SS2Content.Instance.SerializableContentPack;

        public static ConfigurableBool EnableInteractables = SS2Config.MakeConfigurableBool(true, (b) =>
        {
            b.Section = "Enable All Interactables";
            b.Key = "Enable All Interactables";
            b.Description = "Enables Starstorm 2's interactables. Set to false to disable interactables.";
            b.ConfigFile = SS2Config.ConfigMain;
            b.CheckBoxConfig = new CheckBoxConfig
            {
                restartRequired = true,
            };
        }).DoConfigure();

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            if (!EnableInteractables) return;
            SS2Log.Info($"Initializing Interactables.");
            GetInteractableBases();
        }

        protected override IEnumerable<SS2Interactable> GetInteractableBases()
        {
            //string niceName = MSUtil.NicifyString(item.GetType().Name);

            base.GetInteractableBases()
                .Where(interactable =>
                {
                    if (!EnableInteractables)
                    {
                        return false;
                    }

                    return SS2Config.MakeConfigurableBool(true, (b) =>
                    {
                        b.Section = "Interactables";
                        b.Key = MSUtil.NicifyString(interactable.Interactable.name);
                        b.Description = "Enable/Disable this Interactable";
                        b.ConfigFile = SS2Config.ConfigMain;
                        b.CheckBoxConfig = new CheckBoxConfig
                        {
                            checkIfDisabled = () => !EnableInteractables,
                            restartRequired = true
                        };
                    }).DoConfigure();
                })
                .ToList()
                .ForEach(interactable => AddInteractable(interactable));
            return null;
        }
    }
}