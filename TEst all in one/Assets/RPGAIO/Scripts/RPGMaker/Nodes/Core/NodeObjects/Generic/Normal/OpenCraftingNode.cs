using LogicSpawn.RPGMaker.API;
using Newtonsoft.Json;

namespace LogicSpawn.RPGMaker.Core
{
    [NodeCategory("Game", "")]
    public class OpenCraftingNode : SimpleNode
    {
        public override string Name
        {
            get { return "Open Crafting"; }
        }

        public override string Description
        {
            get { return "Opens the crafting window."; }
        }

        public override string SubText
        {
            get { return ""; }
        }

        public override bool CanBeLinkedTo
        {
            get
            {
                return true;
            }
        }

        public override string NextNodeLinkLabel(int index)
        {
            return "Next";
        }

        protected override void SetupParameters()
        {

        }

        protected override void Eval(NodeChain nodeChain)
        {
            RPG.Events.OnOpenCrafting(new RPGEvents.OpenCraftingEventArgs());
        }
    }
}