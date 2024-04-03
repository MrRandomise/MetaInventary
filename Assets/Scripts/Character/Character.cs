using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace CharacterCore
{
    //Нельзя менять!
    public sealed class Character
    {
        [ShowInInspector] [ReadOnly] private readonly Dictionary<string, int> stats;

        public Character(params KeyValuePair<string, int>[] stats)
        {
            this.stats = new Dictionary<string, int>(stats);
        }

        public event Action OnStateChanged;

        public int GetStat(string name)
        {
            return stats[name];
        }

        public void SetStat(string name, int value)
        {
            stats[name] = value;
            OnStateChanged?.Invoke();
        }

        public void RemoveStat(string name, int value)
        {
            if (stats.Remove(name)) OnStateChanged?.Invoke();
        }

        public KeyValuePair<string, int>[] GetStats()
        {
            return stats.ToArray();
        }
    }
}