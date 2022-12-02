using System.Linq;
using RimWorld;
using Verse;

namespace InfiniteTrader;

public class InfiniteTraderManager : GameComponent
{
    private TradeShip infiniteTrader;
    private int tickToRestock;

    public InfiniteTraderManager()
    {
    }

    public InfiniteTraderManager(Game game)
    {
    }

    public override void StartedNewGame()
    {
        base.StartedNewGame();
        if (infiniteTrader is null)
        {
            SpawnTradeShip();
        }
    }

    public override void LoadedGame()
    {
        base.LoadedGame();
        if (infiniteTrader is null)
        {
            SpawnTradeShip();
        }
    }

    public override void GameComponentTick()
    {
        base.GameComponentTick();
        if (Find.TickManager.TicksGame > tickToRestock)
        {
            Stock();
        }
    }

    private void SpawnTradeShip()
    {
        infiniteTrader = new TradeShip(IT_DefOf.IT_Orbital_InfiniteTrader);
        foreach (var map in Find.Maps.Where(x => x.IsPlayerHome))
        {
            SendStandardLetter(infiniteTrader.def.LabelCap, "TraderArrival".Translate(infiniteTrader.name,
                    infiniteTrader.def.label,
                    infiniteTrader.Faction == null
                        ? "TraderArrivalNoFaction".Translate()
                        : "TraderArrivalFromFaction".Translate(infiniteTrader.Faction.Named("FACTION"))),
                LetterDefOf.PositiveEvent, LookTargets.Invalid);

            map.passingShipManager.AddShip(infiniteTrader);
        }

        infiniteTrader.ticksUntilDeparture = int.MaxValue;
        Stock();
    }

    private void Stock()
    {
        infiniteTrader.GetDirectlyHeldThings().ClearAndDestroyContents();
        infiniteTrader.GenerateThings();
        tickToRestock = Find.TickManager.TicksGame + InfiniteTraderMod.settings.ticksToNextRestock;
    }

    protected void SendStandardLetter(TaggedString baseLetterLabel, TaggedString baseLetterText,
        LetterDef baseLetterDef, LookTargets lookTargets, params NamedArgument[] textArgs)
    {
        if (baseLetterLabel.NullOrEmpty() || baseLetterText.NullOrEmpty())
        {
            Log.Error("Sending standard incident letter with no label or text.");
        }

        var taggedString = baseLetterText.Formatted(textArgs);
        TaggedString text;
        text = taggedString;

        var taggedString2 = baseLetterLabel.Formatted(textArgs);
        TaggedString label;
        label = taggedString2;

        var choiceLetter = LetterMaker.MakeLetter(label, text, baseLetterDef, lookTargets);
        Find.LetterStack.ReceiveLetter(choiceLetter);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_References.Look(ref infiniteTrader, "infiniteTrader");
        Scribe_Values.Look(ref tickToRestock, "tickToRestock");
    }
}