using LogicSpawn.RPGMaker.Beta;
using LogicSpawn.RPGMaker.Generic;
using UnityEngine;

namespace LogicSpawn.RPGMaker.Core
{
    public class PlayerMono : BaseCharacterMono
    {
        public PlayerCharacter Player;

        private PlayerSave _playerSave;

        public PlayerSave PlayerSave
        {
            get { return _playerSave; }
            set { _playerSave = value; }
        }

        protected override void DoUpdate()
        {

        }

        protected override void DoStart()
        {
        }

        public void SetPlayerSave(PlayerSave loadedPlayer)
        {
            _playerSave = loadedPlayer;
            Player = _playerSave.Character;
            Player.VitalHandler = new VitalHandler(Player);
            Player.Equipment.Player = Player;
            Player.Inventory.Player = Player;
            Player.CharacterMono = this;
            Character = Player;
            Controller.SetPlayerControl(gameObject);
            Controller.IsPlayerCharacter = true;
            Initialised = true;

            Player.Equipment.UpdateDynamicItems();
            RefreshPrefabs();
        }
    }
}