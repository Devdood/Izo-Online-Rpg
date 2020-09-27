using System;
using System.Collections.Generic;
using System.Text;
using WebSocketMMOServer.Database;

namespace WebSocketMMOServer.GameServer.Managers
{
    public class QuestFlagsManager
    {
        public Dictionary<int, FlagsContainer> containers = new Dictionary<int, FlagsContainer>();

        public FlagsContainer GetContainer(int playerId)
        {
            if(containers.ContainsKey(playerId))
            {
                return containers[playerId];
            }

            return null;
        }

        public void AddContainer(int playerId)
        {
            if (containers.ContainsKey(playerId))
            {
                return;
            }

            containers.Add(playerId, new FlagsContainer()
            {
                id = playerId
            });
        }
     
        public void RenoiveContainer(int playerId)
        {
            if (containers.ContainsKey(playerId))
            {
                containers.Remove(playerId);
            }
        }

        public void SaveFlagsToDatabase(Player selectedCharacter)
        {
            string flags = "";

            foreach (var quest in GetContainer(selectedCharacter.Id).flags)
            {
                foreach (var item in quest.Value)
                {
                    flags += string.Format("('{0}', '{1}', '{2}', '{3}'),", selectedCharacter.DatabaseId, item.Value.flag, item.Value.value, item.Value.questName);
                }
            }

            if(flags.Length == 0)
            {
                return;
            }

            flags = flags.Remove(flags.Length - 1);

            string query = string.Format(@"INSERT INTO quest_flags(player_id, flag, flag_val, quest_name) VALUES {0} ON DUPLICATE KEY UPDATE 
                                flag_val = VALUES(flag_val)", flags);

            DatabaseManager.ReturnQuery(query);
        }
    }

    public class FlagsContainer
    {
        public int id;
        public Dictionary<string, Dictionary<string, QuestFlag>> flags = new Dictionary<string, Dictionary<string, QuestFlag>>();

        public int GetQuestFlagInt(string questName, string flag)
        {
            if(!EnsureFlagExist(questName, flag))
            {
                return 0;
            }

            return flags[questName][flag].value;
        }

        private bool EnsureQuestExist(string questName)
        {
            return flags.ContainsKey(questName);
        }

        private bool EnsureFlagExist(string questName, string flag)
        {
            return flags.ContainsKey(questName) && flags[questName].ContainsKey(flag);
        }

        public void SetQuestFlagInt(string questName, QuestFlag flag)
        {
            if(!EnsureQuestExist(questName))
            {
                flags.Add(questName, new Dictionary<string, QuestFlag>());
            }

            if (!EnsureFlagExist(questName, flag.flag))
            {
                flags[questName].Add(flag.flag, flag);
            }
            else
            {
                flags[questName][flag.flag].value = flag.value;
            }
        }
    }

    public class QuestFlag
    {
        public string questName;
        public string flag;
        public int value;
    }
}
