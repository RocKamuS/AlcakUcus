using System.Collections.Generic;
using UnityEngine;

public class CitySpawner : MonoBehaviour
{
    public List<GameObject> buildingPrefabs; // Inspector'dan atayaca��m�z binalar
    public int gridWidth = 5;
    public int gridHeight = 5;
    public float spacing = 10f;

    void Start()
    {
        SpawnCity();
    }

    void SpawnCity()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                // Rastgele bina se�
                GameObject building = buildingPrefabs[Random.Range(0, buildingPrefabs.Count)];

                // Yeni pozisyon hesapla
                Vector3 position = new Vector3(x * spacing, 0, z * spacing + 50); // +50 kameraya yakla�mas� i�in


                // Instantiate et (olu�tur)
                Instantiate(building, position, Quaternion.identity);
            }
        }
    }
}
