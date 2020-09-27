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
            { QuestEvent.NPC_CLICK, "npcClick" },
            { QuestEvent.LOGIN, "login" },
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
                state["playerId"] = 1;

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

        public void ExecuteEventForSingle(QuestEvent ev, int playerId, string questName, params object[] args)
        {
            var impl = questImplementations[questName];
            string func = functions[ev];

            var container = ServerManager.Instance.QuestFlags.GetContainer(playerId);
            if(container == null)
            {
                Console.WriteLine("Quest container: " + playerId + " is null");
                return;
            }

            impl["state"] = container.GetQuestFlagInt(questName, "state");
            impl["playerId"] = playerId;

            var scriptFunc = impl[func] as LuaFunction;
            if (scriptFunc != null)
            {
                var res = scriptFunc.Call(args);
                Console.WriteLine("Execute: " + questName + " EV: " + ev);
            }
        }

        public void ExecuteEventsForAll(QuestEvent ev, int playerId, params object[] args)
        {
            string func = functions[ev];

            foreach (var impl in questImplementations)
            {
                ExecuteEventForSingle(ev, playerId, impl.Key, args);
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
    NPC_CLICK = 5,
    LOGIN = 6,
}
