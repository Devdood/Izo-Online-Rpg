using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WebSocketMMOServer.Database;

namespace WebSocketMMOServer.GameServer
{
    public class ItemsManager
    {
        private Dictionary<int, ItemPrototype> itemsProto = new Dictionary<int, ItemPrototype>();
        private Dictionary<int, Dictionary<ItemsContainerId, ItemsContainer>> inventoryContainers = new Dictionary<int, Dictionary<ItemsContainerId, ItemsContainer>>();
        public int lastItemId = 1;

        public ItemsManager()
        {
            lastItemId = DatabaseManager.GetLastInsertedId("items");

            DataTable itemsProtoTable = DatabaseManager.ReturnQuery("SELECT * FROM items_proto");
            for (int i = 0; i < itemsProtoTable.Rows.Count; i++)
            {
                DataRow row = itemsProtoTable.Rows[i];

                ItemPrototype data = new ItemPrototype()
                {
                    id = (int)row["id"],
                    name = (string)row["name"],
                    reqLvl = (sbyte)row["req_level"],
                    price = (int)row["price"],
                };

                itemsProto.Add(data.id, data);
            }
        }

        public ItemsContainer GetContainer(ItemsContainerId id, int containerId)
        {
            if(inventoryContainers.ContainsKey(containerId))
            {
                return inventoryContainers[containerId][id];
            }

            return null;
        }

        public Dictionary<ItemsContainerId, ItemsContainer> GetContainers(int containerId)
        {
            if (inventoryContainers.ContainsKey(containerId))
            {
                return inventoryContainers[containerId];
            }

            return null;
        }

        public bool GetItemPrototype(int id, out ItemPrototype prototype)
        {
            if(itemsProto.TryGetValue(id, out prototype))
            {
                return true;
            }

            return false;
        }

        public ItemData CreateItemData(ItemData item)
        {
            item.uniqueId = lastItemId++;
            return item;
        }

        public void AddInventoryForCharacter(Player character)
        {
            if (inventoryContainers.ContainsKey(character.Id))
            {
                Console.WriteLine("Trying to duplicate stats container for id: " + character.Id);
                return;
            }

            inventoryContainers.Add(character.Id, new Dictionary<ItemsContainerId, ItemsContainer>()
            {
                { ItemsContainerId.INVENTORY, new ItemsContainer(ItemsContainerId.INVENTORY,character.Id) },
                { ItemsContainerId.WAREHOUSE, new ItemsContainer(ItemsContainerId.WAREHOUSE,character.Id) },
                { ItemsContainerId.EQUIPMENT, new ItemsContainer(ItemsContainerId.EQUIPMENT,character.Id) },
                { ItemsContainerId.SHOP, new ItemsContainer(ItemsContainerId.SHOP,character.Id) },
            });
        }

        public void RemoveInventoryForCharacter(int id)
        {
            if (inventoryContainers.ContainsKey(id))
            {
                inventoryContainers.Remove(id);
            }
        }
    }
}

public class ItemPrototype
{
    public int id;
    public string name;
    public int reqLvl;
    public int price;
}