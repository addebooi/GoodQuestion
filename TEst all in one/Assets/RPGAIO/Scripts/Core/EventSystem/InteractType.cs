namespace LogicSpawn.RPGMaker.Core
{
    public enum InteractType
    {
        //Help:
        //Interaction:      | Interaction Parameter:
        Click,            //| N/A
        NearTo,           //| Distance to GameObject script is on
        Speak,            //| N/A : Must be used on NPC character
        CompleteQuest     //| Quest ID of NPC GameObject script is attached to.
    }
}