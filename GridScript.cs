using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    private static Dictionary<string, GridScript> gridDictionary = new Dictionary<string, GridScript>();


    private SpriteRenderer spriteRenderer;
    private Sprite tileSprite;
    private Sprite xSprite;
    private bool isToggled;

    private void OnParticleTrigger()
    {
        throw new NotImplementedException();
    }


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        var tilePrefab = Resources.Load<GameObject>("tile");
        var xPrefab = Resources.Load<GameObject>("x");

        if (tilePrefab && tilePrefab.GetComponent<SpriteRenderer>())
        {
            tileSprite = tilePrefab.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            Debug.LogError("Tile prefab could not be loaded or does not have a SpriteRenderer");
        }

        if (xPrefab && xPrefab.GetComponent<SpriteRenderer>())
        {
            xSprite = xPrefab.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            Debug.LogError("X prefab could not be loaded or does not have a SpriteRenderer");
        }

        gridDictionary.Add(gameObject.name, this);
    }

    private void OnMouseDown()
    {
        isToggled = !isToggled;
        spriteRenderer.sprite = isToggled ? xSprite : tileSprite;

        CheckCondition();
    }

    private void CheckCondition()
    {
        if (!isToggled)
            return;

        var currentRow = int.Parse(gameObject.name.Substring(0, 2));
        var currentCol = int.Parse(gameObject.name.Substring(2, 2));

        // Check horizontal, vertical, and diagonal neighbors
        var horizontalNeighbors = new List<string>
        {
            (currentRow).ToString("D2") + (currentCol - 1).ToString("D2"),
            (currentRow).ToString("D2") + (currentCol + 1).ToString("D2")
        };

        var verticalNeighbors = new List<string>
        {
            (currentRow - 1).ToString("D2") + (currentCol).ToString("D2"),
            (currentRow + 1).ToString("D2") + (currentCol).ToString("D2")
        };

        var diagonalNeighbors1 = new List<string>
        {
            (currentRow - 1).ToString("D2") + (currentCol - 1).ToString("D2"),
            (currentRow + 1).ToString("D2") + (currentCol + 1).ToString("D2")
        };

        var diagonalNeighbors2 = new List<string>
        {
            (currentRow - 1).ToString("D2") + (currentCol + 1).ToString("D2"),
            (currentRow + 1).ToString("D2") + (currentCol - 1).ToString("D2")
        };


        var neighborGroups = new List<List<string>>
        {
            horizontalNeighbors, verticalNeighbors, diagonalNeighbors1, diagonalNeighbors2
        };

        var tilesToToggle = new List<GridScript>();

        foreach (var neighbors in neighborGroups)
        {
            // Counts the neighbors that fulfill the specified conditions
            var validNeighbors = neighbors.Where(name =>
                GridManager.Instance.gridDictionary.ContainsKey(name) &&
                GridManager.Instance.gridDictionary[name].isToggled);
            var neighborCount = validNeighbors.Count();

            // If the number of neighbors is 2 or more, check for the additional condition
            if (neighborCount < 2) continue;
            {
                tilesToToggle.AddRange(validNeighbors.Select(name => GridManager.Instance.gridDictionary[name]));

                foreach (var extraValidNeighbors in from neighborName in neighbors where GridManager.Instance.gridDictionary.ContainsKey(neighborName) &&
                             GridManager.Instance.gridDictionary[neighborName].isToggled let neighborRow = int.Parse(neighborName.Substring(0, 2)) let neighborCol = int.Parse(neighborName.Substring(2, 2)) select new List<string>
                         {
                             (neighborRow).ToString("D2") + (neighborCol - 1).ToString("D2"), // Left
                             (neighborRow).ToString("D2") + (neighborCol + 1).ToString("D2"), // Right
                             (neighborRow - 1).ToString("D2") + (neighborCol).ToString("D2"), // Up
                             (neighborRow + 1).ToString("D2") + (neighborCol).ToString("D2"), // Down
                             (neighborRow - 1).ToString("D2") + (neighborCol - 1).ToString("D2"), // Top left
                             (neighborRow + 1).ToString("D2") + (neighborCol + 1).ToString("D2"), // Bottom right
                             (neighborRow - 1).ToString("D2") + (neighborCol + 1).ToString("D2"), // Top right
                             (neighborRow + 1).ToString("D2") + (neighborCol - 1).ToString("D2") // Bottom left
                         } into extraNeighbors select extraNeighbors.Where(name =>
                             GridManager.Instance.gridDictionary.ContainsKey(name) &&
                             GridManager.Instance.gridDictionary[name].isToggled))
                {
                    tilesToToggle.AddRange(extraValidNeighbors.Select(name =>
                        GridManager.Instance.gridDictionary[name]));
                }
            }
        }

        // Check the special condition for the current object and its horizontal and diagonal neighbors
        if (GridManager.Instance.gridDictionary.ContainsKey((currentRow).ToString("D2") +
                                                            (currentCol - 1).ToString("D2")) &&
            GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol - 1).ToString("D2")]
                .isToggled &&
            GridManager.Instance.gridDictionary.ContainsKey((currentRow).ToString("D2") +
                                                            (currentCol + 1).ToString("D2")) &&
            GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol + 1).ToString("D2")]
                .isToggled)
        {
            if (GridManager.Instance.gridDictionary.ContainsKey((currentRow - 1).ToString("D2") +
                                                                (currentCol - 1).ToString("D2")) &&
                GridManager.Instance.gridDictionary[(currentRow - 1).ToString("D2") + (currentCol - 1).ToString("D2")]
                    .isToggled)
            {
                tilesToToggle.Add(
                    GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol - 1).ToString("D2")]);
                tilesToToggle.Add(
                    GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol + 1).ToString("D2")]);
                tilesToToggle.Add(
                    GridManager.Instance.gridDictionary[
                        (currentRow - 1).ToString("D2") + (currentCol - 1).ToString("D2")]);
                tilesToToggle.Add(this);
            }
            else if (GridManager.Instance.gridDictionary.ContainsKey((currentRow + 1).ToString("D2") +
                                                                     (currentCol + 1).ToString("D2")) &&
                     GridManager.Instance
                         .gridDictionary[(currentRow + 1).ToString("D2") + (currentCol + 1).ToString("D2")].isToggled)
            {
                tilesToToggle.Add(
                    GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol - 1).ToString("D2")]);
                tilesToToggle.Add(
                    GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol + 1).ToString("D2")]);
                tilesToToggle.Add(
                    GridManager.Instance.gridDictionary[
                        (currentRow + 1).ToString("D2") + (currentCol + 1).ToString("D2")]);
                tilesToToggle.Add(this);
            }
        }

        // Check the special condition for the current object and its L-shaped neighbors
        if (GridManager.Instance.gridDictionary.ContainsKey((currentRow).ToString("D2") +
                                                            (currentCol - 1).ToString("D2")) &&
            GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol - 1).ToString("D2")]
                .isToggled &&
            GridManager.Instance.gridDictionary.ContainsKey((currentRow - 1).ToString("D2") +
                                                            (currentCol - 1).ToString("D2")) &&
            GridManager.Instance.gridDictionary[(currentRow - 1).ToString("D2") + (currentCol - 1).ToString("D2")]
                .isToggled)
        {
            tilesToToggle.Add(
                GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol - 1).ToString("D2")]);
            tilesToToggle.Add(
                GridManager.Instance.gridDictionary[(currentRow - 1).ToString("D2") + (currentCol - 1).ToString("D2")]);
            tilesToToggle.Add(this);
        }

        if (GridManager.Instance.gridDictionary.ContainsKey((currentRow).ToString("D2") +
                                                            (currentCol + 1).ToString("D2")) &&
            GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol + 1).ToString("D2")]
                .isToggled &&
            GridManager.Instance.gridDictionary.ContainsKey((currentRow - 1).ToString("D2") +
                                                            (currentCol + 1).ToString("D2")) &&
            GridManager.Instance.gridDictionary[(currentRow - 1).ToString("D2") + (currentCol + 1).ToString("D2")]
                .isToggled)
        {
            tilesToToggle.Add(
                GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol + 1).ToString("D2")]);
            tilesToToggle.Add(
                GridManager.Instance.gridDictionary[(currentRow - 1).ToString("D2") + (currentCol + 1).ToString("D2")]);
            tilesToToggle.Add(this);
        }

        if (GridManager.Instance.gridDictionary.ContainsKey((currentRow).ToString("D2") +
                                                            (currentCol + 1).ToString("D2")) &&
            GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol + 1).ToString("D2")]
                .isToggled &&
            GridManager.Instance.gridDictionary.ContainsKey((currentRow + 1).ToString("D2") +
                                                            (currentCol + 1).ToString("D2")) &&
            GridManager.Instance.gridDictionary[(currentRow + 1).ToString("D2") + (currentCol + 1).ToString("D2")]
                .isToggled)
        {
            tilesToToggle.Add(
                GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol + 1).ToString("D2")]);
            tilesToToggle.Add(
                GridManager.Instance.gridDictionary[(currentRow + 1).ToString("D2") + (currentCol + 1).ToString("D2")]);
            tilesToToggle.Add(this);
        }

        if (GridManager.Instance.gridDictionary.ContainsKey((currentRow).ToString("D2") +
                                                            (currentCol - 1).ToString("D2")) &&
            GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol - 1).ToString("D2")]
                .isToggled &&
            GridManager.Instance.gridDictionary.ContainsKey((currentRow + 1).ToString("D2") +
                                                            (currentCol - 1).ToString("D2")) &&
            GridManager.Instance.gridDictionary[(currentRow + 1).ToString("D2") + (currentCol - 1).ToString("D2")]
                .isToggled)
        {
            tilesToToggle.Add(
                GridManager.Instance.gridDictionary[(currentRow).ToString("D2") + (currentCol - 1).ToString("D2")]);
            tilesToToggle.Add(
                GridManager.Instance.gridDictionary[(currentRow + 1).ToString("D2") + (currentCol - 1).ToString("D2")]);
            tilesToToggle.Add(this);
        }
       

        // Toggle all valid tiles
        foreach (GridScript tile in tilesToToggle.Distinct())
        {
            tile.ToggleSprite();
        }
    }


    private void Update()
    {
        CheckCondition();
    }


    private void ToggleSprite()
    {
        if (isToggled)
        {
            GetComponent<SpriteRenderer>().sprite = tileSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = xSprite;
        }

        isToggled = !isToggled;
    }
}



































