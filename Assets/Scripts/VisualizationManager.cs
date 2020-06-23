using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class VisualizationManager : MonoBehaviour
{
    public GameObject manager;
    
    private List<string[]> _dataset;
    private List<string> _datasetLabel = new List<string>();
    private List<Type> _labelTypes = new List<Type>();
    
    public void GenerateVisualization(ChartGenerator.ChartType visType, bool isFiltredVisualization, string visName)
    {
        print(visName);
        GameObject canvas = GameObject.Find("Canvas");
        List<int> indexOfSelectedAttributes = canvas.GetComponent<UIManager>().GetIndexOfSelectedAttributes();

        _dataset = isFiltredVisualization ? 
            manager.GetComponent<FilterManager>().GetFilteredDatabase() : manager.GetComponent<DatasetReader>().GetDataset();
        
        _datasetLabel = new List<string>(manager.GetComponent<DatasetReader>().GetLabels());
        _labelTypes = new List<Type>(manager.GetComponent<DatasetReader>().GetTypes());
        
        List<string> chartAttributes = new List<string>();

        foreach (int index in indexOfSelectedAttributes)
        {
            string temp = "";
            for (int i = 0; i < _dataset.Count; i++)
            {
                temp += _dataset[i][index];
                if (i + 1 == _dataset.Count)
                    chartAttributes.Add(temp);
                else
                    temp += ",";
            }
        }

        GetComponent<ChartGenerator>().title = visName;
        GetComponent<ChartGenerator>().charttype = visType;

        GetComponent<ChartGenerator>().x = chartAttributes[0];
        GetComponent<ChartGenerator>().xlabel = _datasetLabel[indexOfSelectedAttributes[0]];

        GetComponent<ChartGenerator>().y = chartAttributes[1];
        GetComponent<ChartGenerator>().ylabel = _datasetLabel[indexOfSelectedAttributes[1]];

        GetComponent<ChartGenerator>().getchart();

        if (indexOfSelectedAttributes.Count < 3) return;
        if (chartAttributes.Count < 3) return;
        
        GetComponent<ChartGenerator>().z = chartAttributes[2];
        GetComponent<ChartGenerator>().zlabel = _datasetLabel[indexOfSelectedAttributes[2]];
        GetComponent<ChartGenerator>().getchart();

        if (chartAttributes.Count != 4) return;
        
        GetComponent<ChartGenerator>().w = chartAttributes[3];
        GetComponent<ChartGenerator>().wlabel = _datasetLabel[indexOfSelectedAttributes[3]];
        GetComponent<ChartGenerator>().getchart();
    }
}
