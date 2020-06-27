using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class FilterManager : MonoBehaviour
{
    private Dictionary<int, List<string>> _filterValues = new Dictionary<int, List<string>>();
    private List<string[]> _database;
    private List<string> _databaseLabels;
    private List<Type> _attributeTypes;
    private string _filter = "";

    // seta a base de dados que será utilizada para aplicar os filtros
    // é chamada toda vez que a base muda
    public void SetDatabase(List<string[]> originalDatabase, List<Type> attTypes, List<string> labels)
    {
        _database = new List<string[]>(originalDatabase);
        _attributeTypes = attTypes;
        _databaseLabels = labels;
    }

    // atualiza a lista de filtros de acordo com os filtros adicionados pelo usuário
    public void UpdateListOfFilters(int index, List<string> filteredParameters)
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

    // retorna a base de dados atual, com todos os filtros aplicados
    public List<string[]> GetFilteredDatabase()
    {
        if (_filterValues.Count == 0) return null;

        List<string[]> filteredDatabase = new List<string[]>();
        
        //para cada linha na base de dados original
        foreach (string[] line in _database)
        {
            bool find = true;
            //para cada filtro na lista de filtros
            foreach (KeyValuePair<int, List<string>> filter in _filterValues)
            {
                if (!find) continue;
                //para cada parâmetro dentro do filtro atual
                foreach (string parameter in filter.Value)
                {
                    if (_attributeTypes[filter.Key] != typeof(string)) //continuum
                    {
                        float value = float.Parse(line[filter.Key],CultureInfo.InvariantCulture);
                        float minValue = float.Parse(filter.Value[0], CultureInfo.InvariantCulture);
                        float maxValue = float.Parse(filter.Value[1], CultureInfo.InvariantCulture);
                        bool invSelection = Convert.ToBoolean(filter.Value[2]);
                        
                        //seleção invertida
                        if (invSelection)
                        {
                            if (value < minValue || value > maxValue)
                            {
                                find = true;
                                break;
                            }
                            find = false;
                        }
                        else
                        {
                            if (value >= minValue && value <= maxValue)
                            {
                                find = true;
                                break;
                            }
                            find = false;
                        }
                    }
                    else //categoric
                    {
                        if (line[filter.Key] == parameter)
                        {
                            find = true;
                            break;
                        }
                        find = false;
                    }
                }
            }

            if (find)
                filteredDatabase.Add(line);
        }
        
        SetFilter();
        return filteredDatabase;
    }


    // atualiza a string que contém os filtros que serão
    // utilizados para a geração da visualização
    public void SetFilter()
    {
        _filter = "[";

        int index = 0;
        foreach (KeyValuePair<int, List<string>> filterbyindex in _filterValues)
        {

            if (_attributeTypes[filterbyindex.Key] != typeof(string))
            { // continuum
                if (Convert.ToBoolean(filterbyindex.Value[2])) //if seleção invertida
                {
                    _filter += "{\"not\": {\"field\": \"" + _databaseLabels[filterbyindex.Key] + "\", ";
                    _filter += "\"range\":[" + filterbyindex.Value[0] + "," + filterbyindex.Value[1] + "]}}";
                }
                else
                {
                    _filter += "{\"field\": \"" + _databaseLabels[filterbyindex.Key] + "\", ";
                    _filter += "\"range\":[" + filterbyindex.Value[0] + "," + filterbyindex.Value[1] + "]}";
                }
            }
            else
            {
                _filter += "{\"field\": \"" + _databaseLabels[filterbyindex.Key] + "\", ";
                _filter += "\"oneOf\": [";

                for (int j = 0; j < filterbyindex.Value.Count; j++)
                {
                    _filter += "\"" + filterbyindex.Value[j] + "\"";

                    if (j + 1 == filterbyindex.Value.Count)
                    {
                        _filter += "]}";
                        break;
                    }

                    _filter += ", ";
                }
            }

            if (index + 1 == _filterValues.Count)
            {
                _filter += "]";
                print(_filter);
                return;
            }
            
            index += 1;
            _filter += ", ";
        }
    }

    public string GetFilters()
    {
        return _filter;
    }
}
