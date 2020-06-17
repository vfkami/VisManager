using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWithoutPrefab : MonoBehaviour
{
    void Start() //pass parameters to visualization creation
    {
        //disable component so that views are not created from it
        ChartGenerator chartgen = gameObject.AddComponent<ChartGenerator>() ;

        //chartgen.autostart = true;

        //Passing parameters for visualization creation
        chartgen.charttype = ChartGenerator.ChartType.BarChartVertical;
        chartgen.title = "Chart title";
        chartgen.xlabel = "x label";
        chartgen.x = "orange, pineapple, apple, blueberry";
        chartgen.ylabel = "y label";
        chartgen.y = "2, 2.30, 6, 2.1";
        //chartgen.zlabel = "z label";
        //chartgen.z = "xiaomi, xbox, hp, aoc";
        
        Color[] colors = {Color.blue, Color.gray, Color.magenta, Color.white};
        chartgen.colors = colors;


        //getchart voilà - only with autostart == false
        chartgen.getchart();
    }
    

}
