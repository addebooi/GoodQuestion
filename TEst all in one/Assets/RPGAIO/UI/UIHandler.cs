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

public class UIHandler : MonoBehaviour
{

    public static UIHandler Instance;
    public static bool MouseOnUI = true;
    public static bool ShowFPSCounter = true;
    public bool DebugShowFPS = true;

    public ShowFps FPSCounter;
    public AchievementUI AchievementsUI;
    public InventoryUI InventoryUI;
    public QuestTrackerUI QuestTrackerUI;
    public CharacterUI CharacterUI;
    public CraftingUI CraftingUI;
    public WorldMapUI WorldMapUI;
    public CoreUI CoreUI;
    public DialogUI DialogUI;
    public BookUI BookUI;
    public QuestLogUI QuestLogUI;
    public AbilityLogUI AbilityLogUI;
    public SkillBarUI SkillBarUI;
    public TooltipUI TooltipUI;
    public HarvestingUI HarvestingUI;
    public PopupUI PopupUI;
    public PauseMenuUI PauseMenuUI;
    public EventSystem EventSystem;

    void Awake()
    {
        Instance = this;
        PauseMenuUI.gameObject.SetActive(false);
        InventoryUI.Instance = InventoryUI;
        TooltipUI.Instance = TooltipUI;
    }

    void OnEnable()
    {
        RPG.Events.MenuOpened += ToggleMenu;
        RPG.Events.OpenCrafting += OpenCrafting;
        RPG.Events.OpenVendor += OpenVendor;
    }

    private void OpenCrafting(object sender, RPGEvents.OpenCraftingEventArgs e)
    {
        CraftingUI.Show = false;
        CraftingUI.ToggleCraftingUI();
    }
    
    private void OpenVendor(object sender, RPGEvents.OpenVendorEventArgs e)
    {
        //VendorUI.Show = false;
        //VendorUI.ToggleVendorUI();
    }

    void OnDisable()
    {
        RPG.Events.MenuOpened -= ToggleMenu;
        RPG.Events.OpenCrafting -= OpenCrafting;
        RPG.Events.OpenVendor -= OpenVendor;
    }

    public void Init()
    {
        CoreUI.Init();
        SkillBarUI.Init();
    }

    void Update()
    {

        MouseOnUI = EventSystem.current.IsPointerOverGameObject() || (SkillDragHandler.itemBeingDragged != null);
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameMaster.ShowUI = !GameMaster.ShowUI;
            if(GameMaster.ShowUI)
            {
                InventoryUI.UpdateItemContainer();
            }
        }

        if (RPG.Input.GetKeyDown(RPG.Input.WorldMap))
        {
            WorldMapUI.ToggleMap();
        }

        if (RPG.Input.GetKeyDown(RPG.Input.Inventory))
        {
            InventoryUI.ToggleInventory();
            TooltipUI.Clear();
        }

        if (RPG.Input.GetKeyDown(RPG.Input.CharacterSheet))
        {
            CharacterUI.ToggleCharacterSheet();
            TooltipUI.Clear();
        }
        if (RPG.Input.GetKeyDown(RPG.Input.Crafting))
        {
            CraftingUI.ToggleCraftingUI();
            TooltipUI.Clear();
        }
        if (RPG.Input.GetKeyDown(RPG.Input.QuestBook))
        {
            QuestLogUI.ToggleQuestLogUI();
            TooltipUI.Clear();
        }
        if (RPG.Input.GetKeyDown(RPG.Input.SkillBook))
        {
            AbilityLogUI.ToggleAbilityLogUI();
            TooltipUI.Clear();
        }

        FPSCounter.gameObject.SetActive(DebugShowFPS && ShowFPSCounter);

        AchievementsUI.gameObject.SetActive(GameMaster.ShowUI);
        InventoryUI.gameObject.SetActive(GameMaster.ShowUI && InventoryUI.Show);
        CharacterUI.gameObject.SetActive(GameMaster.ShowUI && CharacterUI.Show);
        CraftingUI.gameObject.SetActive(GameMaster.ShowUI && CraftingUI.Show);
        QuestLogUI.gameObject.SetActive(GameMaster.ShowUI && QuestLogUI.Show);
        AbilityLogUI.gameObject.SetActive(GameMaster.ShowUI && AbilityLogUI.Show);
        WorldMapUI.gameObject.SetActive(GameMaster.ShowUI);
        HarvestingUI.gameObject.SetActive(GameMaster.ShowUI);
        CoreUI.gameObject.SetActive(GameMaster.ShowUI);
        DialogUI.gameObject.SetActive(GameMaster.ShowUI);
        BookUI.gameObject.SetActive(GameMaster.ShowUI);
        QuestTrackerUI.gameObject.SetActive(GameMaster.ShowUI);
        SkillBarUI.gameObject.SetActive(GameMaster.ShowUI);
        PopupUI.gameObject.SetActive(GameMaster.ShowUI);
    }

    public void ToggleMenu(object sender, RPGEvents.OpenMenuEventArgs e)
    {
        if(!PauseMenuUI.Showing)
        {
            GameMaster.GamePaused = true;
            PauseMenuUI.gameObject.SetActive(true);
            PauseMenuUI.Showing = true;
        }
        else
        {
            if (PauseMenuUI.LoadMenu.activeInHierarchy || PauseMenuUI.OptionsMenu.activeInHierarchy || PauseMenuUI.ConfirmExit.activeInHierarchy)
            {
                PauseMenuUI.BackToDefaultMenu();
            }
            else
            {
                GameMaster.GamePaused = false;
                PauseMenuUI.gameObject.SetActive(false);
                PauseMenuUI.Showing = false;
            }
        }
    }
}
