using UnityEngine;

public class WindowSpawner : MonoBehaviour
{
    public GameObject windowPrefab;
    public GameObject windowParent;

    private int windowCount = 4;

    private float xPos = -4.5f;
    private float xOffset = 3f;
    private float yPos = 4.7f;
    private float yOffset = 2.5f;
    private float zPos = -0.5f;

    public GameObject[,] windows;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        windows = new GameObject[windowCount, windowCount];
        SpawnWindow();
    }

    void SpawnWindow()
    {
        for (int i = 0; i < windowCount; i++)
        {
            for (int j = 0; j < windowCount; j++)
            {
                windows[i, j] = Instantiate(windowPrefab, new Vector3(xPos + i * xOffset, yPos - j * yOffset, zPos), Quaternion.identity);
                windows[i, j].transform.SetParent(windowParent.transform, true);
            }
        }
    }
}
