﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Executioner2
{
    public class ExecuteHold : BaseSkillState
    {
        public static float duration = 1f;

        public static GameObject jumpEffect;
        public static string ExhaustL;
        public static string ExhaustR;

        public static GameObject areaIndicator;

        [HideInInspector]
        public static GameObject areaIndicatorInstance;

        private CameraTargetParams.CameraParamsOverrideHandle camOverrideHandle;
        private CharacterCameraParamsData slamCameraParams = new CharacterCameraParamsData
        {
            maxPitch = 88f,
            minPitch = 45f,
            pivotVerticalOffset = 1f,
            idealLocalCameraPos = slamCameraPosition,
            wallCushion = 0.1f,
        };

        [HideInInspector]
        public static Vector3 slamCameraPosition = new Vector3(2.6f, -2.0f, -4f);

        public override void OnEnter()
        {
            base.OnEnter();

            PlayAnimation("FullBody, Override", "SpecialHang", "Special.playbackRate", duration);

            if (isAuthority)
            {
                EffectManager.SimpleMuzzleFlash(jumpEffect, gameObject, ExhaustL, false);
                EffectManager.SimpleMuzzleFlash(jumpEffect, gameObject, ExhaustR, false);

                CameraTargetParams.CameraParamsOverrideRequest request = new CameraTargetParams.CameraParamsOverrideRequest
                {
                    cameraParamsData = slamCameraParams,
                    priority = 0f
                };

                camOverrideHandle = cameraTargetParams.AddParamsOverride(request, 0f);

                areaIndicatorInstance = UnityEngine.Object.Instantiate(areaIndicator);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
                FixedUpdateAuthority();
        }

        public override void Update()
        {
            base.Update();
            UpdateAreaIndicator();
        }

        private void UpdateAreaIndicator()
        {
            if (areaIndicatorInstance)
            {
                float maxDistance = 256f;

                Ray aimRay = GetAimRay();
                RaycastHit raycastHit;
                if (Physics.Raycast(aimRay, out raycastHit, maxDistance, LayerIndex.CommonMasks.bullet))
                {
                    areaIndicatorInstance.transform.position = raycastHit.point;
                    areaIndicatorInstance.transform.up = raycastHit.normal;
                }
                else
                {
                    areaIndicatorInstance.transform.position = aimRay.GetPoint(maxDistance);
                    areaIndicatorInstance.transform.up = -aimRay.direction;
                }
            }
        }

        private void FixedUpdateAuthority()
        {
            if (fixedAge >= duration || !inputBank.skill4.down || inputBank.skill1.down)
            {
                ExecuteSlam nextState = new ExecuteSlam();
                outer.SetNextState(nextState);
            }  
            else
                HandleMovement();
        }

        public void HandleMovement()
        {
            characterMotor.velocity = Vector3.zero;
        }

        public override void OnExit()
        {
            base.OnExit();

            if (cameraTargetParams)
            {
                cameraTargetParams.RemoveParamsOverride(camOverrideHandle, 1f);
            }

            if (areaIndicatorInstance)
            {
                Destroy(areaIndicatorInstance.gameObject);
            }
        }
    }
}

