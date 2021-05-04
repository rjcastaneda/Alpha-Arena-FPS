using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PGCTerrain))]
public class Editor_PGCTerrain : Editor
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
        perlinMTHi = serializedObject.FindProperty("PerlinMTHi");
        perlinMTLow = serializedObject.FindProperty("PerlinMTLow");
        perlinTileSize = serializedObject.FindProperty("PerlinTileSize");
        amplitude = serializedObject.FindProperty("Amplitude");
        wavelength = serializedObject.FindProperty("Wavelength");
        time = serializedObject.FindProperty("Time");
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
        trigVals = EditorGUILayout.Foldout(trigVals, "Trigonometric Values");
        if (trigVals)
        {
            EditorGUILayout.IntSlider(time, 1, 100, new GUIContent("Time"));
            EditorGUILayout.Slider(amplitude, 1.0f, 600.0f, new GUIContent("Amplitude"));
            EditorGUILayout.Slider(wavelength, 1.0f, 5f, new GUIContent("Wavelength"));
            if (GUILayout.Button("Set Default Trigonometric")) { 
                terrain.SetDefaultsTrig(); 
            }
        }

        //GUI for perlin functions.
        perlinVals = EditorGUILayout.Foldout(perlinVals, "Perlin Noise Values");
        if (perlinVals)
        {
            EditorGUILayout.Slider(perlinMTHi, 10.0f, 600.0f, new GUIContent("Perlin MT High"));
            EditorGUILayout.Slider(perlinMTLow, -100.0f, 9.8f, new GUIContent("Perlin MT Low"));
            EditorGUILayout.IntSlider(perlinTileSize, 1, 100, new GUIContent("Perlin Tile Size"));
            if (GUILayout.Button("Set Default Perlin Noise")) { 
                terrain.SetDefaultsPerlin(); 
            }
        }

        //GUI for multiperlin functions.
        multiPerlin = EditorGUILayout.Foldout(multiPerlin, "Multi Perlin Noise Values");
        if (multiPerlin)
        {
            EditorGUILayout.IntSlider(mPerlinOctaves, 1, 8, new GUIContent("Octaves"));
            EditorGUILayout.Slider(mPerlinFreq, 1.0f, 16.0f, new GUIContent("Frequency"));
            EditorGUILayout.Slider(mPerlinAmp, 0.1f, 1.0f, new GUIContent("Amplitude"));
            EditorGUILayout.Slider(mPerlinPersistence, .1f, 1.0f, new GUIContent("Persistence"));
            EditorGUILayout.Slider(mPerlinLacunarity, 1.0f, 2.0f, new GUIContent("Lacunarity"));
            if (GUILayout.Button("Set Default Multiple Perlin Noise")) {
                terrain.SetDefaultsMPerlin(); 
            }
        }


        //GUI for change buttons
        EditorGUILayout.LabelField("Apply Terrain", EditorStyles.label);
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Apply Perlin Noise")) { 
            terrain.PerlinNoiseTerrain(); 
        }
        if (GUILayout.Button("Apply Multiple Perlin Noise")) {
            terrain.MultiPerlinTerrain(); 
        }
        if (GUILayout.Button("Apply TrigonoMetric")) {
            terrain.TrigTerrain(); 
        }
        if (GUILayout.Button("Reset Terrain")) {
            terrain.ResetTerrain(); 
        }
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
