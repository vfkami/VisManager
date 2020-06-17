using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ReadDatabase
{
    public static string
        file1 = "Assets/Datasets/iris.csv",
        file2 = "Assets/Datasets/matrix_scatterplot.csv",
        file3 = "Assets/Datasets/carros_teste3.csv",
        logPath = "Assets/Datasets/log.txt";
    private string pathFile;
    public string path {
        get { return pathFile; } set { pathFile = value; }
    }
    List<String[]> Attributes = new List<string[]>();
    public string[] columnnames {
        get { return Attributes[0]; } set { }
    }
    List<String> TypesOfAttributes = new List<string>();

    bool autostart = false;

    public ReadDatabase() { pathFile = ""; }

    public ReadDatabase(string filepath) { pathFile = filepath; }

    public void loadfile()
    {
        CSVReader();
    }

    private void CSVReader()
    {
        StreamReader database = new StreamReader(pathFile);
        
        string Line; // = Database.ReadLine();
        string[] Split_Line; //= Line.Split(new[] {","}, System.StringSplitOptions.None);

        int index = 0;

        while ((Line = database.ReadLine()) != null)
        {
            Split_Line = Line.Split(new[] {","}, StringSplitOptions.None);
            Attributes.Add(Split_Line);
        }
        foreach (string[] x in Attributes)
        {
            foreach (string y in x) //Descobrir se é categórico ou contínuo
            {
                if (index == 1) //verifica primeira linha de atributos apenas
                {
                    if (float.TryParse(y, NumberStyles.Any, CultureInfo.InvariantCulture, out float temp)) //se conseguir passar pra float -> Contínuo
                    {
                        TypesOfAttributes.Add("CONT");
                    }
                    else
                    {
                        TypesOfAttributes.Add("CAT");
                    }
                }
            }

            index++;
        }
        database.Close();
    }
    
    public List<string> getLine(int index)
    {
        List<string> Line = new List<string>();

        foreach (string x in Attributes[index])
        {
            Line.Add(x);
        }
        return (Line);
    }
    
    public List<List<string>> getLine(int firstindex, int lastindex)
    {
        List<List<string>> Lines = new List<List<string>>();
        
        for (int i = firstindex; i<=lastindex; i++)
        {
            List<string> Temp = new List<string>();
            
            for (int j = 0; j < Attributes[0].Length; j++)
            {
                Temp.Add(Attributes[i][j]); 
            } 
            Lines.Add(Temp);
        }
        return (Lines);
    }

    public List<string> getColumn(string attr)
    {
        List<string> Column = new List<string>();
        int index = Array.IndexOf(Attributes[0], attr);

        if (index > -1)
        {
            for (int i = 1; i < Attributes.Count; i++)
            {
                Column.Add(Attributes[i][index]);
            }
            return (Column);
        }

        //print("Coluna não existe");
        return null;
    }

    private void UpdateLog()
    {
        File.WriteAllText(logPath, "");
        foreach (string x in Attributes[0])
        {
            string line = x + ", true";

            File.AppendAllText(logPath,   line + "\n");
            //print(line);
        }
        
        AssetDatabase.Refresh();
    }
}
