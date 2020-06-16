using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class PoiManagerBehavior : MonoBehaviour
{
    private GameObject[] _poiList;
    private GameObject _mainCanvas;
    
    Dictionary<int, List<string>> filterValues = 
        new Dictionary<int, List<string>>();
    
    private List<string[]> _poiInfos;
    private List<string> _poiLabels;
    private List<Type> _attributeTypes;

    private GameObject _utils;

    
    private readonly List<Color> _colorPallet = new List<Color>
    {
        new Color(0.992F, 0.749F, 0.435F),
        new Color(0.890F, 0.101F, 0.109F),
        new Color(0.121F, 0.470F, 0.705F),
        new Color(0.415F, 0.239F, 0.603F),
        new Color(0.2F, 0.627F, 0.172F),
        new Color(1F, 0.498F, 0F),
        new Color(0.984F, 0.603F, 0.6F),
        new Color(0.698F, 0.874F, 0.541F),
        new Color(0.792F, 0.698F, 0.839F),
        new Color(0.650F, 0.807F, 0.890F),
        new Color(1F, 1F, 0.6F),
        new Color(0.694F, 0.349F, 0.156F)
    };
    
    private void Start()
    {
        _poiInfos = GetComponent<DatasetReader>().GetDataset();
        _poiLabels = GetComponent<DatasetReader>().GetDatabaseLabel();
        _attributeTypes = GetComponent<DatasetReader>().GetLabelTypes();
        
        _mainCanvas = GameObject.Find("Canvas");
        _utils = GameObject.Find("Utils");
    }

    private void Update()
    {
        _poiList = GameObject.FindGameObjectsWithTag("poi");
    }
    public void RefreshVisualization()
    {
        // verifica todos os POIs da Lista
        foreach (GameObject poi in _poiList)
        {
            bool find = false;
            
            // verifica cada linha da base de dados
            foreach (string[] line in _poiInfos)
            {
                // verifica se o poi atual é o da linha consultada
                if (line[0] == poi.name) 
                {
                    //para cada filtro na lista de filtros
                    foreach (KeyValuePair<int, List<string>> filter in filterValues)
                    {
                        //para cada parâmetro dentro do atributo atual
                        foreach (string par in filter.Value)
                        {
                            if (_attributeTypes[filter.Key] != typeof(string))
                            {
                                int value = Convert.ToInt32(line[filter.Key]);
                                int minValue = Convert.ToInt32(filter.Value[0]);
                                int maxValue = Convert.ToInt32(filter.Value[1]);
                                bool invSelection = Convert.ToBoolean(filter.Value[2]);
                                
                                //seleção invertida
                                if (invSelection)
                                {
                                    print("Seleção invertida aplicada");
                                    if (!(value < minValue || value > maxValue))
                                    {
                                        poi.GetComponent<Renderer>().enabled = false;
                                        find = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (!(value >= minValue && value <= maxValue))
                                    {
                                        poi.GetComponent<Renderer>().enabled = false;
                                        find = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (line[filter.Key] == par)
                                {
                                    poi.GetComponent<Renderer>().enabled = false;
                                    find = true;
                                    break;
                                }
                            }

                        }
                    }
                }
            }
            if(!find)
                poi.GetComponent<Renderer>().enabled = true;
        }
    }
    public void UpdateFilterList(int index, List<string> parameters)
    {
        print("Aplicando filtros");
        try
        {
            filterValues.Add(index, parameters);
        }
        catch (ArgumentException)
        {
            filterValues[index] = parameters;
        }
        
        RefreshVisualization();
    }
}
