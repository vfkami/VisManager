using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizationManager : MonoBehaviour
{
    public GameObject genViz;
    public GameObject[] attDropdowns;
    
    private List<string[]> _dataset;
    private List<string> _datasetLabel = new List<string>();
    private List<Type> _labelTypes = new List<Type>();
    
    public void GenerateVisualization(string visType, bool isFiltredVisualization)
    {
        List<int> indexOfSelectedAttributes = new List<int>();

        _dataset = isFiltredVisualization ? 
            GetComponent<FilterManager>().GetFilteredDatabase() : GetComponent<DatasetReader>().GetDataset();

        _datasetLabel = new List<string>(GetComponent<DatasetReader>().GetLabels());
        _labelTypes = new List<Type>(GetComponent<DatasetReader>().GetTypes());
        
        
        foreach (var dropdown in attDropdowns)
        {
            if (!dropdown.gameObject.activeSelf) continue;

            int index = dropdown.GetComponent<Dropdown>().value; // cause 0 is always default

            if (index == 0) continue;

            string label = dropdown.GetComponent<Dropdown>().options[index].text;
            int trueIndex = GetComponent<ProjectUtils>().GetIndexOfDropdownOption(label, _datasetLabel);
            indexOfSelectedAttributes.Add(trueIndex);
        }

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
        
        genViz.GetComponent<ChartGenerator>().x = chartAttributes[0];
        genViz.GetComponent<ChartGenerator>().xlabel = _datasetLabel[indexOfSelectedAttributes[0]];

        genViz.GetComponent<ChartGenerator>().y = chartAttributes[1];
        genViz.GetComponent<ChartGenerator>().ylabel = _datasetLabel[indexOfSelectedAttributes[1]];

        genViz.GetComponent<ChartGenerator>().getchart();

        if (indexOfSelectedAttributes.Count <= 2) return;

        genViz.GetComponent<ChartGenerator>().z = chartAttributes[2];
        genViz.GetComponent<ChartGenerator>().zlabel = _datasetLabel[indexOfSelectedAttributes[3]];
        genViz.GetComponent<ChartGenerator>().getchart();

        if (indexOfSelectedAttributes.Count != 4) return;
        
        genViz.GetComponent<ChartGenerator>().w = chartAttributes[3];
        genViz.GetComponent<ChartGenerator>().wlabel = _datasetLabel[indexOfSelectedAttributes[3]];
        genViz.GetComponent<ChartGenerator>().getchart();
    }
}
