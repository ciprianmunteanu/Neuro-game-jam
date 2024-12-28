using Godot;
using System;

public abstract class MapNodeController
{
    public abstract void StartEncounter(Node rootNode);
    public abstract void CleanupEncounter();

    public event Action OnRoomClear;

    protected void RoomCleared()
    {
        var reward = new Label() { Text = "10g", Position = new Vector2(50, 100), Size = new Vector2(200, 50) };
        UiController.Instance.RewardsMenu.AddChild(reward);
        UiController.Instance.RewardsMenu.Show();
        UiController.Instance.CombatMenu.Hide();

        CleanupEncounter();

        OnRoomClear?.Invoke();
    }
}
