using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PGCTerrain))]
[CanEditMultipleObjects]
public class Editor_PGCTerrain : Editor
{
    SerializedProperty PerlinMTHi;
    SerializedProperty PerlinMTLow;
    SerializedProperty PerlinTileSize;
    SerializedProperty Time;
    SerializedProperty Amplitude;
    SerializedProperty Wavelength;
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
        PerlinMTHi = serializedObject.FindProperty("PerlinMTHi");
        PerlinMTLow = serializedObject.FindProperty("PerlinMTLow");
        PerlinTileSize = serializedObject.FindProperty("PerlinTileSize");
        Amplitude = serializedObject.FindProperty("Amplitude");
        Wavelength = serializedObject.FindProperty("Wavelength");
        Time = serializedObject.FindProperty("Time");
        mPerlinAmp = serializedObject.FindProperty("mPerlinAmp");
        mPerlinFreq = serializedObject.FindProperty("mPerlinFreq");
        mPerlinPersistence = serializedObject.FindProperty("mPerlinPersistence");
        mPerlinOctaves = serializedObject.FindProperty("mPerlinOctaves");
        mPerlinLacunarity = serializedObject.FindProperty("mPerlinLacunarity");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        PGCTerrain pgcTerrain = (PGCTerrain)target;
        EditorGUILayout.LabelField("PGC Terrain Editor", EditorStyles.boldLabel);

        //GUI for Trigonometry Functions
        trigVals = EditorGUILayout.Foldout(trigVals, "Trigonometric Values");
        if (trigVals)
        {
            EditorGUILayout.IntSlider(Time, 1, 100, new GUIContent("Time"));
            EditorGUILayout.Slider(Amplitude, 1.0f, 600.0f, new GUIContent("Amplitude"));
            EditorGUILayout.Slider(Wavelength, 1.0f, 5f, new GUIContent("Wavelength"));
            if (GUILayout.Button("Set Default Trigonometric")) { 
                pgcTerrain.SetDefaultsTrig(); 
            }
        }

        //GUI for perlin functions.
        perlinVals = EditorGUILayout.Foldout(perlinVals, "Perlin Noise Values");
        if (perlinVals)
        {
            EditorGUILayout.Slider(PerlinMTHi, 10.0f, 600.0f, new GUIContent("Perlin MT High"));
            EditorGUILayout.Slider(PerlinMTLow, -100.0f, 9.8f, new GUIContent("Perlin MT Low"));
            EditorGUILayout.IntSlider(PerlinTileSize, 1, 100, new GUIContent("Perlin Tile Size"));
            if (GUILayout.Button("Set Default Perlin Noise")) { 
                pgcTerrain.SetDefaultsPerlin(); 
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
                pgcTerrain.SetDefaultsMPerlin(); 
            }
        }


        //GUI for change buttons
        EditorGUILayout.LabelField("Apply Terrain", EditorStyles.label);
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Apply Perlin Noise")) { 
            pgcTerrain.PerlinNoiseTerrain(); 
        }
        if (GUILayout.Button("Apply Multiple Perlin Noise")) {
            pgcTerrain.MultiPerlinTerrain(); 
        }
        if (GUILayout.Button("Apply TrigonoMetric")) {
            pgcTerrain.TrigTerrain(); 
        }
        if (GUILayout.Button("Reset Terrain")) {
            pgcTerrain.ResetTerrain(); 
        }
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
