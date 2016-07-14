using LogicSpawn.RPGMaker.Core;
using UnityEngine;

namespace LogicSpawn.RPGMaker.Core
{
    public class SavableGameObjectData
    {
        public string UniqueID;
        public string SceneName;

        public SerializableVector3 Position;
        public SerializableQuaternion Rotation;
        public bool Active;
        public bool Destroyed;

        public CombatCharacter AdditionalData;

        public SavableGameObjectData()
        {
            SceneName = "";
            Position = new SerializableVector3(Vector3.zero);
            Rotation = new SerializableQuaternion(Quaternion.identity);
            Active = true;
            Destroyed = false;
        }
    }
}