using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PGCTerrain : MonoBehaviour
{
    public Terrain terrain;
    public TerrainData terrainData;
    public float[,] heightMap;
    int xSize, zSize;

    //For Perlin Functions
    public float perlinMTHi;
    public float perlinMTLow;
    public int perlinTileSize;

    //For MultiPerlinFunctions
    public float mPerlinAmp;
    public float mPerlinFreq;
    public float mPerlinPersistence;
    public float mPerlinLacunarity;
    public int mPerlinOctaves;

    //For Trig Functions
    public int time;
    public float amplitude;
    public float wavelength;

    void OnEnable()
    {
        terrain = this.gameObject.GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        xSize = (int)terrainData.size.x;
        zSize = (int)terrainData.size.z;

        SetDefaultsPerlin();
        SetDefaultsTrig();
        SetDefaultsMPerlin();
    }

    public void SetDefaultsPerlin()
    {
        perlinMTHi = 10.0f;
        perlinMTLow = -100.0f;
        perlinTileSize = 1;
    }

    public void SetDefaultsMPerlin()
    {
      mPerlinAmp = 0.1f;
      mPerlinFreq = 1.0f;
      mPerlinPersistence = 0.1f;
      mPerlinLacunarity = 1.0f;
      mPerlinOctaves = 1;
    }

    public void SetDefaultsTrig()
    {
        time = 1;
        amplitude = 1.0f;
        wavelength = 1.0f;
    }

    public void ResetTerrain()
    {
        float[,] newHeightMap = new float[terrainData.detailWidth, terrainData.detailHeight];
        for (int x = 0; x < xSize; x++)
            for (int z = 0; z < zSize; z++) 
            { newHeightMap[x, z] = 0; }
        terrainData.SetHeights(0, 0, newHeightMap);
    }

    public void TrigTerrain()
    {
        float[,] newHeightMap = new float[terrainData.detailWidth, terrainData.detailHeight];
        float heightMapRes = (float)terrain.terrainData.heightmapResolution;
        float mtHeight = amplitude / (heightMapRes / 2.0f);
        float baseHeight = 5.0f / (heightMapRes / 2.0f);
        for (int x = 0; x < xSize; x++)
            for (int z = 0; z < zSize; z++)
            {
                newHeightMap[x, z] = baseHeight + (mtHeight * (Mathf.Sin((float) Mathf.PI * 2.0f * ((float) x / (float) xSize) * time / wavelength)));               
            }                                     

       terrainData.SetHeights(0, 0, newHeightMap);
    }

    public void PerlinNoiseTerrain()
    {
        //Calculations and initialization of height related variables
        float[,] newHeightMap = new float[terrainData.detailWidth, terrainData.detailHeight];
        float heightMapRes = (float)terrain.terrainData.heightmapResolution;
        float mtHeight = (perlinMTHi - perlinMTLow) / ( heightMapRes / 2.0f);
        float baseHeight = 5.0f / (heightMapRes / 2);

        for(int x = 0; x < xSize; x++)
            for(int z = 0; z < zSize; z++)
            {
                newHeightMap[x, z] = baseHeight + (Mathf.PerlinNoise(((float)x / (float)xSize) * perlinTileSize, ((float)z / (float)zSize * perlinTileSize) * mtHeight));
            }

        terrainData.SetHeights(0, 0, newHeightMap);
    }

    public void MultiPerlinTerrain()
    {
        //Calculations and initialization of height related variables
        float[,] newHeightMap = new float[terrainData.detailWidth, terrainData.detailHeight];
        float heightMapRes = (float)terrain.terrainData.heightmapResolution;
        float baseHeight = 5.0f / (heightMapRes / 2);
        float noiseVal;
        float heightVal;

        //Array of values of the octaves.
        float[] octaves = new float[mPerlinOctaves];

        //Variables to help calculate for each octave
        float amplitude;
        float frequency;
        float persistence;
       

        for (int x = 0; x < xSize; x++)
            for (int z = 0; z < zSize; z++)
            {
                //Set variables back to default for next (x,z)
                amplitude = mPerlinAmp;
                frequency = mPerlinFreq;
                persistence = mPerlinPersistence;
                heightVal = 0.0f;

                //We calculate the values of all octaves for particuler (x,z)
                for (int n = 0; n < mPerlinOctaves; n++)
                {
                    
                    noiseVal = Mathf.PerlinNoise(((float)x / (float)xSize) * frequency, ((float)z / (float)zSize) * frequency);
                    octaves[n] = noiseVal * amplitude;
                    //octaves[n] -= superpositionCompensation;

                    //Calculations of persistence and lacunarity for next octave.
                    amplitude *= persistence;
                    frequency *= mPerlinLacunarity;

                }

                //Sum up all the octave values
                for(int v = 0; v < mPerlinOctaves; v++) { heightVal += octaves[v]; }

                newHeightMap[x, z] = baseHeight + Mathf.Clamp(heightVal, 0.0f, 1.0f);
            }

        

        terrainData.SetHeights(0, 0, newHeightMap);
    }
}
