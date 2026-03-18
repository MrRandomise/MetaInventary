# 🎒 MetaInventory — Система мета-механики инвентаря

> **Учебный проект курса [OTUS Game Development](https://otus.ru)**  
> Разработка гибкой системы инвентаря и экипировки для RPG-игр на Unity

---

## 📖 О проекте

**MetaInventory** — это учебный проект, в рамках которого была спроектирована и реализована система мета-механики инвентаря для игры в жанре RPG. Ключевая идея — **компонентная архитектура предметов**: каждый предмет является контейнером для набора компонентов (статы, тип экипировки, флаги), что обеспечивает максимальную гибкость и расширяемость без изменения базовых классов.

Система включает взаимосвязанные подсистемы:
- Инвентарь персонажа с событийной моделью
- Экипировку с 5 слотами и автоматической заменой предметов
- Систему статов персонажа, реагирующую на надевание/снятие экипировки

---

## 🚀 Технологии и инструменты

| Технология | Версия | Назначение |
|---|---|---|
| **Unity** | 2021.3.7f1 | Игровой движок |
| **C#** | 9.0 | Язык программирования |
| **Zenject** | — | Dependency Injection контейнер |
| **Sirenix OdinInspector** | — | Расширенный Inspector в редакторе Unity |
| **NUnit** | — | Юнит-тестирование через Unity Test Framework |

---

## 🏗️ Архитектура

### Компонентная модель предмета

Каждый `Item` — это контейнер компонентов. Предмет не имеет жёстко заданных свойств; вместо этого его поведение определяется набором прикреплённых компонентов:

```
Item ("GoldBoots")
├── Stats("speed", 10)            ← добавляет +10 скорости при надевании
└── EquipmentTypeComponent(LEGS)  ← определяет слот экипировки
```

```
Item ("AirBots")
├── Stats("health", 10)           ← добавляет +10 здоровья
├── Stats("speed", 10)            ← добавляет +10 скорости
└── EquipmentTypeComponent(LEGS)  ← тот же слот, заменит GoldBoots
```

### Слоты экипировки

```
┌─────────────┐
│    HEAD      │  ← Шлем (+12 HP)
├─────────────┤
│    BODY      │  ← Броня (+16 HP)
├──────┬──────┤
│LEFT  │RIGHT │  ← Щит (+15 HP) / Меч (+15 DMG)
│HAND  │HAND  │
├─────────────┤
│    LEGS      │  ← Сапоги (+10 SPD)
└─────────────┘
```

### Диаграмма взаимодействия систем

```
ItemConfig (ScriptableObject)
       │ создаёт Item
       ▼
   Inventory ──── AddItem/RemoveItem ──── события OnItemAdded / OnItemRemoved
       │
       │ FindItem
       ▼
  Equipment ──── EquipItem / UnequipItem
       │               │
       │               ▼
       │         EquipmentEffect
       │               │ AddEffectToCharacter / RemoveEffectFromCharacter
       ▼               ▼
   Character ──── SetStat / GetStat ──── событие OnStateChanged
```

---

## ✨ Реализованные паттерны проектирования

| Паттерн | Где используется |
|---|---|
| **Компонент (Component)** | `Item` — контейнер компонентов (`Stats`, `EquipmentTypeComponent`) |
| **Наблюдатель (Observer)** | `Inventory.OnItemAdded/OnItemRemoved`, `Character.OnStateChanged` |
| **Внедрение зависимостей (DI)** | Zenject — `Installer` связывает `Character`, `Inventory`, `Equipment` |
| **Прототип (Prototype)** | `Item.Clone()` — глубокое копирование предмета |
| **Стратегия (Strategy)** | `EquipmentEffect` — применяет разные эффекты в зависимости от компонентов предмета |
| **Фабрика / Шаблон (Template)** | `ItemConfig` (ScriptableObject) — шаблон для создания предметов в редакторе |
| **Флаги (Flags Enum)** | `ItemFlags` — битовые флаги свойств предмета (STACKABLE, CONSUMABLE, EQUIPPABLE, EFFECTIBLE) |

---

## 📦 Структура проекта

```
Assets/
├── Scripts/
│   ├── Character/
│   │   ├── Character.cs              # Статы персонажа (HP, DMG, SPD)
│   │   ├── Stats.cs                  # Компонент: одна характеристика
│   │   └── DebugCharacter.cs         # MonoBehaviour для отладки в редакторе
│   ├── Equipment/
│   │   ├── Equipment.cs              # Управление 5 слотами экипировки
│   │   ├── EquipmentType.cs          # Enum: LEGS, BODY, HEAD, LEFT_HAND, RIGHT_HAND
│   │   ├── EquipmentTypeComponent.cs # Компонент: слот экипировки предмета
│   │   ├── EquipmentEffect.cs        # Применение/снятие эффектов статов
│   │   └── DebugEquipment.cs         # MonoBehaviour для отладки в редакторе
│   ├── Inventory/
│   │   └── Inventory.cs              # Список предметов с событиями
│   ├── Item/
│   │   ├── Item.cs                   # Базовый класс предмета
│   │   ├── ItemFlags.cs              # Битовые флаги свойств предмета
│   │   └── ItemConfig.cs             # ScriptableObject-шаблон предмета
│   ├── Installer/
│   │   └── Installer.cs              # Zenject DI-конфигурация
│   └── Test/
│       └── EquipmentUniTests.cs      # 11 юнит-тестов
└── GameConfig/
    └── Items/                        # 9 готовых ScriptableObject-предметов
        ├── GoldBoots.asset           # Сапоги (+10 SPD)
        ├── AirBots.asset             # Сапоги (+10 HP, +10 SPD)
        ├── StrongSword.asset         # Меч (+15 DMG)
        ├── MetallShield.asset        # Щит (+15 HP)
        ├── BloodMail.asset           # Броня (+16 HP)
        ├── Helmet.asset              # Шлем (+12 HP)
        └── ...
```

---

## 🧪 Тестирование

Проект покрыт **24 тест-кейсами** (8 тестовых методов) с использованием NUnit:

| Тест | Кол-во | Описание |
|---|---|---|
| `EquipItem` | ×5 | Проверка надевания предмета в каждый из 5 слотов |
| `UnequipItem` | ×5 | Проверка снятия предмета из каждого слота |
| `WhenEquip_CheckStats` | ×5 | Проверка, что статы персонажа увеличились при надевании |
| `WhenUnequip_CheckStats` | ×5 | Проверка, что статы вернулись к исходным после снятия |
| `SwapItems` | ×1 | Автоматическая замена предмета в занятом слоте |
| `EquipTwice` | ×1 | Идемпотентность повторного надевания |
| `UnequipTwice` | ×1 | Обработка повторного снятия |
| `UnequipEmpty` | ×1 | Снятие из пустого слота не вызывает ошибок |

---

## ⚙️ Как запустить

1. Установите **Unity 2021.3.7f1** через [Unity Hub](https://unity.com/download)
2. Клонируйте репозиторий:
   ```bash
   git clone https://github.com/MrRandomise/MetaInventary.git
   ```
3. Откройте папку проекта в Unity Hub → **Add project from disk**
4. Откройте сцену `Assets/Scenes/SampleScene.unity`
5. Нажмите **Play** для запуска демо-сцены

### Запуск тестов
- Откройте **Window → General → Test Runner**
- Выберите вкладку **Edit Mode**
- Нажмите **Run All**

---

## 💡 Чему научился в ходе проекта

- Проектирование **компонентной архитектуры** (Entity-Component) без наследования
- Применение **Dependency Injection** с Zenject в Unity
- Работа с **ScriptableObject** для хранения конфигурации предметов
- Реализация **событийной модели** (Observer pattern) для реактивного UI
- Написание **юнит-тестов** для игровой логики без MonoBehaviour
- Использование **битовых масок** для флагов предметов

---

## 📚 Курс

Проект разработан в рамках курса **Game Developer на Unity** платформы [OTUS](https://otus.ru).

---

## 👤 Автор

**MrRandomise** — [GitHub](https://github.com/MrRandomise)
