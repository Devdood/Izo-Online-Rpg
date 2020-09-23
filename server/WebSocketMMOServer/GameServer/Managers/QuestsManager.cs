using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace WebSocketMMOServer.GameServer.Managers
{
    public class QuestsManager
    {
        public Dictionary<string, string> quests = new Dictionary<string, string>();
        public Dictionary<string, Lua> questImplementations = new Dictionary<string, Lua>();

        public Dictionary<QuestEvent, string> functions = new Dictionary<QuestEvent, string>()
        {
            { QuestEvent.MONSTER_DEFEAT, "killedMob" },
            { QuestEvent.DIED, "died" },
            { QuestEvent.INVENTORY_CHANGED, "inventoryChanged" },
            { QuestEvent.QUEST_STATE_CHANGED, "questUpdated" },
        };

        public QuestsManager()
        {
            string[] quests = System.IO.Directory.GetFiles("quests");

            foreach (var item in quests)
            {
                string name = Path.GetFileNameWithoutExtension(item);
                string questString = File.ReadAllText(item);
                this.quests.Add(name, questString);
            }

            InitializeQuests();
        }

        private void InitializeQuests()
        {
            LuaQuest q = new LuaQuest();

            foreach (var item in this.quests)
            {
                Lua state = new Lua();
                state["quest"] = q;
                state["state"] = 1;

                try
                {
                    state.DoString(item.Value);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Couldnt load quest: " + item.Key + " Err:" + ex.ToString());
                }
                questImplementations.Add(item.Key, state);
            }
        }

        public void ExecuteEvent(QuestEvent ev, params object[] args)
        {
            string func = functions[ev];

            foreach (var impl in questImplementations)
            {
                impl.Value["state"] = 1;
                var scriptFunc = impl.Value[func] as LuaFunction;
                if (scriptFunc != null)
                {
                    var res = scriptFunc.Call(args);
                    Console.WriteLine("Execute q");
                }
            }
        }
    }

    public class LuaQuest
    {
        public int GetPlayerQuestFlagInt(int playerId, string questName, string questFlag)
        {
            return 3;
        }

        public void SetPlayerQuestFlag(int playerId, string questName, string questFlag)
        {

        }

        public void SetQuestState(int playerId, string questName, int state)
        {
            ServerManager.Instance.QuestsManager.ExecuteEvent(QuestEvent.QUEST_STATE_CHANGED, playerId, questName, state);
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
            if(c != null)
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

public enum QuestEvent
{
    MONSTER_DEFEAT = 1,
    DIED = 2,
    INVENTORY_CHANGED = 3,
    QUEST_STATE_CHANGED = 4,
}
