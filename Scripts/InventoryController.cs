using Godot;
using System.Collections.Generic;
using System.Linq;

public record Item(CombatEntityStats StatModifiers, string Name)
{
    public Button Button { get; set; }
}

public partial class InventoryController : Control
{
    public static InventoryController Instance;

    // these positions are the top left corner
    // to get the center, add half the slot size
    private static readonly List<Vector2> inventorySlots = new()
    {
        new Vector2(535, 241),
        new Vector2(733,238),
        new Vector2(942,234),
        new Vector2(522,382),
        new Vector2(726, 379),
        new Vector2(932, 380),
        new Vector2(529, 581),
        new Vector2(723, 581),
        new Vector2(932, 581)
    };

    private static readonly List<Vector2> equipmentSlots = new()
    {
        new Vector2(153, 143),
        new Vector2(158, 374)
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
        AddItemInternal(item, inventorySlots[0]);
    }

    public void AddItem(Item item, Vector2 position)
    {
        // compensate for the center of UI elements being in the top left
        position.X -= slotSizePx / 2;
        position.Y -= slotSizePx / 2;

        // get the closest inventory slot
        var inventorySlot = inventorySlots.OrderBy(v2 => v2.DistanceSquaredTo(position)).First();
        var inventoryDist = inventorySlot.DistanceSquaredTo(position);

        // get closest equipment slot
        // TODO these have types and they only accept a matching item type
        var equipSlot = equipmentSlots.OrderBy(v2 => v2.DistanceSquaredTo(position)).First();
        var equipDist = equipSlot.DistanceSquaredTo(position);

        if(inventoryDist < equipDist)
        {
            AddItemInternal(item, inventorySlot);
        }
        else
        {
            EquipItem(item, equipSlot);
        }
    }

    private void AddItemInternal(Item item, Vector2 slot)
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

        item.Button.Position = slot;

        items.Add(item);
    }

    private void EquipItem(Item item, Vector2 slot)
    {
        PlayerManager.UpdateStats(PlayerManager.Stats + item.StatModifiers);
        item.Button.Position = slot;
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