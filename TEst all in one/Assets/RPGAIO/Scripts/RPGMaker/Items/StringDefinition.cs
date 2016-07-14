using System;

namespace LogicSpawn.RPGMaker
{
    public class StringDefinition
    {

        public string ID;
        public string Name;

        public StringDefinition()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}