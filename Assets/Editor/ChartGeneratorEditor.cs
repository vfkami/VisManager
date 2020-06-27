using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(ChartGenerator))]
[System.Serializable]
public class ChartGeneratorEditor : Editor
{
    [SerializeField]
    ChartGenerator script;

    // ReadDatabaseGenVis datasetbuffer = new ReadDatabaseGenVis(); // TODO: buffer every dataset on a object (big think: is it possible to save on disk pkl like?)

    void OnEnable()
    {
        script = (ChartGenerator) target;
    }

    public override void OnInspectorGUI()
    {
        script.datatype = (ChartGenerator.DataType)EditorGUILayout.EnumPopup("Data: ", script.datatype);
        EditorGUILayout.Space();

        script.autostart = EditorGUILayout.Toggle("Auto start", script.autostart);
        script.autoupdate = EditorGUILayout.Toggle("Auto update", script.autoupdate);
        script.charttype = (ChartGenerator.ChartType)EditorGUILayout.EnumPopup("Visualization", script.charttype);
        script.title = EditorGUILayout.TextField("Title", script.title);
        EditorGUILayout.Space();

        switch (script.datatype) {
                case ChartGenerator.DataType.Manual:
                {
                    script.xlabel = EditorGUILayout.TextField("Label X", script.xlabel);
                    script.x = EditorGUILayout.TextField("x", script.x);
                    EditorGUILayout.LabelField("Usage example", "pear,orange,pineapple,blueberry");
                    EditorGUILayout.Space();

                    script.ylabel = EditorGUILayout.TextField("Label Y", script.ylabel);
                    script.y = EditorGUILayout.TextField("y", script.y);
                    EditorGUILayout.LabelField("Usage example", "1,2,3,4");
                    EditorGUILayout.Space();

                    if (script.maxdimensions() > 2)
                    {
                        script.zlabel = EditorGUILayout.TextField("Label Z", script.zlabel);
                        script.z = EditorGUILayout.TextField("z", script.z);
                        EditorGUILayout.LabelField("Usage example", "america,america,europe,europe");
                        EditorGUILayout.Space();
                    }

                    if (script.maxdimensions() > 3)
                    {
                        script.wlabel = EditorGUILayout.TextField("Label w", script.wlabel);
                        script.w = EditorGUILayout.TextField("w", script.w);
                        EditorGUILayout.LabelField("Usage example", "1,3,5,7");
                        EditorGUILayout.Space();
                    }

                    gencolors();

                break;
            }
            case ChartGenerator.DataType.Placeholder:
            {
                    break;
            }
            case ChartGenerator.DataType.Dataset:
            {
                    string[] datasetnames = { "iris", "automobile"};
                    script.indexdataset = EditorGUILayout.Popup("Select dataset", script.indexdataset, datasetnames);
                    script.dataset_name = datasetnames[script.indexdataset];
                    
                    script.x = EditorGUILayout.TextField("Column X", script.x);
                    script.xlabel = EditorGUILayout.TextField("Label X", script.xlabel);
                    EditorGUILayout.Space();

                    script.y = EditorGUILayout.TextField("Column Y", script.y);
                    script.ylabel = EditorGUILayout.TextField("Label Y", script.ylabel);
                    EditorGUILayout.Space();

                    if (script.maxdimensions() > 2)
                    {
                        script.z = EditorGUILayout.TextField("Column Z", script.z);
                        script.zlabel = EditorGUILayout.TextField("Label Z", script.zlabel);
                        EditorGUILayout.Space();
                    }

                    if (script.maxdimensions() > 3)
                    {
                        script.w = EditorGUILayout.TextField("Column W", script.w);
                        script.wlabel = EditorGUILayout.TextField("Label W", script.wlabel);
                        EditorGUILayout.Space();
                    }

                    gencolors();
                    break;
            }
        }
        script.filter = EditorGUILayout.TextField("Filter", script.filter);

        if (EditorApplication.isPlaying)
            Repaint();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
            serializedObject.ApplyModifiedProperties();
        }
    }

    public void gencolors()
    {
        var newc = script.numcolors();
        switch (script.charttype)
        {
            case ChartGenerator.ChartType.BarChartVertical:
                // script.colors[0] = EditorGUILayout.ColorField("Bars color", script.colors[0]);
                // break;
            case ChartGenerator.ChartType.LineChart:
            case ChartGenerator.ChartType.PieChart:
            case ChartGenerator.ChartType.AreaChart:
            case ChartGenerator.ChartType.Scatterplot:
                // for (int i = 0; i < newc; i++)
                //     script.colors[i] = EditorGUILayout.ColorField("Color " + i, script.colors[i]);
                // break;
                break;
        }
    }
}
