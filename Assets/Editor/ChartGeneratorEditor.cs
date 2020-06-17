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

    ReadDatabase datasetbuffer = new ReadDatabase(); // TODO: buffer every dataset on a object (big think: is it possible to save on disk pkl like?)

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
                    string[] datasetpaths = { "outro", ReadDatabase.file1, ReadDatabase.file3};
                    string[] datasetnames = { "outro", "iris", "carros"};
                    script.indexdataset = EditorGUILayout.Popup("Select dataset", script.indexdataset, datasetnames);
                    if (script.indexdataset == 0)
                    {
                        string temppath = EditorGUILayout.TextField("dataset name:", datasetbuffer.path);
                        if (temppath != datasetbuffer.path)
                        {
                            datasetbuffer = new ReadDatabase(temppath);
                            datasetbuffer.loadfile();
                        }
                    } else { 
                        if (!datasetpaths[script.indexdataset].Equals(datasetbuffer.path))
                        {
                            datasetbuffer = new ReadDatabase(datasetpaths[script.indexdataset]);
                            datasetbuffer.loadfile();
                        }
                    }

                    script.xlabel = EditorGUILayout.TextField("Label X", script.xlabel);
                    script.xindex = EditorGUILayout.Popup("Column for X label", script.xindex, datasetbuffer.columnnames);
                    script.x = System.String.Join(",", datasetbuffer.getColumn(datasetbuffer.columnnames[script.xindex]));
                    EditorGUILayout.Space();

                    script.ylabel = EditorGUILayout.TextField("Label Y", script.ylabel);
                    script.yindex = EditorGUILayout.Popup("Column for Y label", script.yindex, datasetbuffer.columnnames);
                    script.y = System.String.Join(",", datasetbuffer.getColumn(datasetbuffer.columnnames[script.yindex]));
                    EditorGUILayout.Space();

                    if (script.maxdimensions() > 2)
                    {
                        script.zlabel = EditorGUILayout.TextField("Label Z", script.zlabel);
                        script.zindex = EditorGUILayout.Popup("Column for Z label", script.zindex, datasetbuffer.columnnames);
                        script.z = System.String.Join(",", datasetbuffer.getColumn(datasetbuffer.columnnames[script.zindex]));
                        EditorGUILayout.Space();
                    }

                    if (script.maxdimensions() > 3)
                    {
                        script.wlabel = EditorGUILayout.TextField("Label W", script.wlabel);
                        script.windex = EditorGUILayout.Popup("Column for W label", script.windex, datasetbuffer.columnnames);
                        script.w = System.String.Join(",", datasetbuffer.getColumn(datasetbuffer.columnnames[script.windex]));
                        EditorGUILayout.Space();
                    }

                    gencolors();
                    break;
            }
        }


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
                script.colors[0] = EditorGUILayout.ColorField("Bars color", script.colors[0]);
                break;
            case ChartGenerator.ChartType.LineChart:
            case ChartGenerator.ChartType.PieChart:
            case ChartGenerator.ChartType.AreaChart:
            case ChartGenerator.ChartType.Scatterplot:
                for (int i = 0; i < newc; i++)
                    script.colors[i] = EditorGUILayout.ColorField("Color " + i, script.colors[i]);
                break;
        }
    }
}
