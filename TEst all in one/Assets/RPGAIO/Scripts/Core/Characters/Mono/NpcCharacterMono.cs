using System.Linq;
using Assets.Scripts.Beta.NewImplementation;
using Assets.Scripts.Testing;
using LogicSpawn.RPGMaker.Beta;
using LogicSpawn.RPGMaker.Generic;
using UnityEngine;

namespace LogicSpawn.RPGMaker.Core
{
    public class NpcCharacterMono : BaseCharacterMono
    {
        public string NpcID = "";
        public NonPlayerCharacter NPC;
        protected override void DoStart()
        {

        }

        protected override void DoUpdate()
        {
            if (!Initialised) return;

            if (NPC == null)
            {
                Debug.LogError("Destroying NPC GameObject, Reason: Game data not found for : " + gameObject.name);
                Destroy(gameObject);
            }
        }

        protected void OnEnable()
        {
            if (Initialised) return;

            if (!string.IsNullOrEmpty(NpcID))
            {
                var getNpc = Rm_RPGHandler.Instance.Repositories.Interactable.AllNpcs.FirstOrDefault(i => i.ID == NpcID);
                if (getNpc != null)
                {
                    SetNPC(getNpc);
                }
                else
                {
                    Debug.LogError("Could not find NPC data for Spawned NPC: " + NpcID + ". Destroying.");
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogError("Could not find NPC data for Spawned NPC: " + NpcID + ". Destroying.");
                Destroy(gameObject);
            }
        }


        protected void OnDisable()
        {
            Initialised = false;
        }

        public void SetNPC(NonPlayerCharacter player)
        {
            player = GeneralMethods.CopyObject(player);
            NPC = player;
            NPC.CharacterMono = this;
            Controller.SpawnPosition = transform.position;
            Controller.State = RPGControllerState.Idle;
            NPC.VitalHandler = new VitalHandler(NPC);
            NPC.VitalHandler.Health.CurrentValue = NPC.VitalHandler.Health.MaxValue;
            Character = player;
            NPC.CCInit();
            Controller.SpawnPosition = transform.position;
            GetComponent<InteractableNPC>().ObjectID = NPC.ID;
            RefreshPrefabs();
            Initialised = true;
        }
    }
}