using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PGCTerrain))]
[CanEditMultipleObjects]

public class PGCTerrainEditor : Editor
{
    SerializedProperty perlinMTHi;
    SerializedProperty perlinMTLow;
    SerializedProperty perlinTileSize;
    SerializedProperty time;
    SerializedProperty amplitude;
    SerializedProperty wavelength;
    SerializedProperty mPerlinAmp;
    SerializedProperty mPerlinFreq;
    SerializedProperty mPerlinPersistence;
    SerializedProperty mPerlinLacunarity;
    SerializedProperty mPerlinOctaves;

    bool perlinVals = false;
    bool trigVals = false;
    bool multiPerlin = false;
    private void OnEnable()
    {
        perlinMTHi = serializedObject.FindProperty("perlinMTHi");
        perlinMTLow = serializedObject.FindProperty("perlinMTLow");
        perlinTileSize = serializedObject.FindProperty("perlinTileSize");
        amplitude = serializedObject.FindProperty("amplitude");
        wavelength = serializedObject.FindProperty("wavelength");
        time = serializedObject.FindProperty("time");
        mPerlinAmp = serializedObject.FindProperty("mPerlinAmp");
        mPerlinFreq = serializedObject.FindProperty("mPerlinFreq");
        mPerlinPersistence = serializedObject.FindProperty("mPerlinPersistence");
        mPerlinOctaves = serializedObject.FindProperty("mPerlinOctaves");
        mPerlinLacunarity = serializedObject.FindProperty("mPerlinLacunarity");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        PGCTerrain terrain = (PGCTerrain)target;
        EditorGUILayout.LabelField("PGC Terrain Editor", EditorStyles.boldLabel);

        //GUI for Trigonometry Functions
        trigVals = EditorGUILayout.Foldout(trigVals, "Trig Values");
        if (trigVals)
        {
            GUILayout.Label("Set Trig Values Here", EditorStyles.label);
            EditorGUILayout.IntSlider(time, 1, 100, new GUIContent("Time"));
            EditorGUILayout.Slider(amplitude, 1.0f, 600.0f, new GUIContent("Amplitude"));
            EditorGUILayout.Slider(wavelength, 1.0f, 5f, new GUIContent("Wavelength"));
            if (GUILayout.Button("Set Defaults Trig")) { terrain.SetDefaultsTrig(); }
        }

        //GUI for perlin functions.
        perlinVals = EditorGUILayout.Foldout(perlinVals, "Perlin values");
        if (perlinVals)
        {
            GUILayout.Label("Set Perlin Values Here", EditorStyles.label);
            EditorGUILayout.Slider(perlinMTHi, 10.0f, 600.0f, new GUIContent("Perlin MT High"));
            EditorGUILayout.Slider(perlinMTLow, -100.0f, 9.8f, new GUIContent("Perlin MT Low"));
            EditorGUILayout.IntSlider(perlinTileSize, 1, 100, new GUIContent("Perlin Tile Size"));
            if (GUILayout.Button("Set Defaults Perlin")) { terrain.SetDefaultsPerlin(); }
        }

        //GUI for multiperlin functions.
        multiPerlin = EditorGUILayout.Foldout(multiPerlin, "Multi Perlin values");
        if (multiPerlin)
        {
            GUILayout.Label("Set MPerlin Values Here", EditorStyles.label);
            EditorGUILayout.IntSlider(mPerlinOctaves, 1, 8, new GUIContent("Octaves"));
            EditorGUILayout.Slider(mPerlinFreq, 1.0f, 16.0f, new GUIContent("Frequency"));
            EditorGUILayout.Slider(mPerlinAmp, 0.1f, 1.0f, new GUIContent("Amplitude"));
            EditorGUILayout.Slider(mPerlinPersistence, .1f, 1.0f, new GUIContent("Persistence"));
            EditorGUILayout.Slider(mPerlinLacunarity, 1.0f, 2.0f, new GUIContent("Lacunarity"));
            if (GUILayout.Button("Set Defaults MPerlin")) { terrain.SetDefaultsMPerlin(); }
        }


        //GUI for change buttons
        EditorGUILayout.LabelField("Apply Terrain", EditorStyles.label);
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Apply Perlin")) { terrain.PerlinNoiseTerrain(); }
        if (GUILayout.Button("Apply Multi Perlin")) { terrain.MultiPerlinTerrain(); }
        if (GUILayout.Button("Apply Trig")) { terrain.TrigTerrain(); }
        if (GUILayout.Button("Reset Terrain")) { terrain.ResetTerrain(); }
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
