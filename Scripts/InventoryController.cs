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
        new Vector2(35, 241),
        new Vector2(233,238),
        new Vector2(442,234),
        new Vector2(22,382),
        new Vector2(226, 379),
        new Vector2(432, 380),
        new Vector2(29, 581),
        new Vector2(223, 581),
        new Vector2(432, 581)
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
        // get the first available slot
        AddItemInternal(item, inventorySlots[0]);
    }

    public void AddItem(Item item, Vector2 position)
    {
        // get the closest slot
        position.X -= slotSizePx / 2;
        position.Y -= slotSizePx / 2;

        var slot = inventorySlots.OrderBy(v2 => v2.DistanceSquaredTo(position)).First();
        AddItemInternal(item, slot);
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