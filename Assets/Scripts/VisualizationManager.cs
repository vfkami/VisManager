using System.Collections.Generic;
using UnityEngine;

public class VisualizationManager : MonoBehaviour
{
    // passa os atributos do Canvas para o ChartGenerator
    
    public GameObject manager;
    public string datasetname;
    private List<string> _datasetLabel = new List<string>();
    public void GenerateVisualization(ChartGenerator.ChartType visType, string visName, string filter, List<int> selectedAttributes)
    {
        _datasetLabel = new List<string>(manager.GetComponent<DatasetReader>().GetLabels());

        GetComponent<ChartGenerator>().datatype = ChartGenerator.DataType.Dataset;
        GetComponent<ChartGenerator>().dataset_name = datasetname;
        GetComponent<ChartGenerator>().charttype = visType;
        GetComponent<ChartGenerator>().title = visName;
        GetComponent<ChartGenerator>().filter = filter;

        GetComponent<ChartGenerator>().x = _datasetLabel[selectedAttributes[0]];
        GetComponent<ChartGenerator>().xlabel = _datasetLabel[selectedAttributes[0]];
        GetComponent<ChartGenerator>().y = _datasetLabel[selectedAttributes[1]];
        GetComponent<ChartGenerator>().ylabel = _datasetLabel[selectedAttributes[1]];

        GetComponent<ChartGenerator>().getchart();

        if (selectedAttributes.Count < 3) return;

        GetComponent<ChartGenerator>().z = _datasetLabel[selectedAttributes[2]];
        GetComponent<ChartGenerator>().zlabel = _datasetLabel[selectedAttributes[2]];
        GetComponent<ChartGenerator>().getchart();

        if (selectedAttributes.Count != 4) return;
        
        GetComponent<ChartGenerator>().w = _datasetLabel[selectedAttributes[3]];
        GetComponent<ChartGenerator>().wlabel = _datasetLabel[selectedAttributes[3]];
        GetComponent<ChartGenerator>().getchart();
    }
}
