using LogicSpawn.RPGMaker.API;
using LogicSpawn.RPGMaker.Core;
using LogicSpawn.RPGMaker.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Testing
{
    public class LootItemMono : MonoBehaviour
    {
        public LootItem LootItem;
        public GameObject Canvas;
        public LootItemModel Model;
        public Text ItemText;

        void Awake()
        {
            Canvas.GetComponent<Canvas>().worldCamera = GetObject.RPGCamera.GetComponent<Camera>();
            Model = Canvas.GetComponent<LootItemModel>();
        }
        public void Init()
        {
            Model.Item = LootItem.Item;
            SetLootText();
        }

        void SetLootText()
        {
            var lootText = LootItem.Gold != 0 ? LootItem.Gold + "g" : LootItem.Item.Name;
            if (LootItem.Item != null)
            {
                var stackable = LootItem.Item as IStackable;
                if (stackable != null && stackable.CurrentStacks > 0)
                {
                    lootText += " x" + stackable.CurrentStacks;
                }

                var color = RPG.Items.GetRarityColorById(LootItem.Item.RarityID);
                lootText = RPG.UI.FormatString(color, lootText);
            }

            ItemText.text = lootText;
        }

        public void Update()
        {
            SetLootText();
            if(LootItem == null)
            {
                Destroy(gameObject);
                return;
            }

            if (Canvas == null)
            {
                return;
            }

            Canvas.transform.rotation = GetObject.RPGCamera.transform.rotation;

        }

        public void Loot()
        {
            if(LootItem.Gold != 0)
            {
                GetObject.PlayerMono.Player.Inventory.AddGold(LootItem.Gold);
                Debug.Log("You looted: " + LootItem.Gold + " gold.");
                Destroy(gameObject);
            }
            else
            {
                var looted = GetObject.PlayerMono.Player.Inventory.AddItem(LootItem.Item);

                if (looted)
                {
                    Debug.Log("You looted: " + LootItem.Item.Name);
                    Destroy(gameObject);
                }

            }
            TooltipUI.Clear();
        }

        
    }
}