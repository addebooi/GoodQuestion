using System;
using UnityEngine;

public class TargetLockPrefab : MonoBehaviour
{
    public GameObject SelectedGameObject;
    public GameObject InCombatGameObject;

    public void Set(TargetLockState state)
    {
        switch (state)
        {
            case TargetLockState.Unselected:
                if(SelectedGameObject != null) SelectedGameObject.SetActive(false);
                if(InCombatGameObject != null) InCombatGameObject.SetActive(false);
                break;
            case TargetLockState.Selected:
                if(SelectedGameObject != null) SelectedGameObject.SetActive(true);
                if(InCombatGameObject != null) InCombatGameObject.SetActive(false);
                break;
            case TargetLockState.InCombat:
                if(SelectedGameObject != null) SelectedGameObject.SetActive(false);
                if(InCombatGameObject != null) InCombatGameObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException("state");
        }
    }
}