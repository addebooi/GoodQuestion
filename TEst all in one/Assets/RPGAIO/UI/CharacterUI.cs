using System;
using System.Collections.Generic;
using System.Linq;
using LogicSpawn.RPGMaker;
using LogicSpawn.RPGMaker.API;
using LogicSpawn.RPGMaker.Core;
using LogicSpawn.RPGMaker.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public static CharacterUI Instance;
    public List<CharacterItemModel> EquipmentSlots;
    public Text CharacterInfoBox;
    public bool Show;

    private EventSystem EventSystem
    {
        get { return UIHandler.Instance.EventSystem; }
    }

	// Use this for initialization
	void Awake () {
	    Instance = this;
        RPG.Events.EquippedItem += UpdateFromEquip;
        RPG.Events.UnEquippedItem += UpdateFromUnequip;
	}

    void Update()
    {
        var charText = "";

        var seperator = "\n\n";
        var Player = GetObject.PlayerCharacter;
        var damageString = "<size=20><b>DAMAGE</b></size>" + seperator;
        damageString += Player.DamageDealable.ToString();
        damageString += "\n\n";

        var vitalsString = "<size=20><b>VITALS</b></size>" + seperator;
        foreach (var vit in Player.Vitals)
        {
            var vitalName = RPG.Stats.GetVitalName(vit.ID);
            vitalsString += string.Format("<b>{0}</b> |  {1} / {2} ", vitalName, vit.CurrentValue, vit.MaxValue) + "\n";
        }
        vitalsString += "\n";

        var attributesString = "<size=20><b>ATTRIBUTES</b></size>" + seperator;
        foreach (var attr in Player.Attributes)
        {
            var attrName = RPG.Stats.GetAttributeName(attr.ID);
            attributesString += string.Format("<b>{0}</b> |  {1} ", attrName, attr.TotalValue) + "\n";
        }
        attributesString += "\n";

        var statsString = "<size=20><b>STATISTICS</b></size>" + seperator;
        foreach (var stat in Player.Stats)
        {
            var statName = RPG.Stats.GetStatisticName(stat.ID);
            statsString += string.Format("<b>{0}</b> |  {1} ", statName, stat.TotalValue) + "\n";
        }
        statsString += "\n";

        var traitsString = "<size=20><b>TRAITS</b></size>" + seperator;
        foreach (var trait in Player.Traits)
        {
            var traitName = RPG.Stats.GetTraitName(trait.ID);
            traitsString += string.Format("<b>{0}</b> |  Level {1} [{2} / {3}]", traitName, trait.Level, trait.Exp, trait.ExpToLevel) + "\n";
        }
        traitsString += "\n";


        var repString = "<size=20><b>REPUTATIONS</b></size>" + seperator;
        foreach (var rep in GetObject.PlayerSave.QuestLog.AllReputations)
        {
            var repName = RPG.Stats.GetReputationName(rep.ReputationID);
            if (repName == "NonPlayerCharacters" || repName == "EnemyCharacters") continue; 

            repString += repName + " | " + rep.Value + "\n";
        }
        repString += "\n";

        charText = damageString + vitalsString + attributesString + statsString + traitsString +
            repString;
        CharacterInfoBox.text = charText;
    }

    private void UpdateFromUnequip(object sender, RPGEvents.UnEquippedItemEventArgs e)  
    {
        var item = e.Item;
        var slot = EquipmentSlots.FirstOrDefault(i => i.ItemID == item.ID);
        if(slot != null)
        {
            slot.EmptyThisSlot();
        }
    }

    private void UpdateFromEquip(object sender, RPGEvents.EquippedItemEventArgs e)
    {
        UpdateEquippedItems();
    }


    public void ToggleCharacterSheet()
    {
        Show = !Show;
        if(Show)
        {
            UpdateEquippedItems();
        }
    }

    public void CloseCharacterSheet()
    {
        Show = false;
    }

    public void UpdateEquippedItems()
    {
        if (!Show) return;

        var player = GetObject.PlayerCharacter;

        foreach(var slot in EquipmentSlots)
        {
            slot.EmptyThisSlot();
        }

        foreach(var equippedItem in player.Equipment.EquippedItems.Where(i => i.Item != null))
        {
            var item = equippedItem.Item;
            if (item.ItemType == ItemType.Apparel)
            {
                var apparel = (Apparel)item;
                if (apparel.ApparelSlotName == "OffHand")
                {
                    var slot = EquipmentSlots.First(eq => eq.SlotName == "OffHand");
                    slot.Init(item);
                }
                else
                {
                    var slot = EquipmentSlots.First(eq => eq.SlotName == apparel.ApparelSlotName);
                    slot.Init(item);
                }
            }
            else
            {
                if(equippedItem.SlotName == "Weapon")
                {
                    var slot = EquipmentSlots.First(eq => eq.SlotName == "Weapon");
                    slot.Init(item);
                }
                else
                {
                    var slot = EquipmentSlots.First(eq => eq.SlotName == "OffHand");
                    slot.Init(item);
                }
            }
        }
    }
}
