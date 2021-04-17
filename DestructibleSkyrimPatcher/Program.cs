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
        private static FormLink<IMoveableStaticGetter> ConstructNonLootable(uint id) => new FormLink<IMoveableStaticGetter>(LootableThings.MakeFormKey(id));

        private static readonly Dictionary<FormKey, IFormLinkGetter<IActivatorGetter>> OriginalDestructibleActivatorPair = new()
        {
            { Skyrim.Static.CommonChair01Static.FormKey, ConstructLootableContainer(0x002326) },
            { Skyrim.Static.CommonChair02Static.FormKey, ConstructLootableContainer(0x002327) },
            { Skyrim.Static.RuinsPot01Static.FormKey, ConstructLootableContainer(0x006F8B) },
            { Skyrim.Static.RuinsPot02Static.FormKey, ConstructLootableContainer(0x006F8C) },
            { Skyrim.Static.RuinsPot03Static.FormKey, ConstructLootableContainer(0x002328) },
            { Skyrim.Static.RuinsPot04Static.FormKey, ConstructLootableContainer(0x002329) },
            { Skyrim.Static.RuinsPot05Static.FormKey, ConstructLootableContainer(0x00232A) },
            { Skyrim.Static.RuinsPot06Static.FormKey, ConstructLootableContainer(0x00232B) },
            { Skyrim.Static.Barrel02Static.FormKey, ConstructLootableContainer(0x00232C) },
            { Skyrim.Static.WoodenBarStool01Static.FormKey, ConstructLootableContainer(0x002891) },
            { Skyrim.Static.CommonShelf01.FormKey, ConstructLootableContainer(0x003E23) },
            { Skyrim.Static.CommonTable01.FormKey, ConstructLootableContainer(0x006456) },
            { Skyrim.Static.CommonTableRound01.FormKey, ConstructLootableContainer(0x0038B8) },
            { Skyrim.Static.CommonTableSquare01.FormKey, ConstructLootableContainer(0x0038BA) },
            { Skyrim.Static.UpperChairStatic.FormKey, ConstructLootableContainer(0x004386) },
            { Skyrim.Static.NobleChair01Static.FormKey, ConstructLootableContainer(0x0048E9) },
            { Skyrim.Static.NobleChair02Static.FormKey, ConstructLootableContainer(0x004E4C) },
            { Skyrim.Static.WoodenChair01Static.FormKey, ConstructLootableContainer(0x0053B0) },
            { Skyrim.Static.OrcChair01Static.FormKey, ConstructLootableContainer(0x0053B1) },
            { Skyrim.Static.UpperNightstand01.FormKey, ConstructLootableContainer(0x005914) },
            { Skyrim.Static.UpperTableRound01.FormKey, ConstructLootableContainer(0x005915) },
            { Skyrim.Static.UpperTableSquare01.FormKey, ConstructLootableContainer(0x005916) },
            { Skyrim.Static.UpperTable01.FormKey, ConstructLootableContainer(0x005E79) },


        };
        private static readonly Dictionary<FormKey, IFormLinkGetter<IMoveableStaticGetter>> OriginalDestructibleMoveableStaticPair = new()
        {
            { Skyrim.Static.Lantern.FormKey, ConstructNonLootable(0x005E81)},
            //{ Skyrim.MiscItem.Lantern01.FormKey, ConstructNonLootable(0x005E81) }, - ITM? I suspect the author was running a bad script that was checking by name or something, since all these miscellaneous items are in Destructible Skyrim.esp but they didn't get swapped to the moveable static.
            { Skyrim.Static.CandleLanternwithCandle01.FormKey, ConstructNonLootable(0x005E80)},
            { Skyrim.Static.CandleHornTable01.FormKey, ConstructNonLootable(0x0038C0) },
            { Skyrim.Static.SilverCandleStick01.FormKey, ConstructNonLootable(0x005E7D)},
            { Skyrim.Static.SilverCandleStick02.FormKey, ConstructNonLootable(0x005E7E)},
            { Skyrim.Static.SilverCandleStick03.FormKey, ConstructNonLootable(0x005E7F)},
            { Skyrim.Static.GlazedCandlesStatic.FormKey, ConstructNonLootable(0x0038BE) },
            { Skyrim.Static.GlazedCandlesStatic02.FormKey, ConstructNonLootable(0x0038BD) },
        };
        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            state.LoadOrder.AsEnumerable().ToList().ForEach(plugin => Console.WriteLine(plugin.Key));
            if (!state.LoadOrder.ContainsKey(LootableThings))
                throw new Exception("ERROR: MisterB's Lootable Things not detected in load order. You need to install it prior to running this patcher!");

            foreach (var placedObjectGetter in state.LoadOrder.PriorityOrder.PlacedObject().WinningContextOverrides(state.LinkCache))
            {
                if (OriginalDestructibleActivatorPair.TryGetValue(placedObjectGetter.Record.Base.FormKey, out var destructibleActivator))
                {
                    IPlacedObject copiedPlacedObject = placedObjectGetter.GetOrAddAsOverride(state.PatchMod);
                    copiedPlacedObject.Base.SetTo(destructibleActivator);
                }
                else if (OriginalDestructibleMoveableStaticPair.TryGetValue(placedObjectGetter.Record.Base.FormKey, out var destructibleMoveableStatic))
                {
                    IPlacedObject copiedPlacedObject = placedObjectGetter.GetOrAddAsOverride(state.PatchMod);
                    copiedPlacedObject.Base.SetTo(destructibleMoveableStatic);
                }
            }
        }
    }
}
