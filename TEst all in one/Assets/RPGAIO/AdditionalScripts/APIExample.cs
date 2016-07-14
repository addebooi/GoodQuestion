using System.Collections;
using LogicSpawn.RPGMaker.API;
using UnityEngine;

public class APIExample : MonoBehaviour {

    private bool ShowGUI;
    public Color alphaColor;
    private int ExpGained;
    private float yValue;

    void Start()
    {
        RPG.Events.GainedExp += GainedExp;
        RPG.Events.GainedExp += GainedExpAlt;    
    }

    private void GainedExpAlt(object sender, RPGEvents.GainedExpEventArgs e)
    {
        StartCoroutine("ShowExpGained",e.ExpGained);
    }

    void Update()
    {       
        if(ShowGUI)
        {
            alphaColor.a -= 1*Time.deltaTime;
            yValue -= 100.00f * Time.deltaTime;
        }
    }

    void GainedExp(object sender, RPGEvents.GainedExpEventArgs e)
    {
        Debug.Log(string.Format("Gained {0} Exp and Leveled is {1}", e.ExpGained, e.Leveled));
    }

    IEnumerator ShowExpGained(int Exp)
    {
        if(ShowGUI) StopCoroutine("ShowExpGained");

        ExpGained = Exp;
        ToggleGUI();
        yield return new WaitForSeconds(1);
        ToggleGUI();
    }

    private void ToggleGUI()
    {
        ShowGUI = !ShowGUI;
        if (ShowGUI)
        {
            
            alphaColor = new Color(GUI.color.a,GUI.color.g,GUI.color.b,255);
            yValue = Screen.height - 100;
        }
    }

    void OnGUI()
    {

        if(ShowGUI)
        {
            GUI.color = alphaColor;
            GUI.Label(new Rect(Screen.width/2 - 50, yValue, 100,20 ),"+" + ExpGained + " Exp!" );
        }
    }
}
