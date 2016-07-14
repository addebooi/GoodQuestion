using System;
using System.Linq;
using System.Reflection;
using LogicSpawn.RPGMaker.Core;
using LogicSpawn.RPGMaker.Generic;
using UnityEngine;

namespace LogicSpawn.RPGMaker.Core
{
    public class Rm_CustomEventMono: MonoBehaviour
    {
        //todo: custom inspector links to EventNodeBank events
        public string EventID; //Used to store in player info so event isn't used multiple times if Reusable = false
        public bool Reusable;
        public string EventToPerform;
        public InteractType InteractType;
        public float InteractionParameter;
        public string InteractionStringParameter;
        private Transform _myTransform;

        void Awake()
        {
            _myTransform = transform;
        }

        void OnMouseDown()
        {
            if(InteractType == InteractType.Click)
            {
                PerformEvent();
            }
        }

        void Update()
        {
            if(InteractType == InteractType.NearTo)
            {
                if(Vector3.Distance(_myTransform.position, GetObject.PlayerMono.transform.position) < InteractionParameter)
                {
                    PerformEvent();
                }
            }
            else if(InteractType == InteractType.Speak)
            {
                //todo: some code here that checks GUI if the npc is in convo mode, then
                var spokenTo = false;
                if(spokenTo)
                {
                    PerformEvent();
                }
                
            }
            else if(InteractType == InteractType.CompleteQuest)
            {
                if (GetObject.PlayerSave.QuestLog.AllObjectives.First(o => o.ID == InteractionStringParameter).TurnedIn)
                {
                    PerformEvent();    
                }
            }
        }

        void PerformEvent()
        {
            //todo: code here that run's event (Event.Evaluate())
        }
    }
}