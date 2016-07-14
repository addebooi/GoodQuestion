using System.Collections.Generic;
using System.Linq;
using LogicSpawn.RPGMaker.Generic;

namespace LogicSpawn.RPGMaker.API
{
    public static partial class RPG
    {
        public static class Player
        {
            /// <summary>
            /// Get the string ID of the vital being used to represent health.
            /// </summary>
            public static string HealthVitalID
            {
                get { return GetObject.PlayerMono.Player.Vitals.First(a => a.IsHealth).ID; }
            }

            /// <summary>
            /// Get a character class name by it's id.
            /// </summary>
            /// <param name="id">The ID of the class definition</param>
            /// <returns>Name of class</returns>
            public static string GetClassName(string id)
            {
                var rmStatisticDefintion = Rm_RPGHandler.Instance.Player.ClassDefinitions.FirstOrDefault(s => s.ID == id);
                if (rmStatisticDefintion != null)
                    return rmStatisticDefintion.Name;

                return null;
            }

            /// <summary>
            /// Get a character class ID by it's name.
            /// </summary>
            /// <param name="name">The name of the class definition</param>
            /// <returns>ID of class</returns>
            public static string GetClassID(string name)
            {
                var rmStatisticDefintion = Rm_RPGHandler.Instance.Player.ClassDefinitions.FirstOrDefault(s => s.Name == name);
                if (rmStatisticDefintion != null)
                    return rmStatisticDefintion.ID;

                return null;
            }

            /// <summary>
            /// Get's a list of all existing class definitions in the game data.
            /// </summary>
            public static List<Rm_ClassDefinition> Classes
            {
                get { return Rm_RPGHandler.Instance.Player.ClassDefinitions; }
            }
        }
    }
}