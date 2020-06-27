using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonBehavior : MonoBehaviour
{
    // define o comportamento dos botões responsáveis por mostrar as abas

    public GameObject Aba1;
    public GameObject Aba2;
    public GameObject Aba3;
    public GameObject DefaultChart;
    public GameObject FiltredChart;
    
    public void ShowAba1()
    {
        Aba1.gameObject.SetActive(true);
        Aba2.gameObject.SetActive(false);  
        Aba3.gameObject.SetActive(false);
        DefaultChart.gameObject.SetActive(false);
        FiltredChart.gameObject.SetActive(false);
    }
    
    public void ShowAba2()
    {
        Aba1.gameObject.SetActive(false);
        Aba2.gameObject.SetActive(true);
        Aba3.gameObject.SetActive(false);
        DefaultChart.gameObject.SetActive(false);
        FiltredChart.gameObject.SetActive(false);
    }

    public void ShowAba3()
    {
        Aba1.gameObject.SetActive(false);
        Aba2.gameObject.SetActive(false); 
        Aba3.gameObject.SetActive(true);
        DefaultChart.gameObject.SetActive(true);
        FiltredChart.gameObject.SetActive(true);
    }
}
