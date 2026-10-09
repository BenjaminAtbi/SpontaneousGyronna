using Kingmaker;
using Kingmaker.PubSubSystem;
using Kingmaker.View;
using Kingmaker.Visual;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.Overtip;
using Kingmaker.UI.Tooltip;
using Kingmaker.Settings;
using HarmonyLib;
using UnityEngine;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using System;
using System.Linq;
using Kingmaker.Blueprints.Classes.Prerequisites;

namespace SpontaneousGyronna
{
    [HarmonyPatch(typeof(BlueprintsCache))]
    static class BlueprintsCache_Patches
    {
        static bool loaded = false;

        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
        static void Postfix()
        {
            if (loaded) return;
            loaded = true;

            try
            {
                // Hag of Gyronna is spontaneous caster
                var DarkSisterArchetype = ResourcesLibrary.TryGetBlueprint<BlueprintArchetype>("411fa458481e44f0855d47a19358874b");
                DarkSisterArchetype.m_ReplaceSpellbook = new BlueprintSpellbookReference() { deserializedGuid = BlueprintGuid.Parse("fb4e216b322d3924e9c7041016b46999") };
            }
            catch (Exception e)
            {
                Main.DebugError(e);
            }


            try
            {
                // Paladins don't have to be lawful good
                var PaladinClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("bfa11238e7ae3544bbeb4d0b92e897ec");

                PaladinClass.m_Overrides = PaladinClass.m_Overrides.Where(o => !o.Contains("PrerequisiteAlignment")).ToList();

                PaladinClass.ComponentsArray = PaladinClass.ComponentsArray
                    .Where(c => !(c is PrerequisiteAlignment) && !(c is PrerequisiteFeaturesFromList) && !(c is DeityDependencyClass) ).ToArray();
            }
            catch (Exception e) 
            {
                Main.DebugError(e);
            }

}
    }
}
