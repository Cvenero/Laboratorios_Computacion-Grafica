using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public Texture2D[] maps;
    public GameObject wallPrefab;
    public GameObject zombiePrefab;

    public bool zombiesCamMove = true;

    public GameObject gemPrefab;//<--

    private Texture2D selectMap;

    private List<Vector3> openPositions = new List<Vector3>();

    private Color wallColor = Color.black;

    private int gemsRemaining;

    public static MapManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        GenerateNewMap();
        GenerateZombies();
        GenerateGems();
    }

    public void GenerateNewMap()
    {
        openPositions.Clear();

        selectMap = maps[Random.Range(0, maps.Length)];

        for (int x = 0; x < selectMap.width; x++)
        {
            for (int y = 0; y < selectMap.height; y++)
            {
                GenerateTile(x,y);
            }
        }
    }

    private void GenerateTile(int x, int y)
    {
        Color pixelColor = selectMap.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            openPositions.Add(new Vector3(x, 0 ,y));
            return;
        }
        if(pixelColor == wallColor)
        {
            Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
        }
    }
    private void GenerateZombies()
    {
        for (int x = 0; x < 7; x++)
        {
            int index = Random.Range(0, openPositions.Count);
            Instantiate(zombiePrefab, openPositions[index], Quaternion.identity);
            openPositions.RemoveAt(index);

        }
    }

    private void GenerateGems()
    {
        for (int x = 0; x < 5; x++)
        {
            int index = Random.Range(0, openPositions.Count);
            Instantiate(gemPrefab, openPositions[index], Quaternion.identity);
            openPositions.RemoveAt(index);

        }
        gemsRemaining = 5;
    }
    public Vector3 GetRandomPos()
    {
        return openPositions[Random.Range(0, openPositions.Count)];
    }

    public void GemPickedUp()
    {
        gemsRemaining--;

        if (gemsRemaining == 0)
        {
            zombiesCamMove = false; 
            UIManager.instance.ShowGameOver(true);
        }
    }

}
