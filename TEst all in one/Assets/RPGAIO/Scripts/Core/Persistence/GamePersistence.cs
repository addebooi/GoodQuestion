using System.Collections.Generic;

namespace LogicSpawn.RPGMaker.Core
{
    public class GamePersistence
    {
        public List<VendorShop> VendorInventories ;
        public List<Item> BuyBackItems ;

        public GamePersistence()
        {
            VendorInventories = Rm_RPGHandler.Instance.Repositories.Vendor.AllVendors;
            BuyBackItems = new List<Item>();
        }
    }
}