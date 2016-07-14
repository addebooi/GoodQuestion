using UnityEngine;

using System.Collections;
using System;           //Acces enums


public class BaseCharacter : MonoBehaviour {
    private string _name;
    private int _level;
    private uint _experience;


	// Use this for initialization
    void Awake() {
        
        _name = string.Empty;
        _level = 0;
    }

	void Start () {
        
	
	}

    public string Name {
        get { return this._name; }
        set { this.name = value; }
    }

    public int Level {
        get { return this._level; }
        set { this._level = value; }
    }

    public uint Experience {
        get { return this._experience; }
        set { this._experience = value; }
    }

    public void addExperience(uint exp) {
        this._experience += exp;

        calulateLevel();
    }

    private void calulateLevel() {

    }


    // Update is called once per frame
    void Update () {

	
	}


}