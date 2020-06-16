using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class DatasetReader : MonoBehaviour
{
    private string _datasetPath;
    private List<string[]> _dataset;
    private List<string> _datasetLabel;
    private List<Type> _labelTypes;
    private List<string> _tempLine;
    
    public void ReadTextFile(string path) // atributos separados por ','
    {
        _dataset = new List<string[]>();
        _datasetLabel = new List<string>();
        _tempLine = new List<string>();
        _labelTypes = new List<Type>();
        _datasetPath = path;
        
        StreamReader file = new StreamReader(path);
        bool firstLine = true;
        
        while (!file.EndOfStream)
        {
            string line = file.ReadLine();
            if(firstLine) // armazenar label do dataset
            {
                string[] lineSplited = line.Split(',');
                foreach (var label in lineSplited)
                {
                    _datasetLabel.Add(label);
                }
                firstLine = false;
            }
            else // armazenar dataset
            {
                _tempLine.Add(line);
            }
        }
        
        file.Close();
        ParseList();
    }

    private void ParseList()
    {
        foreach (var line in _tempLine) 
        {
            string[] lineSplited = line.Split(',');
            
            if (_labelTypes.Count < 1) // se primeira linha de atributos, verificar o tipo deles
            {
                DefineLabelTypes(lineSplited);
            }
            
            _dataset.Add(lineSplited); 
        }
    }

    private void DefineLabelTypes(string[] dataExample) // cria uma lista de tipos para cada atributo da database
    {
        foreach (string data in dataExample)
        {
            if (int.TryParse(data, out int _)) { _labelTypes.Add(typeof(int)); }
            else if (double.TryParse(data, NumberStyles.Number,CultureInfo.InvariantCulture, out _)) 
            { _labelTypes.Add(typeof(double)); }
            else if (float.TryParse(data, out _)){ _labelTypes.Add(typeof(float)); }
            else if (bool.TryParse(data, out _)) { _labelTypes.Add(typeof(bool)); }
            else { _labelTypes.Add(typeof(string)); }
        }
    }
    
    public List<string[]> GetDataset()
    {
        return _dataset;
    }

    public List<string> GetDatabaseLabel()
    {
        return _datasetLabel;
    }

    public List<Type> GetLabelTypes()
    {
        return _labelTypes;
    }


    public string[] GetPoiInformation(string poiName)
    {
        foreach (string[] line in _dataset)
        {
            if (line[0] == poiName)
                return line;
        }
        return new string[] {};
    }

    public void SetDatabasePath(string path)
    {
        _datasetPath = path;
    }
    
    public string GetDatabasePath()
    {
        return _datasetPath;
    }
}
