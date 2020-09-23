using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WebSocketMMOServer.Database;

namespace WebSocketMMOServer.GameServer
{
    public class ShopManager
    {
        public Dictionary<int, ShopContainer> shops = new Dictionary<int, ShopContainer>();

        public ShopManager()
        {
            DataTable shopTable = DatabaseManager.ReturnQuery("SELECT * FROM shop");
            for (int i = 0; i < shopTable.Rows.Count; i++)
            {
                DataRow row = shopTable.Rows[i];
                shops.Add((int)row["id"], new ShopContainer((int)row["npc_id"], new ItemsContainer(ItemsContainerId.SHOP, (int)row["npc_id"])));
            }

            DataTable shopItemsTable = DatabaseManager.ReturnQuery("SELECT * FROM shop_item");
            for (int i = 0; i < shopItemsTable.Rows.Count; i++)
            {
                DataRow row = shopItemsTable.Rows[i];
                ItemData item = new ItemData()
                {
                    baseId = (int)row["item_id"],
                    amount = (int)row["amount"],
                };

                int shopId = (int)row["shop_id"];
                if (shops.ContainsKey(shopId))
                {
                    int slot = shops[shopId].items.GetFreeSlot();
                    if(slot != -1)
                    {
                        shops[shopId].items.AddItem(slot, item);
                    }
                }
            }
        }

        public bool GetShop(int baseId, out ShopContainer shopContainer)
        {
            ShopContainer container = shops.Values.FirstOrDefault(s => s.npcId == baseId);
            shopContainer = container;
            return container != null;
        }

        public ShopContainer GetShop(int baseId)
        {
            ShopContainer container = shops.Values.FirstOrDefault(s => s.npcId == baseId);
            return container;
        }
    }

    public class ShopContainer
    {
        public int npcId;
        public ItemsContainer items;

        public ShopContainer(int npcId, ItemsContainer items)
        {
            this.npcId = npcId;
            this.items = items;
        }
    }
}
