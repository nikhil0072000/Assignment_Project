using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Text tileInfoText;
   public Text coordinatesText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Show tile information on screen
    public void UpdateTileInfo(TileData tile)
    {
        if (tile != null)
        {
            if (tileInfoText != null)
            {
                string walkable = tile.IsWalkable() ? "Walkable" : "Blocked";
                string position = $"Position: ({tile.GetGridX()}, {tile.GetGridZ()})";
                tileInfoText.text = $"{position}{walkable}";
                //tileInfoText.text = $"{walkable}";
            }

            if (coordinatesText != null)
            {
                string position = $"Position: ({tile.GetGridX()}, {tile.GetGridZ()})";
                string walkable = tile.IsWalkable() ? "Walkable" : "Blocked";
                 coordinatesText.text =$"{position}{walkable}";
            }
              
        }
    }

    // Show text on screen
    public void DisplayTileInfo(string info,String coordinates)
    {
        if (tileInfoText != null)
            tileInfoText.text = info;
             if (coordinatesText != null)
            coordinatesText.text = coordinates;
    }

    // Clear text from screen
    public void ClearTileInfo()
    {
        if (tileInfoText != null)
            tileInfoText.text = "";

        if (coordinatesText != null)
           coordinatesText.text = "";
    }
}
