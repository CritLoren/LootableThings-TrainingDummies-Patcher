using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using System.Threading.Tasks;
using Mutagen.Bethesda.FormKeys.SkyrimSE;
using Activator = Mutagen.Bethesda.Skyrim.Activator;

namespace DestructibleSkyrimPatcher
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "DestructibleSkyrimPatcher.esp")
                .Run(args);
        }
        private static ModKey DestructibleSkyrim => ModKey.FromNameAndExtension("Destructible Skyrim.esp");
        private static FormLink<IActivatorGetter> ConstructDestructibleActivator(uint id) => new FormLink<IActivatorGetter>(DestructibleSkyrim.MakeFormKey(id));
        private static FormLink<IMoveableStaticGetter> ConstructDestructibleMoveableStatic(uint id) => new FormLink<IMoveableStaticGetter>(DestructibleSkyrim.MakeFormKey(id));

        private static readonly Dictionary<FormKey, IFormLinkGetter<IActivatorGetter>> OriginalDestructibleActivatorPair = new()
        {
            { Skyrim.Static.CommonChair01Static.FormKey, ConstructDestructibleActivator(0x002326) },
            { Skyrim.Static.CommonChair02Static.FormKey, ConstructDestructibleActivator(0x002327) },
            { Skyrim.Static.RuinsPot01Static.FormKey, ConstructDestructibleActivator(0x006F8B) },
            { Skyrim.Static.RuinsPot02Static.FormKey, ConstructDestructibleActivator(0x006F8C) },
            { Skyrim.Static.RuinsPot03Static.FormKey, ConstructDestructibleActivator(0x002328) },
            { Skyrim.Static.RuinsPot04Static.FormKey, ConstructDestructibleActivator(0x002329) },
            { Skyrim.Static.RuinsPot05Static.FormKey, ConstructDestructibleActivator(0x00232A) },
            { Skyrim.Static.RuinsPot06Static.FormKey, ConstructDestructibleActivator(0x00232B) },
            { Skyrim.Static.Barrel02Static.FormKey, ConstructDestructibleActivator(0x00232C) },
            { Skyrim.Static.WoodenBarStool01Static.FormKey, ConstructDestructibleActivator(0x002891) },
            { Skyrim.Static.CommonShelf01.FormKey, ConstructDestructibleActivator(0x003E23) },
            { Skyrim.Static.CommonTable01.FormKey, ConstructDestructibleActivator(0x006456) },
            { Skyrim.Static.CommonTableRound01.FormKey, ConstructDestructibleActivator(0x0038B8) },
            { Skyrim.Static.CommonTableSquare01.FormKey, ConstructDestructibleActivator(0x0038BA) },
            { Skyrim.Static.UpperChairStatic.FormKey, ConstructDestructibleActivator(0x004386) },
            { Skyrim.Static.NobleChair01Static.FormKey, ConstructDestructibleActivator(0x0048E9) },
            { Skyrim.Static.NobleChair02Static.FormKey, ConstructDestructibleActivator(0x004E4C) },
            { Skyrim.Static.WoodenChair01Static.FormKey, ConstructDestructibleActivator(0x0053B0) },
            { Skyrim.Static.OrcChair01Static.FormKey, ConstructDestructibleActivator(0x0053B1) },
            { Skyrim.Static.UpperNightstand01.FormKey, ConstructDestructibleActivator(0x005914) },
            { Skyrim.Static.UpperTableRound01.FormKey, ConstructDestructibleActivator(0x005915) },
            { Skyrim.Static.UpperTableSquare01.FormKey, ConstructDestructibleActivator(0x005916) },
            { Skyrim.Static.UpperTable01.FormKey, ConstructDestructibleActivator(0x005E79) },


        };
        private static readonly Dictionary<FormKey, IFormLinkGetter<IMoveableStaticGetter>> OriginalDestructibleMoveableStaticPair = new()
        {
            { Skyrim.Static.Lantern.FormKey, ConstructDestructibleMoveableStatic(0x005E81)},
            //{ Skyrim.MiscItem.Lantern01.FormKey, ConstructDestructibleMoveableStatic(0x005E81) }, - ITM? I suspect the author was running a bad script that was checking by name or something, since all these miscellaneous items are in Destructible Skyrim.esp but they didn't get swapped to the moveable static.
            { Skyrim.Static.CandleLanternwithCandle01.FormKey, ConstructDestructibleMoveableStatic(0x005E80)},
            { Skyrim.Static.CandleHornTable01.FormKey, ConstructDestructibleMoveableStatic(0x0038C0) },
            { Skyrim.Static.SilverCandleStick01.FormKey, ConstructDestructibleMoveableStatic(0x005E7D)},
            { Skyrim.Static.SilverCandleStick02.FormKey, ConstructDestructibleMoveableStatic(0x005E7E)},
            { Skyrim.Static.SilverCandleStick03.FormKey, ConstructDestructibleMoveableStatic(0x005E7F)},
            { Skyrim.Static.GlazedCandlesStatic.FormKey, ConstructDestructibleMoveableStatic(0x0038BE) },
            { Skyrim.Static.GlazedCandlesStatic02.FormKey, ConstructDestructibleMoveableStatic(0x0038BD) },
        };
        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            if (!state.LoadOrder.ContainsKey(DestructibleSkyrim))
                throw new Exception("ERROR: Destructible Skyrim not detected in load order. You need to install Destructible Skyrim prior to running this patcher!");

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
