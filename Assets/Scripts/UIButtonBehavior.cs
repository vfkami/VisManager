using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonBehavior : MonoBehaviour
{
    public GameObject Aba1;
    public GameObject Aba2;
    public GameObject Aba3;

    public GameObject Chart;
    
    public void ShowAba1()
    {
        Aba1.gameObject.SetActive(true);
        Aba2.gameObject.SetActive(false);  
        Aba3.gameObject.SetActive(false);
        Chart.gameObject.SetActive(false);

    }
    
    public void ShowAba2()
    {
        Aba1.gameObject.SetActive(false);
        Aba2.gameObject.SetActive(true);
        Aba3.gameObject.SetActive(false);
        Chart.gameObject.SetActive(false);

    }

    public void ShowAba3()
    {
        Aba1.gameObject.SetActive(false);
        Aba2.gameObject.SetActive(false); 
        Aba3.gameObject.SetActive(true);
        Chart.gameObject.SetActive(true);
    }
}
