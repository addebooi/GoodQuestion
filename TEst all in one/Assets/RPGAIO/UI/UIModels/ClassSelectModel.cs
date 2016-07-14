using LogicSpawn.RPGMaker.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClassSelectModel : MonoBehaviour, IPointerClickHandler
{
    public Text ButtonText;
    public Image ButtonImage;
    public string ClassID;

    public void OnPointerClick(PointerEventData eventData)
    {
        CharacterCreationMono.Instance.SetClass(ClassID);
    }
}