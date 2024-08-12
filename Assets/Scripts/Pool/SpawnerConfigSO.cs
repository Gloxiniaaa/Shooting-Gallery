using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerConfigSO", menuName = "SpawnerConfigSO")]
public class SpawnConfigSO : ScriptableObject
{
    public int[] SortingOrder;
    public float[] SpawnPosX;
    public float[] SpawnPosY;
    public SpawnPosition[] spawnPositions { get; private set; }
    public float TargetLifeTime;


    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        spawnPositions = new SpawnPosition[SpawnPosX.Length * SpawnPosY.Length];
        int idx = 0;
        foreach (float x in SpawnPosX)
        {
            int idxY = 0;
            foreach (float y in SpawnPosY)
            {
                int order = SortingOrder[idxY];
                idxY++;
                spawnPositions[idx] = new SpawnPosition(x, y, order);
                idx++;
            }
        }
        ShufflePos();
    }

    public void ShufflePos()
    {
        System.Random rng = new System.Random();
        int n = spawnPositions.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            SpawnPosition random = spawnPositions[k];
            spawnPositions[k] = spawnPositions[n];
            spawnPositions[n] = random;
        }
    }
}

[Serializable]
public class SpawnPosition
{
    public float X;
    public float Y;
    public int SortingOrder;

    public SpawnPosition(float x, float y, int order)
    {
        X = x;
        Y = y;
        SortingOrder = order;
    }
}