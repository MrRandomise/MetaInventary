using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using System.Reflection;
using CharacterCore;

namespace Debug
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
            var printer="";
            foreach (var (key, value) in _character.GetStats()) 
                printer+=($"{key} : {value} \n");
            print(printer);
        }
    }
}