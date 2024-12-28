using Godot;
using System.Collections.Generic;
using System.Linq;

public record Item(CombatEntityStats StatModifiers, string Name)
{
    public Button Button { get; set; }
}

public record ItemSlot(Vector2 Position, bool isEquipment)
{
    public Item HeldItem { get; set; }
}

public partial class InventoryController : Control
{
    public static InventoryController Instance;

    // these positions are the top left corner
    // to get the center, add half the slot size
    private static readonly List<ItemSlot> inventorySlots = new()
    {
        new ItemSlot(new Vector2(535, 241), false),
        new ItemSlot(new Vector2(733,238), false),
        new ItemSlot(new Vector2(942,234), false),
        new ItemSlot(new Vector2(522,382), false),
        new ItemSlot(new Vector2(726, 379),false),
        new ItemSlot(new Vector2(932, 380), false),
        new ItemSlot(new Vector2(529, 581), false),
        new ItemSlot(new Vector2(723, 581), false),
        new ItemSlot(new Vector2(932, 581), false),
        new ItemSlot(new Vector2(153, 143), true),
        new ItemSlot(new Vector2(158, 374), true)
    };

    private readonly int slotSizePx = 180;

    private List<Item> items = new();

    private bool isDragging = false;
    private Item draggedItem;

    public override void _Ready()
    {
        Instance = this;

        AddItem(new Item(new CombatEntityStats() { MaxHealth = 10, CurrentHealth = 10 }, "+Health armor"));
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

        if(availableSlot.isEquipment)
        {
            EquipItem(item, availableSlot);
        }
        else
        {
            AddItemInternal(item, availableSlot);
        }
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

        item.Button.Position = slot.Position;

        items.Add(item);
        slot.HeldItem = item;
    }

    private void EquipItem(Item item, ItemSlot slot)
    {
        PlayerManager.UpdateStats(PlayerManager.Stats + item.StatModifiers);
        item.Button.Position = slot.Position;

        slot.HeldItem = item;
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