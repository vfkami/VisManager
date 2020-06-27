using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Toggle = UnityEngine.Experimental.UIElements.Toggle;

public class TgroupBehavior : MonoBehaviour
{
    public GameObject[] checkboxes;
    
    private List<GameObject> _checkBoxList = new List<GameObject>();
    private List<bool> _checkStatus = new List<bool>();
    private List<string> _parameters;

    private int _id;
    public void UpdateCheckBoxes(int index, List<string> categories)
    {
        _id = index;
        
        for(int i = 0; i < checkboxes.Length; i++)
        {
            if (i < categories.Count)
            {
                checkboxes[i].SetActive(true);
                checkboxes[i].GetComponentInChildren<Text>().text = categories[i];
                _checkBoxList.Add(checkboxes[i]);
            }
            else { Destroy(checkboxes[i]); }
        }
    }
    
    public void OnValueChanged()
    {
        _parameters = new List<string>();
        
        foreach (GameObject toggle in _checkBoxList)
        {
            bool isOn = toggle.GetComponent<UnityEngine.UI.Toggle>().isOn;
            //se toggle desativado = adiciona a lista de parametros para deixarem de ser exibido
            if (isOn){ _parameters.Add(toggle.GetComponentInChildren<Text>().text); } 
        }
        GameObject manager = GameObject.Find("Manager");
        manager.GetComponent<FilterManager>().UpdateListOfFilters(_id, _parameters);
    }

    public List<string> GetParameters()
    {
        return _parameters;
    }
}
