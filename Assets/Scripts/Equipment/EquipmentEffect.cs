using CharacterCore;

namespace InventoryCore
{
    public class EquipmentEffect
    {
        public void AddEffectToCharacter(Item obj, Character character)
        {
            var stats = obj.GetComponents<Stats>();
            if (stats.Length==0)
                return;
            
            foreach (var stat in stats)
            {
                var statValue = character.GetStat(stat.Name);
                character.SetStat(stat.Name, statValue + stat.Value);
            }
        }

        public void RemoveEffectFromCharacter(Item obj, Character character)
        {
            var stats = obj.GetComponents<Stats>();
            if (stats.Length==0)
                return;

            foreach (var stat in stats)
            {
                var statValue = character.GetStat(stat.Name);
                character.SetStat(stat.Name, statValue - stat.Value);
            }
        }
    }
}