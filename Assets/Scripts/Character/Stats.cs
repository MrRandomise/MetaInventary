using System;
using UnityEngine;

namespace CharacterCore
{
    [Serializable]
    public class Stats
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Value { get; private set; }

        public Stats(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}