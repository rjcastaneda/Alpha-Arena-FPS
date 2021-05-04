using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PGCTerrain : MonoBehaviour
{
    // Initialize all values for procedurally generated terrain
    public Terrain Terrain;
    public TerrainData TerrainData;
    public float[,] HeightMap;
    private int XSize, ZSize;

    public float PerlinMTHi;
    public float PerlinMTLow;
    public int PerlinTileSize;

    public float mPerlinAmp;
    public float mPerlinFreq;
    public float mPerlinPersistence;
    public float mPerlinLacunarity;
    public int mPerlinOctaves;

    public int Time;
    public float Amplitude;
    public float Wavelength;

    private void OnEnable()
    {
        // Setup default variables on enable
        Terrain = this.GetComponent<Terrain>();
        TerrainData = Terrain.terrainData;
        XSize = (int)TerrainData.size.x;
        ZSize = (int)TerrainData.size.z;

        SetDefaultsPerlin();
        SetDefaultsTrig();
        SetDefaultsMPerlin();
    }

    public void SetDefaultsPerlin()
    {
        PerlinMTHi = 10.0f;
        PerlinMTLow = -100.0f;
        PerlinTileSize = 5;
    }

    public void SetDefaultsTrig()
    {
        Time = 1;
        Amplitude = 1.0f;
        Wavelength = 1.0f;
    }

    public void SetDefaultsMPerlin()
    {
        mPerlinAmp = 0.1f;
        mPerlinFreq = 1.0f;
        mPerlinPersistence = 0.1f;
        mPerlinLacunarity = 1.0f;
        mPerlinOctaves = 1;
    }

    public void TrigTerrain()
    {
        // Initialize variables used to calculate terrain
        float[,] newHeightMap = new float[TerrainData.detailWidth, TerrainData.detailHeight];
        float heightMapRes = (float)Terrain.terrainData.heightmapResolution;
        float mtHeight = Amplitude / (heightMapRes / 2.0f);
        float baseHeight = 5.0f / (heightMapRes / 2.0f);

        for (int x = 0; x < XSize; x++)
        {
            for (int z = 0; z < ZSize; z++)
            {
                // Math function that calculates terrain heights using sine
                newHeightMap[x, z] = baseHeight + (mtHeight * (Mathf.Sin((float)Mathf.PI * 2.0f * ((float)x / (float)XSize) * Time / Wavelength)));
            }
        }

        // Apply calculated terrain data
        TerrainData.SetHeights(0, 0, newHeightMap);
    }

    public void ResetTerrain()
    {
        float[,] newHeightMap = new float[TerrainData.detailWidth, TerrainData.detailHeight];

        for (int x = 0; x < XSize; x++)
        {
            for (int z = 0; z < ZSize; z++)
            {
                // Set every x and z to zero
                newHeightMap[x, z] = 0;
            }
        }

        // Apply calculated terrain data
        TerrainData.SetHeights(0, 0, newHeightMap);
    }

    public void PerlinNoiseTerrain()
    {
        // Initialize variables used to calculate terrain
        float[,] newHeightMap = new float[TerrainData.detailWidth, TerrainData.detailHeight];
        float heightMapRes = (float)TerrainData.heightmapResolution;
        float mtHeight = (PerlinMTHi - PerlinMTLow) / (heightMapRes / 2.0f);
        float baseHeight = 5.0f / (heightMapRes / 2.0f);

        for (int x = 0; x < XSize; x++)
        {
            for (int z = 0; z < ZSize; z++)
            {
                // Math function that calculates terrain using Unity implemented Perlin Noise
                newHeightMap[x, z] = baseHeight + (Mathf.PerlinNoise(((float)x / (float)XSize) * PerlinTileSize, ((float)z / (float)ZSize * PerlinTileSize) * mtHeight));
            }
        }

        // Apply calculated terrain data
        TerrainData.SetHeights(0, 0, newHeightMap);
    }

    public void MultiPerlinTerrain()
    {
        // Initialize variables used to calculate terrain
        float[,] newHeightMap = new float[TerrainData.detailWidth, TerrainData.detailHeight];
        float heightMapRes = (float)TerrainData.heightmapResolution;
        float baseHeight = 5.0f / (heightMapRes / 2.0f);
        float noiseVal;
        float heightVal;

        float[] octaves = new float[mPerlinOctaves];

        float amplitude;
        float frequency;
        float persistence;

        for (int x = 0; x < XSize; x++)
        {
            for (int z = 0; z < ZSize; z++)
            {
                // Reset values for every z
                amplitude = mPerlinAmp;
                frequency = mPerlinFreq;
                persistence = mPerlinPersistence;
                heightVal = 0.0f;

                for (int n = 0; n < mPerlinOctaves; n++)
                {
                    // Apply multiple perlin noises per every perlin octave allocated
                    noiseVal = Mathf.PerlinNoise(((float)x / (float)XSize) * frequency, ((float)z / (float)ZSize) * frequency);
                    octaves[n] = noiseVal * amplitude;

                    amplitude *= persistence;
                    frequency *= mPerlinLacunarity;
                }

                // Sum of all perlin octaves
                for (int v = 0; v < mPerlinOctaves; v++)
                {
                    heightVal += octaves[v];
                }

                // Apply calculated terrain data
                newHeightMap[x, z] = baseHeight + Mathf.Clamp(heightVal, 0.0f, 1.0f);
            }
        }

        TerrainData.SetHeights(0, 0, newHeightMap);
    }
}
