using System.Collections.Generic;
using CharacterCore;
using Zenject;
using InventoryCore;

public class Installer : MonoInstaller
{
    public override void InstallBindings()
    {
        var stats = new KeyValuePair<string, int>[]
        {
            new("damage", 11),
            new("health", 12),
            new("speed", 5)
        };

        Container.Bind<Character>().AsSingle().WithArguments(stats).NonLazy();

        Container.Bind<Inventory>().AsSingle().NonLazy();
        Container.Bind<Equipment>().AsSingle().NonLazy();
        Container.Bind<EquipmentEffect>().AsSingle().NonLazy();
    }
}