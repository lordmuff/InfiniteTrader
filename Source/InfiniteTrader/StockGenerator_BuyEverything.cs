using System.Collections.Generic;
using RimWorld;
using Verse;

namespace InfiniteTrader;

public class StockGenerator_BuyEverything : StockGenerator
{
    public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
    {
        yield break;
    }

    public override bool HandlesThingDef(ThingDef thingDef)
    {
        return true;
    }
}