using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

internal record MapNode(List<MapNode> childern, Button uiButton);

public partial class MapController : Node
{
    private readonly CombatNodeController combatNodeController = new();

    private const int buttonSize = 100;
    private const int buttonSpacing = 20;

    private List<List<MapNode>> MapNodes = new();
    private MapNode currentNode;

    public void GenerateMap(Control mapRoot)
    {
        // TODO generate this
        int[] floorSizes = { 1, 3, 5, 11, 6, 2, 1 };
        // add starting location
        currentNode = new MapNode(new List<MapNode>(), GenerateStartingLocationButton(mapRoot));
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
            for (int roomIndex = 0; roomIndex < floorSizes[floorIndex]; roomIndex ++)
            {
                // TODO verify vector2
                var btn = GenerateRandomButton(mapRoot, new Vector2((buttonSize + buttonSpacing) * roomIndex, (buttonSize + buttonSpacing) * floorIndex));
                var currentRoom = new MapNode(new List<MapNode>(), btn);

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

        EnableConnectedMapButtons();
    }

    private Button GenerateRandomButton(Control mapRoot,Vector2 position)
    {
        var testButton = new Button()
        {
            Text = "[Combat]",
            Size = new Vector2(buttonSize, buttonSize),
            Position = position,
            Disabled = true
        };
        mapRoot.AddChild(testButton);
        testButton.Pressed += () =>
        {
            combatNodeController.StartEncounter(this);
            UiController.Instance.ShowMap(false);
        };

        return testButton;
    }

    private Button GenerateStartingLocationButton(Control mapRoot)
    {
        var testButton = new Button()
        {
            Text = "[Start]",
            Size = new Vector2(buttonSize, buttonSize),
            Position = new Vector2(0, 0),
            Disabled = true
        };
        mapRoot.AddChild(testButton);

        return testButton;
    }

    private void EnableConnectedMapButtons()
    {
        foreach(var node in currentNode.childern)
        {
            node.uiButton.Disabled = false;
        }
    }
}
