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
    public GameObject manager;
    
    [Header("Leitura da Base")]
    public GameObject divLabels;
    public GameObject divTypes;
    public GameObject divContent;
    public GameObject divPath;

    [Header("Filtragem")] 
    public GameObject divFilteredContent;
    public Dropdown dpdAttributes;
    public GameObject sliderTemplate;
    public GameObject groupBoxTemplate;
    public GameObject anchor;

    [Header("Viz Gen")] 
    public GameObject visManager;
    public GameObject genViz;
    public GameObject dpdVizTypes;
    public GameObject[] attDropdowns;


    private List<string[]> _dataset = new List<string[]>();
    private List<string> _datasetLabel = new List<string>();
    private List<Type> _labelTypes = new List<Type>();
    private string _path;

    private List<GameObject> _filters = new List<GameObject>();

    private void UpdateUI()
    {
        _datasetLabel = manager.GetComponent<DatasetReader>().GetLabels();
        _labelTypes = manager.GetComponent<DatasetReader>().GetTypes();
        _dataset = manager.GetComponent<DatasetReader>().GetDataset();
        _path = manager.GetComponent<DatasetReader>().GetDatabasePath();

        divLabels.GetComponentInChildren<Text>().text = "Database Labels: ";
        divTypes.GetComponentInChildren<Text>().text = "Content Types: "; 
        divContent.GetComponentInChildren<Text>().text = "Database Content: \n";
        divPath.GetComponentInChildren<Text>().text = "Database Path: " + _path;

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
        CallEssentialFunctions();
    }

    private void CallEssentialFunctions()
    {
        manager.GetComponent<FilterManager>().SetDatabase(_dataset, _labelTypes);
        DestroyAllFilters();
        PopulateVizDropdown();
        UpdateDropdownOptions();
    }
    
    public void CallByButton1()
    {
        manager.GetComponent<DatasetReader>().ReadFile("Assets/Databases/carros_teste3.csv");
        UpdateUI();
    }
    public void CallByButton2()
    {
        manager.GetComponent<DatasetReader>().ReadFile("Assets/Databases/iris.csv");
        UpdateUI();
    }
    public void CallByButton3()
    {
        manager.GetComponent<DatasetReader>().ReadFile("Assets/Databases/pokemon.csv");
        UpdateUI();
    }
    public void CallByButton4()
    {
        manager.GetComponent<DatasetReader>().ReadFile("Assets/Databases/matrix_scatterplot.csv");
        UpdateUI();
    }
    public void CallByButton5()
    {
        manager.GetComponent<DatasetReader>().ReadFile("Assets/Databases/ufpapredios_fake.csv");
        UpdateUI();
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
        Vector2 minMax = manager.GetComponent<ProjectUtils>().GetMinMaxValues(index, _dataset);
        GameObject slider = Instantiate(sliderTemplate, anchor.transform);
        
        slider.GetComponent<MinMaxSliderBehavior>().UpdateMinMaxSlider(index, minMax);
        return slider;
    }

    private GameObject InitializeCheckGroup(int index)
    {
        GameObject checkGroup = Instantiate(groupBoxTemplate, anchor.transform);
        List<string> categories = manager.GetComponent<ProjectUtils>().GetAttributes(index, _dataset);
        
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
        manager.GetComponent<FilterManager>().DestroyFilterList();
    }

    public void OnVizDropdownValueChanged()
    {
        int index = dpdVizTypes.GetComponent<Dropdown>().value;
        if (index == 0) return;

        switch (index)
        {
            case 1:
                attDropdowns[0].GetComponent<Dropdown>().interactable = true;
                attDropdowns[1].GetComponent<Dropdown>().interactable = true;
                attDropdowns[2].GetComponent<Dropdown>().interactable = false;
                attDropdowns[3].GetComponent<Dropdown>().interactable = false;
                break;
            
            case 2:
                attDropdowns[0].GetComponent<Dropdown>().interactable = true;
                attDropdowns[1].GetComponent<Dropdown>().interactable = true;
                attDropdowns[2].GetComponent<Dropdown>().interactable = false;
                attDropdowns[3].GetComponent<Dropdown>().interactable = false;
                break;
            
            case 3:
                attDropdowns[0].GetComponent<Dropdown>().interactable = true;
                attDropdowns[1].GetComponent<Dropdown>().interactable = true;
                attDropdowns[2].GetComponent<Dropdown>().interactable = false;
                attDropdowns[3].GetComponent<Dropdown>().interactable = false;
                break;
            
            case 4:
                attDropdowns[0].GetComponent<Dropdown>().interactable = true;
                attDropdowns[1].GetComponent<Dropdown>().interactable = true;
                attDropdowns[2].GetComponent<Dropdown>().interactable = false;
                attDropdowns[3].GetComponent<Dropdown>().interactable = false;
                break;
            
            case 5:
                attDropdowns[0].GetComponent<Dropdown>().interactable = true;
                attDropdowns[1].GetComponent<Dropdown>().interactable = true;
                attDropdowns[2].GetComponent<Dropdown>().interactable = false;
                attDropdowns[3].GetComponent<Dropdown>().interactable = true;
                break;
        }
    }

    private void PopulateVizDropdown()
    {
        List<string> tempListOfAttributes = new List<string>();

        foreach (GameObject dpd in attDropdowns)
            dpd.GetComponent<Dropdown>().options = new List<Dropdown.OptionData>();
        
        for (int i = 0; i < attDropdowns.Length; i++)
        {
            if (i == 0 || i == 2)
                tempListOfAttributes = new List<string>
                    (manager.GetComponent<ProjectUtils>().GetCategoricAttributes(_labelTypes, _datasetLabel));

            else
                tempListOfAttributes = new List<string>
                    (manager.GetComponent<ProjectUtils>().GetContinuumAtributes(_labelTypes, _datasetLabel));

            
            Dropdown.OptionData tempData = new Dropdown.OptionData("- selecione -");
            attDropdowns[i].GetComponent<Dropdown>().options.Add(tempData);

            foreach (string label in tempListOfAttributes)
            {
                tempData = new Dropdown.OptionData(label);
                attDropdowns[i].GetComponent<Dropdown>().options.Add(tempData);
            }
        }
    }
    
    public void UpdateFilteredDatabaseDiv()
    {
        List<string[]> filteredDatabase = manager.GetComponent<FilterManager>().GetFilteredDatabase();
        
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
