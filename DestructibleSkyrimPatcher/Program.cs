using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using System.Threading.Tasks;
using Mutagen.Bethesda.FormKeys.SkyrimSE;
using Activator = Mutagen.Bethesda.Skyrim.Activator;

namespace LootableThingsPatcher
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "LootableThingsPatcher.esp")
                .Run(args);
        }
        private static ModKey LootableThings => ModKey.FromNameAndExtension("MisterBs Lootable Things.esp");
        private static FormLink<IActivatorGetter> ConstructLootableContainer(uint id) => new FormLink<IActivatorGetter>(LootableThings.MakeFormKey(id));

        private static readonly Dictionary<FormKey, IFormLinkGetter<IActivatorGetter>> OriginalLootableContainerPair = new()
        {
            { Skyrim.Static.Barrel02Static.FormKey, ConstructLootableContainer(0x35D827) },
            { Skyrim.Static.Barrel02Static_HeavySN.FormKey, ConstructLootableContainer(0x3A983D) },
            { Dragonborn.Static.Barrel02Static_LightSN.FormKey, ConstructLootableContainer(0x3A983E) },
            { Skyrim.Static.CrateSmall01.FormKey, ConstructLootableContainer(0x0012D0) },
            { Skyrim.Static.CrateSmall01EECo.FormKey, ConstructLootableContainer(0x3442B0) },
            { Skyrim.Static.CrateSmall01Weathered.FormKey, ConstructLootableContainer(0x0043C4) },
            { Skyrim.Static.CrateSmall01Weathered_LightSN.FormKey, ConstructLootableContainer(0x3A9844) },
            { Skyrim.Static.CrateSmall01_LightSN.FormKey, ConstructLootableContainer(0x3A9845) },
            { Skyrim.Static.CrateSmall02.FormKey, ConstructLootableContainer(0x0069C1) },
            { Skyrim.Static.CrateSmall02Weathered.FormKey, ConstructLootableContainer(0x0069BF) },
            { Skyrim.Static.CrateSmall02_LightSN.FormKey, ConstructLootableContainer(0x3A9846) },
            { Skyrim.Static.CrateSmall03.FormKey, ConstructLootableContainer(0x0074CE) },
            { Skyrim.Static.CrateSmall03EECo.FormKey, ConstructLootableContainer(0x3442B1) },
            { Skyrim.Static.CrateSmall03Weathered.FormKey, ConstructLootableContainer(0x0012C5) },
            { Skyrim.Static.CrateSmall03WeatheredLight_SN.FormKey, ConstructLootableContainer(0x3A9847) },
            { Skyrim.Static.CrateSmall03WeatheredSnow.FormKey, ConstructLootableContainer(0x3AE955) },
            { Skyrim.Static.CrateSmall04.FormKey, ConstructLootableContainer(0x004961) },
            { Skyrim.Static.CrateSmall04Weathered.FormKey, ConstructLootableContainer(0x006F6B) },
            { Skyrim.Static.CrateSmall04WeatheredLight_SN.FormKey, ConstructLootableContainer(0x3A9849) },
            { Skyrim.Static.CrateSmallLong01.FormKey, ConstructLootableContainer(0x003E3C) },
            { Skyrim.Static.CrateSmallLong01EECo.FormKey, ConstructLootableContainer(0x3442B2) },
            { Skyrim.Static.CrateSmallLong01Weathered.FormKey, ConstructLootableContainer(0x007A4F) },
            { Skyrim.Static.CrateSmallLong01WeatheredLight_SN.FormKey, ConstructLootableContainer(0x3A984A) },
            { Skyrim.Static.CrateSmallLong01WeatheredSnow.FormKey, ConstructLootableContainer(0x3A984B) },
            { Skyrim.Static.CrateSmallLong02.FormKey, ConstructLootableContainer(0x004930) },
            { Skyrim.Static.CrateSmallLong02Weathered.FormKey, ConstructLootableContainer(0x008FE3) },
            { Skyrim.Static.CrateSmallLong02WeatheredLight_SN.FormKey, ConstructLootableContainer(0x3A984C) },
            { Skyrim.Static.CrateSmallLong03.FormKey, ConstructLootableContainer(0x003DFE) },
            { Skyrim.Static.CrateSmallLong03Weathered.FormKey, ConstructLootableContainer(0x008A7C) },
            { Dragonborn.Static.CrateSmallLong03WeatheredLightSN.FormKey, ConstructLootableContainer(0x3A984D) },
            { Skyrim.Static.CrateSmallLong03WeatheredSnow.FormKey, ConstructLootableContainer(0x3A9848) },
            { Skyrim.Static.CrateSmallLong04.FormKey, ConstructLootableContainer(0x008FE4) },
            { Skyrim.Static.CrateSmallLong04EECo.FormKey, ConstructLootableContainer(0x3442B3) },
            { Skyrim.Static.CrateSmallLong04Weathered.FormKey, ConstructLootableContainer(0x008FE5) },
            { Skyrim.Static.FirewoodPileSmall01.FormKey, ConstructLootableContainer(0x3209F2) },
            { Skyrim.Static.FirewoodPileSmall01_LightSN.FormKey, ConstructLootableContainer(0x3A9842) },
            { Skyrim.Static.FirewoodPileMedium01.FormKey, ConstructLootableContainer(0x3493E9) },
            { Skyrim.Static.FirewoodPileMedium01_LightSN.FormKey, ConstructLootableContainer(0x3A9841) },
            { Skyrim.Static.FirewoodPileLarge01.FormKey, ConstructLootableContainer(0x3209F3) },
            { Skyrim.Static.FirewoodPileLarge01_LightSN.FormKey, ConstructLootableContainer(0x3A9840) },
            { Skyrim.Static.FirewoodPileHuge1.FormKey, ConstructLootableContainer(0x3493E8) },
            { Skyrim.Static.FirewoodPileHuge1_LightSN.FormKey, ConstructLootableContainer(0x3A9843) },
            { Skyrim.Static.HagravenHangingBall01.FormKey, ConstructLootableContainer(0x32AC7D) },
            { Skyrim.Static.HagravenHangingBall02.FormKey, ConstructLootableContainer(0x32AC7E) },
            { Skyrim.Static.BoneMammothSkull1.FormKey, ConstructLootableContainer(0x39029D) },
            { Skyrim.Static.BoneMammothSkull1Snow.FormKey, ConstructLootableContainer(0x3A984E) },
            { Skyrim.Static.BoneMammothSkull1Sulfur.FormKey, ConstructLootableContainer(0x3A984F) },
            { Skyrim.Static.BoneMammothSkull2.FormKey, ConstructLootableContainer(0x39029E) },
            { Skyrim.Static.BoneMammothSkull2Snow.FormKey, ConstructLootableContainer(0x3A9850) },
            { Skyrim.Static.BoneMammothSkull3.FormKey, ConstructLootableContainer(0x39029F) },
            { Skyrim.Static.BoneMammothSkull3Snow.FormKey, ConstructLootableContainer(0x3A9851) },
            { Skyrim.Static.BoneMammothSkull3Sulfur.FormKey, ConstructLootableContainer(0x3A9852) },
            { Skyrim.Static.BoneMammothSkull4.FormKey, ConstructLootableContainer(0x3A983F) },
            { Skyrim.Static.BoneMammothSkull4Snow.FormKey, ConstructLootableContainer(0x3A9853) },
            { Skyrim.Static.BoneMammothSkull4Sulfur.FormKey, ConstructLootableContainer(0x3A9854) },
            // { Skyrim.Static.MeadBarrel_Honningbrew.FormKey, ConstructLootableContainer(0x37BE95) },  // This is added in the mod, but it doesn't have a clear replaced object
            // { Skyrim.Static.MeadBarrel_Wine.FormKey, ConstructLootableContainer(0x37BE98) },         // This is added in the mod, but it doesn't have a clear replaced object
            { Skyrim.Static.MeadBarrel01.FormKey, ConstructLootableContainer(0x3B3A56) }


        };
        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            if (!state.LoadOrder.ContainsKey(LootableThings))
                throw new Exception("ERROR: MisterB's Lootable Things not detected in load order. You need to install it prior to running this patcher!");

            foreach (var placedObjectGetter in state.LoadOrder.PriorityOrder.PlacedObject().WinningContextOverrides(state.LinkCache))
            {
                if (OriginalLootableContainerPair.TryGetValue(placedObjectGetter.Record.Base.FormKey, out var LootableContainer))
                {
                    IPlacedObject copiedPlacedObject = placedObjectGetter.GetOrAddAsOverride(state.PatchMod);
                    copiedPlacedObject.Base.SetTo(LootableContainer);
                }
            }
        }
    }
}
