using Godot;

public interface MapNodeController
{
    public void StartEncounter(Node rootNode);
    public void CleanupEncounter();
    public bool InProgress();
}
