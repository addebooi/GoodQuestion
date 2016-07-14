using System.Collections.Generic;
using LogicSpawn.RPGMaker.API;
using LogicSpawn.RPGMaker.Core;
using LogicSpawn.RPGMaker.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillBarUI : MonoBehaviour
{
    public static SkillBarUI Instance;
    public bool Show;
    public GameObject SkillButtonContainer;
    public GameObject SkillBarButtonPrefab;
    private List<SkillBarButtonModel> SkillButtons;
    private float timePassed;

    private EventSystem EventSystem
    {
        get { return UIHandler.Instance.EventSystem; }
    }

    private SkillBarSlot GetSlot(int i)
    {
        return RPG.GetPlayerCharacter.SkillHandler.Slots[i];
    }

	// Use this for initialization
	void Awake () {
	    Instance = this;
	}

    public void Init()
    {
        var skillSlots = RPG.GetPlayerCharacter.SkillHandler.Slots.Length;
        if(SkillButtonContainer != null && SkillButtonContainer.transform != null)
        {
            SkillButtonContainer.transform.DestroyChildren();
            SkillButtons = new List<SkillBarButtonModel>();
            for (int i = 0; i < skillSlots; i++)
            {
                var slot = GetSlot(i);
                var go = Instantiate(SkillBarButtonPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                go.transform.SetParent(SkillButtonContainer.transform, false);
                var itemModel = go.GetComponent<SkillBarButtonModel>();
                itemModel.SkillImage.sprite = slot.Image != null ? GeneralMethods.CreateSprite(slot.Image) : null;
                itemModel.SkillImage.color = itemModel.SkillImage.sprite == null ? Color.clear : Color.white;

                var labelNum = (i + 1);
                var label = labelNum.ToString();
                if (labelNum == 10)
                {
                    label = "0";
                }
                else if (labelNum == 11)
                {
                    label = "-";
                }
                else if (labelNum == 12)
                {
                    label = "+";
                }

                itemModel.SkillText.text = string.Format("[{0}]", label);
                itemModel.SkillSlot = i;

                SkillButtons.Add(itemModel);
            }
        }
        
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if(SkillButtons == null)
        {
            Init();
        }
        if(timePassed > 0.2f)
        {
            for (int i = 0; i < SkillButtons.Count; i++)
            {
                var slot = GetSlot(i);
                var itemModel = SkillButtons[i];
                itemModel.SkillImage.sprite = slot.Image != null ? GeneralMethods.CreateSprite(slot.Image) : null;
                itemModel.SkillImage.color = itemModel.SkillImage.sprite == null ? Color.clear : Color.white;

                var labelNum = (i + 1);
                var label = labelNum.ToString();
                if (labelNum == 10)
                {
                    label = "0";
                }
                else if (labelNum == 11)
                {
                    label = "-";
                }
                else if (labelNum == 12)
                {
                    label = "+";
                }

                itemModel.SkillText.text = string.Format("[{0}]", label);
                itemModel.SkillSlot = i;
            }
            timePassed = 0;
        }
        
    }
}
