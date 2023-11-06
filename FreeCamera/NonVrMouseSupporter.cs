﻿namespace FreeCamera
{
    using Boardgame;
    using Boardgame.InputActions;
    using Boardgame.NonVR;
    using Boardgame.NonVR.CameraControl;
    using HarmonyLib;
    using UnityEngine;
    using UnityEngine.InputSystem;

    internal static class NonVrMouseSupporter
    {
        private const float TiltSpeed = 15f;

        private static InputAction _tiltMouseAction;
        private static float _rotateCache;
        private static GameContext _gameContext;
        private static ContinuousInputAction<float> _tiltContinuousInputAction;

        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(NonVrMouseSupporter), nameof(GameStartup_InitializeGame_Postfix)));

            harmony.Patch(
                original: typeof(UnityInputProvider).GetMethod("Awake"),
                postfix: new HarmonyMethod(typeof(NonVrMouseSupporter), nameof(UnityInputProvider_Awake_Postfix)));

            harmony.Patch(
                original: typeof(UnityInputProvider).GetMethod("OnEnable"),
                prefix: new HarmonyMethod(typeof(NonVrMouseSupporter), nameof(UnityInputProvider_OnEnable_Prefix)));

            harmony.Patch(
                original: typeof(UnityInputProvider).GetMethod("OnEnable"),
                postfix: new HarmonyMethod(typeof(NonVrMouseSupporter), nameof(UnityInputProvider_OnEnable_Postfix)));

            harmony.Patch(
                original: typeof(UnityInputProvider).GetMethod("Update"),
                postfix: new HarmonyMethod(typeof(NonVrMouseSupporter), nameof(UnityInputProvider_Update_Postfix)));

            harmony.Patch(
                original: typeof(CameraBehaviorFocus).GetMethod("Activate"),
                postfix: new HarmonyMethod(typeof(NonVrMouseSupporter), nameof(CameraBehaviorFocus_Activate_Postfix)));

            harmony.Patch(
                original: typeof(CameraBehaviorFocus).GetMethod("Initialize"),
                postfix: new HarmonyMethod(typeof(NonVrMouseSupporter), nameof(CameraBehaviorFocus_Initialize_Postfix)));
        }

        private static void GameStartup_InitializeGame_Postfix(GameStartup __instance)
        {
            _gameContext = Traverse.Create(__instance).Field<GameContext>("gameContext").Value;
        }

        private static void UnityInputProvider_Awake_Postfix()
        {
            _tiltMouseAction = new InputAction("TiltMouse", binding: "<Mouse>/delta/y");
        }

        private static void UnityInputProvider_OnEnable_Prefix(UnityInputProvider __instance)
        {
            var enableRotateFocusCameraMouseAction = Traverse.Create(__instance).
                Field<InputAction>("enableRotateFocusCameraMouseAction").
                Value;
            var isRotatingCameraTraverse = Traverse.Create(__instance).Field<bool>("isRotatingCamera");

            enableRotateFocusCameraMouseAction.performed += _ =>
            {
                ShowCursor(false);
                _tiltMouseAction.Enable();
                isRotatingCameraTraverse.Value = true;
            };

            enableRotateFocusCameraMouseAction.canceled += _ =>
            {
                isRotatingCameraTraverse.Value = false;
                ShowCursor(ShouldShowCursor());
                _tiltMouseAction.Disable();
            };

            _tiltMouseAction.Disable();
            return;

            bool ShouldShowCursor()
            {
                var isPanningCamera = Traverse.Create(__instance).
                    Field<bool>("isPanningCamera").
                    Value;
                if (!isPanningCamera)
                {
                    return !isRotatingCameraTraverse.Value;
                }

                return false;
            }

            static void ShowCursor(bool show)
            {
                PointerController.Visible = show;
                PointerController.Locked = !show;
            }
        }

        private static void UnityInputProvider_OnEnable_Postfix()
        {
            _tiltContinuousInputAction = default;
            _tiltContinuousInputAction.Start();
        }

        private static void UnityInputProvider_Update_Postfix()
        {
            var interpolation = Time.deltaTime * TiltSpeed;
            _rotateCache = Mathf.Lerp(_rotateCache, _tiltMouseAction.ReadValue<float>(), interpolation);
            _tiltContinuousInputAction.Update(_rotateCache);
        }

        private static void CameraBehaviorFocus_Activate_Postfix(bool activate)
        {
            if (activate)
            {
                _tiltContinuousInputAction.updated += UpdateTiltInput;
            }
            else
            {
                _tiltContinuousInputAction.updated -= UpdateTiltInput;
            }
        }

        private static void CameraBehaviorFocus_Initialize_Postfix(CameraBehaviorFocus __instance)
        {
            var settings = Traverse.Create(__instance).Field<CameraControlSettings>("settings").Value;
            settings.allowManualPitchChange = true;
            settings.tiltVerticalFactor = settings.aroundRotationSpeedForMouse;
        }

        private static void UpdateTiltInput(float value)
        {
            var cameraBehaviorFocus = _gameContext.nonVr.cameraBehaviour.cameraBehaviorFocus;
            Traverse.Create(cameraBehaviorFocus).Method("UpdateTiltInput", value).GetValue();
        }
    }
}
