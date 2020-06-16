using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProjectUtils : MonoBehaviour
{
    private int _id;

    public Vector2 GetMinMaxValues(int index, List<string[]> database)
    {
        List<string> values = GetAttributes(index, database);

        float min = float.MaxValue;
        float max = float.MinValue;

        foreach (string value in values)
        {
            Double fValue = float.Parse(value);

            if (fValue < min)
                min = (float) fValue;
            else if (fValue > max)
                max = (float) fValue;
        }
        
        return new Vector2(min, max);
    }

    public List<string> GetAttributes(int index, List<string[]> database)
    {
        List<string> attributes = new List<string>();

        foreach (string[] line in database)
        {
            if (!attributes.Contains(line[index]))
                attributes.Add(line[index]);
        }

        return attributes;
    }
    
    public void SetId(int index)
    {
        _id = index;
    }

    public int GetId()
    {
        return _id;
    }
}
