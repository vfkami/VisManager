using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class FilterBehavior : MonoBehaviour
{
    private Dictionary<int, List<string>> _filterValues = new Dictionary<int, List<string>>();
    private List<string[]> _database;
    private List<Type> _attributeTypes;
    
    public void SetDatabase(List<string[]> originalDatabase, List<Type> attTypes)
    {
        _database = new List<string[]>(originalDatabase);
        _attributeTypes = new List<Type>(attTypes);
    }

    public void UpdateFilterListByIndex(int index, List<string> filteredParameters)
    {
        try
        {
            _filterValues.Add(index, filteredParameters);
        }
        catch (ArgumentException)
        {
            _filterValues[index] = filteredParameters;
        }
    }
    
    public void DestroyFilterList()
    {
        _filterValues = new Dictionary<int, List<string>>();
    }

    public List<string[]> GenerateFilteredDatabase()
    {
        List<string[]> _filteredDatabase = new List<string[]>();

        //para cada linha na base de dados original
        foreach (string[] line in _database)
        {
            bool find = false;
            
            //para cada filtro na lista de filtros
            foreach (KeyValuePair<int, List<string>> filter in _filterValues)
            {
                //para cada parâmetro dentro do filtro atual
                foreach (string parameter in filter.Value)
                {
                    if (_attributeTypes[filter.Key] != typeof(string))
                    {
                        float value = float.Parse(line[filter.Key],CultureInfo.InvariantCulture);
                        float minValue = float.Parse(filter.Value[0], CultureInfo.InvariantCulture);
                        float maxValue = float.Parse(filter.Value[1], CultureInfo.InvariantCulture);
                        bool invSelection = Convert.ToBoolean(filter.Value[2]);
                                
                        //seleção invertida
                        if (invSelection)
                        {
                            print("Seleção invertida aplicada");
                            if (!(value < minValue || value > maxValue))
                            {
                                find = true;
                                break;
                            }
                        }
                        else
                        {
                            if (!(value >= minValue && value <= maxValue))
                            {
                                find = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (line[filter.Key] == parameter)
                        {
                            find = true;
                            break;
                        }
                    }
                }
            }

            if (!find)
                _filteredDatabase.Add(line);
        }

        return _filteredDatabase;
    }
    
}
