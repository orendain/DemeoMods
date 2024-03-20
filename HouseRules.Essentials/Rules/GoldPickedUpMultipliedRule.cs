namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.Data;
    using Boardgame.SerializableEvents;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;
    using System;
    using System.Collections.Generic;
    using Utils;       

    public sealed class GoldPickedUpMultipliedRule : Rule, IConfigWritable<float>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "The amount of gold picked up per pile is adjusted";

        private static float _globalMultiplier;
        private static bool _isActivated;

        private readonly float _multiplier;

        public GoldPickedUpMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public float GetConfigObject() => _multiplier;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalMultiplier = _multiplier;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Constructor(
                    typeof(SerializableEventPickup),
                    new[] { typeof(int), typeof(IntPoint2D), typeof(bool) }),
                postfix: new HarmonyMethod(
                    typeof(GoldPickedUpMultipliedRule),
                    nameof(SerializableEventPickup_Constructor_Postfix)));
        }

        private static void SerializableEventPickup_Constructor_Postfix(ref SerializableEventPickup __instance)
        {
            if (!_isActivated)
            {
                return;
            }

            var ruleSet = HR.SelectedRuleset.Name;
            //if (ruleSet.Contains("(LEGENDARY") || ruleSet.Contains("(HARD") || ruleSet.Contains("PROGRESSIVE") || ruleSet.Contains("fastforward") )
            {
                List<int> numbers = new List<int>() { 51, 50 };
                Random rnd = new Random();
                int randIndex = rnd.Next(numbers.Count);
                int randomGold = numbers[randIndex];

                __instance.goldAmount = randomGold;
                //__instance.goldAmount = 69;
            }
            //else
            //{
            //    __instance.goldAmount = (int)(__instance.goldAmount * _globalMultiplier);
            //}            
        }
    }
}
