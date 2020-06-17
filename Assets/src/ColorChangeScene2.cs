using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeScene2 : MonoBehaviour
{
    Color[] newcolors = { new Color(.8f,.1f,.15f), new Color(.15f,.9f,.24f) };
    int framecount =0;

    private void Update()
    {
        framecount++;
        if (framecount == 400)
            ChangeColors();
    }

    public void ChangeColors()
    {
        ChartGenerator chartgen = this.GetComponent<ChartGenerator>();
        Color[] tempcolors = new Color[chartgen.numcolors()];
        
        chartgen.colors.CopyTo(tempcolors,0);
        newcolors.CopyTo(chartgen.colors,0);
        tempcolors.CopyTo(newcolors,0);

        chartgen.getchart();
    }
}
