using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class MinMaxSliderBehavior : MonoBehaviour
{
    private int _id;
    private bool _invertedSelection;

    private List<string> _parameters = new List<string>();
    // Start is called before the first frame update
    public void UpdateMinMaxSlider(int index, Vector2 minMax)
    {
        _id = index;
        GetComponentInChildren<MinMaxSlider>().SetValues(minMax.x, minMax.y, minMax.x, minMax.y);
        GetComponentInChildren<MinMaxSlider>().wholeNumbers = false;
    }

    public void OnValueChanged()
    {
        _parameters = new List<string> {
            GetComponentInChildren<MinMaxSlider>().Values.minValue.ToString("0.000", CultureInfo.InvariantCulture),
            GetComponentInChildren<MinMaxSlider>().Values.maxValue.ToString("0.000", CultureInfo.InvariantCulture),
            GetComponentInChildren<Toggle>().isOn.ToString()
        };
        GameObject manager = GameObject.Find("Manager");
        manager.GetComponent<FilterManager>().UpdateListOfFilters(_id, _parameters);
    }
    

    public int GetId()
    {
        return _id;
    }

    public List<string> GetParameters()
    {
        return _parameters;
    }
    
}
