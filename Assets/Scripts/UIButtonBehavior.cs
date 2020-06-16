using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonBehavior : MonoBehaviour
{
    public GameObject Aba1;
    public GameObject Aba2;
    
    public void ShowAba1()
    {
        Aba1.gameObject.SetActive(true);
        Aba2.gameObject.SetActive(false);  
    }
    
    public void ShowAba2()
    {
        Aba1.gameObject.SetActive(false);
        Aba2.gameObject.SetActive(true);
    }
}
