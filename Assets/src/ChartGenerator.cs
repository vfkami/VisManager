using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ChartGenerator : MonoBehaviour
{    
    public bool autostart = true;
    public bool autoupdate = false;

    public enum ChartType
    {
        BarChartVertical,
        LineChart,
        PieChart,
        AreaChart,
        Scatterplot
    }

    public enum DataType
    {
        Manual,
        Dataset,
        Placeholder,
        Request
    }

    [SerializeField]
    private string Title = "Worldwide fruits market";
    [SerializeField]
    private ChartType Charttype = ChartType.BarChartVertical;
    [SerializeField]
    private DataType Datatype = DataType.Manual;
    [SerializeField]
    public Color Background = new Color(1, 1, 1);
    [SerializeField]
    private string Xlabel = "fruits";
    [SerializeField]
    private string X = "pear,orange,pineapple,blueberry";
    [SerializeField]
    private string Ylabel = "qt. sold";
    [SerializeField]
    private string Base64string = "";
    [SerializeField]
    private string Y = "1,2,3,4";
    [SerializeField]
    private string Zlabel = "Continent";
    [SerializeField]
    private string Z = "america,america,europe,europe";
    [SerializeField]
    private string Wlabel = "Toxins";
    [SerializeField]
    private string W = "1,3,5,7";
    [SerializeField]
    public Color[] Colors = { new Color(70 / 255f, 130 / 255f, 180 / 255f) };
    private Color[] categorycolors = { new Color(70 / 255f, 130 / 255f, 180 / 255f), new Color(30 / 255f, 30 / 255f, 180 / 255f),
    new Color(70 / 255f, 30 / 255f, 10 / 255f), new Color(40 / 255f, 40 / 255f, 80 / 255f), new Color(244 / 255f, 130 / 255f, 180 / 255f)};
    public bool ShowLabels; // not used
    public bool Legends; // not used
    public bool Sort; // not used

    public float Inner; // not used
    public float Padding; // not used

    /* TODO: Mess around with this to try a better solution 
     Variables here for editor (maybe subclass?)*/
    [SerializeField]
    public int indexdataset = 0;
    [SerializeField]
    public int xindex = 0;
    [SerializeField]
    public int yindex = 1;
    [SerializeField]
    public int zindex = 2;
    [SerializeField]
    public int windex = 3;

    public ChartType charttype { get { return Charttype; } set { Charttype = value; if (autoupdate) getchart(); } }
    public DataType datatype { get { return Datatype; } set { Datatype = value; if (autoupdate) getchart(); } }
    public string title { get { return Title; } set { Title = value; if (autoupdate) getchart(); } }
    public string xlabel { get { return Xlabel; } set { Xlabel = value; if (autoupdate) getchart(); } }
    public string x { get { return X; } set { X = value; if (autoupdate) getchart(); } }
    public string ylabel { get { return Ylabel; } set { Ylabel = value; if (autoupdate) getchart(); } }
    public string y { get { return Y; } set { Y = value; if (autoupdate) getchart(); } }
    public string zlabel { get { return Zlabel; } set { Zlabel = value; if (autoupdate) getchart(); } }
    public string z { get { return Z; } set { Z = value; if (autoupdate) getchart(); } }
    public string wlabel { get { return Wlabel; } set { Wlabel = value; if (autoupdate) getchart(); } }
    public string w { get { return W; } set { W = value; if (autoupdate) getchart(); } }
    public Color[] colors { get { return Colors; } set { Colors = value; if (autoupdate) getchart(); } }
    public Color background { get { return Background; } set { Background = value; if (autoupdate) getchart(); } }
    public string base64string { get { return Base64string; } set { } }

    [SerializeField]
    private int oldnumcolors = 1;
    public int numcolors() {
        int newc = 1;

        if (charttype != ChartType.BarChartVertical && charttype != ChartType.PieChart) // TODO: change condition after grouped bars
            newc = z.Split(',').Distinct().ToArray().Length;
        
        if (charttype == ChartType.PieChart)
            newc = x.Split(',').Distinct().ToArray().Length;

        if (newc != oldnumcolors) {
            colors = new Color[newc];
            for (int i = 0; i < newc; i++)
                colors[i] = categorycolors[i % categorycolors.Length]; // update colors with predefined array
            oldnumcolors = newc;
        }
        return newc;
    }

    void Start() {
        if (autostart) getchart(); 
    }

    private void Update()
    {
        //numcolors();
    }

    // start the process of chart requisition for the server
    public void getchart()
    {
        if (verifyparameters()) {
            string url = "http://localhost:3000/chartgen.html?x=" + x + "&y=" + y + "&chart=" + Charttype.ToString().ToLower();

            if (maxdimensions() > 2)
                url += "&z=" + z;
            if (maxdimensions() > 3)
                url += "&w=" + w;

            // url += "&colors=";
            // String[] colorholder = new String[colors.Length];
            // for (int i = 0; i < colors.Length; i++)
            // {
            //     colorholder[i] = colorToWebColor(colors[i]); // "rgb(" + ((int)(colors[i].r * 255f)) + "," + ((int)(colors[i].g * 255f)) + "," + ((int)(colors[i].b * 255f)) + ")";
            //     //Debug.Log(colorholder[i], );
            // }
            // url += String.Join(";", colorholder);
            url += "&title=" + title;
            // url += "&background=" + colorToWebColor(background);

            String[] labels = { xlabel, ylabel, wlabel, zlabel };
            String[] label_axis = { "x","y","w","z" };
            for (int i = 0; i < labels.Length; i++)
            {
                if (labels[i] != "")
                {
                    url += "&" + label_axis[i] + "label=" + labels[i];
                }
            }

            Debug.Log("Requisition: " + url);
            StartCoroutine(GetRequest(url));
        }
    }

    public void getchartfromurl(String url)
    {
        // TODO: new datatype constraint 
        StartCoroutine(GetRequest(url));
    }

    // generate sprite based on base64 string that came from server (save it too)  TODO: two functions, separate, maybe a good place for buffer
    void generateViz(string base64str)
    {
        Base64string = base64str;
        byte[] Bytes = Convert.FromBase64String(base64str);
        Texture2D tex = new Texture2D(900, 465);
        tex.LoadImage(Bytes);
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        Sprite sprite = Sprite.Create(tex, rect, new Vector2(0,0), 100f);
        SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>(); 
        if (renderer == null)
        {
            renderer = this.gameObject.AddComponent<SpriteRenderer>(); // will crash if there's another renderer (like MeshRenderer) as component
        }
        renderer.sprite = sprite;

        PolygonCollider2D collider = gameObject.GetComponent<PolygonCollider2D>();
        if (collider == null) 
            gameObject.AddComponent<PolygonCollider2D>();  //collider will be added after visualization render
    }

    // network connection and image download
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else generateViz(webRequest.downloadHandler.text); // plain base64 string that (hopefully) came without any html tag
            
        }
    }

    public int maxdimensions() // charts have different dimensions so we need to know what options show to user
    {
        int dim = 0;
        switch (charttype)
        {
            case ChartType.BarChartVertical:
                dim = 2;
                break;
            case ChartType.LineChart:
                dim = 3;
                break;
            case ChartType.PieChart:
                dim = 2;
                break;
            case ChartType.AreaChart:
                dim = 3;
                break;
            case ChartType.Scatterplot:
                dim = 4;
                break;
        }
        return dim;
    }

    public bool verifyparameters() // TODO: verify dimensions to avoid send broken requistions
    {
        return true;
    }

    public static string colorToWebColor(Color c)
    {
        return "rgb(" + ((int)(c.r * 255f)) + "," + ((int)(c.g * 255f)) + "," + ((int)(c.b * 255f)) + ")";
    }
}
