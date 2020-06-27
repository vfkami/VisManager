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
    // esse script gerencia a cena UIScene todos os gameobjects são publicos pois
    // são instanciados no editor. Esse script liga os objetos do canvas aos 
    // scripts que gerenciam a base de dados;
    
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
    public GameObject dpdVizTypes;
    public GameObject[] attDropdowns;
    public GameObject ifChartName;
    public GameObject genVizD;
    public GameObject genVizF;
    
    private List<string[]> _dataset = new List<string[]>();
    private List<string> _datasetLabel = new List<string>();
    private List<Type> _labelTypes = new List<Type>();
    private string _path;

    private List<GameObject> _filters = new List<GameObject>();


    private void SetNewDatabase()
    {
        _datasetLabel = manager.GetComponent<DatasetReader>().GetLabels();
        _labelTypes = manager.GetComponent<DatasetReader>().GetTypes();
        _dataset = manager.GetComponent<DatasetReader>().GetDataset();
        _path = manager.GetComponent<DatasetReader>().GetDatabasePath();
        
        UpdateDivsContent();
        UpdateInterface();
    }
    
    private void UpdateInterface()
    { // chama algumas funções para atualizar os valores presentes na interface
        manager.GetComponent<FilterManager>().SetDatabase(_dataset, _labelTypes, _datasetLabel);
        DestroyAllFilters();
        PopulateAxisDropdowns(false);
        UpdateDropdownOptionsFilter();
    }
    
    private void UpdateDivsContent()
    {
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
    }
    
    
    // Utilizada na Aba 2 para atualizar o dropdown de acordo com os atributos da base para que 
    // os filtros sejam realizados corretamente
    
    public void UpdateDropdownOptionsFilter()
    {
        Dropdown.OptionData tempData = new Dropdown.OptionData("- selecione -");
        dpdAttributes.options = new List<Dropdown.OptionData> {tempData};
        
        foreach (string label in _datasetLabel)
        {
            tempData = new Dropdown.OptionData(label);
            dpdAttributes.options.Add(tempData);
        }
    }
    
    // insere um novo filtro na lista de filtros em 
    // <filtermanager>.UpdateListOfFilters
    // a cada filtro novo criado

    public void UpdateFilter()
    {
        int index = dpdAttributes.value - 1; 
        // -1 pq o item 0 sempre é " - selecione", que não deve 
        // ser utilizado fazendo com que a array, AQUI, comece em 1

        if (index < 0)
        {
            print("selecione pelo menos um filtro");
            return;
        }
        
        Type tipo = _labelTypes[index];
        bool exist = false;

        if (_filters.Count != 0)
        {
            foreach (var filter in _filters)
            {
                // verifica se o filtro selecionado pelo usuário já foi instanciado
                // anteriormente e desliga a visualização dos que não forem selecionados.
                filter.SetActive(false);
                
                if (filter.name != "Filter_" + index) continue;
                exist = true;
                filter.SetActive(true);
            }
        }

        if (exist) return; 
        // se não existe, instancia novo filtro
        
        GameObject newFilter = tipo != typeof(string) ? InitializeMinMaxSlider(index) : InitializeCheckGroup(index);
        newFilter.name = "Filter_" + index;
        _filters.Add(newFilter);
    }
    
    // funções utilizadas para instanciar um 
    // minmaxslider = filtro continuo
    // checkgroup = filtro categórico 

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
        divFilteredContent.GetComponentInChildren<Text>().text = "Filtered Database: ";  
        foreach (var filter in _filters)
        {
            Destroy(filter);
        }
        _filters = new List<GameObject>();
        manager.GetComponent<FilterManager>().DestroyFilterList();
    }
    
    // na segunda aba, para atualizar o valor da div que contém a base filtrada.
    public void UpdateDivOfFilteredDatabase()
    {
        List<string[]> filteredDatabase = manager.GetComponent<FilterManager>().GetFilteredDatabase();

        if (filteredDatabase == null)
        {
            print("selecione pelo menos um filtro");
            return;
        }
        
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

    // usado na terceira aba, quando o dropdown que contem os tipos da visualização é utilizado  
    public void OnVizDropdownValueChanged()
    {
        int index = dpdVizTypes.GetComponent<Dropdown>().value;
        if (index == 0) return;

        switch (index)
        {
            case 1:
                PopulateAxisDropdowns(false);
                attDropdowns[0].GetComponent<Dropdown>().interactable = true;
                attDropdowns[1].GetComponent<Dropdown>().interactable = true;
                attDropdowns[2].GetComponent<Dropdown>().interactable = false;
                attDropdowns[3].GetComponent<Dropdown>().interactable = false;
                break;
            
            case 2:
                PopulateAxisDropdowns(false);
                attDropdowns[0].GetComponent<Dropdown>().interactable = true;
                attDropdowns[1].GetComponent<Dropdown>().interactable = true;
                attDropdowns[2].GetComponent<Dropdown>().interactable = false;
                attDropdowns[3].GetComponent<Dropdown>().interactable = false;
                break;
            
            case 3:
                PopulateAxisDropdowns(false);
                attDropdowns[0].GetComponent<Dropdown>().interactable = true;
                attDropdowns[1].GetComponent<Dropdown>().interactable = true;
                attDropdowns[2].GetComponent<Dropdown>().interactable = true;
                attDropdowns[3].GetComponent<Dropdown>().interactable = false;
                break;
            
            case 4:
                PopulateAxisDropdowns(false);
                attDropdowns[0].GetComponent<Dropdown>().interactable = true;
                attDropdowns[1].GetComponent<Dropdown>().interactable = true;
                attDropdowns[2].GetComponent<Dropdown>().interactable = true;
                attDropdowns[3].GetComponent<Dropdown>().interactable = false;
                break;
            
            case 5:
                PopulateAxisDropdowns(true);
                attDropdowns[0].GetComponent<Dropdown>().interactable = true;
                attDropdowns[1].GetComponent<Dropdown>().interactable = true;
                attDropdowns[2].GetComponent<Dropdown>().interactable = true;
                attDropdowns[3].GetComponent<Dropdown>().interactable = true;
                break;
        }
    }

    // utilizado para definir a label de cada atributos que irá em cada dropdown 
    // referente aos eixos da visualização. Se for scatterplot, tudo continuo
    private void PopulateAxisDropdowns(bool isScatterplot)
    {
        List<string> listOfOptions;
        Dropdown.OptionData tempData;

        foreach (GameObject dpd in attDropdowns)
            dpd.GetComponent<Dropdown>().options = new List<Dropdown.OptionData>();
        
        if (isScatterplot)
        { 
            listOfOptions = new List<string>
                (manager.GetComponent<ProjectUtils>().GetContinuumAtributes(_labelTypes, _datasetLabel));
            
            foreach (GameObject dpd in attDropdowns)
            {
                tempData = new Dropdown.OptionData("- contínuo -");
                dpd.GetComponent<Dropdown>().options = new List<Dropdown.OptionData> {tempData};

                foreach (string option in listOfOptions)
                {
                    tempData = new Dropdown.OptionData(option);
                    dpd.GetComponent<Dropdown>().options.Add(tempData);
                }
            }
            return;
        }
        
        for (int i = 0; i < attDropdowns.Length; i++)
        {
            if (i == 0 || i == 2)
            {
                tempData = new Dropdown.OptionData("- categorico -");
                listOfOptions = new List<string>
                    (manager.GetComponent<ProjectUtils>().GetCategoricAttributes(_labelTypes, _datasetLabel));
            }
            else
            {
                tempData = new Dropdown.OptionData("- contínuo -");
                listOfOptions = new List<string>
                    (manager.GetComponent<ProjectUtils>().GetContinuumAtributes(_labelTypes, _datasetLabel));
            }

            attDropdowns[i].GetComponent<Dropdown>().options = new List<Dropdown.OptionData> {tempData};

            foreach (string option in listOfOptions)
            {
                tempData = new Dropdown.OptionData(option);
                attDropdowns[i].GetComponent<Dropdown>().options.Add(tempData);
            }
        }
    }

    // na terceira aba, por um dos dois botões que chamam as visualizações
    public void VisualizationCall(bool isFiltred)
    {
        int index = dpdVizTypes.GetComponent<Dropdown>().value;
        string nameLabel = ifChartName.GetComponent<InputField>().text;

        if (nameLabel == "")
        {
            nameLabel = "new chart"; //default name
        }

        GameObject visObject = isFiltred ? GameObject.Find("GenVizF") : GameObject.Find("GenVizD");
        string filter = isFiltred ? manager.GetComponent<FilterManager>().GetFilters() : "";

        ChartGenerator.ChartType chartType = ChartGenerator.ChartType.BarChartVertical;
        
        switch (index)
        {
            case 1:
                chartType = ChartGenerator.ChartType.BarChartVertical;
                break;
            
            case 2:
                chartType = ChartGenerator.ChartType.PieChart;
                break;
            
            case 3:
                chartType = ChartGenerator.ChartType.LineChart;
                break;
            
            case 4:
                chartType = ChartGenerator.ChartType.AreaChart;
                break;
            
            case 5:
                chartType = ChartGenerator.ChartType.Scatterplot;
                break;
        }
        
        List<int> selectedAttributes = new List<int>(GetIndexOfSelectedAttributes());
        visObject.GetComponent<VisualizationManager>().GenerateVisualization(chartType, nameLabel, filter, selectedAttributes);
    }

    // usado para retornar uma lista de inteiros contendo
    // o index de cada atributos selecionado nos AxisDropdowns
    public List<int> GetIndexOfSelectedAttributes()
    {
        List<int> list = new List<int>();
        
        foreach (var dropdown in attDropdowns)
        {
            if (!dropdown.gameObject.activeSelf) continue;

            int index = dropdown.GetComponent<Dropdown>().value; // cause 0 is always default

            if (index == 0) continue;

            string label = dropdown.GetComponent<Dropdown>().options[index].text;
            int trueIndex = manager.GetComponent<ProjectUtils>().GetIndexOfDropdownOption(label, _datasetLabel);
            list.Add(trueIndex);
        }
        
        return list;
    }
    // Na primeira aba, os botões que possuem o nome das bases de dados chamam cada uma função abaixo
    // para que a mesma seja lida e utilizada corretamente no projeto 
    public void CallByButton1()
    {
        manager.GetComponent<DatasetReader>().ReadFile("Assets/Databases/carros_teste3.csv");
        genVizD.GetComponent<VisualizationManager>().datasetname = "cars";
        genVizF.GetComponent<VisualizationManager>().datasetname = "cars";
        SetNewDatabase();
    }
    public void CallByButton2()
    {
        manager.GetComponent<DatasetReader>().ReadFile("Assets/Databases/iris.csv");
        genVizD.GetComponent<VisualizationManager>().datasetname = "iris";
        genVizF.GetComponent<VisualizationManager>().datasetname = "iris";
        SetNewDatabase();
        
    }
    public void CallByButton3()
    {
        manager.GetComponent<DatasetReader>().ReadFile("Assets/Databases/automobile.csv");
        genVizD.GetComponent<VisualizationManager>().datasetname = "automobile";
        genVizF.GetComponent<VisualizationManager>().datasetname = "automobile";
        SetNewDatabase();
    }
    
    public void CallByButton4()
    {
        manager.GetComponent<DatasetReader>().ReadFile("Assets/Databases/ufpapredios_fake.csv");
        genVizD.GetComponent<VisualizationManager>().datasetname = "ufpa";
        genVizF.GetComponent<VisualizationManager>().datasetname = "ufpa";
        SetNewDatabase();
    }
}
