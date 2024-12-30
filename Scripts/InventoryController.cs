﻿using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

public enum ItemType { NONE, WEAPON, ARMOR }

public class Item
{
    [JsonIgnore]
    public Button Button { get; set; }

    [JsonIgnore]
    private ItemSlot slot;

    [JsonIgnore]
    public ItemSlot Slot {
        get => slot;
        set
        {
            slot = value;
            Button.Position = slot.Position;
        }
    }

    [JsonIgnore]
    public List<CombatAction> Skills { get; set; } = new();

    [JsonIgnore]
    public CombatEntityStats BaseStats { get; set; }

    [JsonIgnore]
    public string SpritePath { get; set; } = "res://Assets/Weapon.png";

    public string Name { get; init; }

    [JsonIgnore]
    public ItemType Type { get; set; }

    public CombatEntityStats StatModifiers { get; init; }

    public Item(CombatEntityStats statModifiers, string name, ItemType type)
    {
        BaseStats = statModifiers;
        Name = name;
        Type = type;
    }

    [JsonConstructor]
    public Item(string name, CombatEntityStats statModifiers) 
    {
        Name = name;
        StatModifiers = statModifiers;
    }

    /// <summary>
    /// This is mostly for ghosts. Ghost data doesn't save things like skills to disk, instead it is loaded from the base version of the item.
    /// </summary>
    /// <param name="baseItem">The item to laod data from</param>
    public void LoadFromBaseItem(Item baseItem)
    {
        Skills = baseItem.Skills;
        BaseStats = baseItem.BaseStats;
        Type = baseItem.Type;
        SpritePath = baseItem.SpritePath;
    }
}

public record ItemSlot(Vector2 Position, bool isEquipment)
{
    public ItemType Type { get; set; } = ItemType.NONE;

    private Item heldItem;
    public Item HeldItem { 
        get => heldItem; 
        set
        {
            if(isEquipment)
            {
                var newStats = PlayerManager.Stats;

                if(value != null)
                {
                    newStats = newStats + value.BaseStats;
                }

                // if we're unequipping an item
                if (heldItem != null)
                {
                    newStats = newStats - heldItem.BaseStats;
                }

                if(Type == ItemType.WEAPON)
                {
                    PlayerManager.WeaponItem = value;
                }
                else if (Type == ItemType.ARMOR)
                {
                    PlayerManager.ArmorItem = value;
                }

                PlayerManager.UpdateStats(newStats);
            }

            heldItem = value;
        }
    }
}

public partial class InventoryController : Control
{
    public static InventoryController Instance;
    public IEnumerable<Item> GetEquippedItems()
    {
        return inventorySlots.Where(slot => slot.isEquipment && slot.HeldItem != null).Select(slot => slot.HeldItem);
    }

    // these positions are the top left corner
    // to get the center, add half the slot size
    private static readonly List<ItemSlot> inventorySlots = new()
    {
        new ItemSlot(new Vector2(535, 241), false),
        new ItemSlot(new Vector2(733,238), false),
        new ItemSlot(new Vector2(942,234), false),
        new ItemSlot(new Vector2(522,382), false),
        new ItemSlot(new Vector2(726, 379), false),
        new ItemSlot(new Vector2(932, 380), false),
        new ItemSlot(new Vector2(529, 581), false),
        new ItemSlot(new Vector2(723, 581), false),
        new ItemSlot(new Vector2(932, 581), false),
        new ItemSlot(new Vector2(153, 143), true) {Type = ItemType.WEAPON},
        new ItemSlot(new Vector2(158, 374), true) {Type = ItemType.ARMOR}
    };

    private readonly int slotSizePx = 180;

    private List<Item> items = new();

    private bool isDragging = false;
    private Item draggedItem;

    public override void _Ready()
    {
        Instance = this;

        AddItem(new Harpoon());
        AddItem(new BananaRum());
        AddItem(new Drones());

        AddItem(new RobotBody());
        AddItem(new ClownOutfit());
    }

    public void AddItem(Item item)
    {
        // get the first available inventory slot
        var availableSlot = inventorySlots.First(s => s.isEquipment == false && s.HeldItem == null);
        if(availableSlot != null)
        {
            AddItemInternal(item, availableSlot);
        }
    }

    public void AddItem(Item item, Vector2 position)
    {
        // compensate for the center of UI elements being in the top left
        position.X -= slotSizePx / 2;
        position.Y -= slotSizePx / 2;

        // get the closest inventory slot
        var availableSlot = inventorySlots.OrderBy(slot => slot.Position.DistanceSquaredTo(position)).First();
        var distance = availableSlot.Position.DistanceSquaredTo(position);
        if (distance > slotSizePx * slotSizePx)
        {
            item.Button.Position = item.Slot.Position;
            return;
        }

        AddItemInternal(item, availableSlot);
    }

    private void AddItemInternal(Item item, ItemSlot slot)
    {
        if (item.Button == null)
        {
            var button = new Button()
            {
                Text = item.Name,
                Size = new Vector2(slotSizePx, slotSizePx),
                ActionMode = BaseButton.ActionModeEnum.Press
            };
            AddChild(button);
            button.Pressed += () => OnItemPressed(item);

            item.Button = button;
        }

        // ignore cases where we're trying to equip the wrong item type
        if(slot.isEquipment && item.Type != slot.Type)
        {
            item.Button.Position = item.Slot.Position;
            return;
        }

        // we have 2 cases, based on whether or not the slot has an item already

        if (slot.HeldItem == null)
        {
            if(item.Slot != null)
            {
                item.Slot.HeldItem = null;
            }

            slot.HeldItem = item;
            item.Slot = slot;
        }
        else
        {
            SwapItemSlots(item, slot.HeldItem);
        }
    }

    private void SwapItemSlots(Item item1, Item item2)
    {
        var slot1 = item1.Slot;
        var slot2 = item2.Slot;

        var aux = item1.Slot;
        item1.Slot = item2.Slot;
        item2.Slot = aux;

        var aux2 = slot1.HeldItem;
        slot1.HeldItem = slot2.HeldItem;
        slot2.HeldItem = aux2;
    }

    private void OnItemPressed(Item item)
    {
        draggedItem = item;
        isDragging = true;
        items.Remove(item);
    }

    public override void _Input(InputEvent @event)
    {
        if(isDragging)
        {
            if(Input.IsMouseButtonPressed(MouseButton.Left))
            {
                draggedItem.Button.Position = GetLocalMousePosition() - new Vector2(slotSizePx / 2, slotSizePx / 2);
            }
            else
            {
                // we released it
                isDragging = false;
                // TODO maybe add a way to drop items
                AddItem(draggedItem, GetLocalMousePosition());
            }
        }
    }

}