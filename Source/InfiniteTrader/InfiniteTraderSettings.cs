using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace InfiniteTrader;

public class InfiniteTraderSettings : ModSettings
{
    public int animalCount = 10;

    private string buf;
    private string buf2;
    public int bulkAmount = 9999;
    public bool disableAnimals = true;
    public bool disableApparels = true;
    public bool disableBuildings = true;
    public bool disableImplants = true;
    public bool disablePawns = true;
    public bool disableWeapons = true;
    public int pawnCount = 20;
    public int ticksToNextRestock = 60000;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref disablePawns, "disablePawns", true);
        Scribe_Values.Look(ref pawnCount, "pawnCount", 20);
        Scribe_Values.Look(ref disableAnimals, "disableAnimals", true);
        Scribe_Values.Look(ref animalCount, "animalCount", 10);
        Scribe_Values.Look(ref disableBuildings, "disableBuildings", true);
        Scribe_Values.Look(ref disableWeapons, "disableWeapons", true);
        Scribe_Values.Look(ref disableApparels, "disableApparels", true);
        Scribe_Values.Look(ref disableImplants, "disableImplants", true);
        Scribe_Values.Look(ref ticksToNextRestock, "ticksToNextRestock", 60000);
        Scribe_Values.Look(ref bulkAmount, "bulkAmount", 9999);
    }

    public void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled("IT.disablePawns".Translate(), ref disablePawns);
        listingStandard.Label("IT.PawnAmount".Translate() + ": " + pawnCount.ToString());
        pawnCount = (int)listingStandard.Slider(pawnCount, 0, 200);
        listingStandard.CheckboxLabeled("IT.disableAnimals".Translate(), ref disableAnimals);
        listingStandard.Label("IT.AnimalAmount".Translate() + ": " + animalCount.ToString());
        animalCount = (int)listingStandard.Slider(animalCount, 0, 200);
        listingStandard.CheckboxLabeled("IT.disableBuildings".Translate(), ref disableBuildings);
        listingStandard.CheckboxLabeled("IT.disableWeapons".Translate(), ref disableWeapons);
        listingStandard.CheckboxLabeled("IT.disableApparels".Translate(), ref disableApparels);
        listingStandard.CheckboxLabeled("IT.disableImplants".Translate(), ref disableImplants);
        listingStandard.Label("IT.RestockInterval".Translate() + ": " + ticksToNextRestock.ToStringTicksToDays());
        ticksToNextRestock = (int)listingStandard.Slider(ticksToNextRestock, 60, GenDate.TicksPerDay * 60);
        var daysValue = ticksToNextRestock / (float)GenDate.TicksPerDay;
        if (ticksToNextRestock != daysValue * GenDate.TicksPerDay)
        {
            buf = (ticksToNextRestock / (float)GenDate.TicksPerDay).ToString();
        }

        listingStandard.TextFieldNumeric(ref daysValue, ref buf, 0, 10000);
        if (daysValue != ticksToNextRestock / GenDate.TicksPerDay)
        {
            ticksToNextRestock = (int)(daysValue * GenDate.TicksPerDay);
        }

        listingStandard.Label("IT.BulkAmount".Translate() + ": " + bulkAmount.ToString());
        bulkAmount = (int)listingStandard.Slider(bulkAmount, 0, 99999);
        if (!buf2.NullOrEmpty() && Convert.ToInt32(buf2) != bulkAmount)
        {
            buf2 = bulkAmount.ToString();
        }

        listingStandard.TextFieldNumeric(ref bulkAmount, ref buf2, 0, 99999);
        listingStandard.End();
        Write();
    }
}