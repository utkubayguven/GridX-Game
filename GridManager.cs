using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    public Dictionary<string, GridScript> gridDictionary;
    private int rows;
    public int columns ;
    [SerializeField]private float tileSize = 1;
    
 
    
    
    // Start is called before the first frame update
    private void Awake()// singleton Design Pattern
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    

    public void GenerateGrid()
    {
        gridDictionary = new Dictionary<string, GridScript>();
        rows=GameManager.instance.InputNumber;
        columns=GameManager.instance.InputNumber;
        
        var referenceTile = (GameObject) Instantiate(Resources.Load("tile"), transform);// referans tile i olusturur
        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < columns; col++)
            {
                var tile = (GameObject) Instantiate(referenceTile, transform);// tile lari olusturur
                var posX = col * tileSize;// tile larin pozisyonlarini ayarlamak icin
                var posY = row * -tileSize;// -tileSize yapmamizin sebebi tile larin pozisyonlarini ayarlamak icin
                tile.transform.position = new Vector2(posX, posY);// tile larin pozisyonunu ayarlar
                tile.name = row.ToString("D2") + col.ToString("D2");
                
                tile.AddComponent<BoxCollider2D>();
                var gridScript = tile.AddComponent<GridScript>();
                gridDictionary.Add(tile.name, gridScript);
            }
        }
        
        Destroy(referenceTile);// referans tile i yok eder
        var gridW = columns * tileSize;// grid in genisligini ayarlar
        var gridH = rows * tileSize;// grid in yuksekligini ayarlar
        transform.position = new Vector2(-gridW / 2 + tileSize / 2, gridH / 2 - tileSize / 2);// grid in pozisyonunu ayarlar
    }
    
    
    
}
