using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using CharacterCore;

namespace DebugTest
{
    public class DebugCharacter : MonoBehaviour
    {
        private Character _character;

        [Inject]
        public void Construct(Character character)
        {
            _character = character;
        }

        [Button]
        public void GetStats()
        {
            string stats="";
            foreach (var (key, value) in _character.GetStats())
                stats += ($"{key} : {value}; ");
            print(stats);
        }
    }
}