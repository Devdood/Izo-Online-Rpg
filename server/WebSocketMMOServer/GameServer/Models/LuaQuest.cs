using System;
using System.Collections.Generic;
using System.Text;
using WebSocketMMOServer.GameServer.Packets.Outgoing;

namespace WebSocketMMOServer.GameServer
{
    public class LuaQuest
    {
        public int GetPlayerQuestFlagInt(int playerId, string questName, string questFlag)
        {
            return ServerManager.Instance.QuestFlags.GetContainer(playerId).GetQuestFlagInt(questName, questFlag);
        }

        public void SetPlayerQuestFlagInt(int playerId, string questName, string questFlag, int value)
        {
            var container = ServerManager.Instance.QuestFlags.GetContainer(playerId);
            if (container == null)
            {
                return;
            }

            container.SetQuestFlagInt(questName, new Managers.QuestFlag()
            {
                questName = questName,
                flag = questFlag,
                value = value
            });
        }

        public void OpenWarehouse(int playerId)
        {
            Client c = ServerManager.Instance.CharactersManager.GetClient(playerId);

            if (c != null)
            {
                Server.Instance.SendData(c.ip, new OpenUIPanelPacket(0));
            }
        }

        public void SetQuestState(int playerId, string questName, int state)
        {
            SetPlayerQuestFlagInt(playerId, questName, "state", state);
            ServerManager.Instance.QuestsManager.ExecuteEventsForAll(QuestEvent.QUEST_STATE_CHANGED, playerId, questName, state);
        }

        public void Log(int log)
        {
            Console.WriteLine(log);
        }

        public void Log(string log)
        {
            Console.WriteLine(log);
        }

        public int FinishQuest(int playerId, string questName)
        {
            Console.WriteLine("Player finished quest: " + questName);
            return 1;
        }

        public void GivePlayerItem(int playerId, int itemId, int amount)
        {
            Console.WriteLine("Player give item: " + itemId);
            Character c = ServerManager.Instance.CharactersManager.GetCharacterById(playerId);
            if (c != null)
            {
                c.GetItemsContainer(ItemsContainerId.INVENTORY).AddItemToFreeSlot(ServerManager.Instance.ItemsManager.CreateItemData(new ItemData()
                {
                    baseId = itemId,
                    amount = amount
                }));
            }
        }
    }
}
