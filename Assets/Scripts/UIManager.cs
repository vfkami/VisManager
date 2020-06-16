using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [Header("Leitura da Base")]
    public GameObject divLabels;
    public GameObject divTypes;
    public GameObject divContent;
    public GameObject divPath;
    public Scrollbar scroll1;

    [Header("Seleção de Atributos")] 
    public GameObject divFilteredContent;
    public Dropdown dpdAttributes;
    public GameObject sliderTemplate;
    public GameObject groupBoxTemplate;
    public GameObject anchor;
    
    
    private List<string[]> _dataset = new List<string[]>();
    private List<string> _datasetLabel = new List<string>();
    private List<Type> _labelTypes = new List<Type>();
    private string _path;

    private List<GameObject> _filters = new List<GameObject>();

    private void UpdateUI()
    {
        _datasetLabel = GetComponent<DatasetReader>().GetDatabaseLabel();
        _labelTypes = GetComponent<DatasetReader>().GetLabelTypes();
        _dataset = GetComponent<DatasetReader>().GetDataset();
        _path = GetComponent<DatasetReader>().GetDatabasePath();

        divLabels.GetComponentInChildren<Text>().text = "Database Labels: ";
        divTypes.GetComponentInChildren<Text>().text = "Content Types: "; 
        divContent.GetComponentInChildren<Text>().text = "Database Content: \n";
        divPath.GetComponentInChildren<Text>().text = "Database Path: " + _path;
        
        GetComponent<FilterBehavior>().SetDatabase(_dataset, _labelTypes);
        DestroyAllFilters();

        foreach (string label in _datasetLabel)
        {
            divLabels.GetComponentInChildren<Text>().text += label + " | ";
        }
        
        foreach (Type tipo in _labelTypes)
        {
            divTypes.GetComponentInChildren<Text>().text += tipo.ToString() + " | ";
        }

        for(int i = 0; i < _dataset.Count/2; i++)
        {
            string[] line = _dataset[i];
            
            foreach (string atribute in line)
            {
                divContent.GetComponentInChildren<Text>().text += atribute + " | ";
            }
            divContent.GetComponentInChildren<Text>().text += "\n";
        }
        
        UpdateDropdownOptions();
    }
    public void CallByButton1()
    {
        GetComponent<DatasetReader>().ReadTextFile("Assets/Databases/carros_teste3.csv");
        UpdateUI();
    }
    public void CallByButton2()
    {
        GetComponent<DatasetReader>().ReadTextFile("Assets/Databases/iris.csv");
        UpdateUI();
    }
    public void CallByButton3()
    {
        GetComponent<DatasetReader>().ReadTextFile("Assets/Databases/pokemon.csv");
        UpdateUI();
    }
    public void CallByButton4()
    {
        GetComponent<DatasetReader>().ReadTextFile("Assets/Databases/matrix_scatterplot.csv");
        UpdateUI();
    }
    public void CallByButton5()
    {
        GetComponent<DatasetReader>().ReadTextFile("Assets/Databases/ufpapredios_fake.csv");
        UpdateUI();
        DestroyAllFilters();
    }

    public void UpdateDropdownOptions()
    {
        dpdAttributes.options = new List<Dropdown.OptionData>();
        
        foreach (string label in _datasetLabel)
        {
            Dropdown.OptionData tempData = new Dropdown.OptionData(label);
            dpdAttributes.options.Add(tempData);
        }
    }

    public void UpdateFilter()
    {
        int index = dpdAttributes.value;
        Type tipo = _labelTypes[index];
        bool exist = false;
        GameObject newFilter = new GameObject("Filter_" + index);

        if (_filters.Count != 0)
        {
            foreach (var filter in _filters)
            {
                filter.SetActive(false);
                
                if (filter.name != "Filter_" + index) continue;
                
                exist = true;
                filter.SetActive(true);
            }
        }

        if (exist) return;
        
        newFilter = tipo != typeof(string) ? InitializeMinMaxSlider(index) : InitializeCheckGroup(index);
        newFilter.name = "Filter_" + index;
        
        _filters.Add(newFilter);
    }

    private GameObject InitializeMinMaxSlider(int index)
    {
        Vector2 minMax = GetComponent<ProjectUtils>().GetMinMaxValues(index, _dataset);
        GameObject slider = Instantiate(sliderTemplate, anchor.transform);
        
        slider.GetComponent<MinMaxSliderBehavior>().UpdateMinMaxSlider(index, minMax);
        return slider;
    }

    private GameObject InitializeCheckGroup(int index)
    {
        GameObject checkGroup = Instantiate(groupBoxTemplate, anchor.transform);
        List<string> categories = GetComponent<ProjectUtils>().GetAttributes(index, _dataset);
        
        checkGroup.GetComponent<TgroupBehavior>().UpdateCheckBoxes(index, categories);
        return checkGroup;
    }

    public void DestroyAllFilters()
    {
        foreach (var filter in _filters)
        {
            Destroy(filter);
        }
        _filters = new List<GameObject>();
        GetComponent<FilterBehavior>().DestroyFilterList();
    }
    
    public void RefreshVisualization()
    {
        List<string[]> filteredDatabase = GetComponent<FilterBehavior>().GenerateFilteredDatabase();
        
        divFilteredContent.GetComponentInChildren<Text>().text = "Filtered Content: \n";
        for(int i = 0; i < filteredDatabase.Count; i++)
        {
            string[] line = filteredDatabase[i];
            foreach (string atribute in line)
            {
                divFilteredContent.GetComponentInChildren<Text>().text += atribute + " | ";
            }
            divFilteredContent.GetComponentInChildren<Text>().text += "\n";
        }
        
        
    }
}
