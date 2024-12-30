using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

internal record MapNode(List<MapNode> childern, Button uiButton);

public partial class MapController : Node
{
    public static MapController Instance;

    private readonly CombatNodeController combatNodeController = new();
    private readonly TreasureNodeController treasureNodeController = new();
    private readonly CampfireNodeController restNodeController = new();
    private readonly FinalNodeController finalNodeController = new();

    private const int buttonSize = 100;
    private const int buttonSpacing = 25;

    private List<List<MapNode>> MapNodes = new();
    private MapNode currentNode;

    public MapController()
    {
        Instance = this;
    }

    public void GenerateMap()
    {
        MapNodes = new List<List<MapNode>>();
        Control mapRoot = UiController.Instance.MapMenu;
        foreach(var n in mapRoot.GetChildren())
        {
            n.Free();
        }
        // TODO generate this
        int[] floorSizes = { 1, 3, 5, 7, 6, 2, 1 };
        //int[] floorSizes = { 1, 3 };
        // add starting location
        currentNode = new MapNode(new List<MapNode>(), GenerateStartingLocationButton(mapRoot, new Vector2(900, 0)));
        MapNodes.Add(new List<MapNode>() { currentNode });
        Random rand = new();

        // add the rest
        for(int floorIndex = 1; floorIndex < floorSizes.Count(); floorIndex++)
        {
            List<MapNode> currentFloor = new();
            // we create a bag of number of connections between this layer and the previous one
            List<int> nrOfConnectionsBag = new();
            int baseNrOfConnections = floorSizes[floorIndex - 1] / floorSizes[floorIndex];
            int remainderConnections = floorSizes[floorIndex - 1] % floorSizes[floorIndex];
            // we add numbers so that they add up to the number of connections we need
            for(int i = 0; i < floorSizes[floorIndex]; i++)
            {
                int crtNr = baseNrOfConnections;
                if(remainderConnections > 0)
                {
                    crtNr++;
                    remainderConnections--;
                }
                nrOfConnectionsBag.Add(crtNr);
            }
            int prevFloorRoomIndex = 0;
            float btnSpacingY = (1800 - (floorSizes[floorIndex] * buttonSize)) / floorSizes[floorIndex];
            for (int roomIndex = 0; roomIndex < floorSizes[floorIndex]; roomIndex ++)
            {
                var btn = GenerateRandomButton(mapRoot, new Vector2((buttonSize + btnSpacingY) * roomIndex + btnSpacingY / 2, (buttonSize + buttonSpacing) * floorIndex));
                var currentRoom = new MapNode(new List<MapNode>(), btn);
                btn.Pressed += () => { currentNode = currentRoom; };

                // take a random number of connections from the bag
                int currentRoomNrOfConnections = nrOfConnectionsBag[rand.Next(nrOfConnectionsBag.Count())];
                if(roomIndex == 0 && currentRoomNrOfConnections == 0)
                {
                    // edge case, the first node has to connect to the neighbor
                    currentRoomNrOfConnections = 1;
                }
                nrOfConnectionsBag.Remove(currentRoomNrOfConnections);

                // TODO flip a coin for connecting with next/prev
                if(currentRoomNrOfConnections == 0)
                {
                    // this means we can't advance prevFloorRoomIndex, but we still have to connect to something
                    // connect to the prev index
                    MapNodes.ElementAt(floorIndex - 1).ElementAt(prevFloorRoomIndex - 1).childern.Add(currentRoom);
                }
                else
                {
                    for (int i = 0; i < currentRoomNrOfConnections; i++)
                    {
                        MapNodes.ElementAt(floorIndex - 1).ElementAt(prevFloorRoomIndex).childern.Add(currentRoom);
                        prevFloorRoomIndex++;
                    }
                }

                currentFloor.Add(currentRoom);
            }

            MapNodes.Add(currentFloor);
        }

        // add final node
        var finalButton = GenerateFinalLocationButton(mapRoot, new Vector2(900, MapNodes.Last().Last().uiButton.Position.Y + buttonSize + buttonSpacing));
        var finalNode = new MapNode(new List<MapNode>() , finalButton);
        foreach(var n in MapNodes.Last())
        {
            n.childern.Add(finalNode);
        }

        List<MapNode> finalFloor = new() { finalNode };
        MapNodes.Add(finalFloor);

        EnableConnectedMapButtons();
        CreateConnectionLines(mapRoot);
    }

    private Button GenerateRandomButton(Control mapRoot, Vector2 position)
    {
        return GenerateRandomButton(mapRoot, position, 2);
    }

    private Button GenerateRandomButton(Control mapRoot, Vector2 position, int roomType)
    {
        string roomName;
        MapNodeController roomController;
        if (roomType == 0)
        {
            roomName = "[Combat]";
            roomController = combatNodeController;
        }
        else if (roomType == 1)
        {
            roomName = "[Treasure]";
            roomController = treasureNodeController;
        }
        else
        {
            roomName = "[Rest]";
            roomController = restNodeController;
        }


        var testButton = new Button()
        {
            Text = roomName,
            Size = new Vector2(buttonSize, buttonSize),
            Position = position,
            Disabled = true
        };
        mapRoot.AddChild(testButton);
        testButton.Pressed += () =>
        {
            roomController.StartEncounter(this);
            UiController.Instance.ShowMap(false);
            EnableConnectedMapButtons(false);
        };

        return testButton;
    }

    private Button GenerateStartingLocationButton(Control mapRoot, Vector2 position)
    {
        var testButton = new Button()
        {
            Text = "[Start]",
            Size = new Vector2(buttonSize, buttonSize),
            Position = position,
            Disabled = true
        };
        mapRoot.AddChild(testButton);

        return testButton;
    }

    private Button GenerateFinalLocationButton(Control mapRoot, Vector2 position)
    {
        var testButton = new Button()
        {
            Text = "[End]",
            Size = new Vector2(buttonSize, buttonSize),
            Position = position,
            Disabled = true
        };
        mapRoot.AddChild(testButton);
        testButton.Pressed += () =>
        {
            finalNodeController.StartEncounter(this);
            UiController.Instance.ShowMap(false);
        };

        return testButton;
    }


    public void EnableConnectedMapButtons(bool enabled = true)
    {
        foreach(var node in currentNode.childern)
        {
            node.uiButton.Disabled = !enabled;
        }
    }

    private void CreateConnectionLines(Control mapRoot)
    {
        foreach(var floor in MapNodes)
        {
            foreach(var room in floor)
            {
                Vector2 btnPos = room.uiButton.Position + new Vector2(buttonSize / 2, buttonSize / 2);
                foreach (var connectedRoom in room.childern)
                {
                    Vector2 connectedBtnPos = connectedRoom.uiButton.Position + new Vector2(buttonSize / 2, buttonSize / 2);
                    var line = new ColorRect()
                    {
                        Size = new Vector2(7, btnPos.DistanceTo(connectedBtnPos)),
                        Position = btnPos,
                        Rotation = (float)Math.Atan((connectedBtnPos.X - btnPos.X) / (btnPos.Y - connectedBtnPos.Y)),
                        Color = new Color(255, 255, 255, 0.5f)
                    };
                    mapRoot.AddChild(line);
                }
            }
        }
    }
}
