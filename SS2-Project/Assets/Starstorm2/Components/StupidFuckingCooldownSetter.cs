﻿using UnityEngine;
using RoR2;
namespace SS2.Components
{

    // doing this after CharacterBody.Start is too early because GenericSkill sets the stock to max in its Start
    //Please, i know youre mad, but dont use weird names like this where we dont even know what its for. -N
    public class StupidFuckingCooldownSetter : MonoBehaviour
    {
        public int primaryStock;
        public float primaryStopwatch;

        public int secondaryStock;
        public float secondaryStopwatch;

        public int utilityStock;
        public float utilityStopwatch;

        public int specialStock;
        public float specialStopwatch;

        private void Start()
        {
            CharacterBody body = base.GetComponent<CharacterBody>();
            if(!body)
            {
                SS2Log.Error("r u fucking stupid?");
                Destroy(this);
                return;
            }
            body.skillLocator.primary.stock = primaryStock;
            body.skillLocator.primary.rechargeStopwatch = primaryStopwatch;
            body.skillLocator.secondary.stock = secondaryStock;
            body.skillLocator.secondary.rechargeStopwatch = secondaryStopwatch;
            body.skillLocator.utility.stock = utilityStock;
            body.skillLocator.utility.rechargeStopwatch = utilityStopwatch;
            body.skillLocator.special.stock = specialStock;
            body.skillLocator.special.rechargeStopwatch = specialStopwatch;
        }
    }
}
