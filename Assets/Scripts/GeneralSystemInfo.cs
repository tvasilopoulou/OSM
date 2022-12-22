using UnityEngine;
using UnityEngine.UI;

public class GeneralSystemInfo : MonoBehaviour
{
    public Text batteryStatus, batteryLevel;
    // public Text position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // adding the battery percentage
        // all values must be handleded here for live update
        // position.text = GameObject.Find("UI").GetComponent<RectTransform>().position.ToString();
        batteryStatus.text = SystemInfo.batteryStatus.ToString();
        float level = SystemInfo.batteryLevel * 100;
        
        if(level < 50.0f){
            batteryLevel.color = Color.yellow;
        }
        else if(level < 30.0f){
            batteryLevel.color = Color.red;
        }
        else{
            batteryLevel.color = Color.green;
        }
        batteryLevel.text = level.ToString("F2") + "%";
    }

    void DrawBatteryInfo()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(-40, 200 + (h * 4 / 100 + 10), w, h * 2 / 100);
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = h * 2 / 100;
        //style.normal.textColor = new Color(255, 255, 255, 255);
        style.normal.textColor = Color.blue;

        batteryStatus.text = SystemInfo.batteryStatus.ToString();

        string status = SystemInfo.batteryStatus.ToString();

        float level = SystemInfo.batteryLevel * 100;
        if(level < 50.0f && level > 30.0f)
        {
            batteryLevel.color = Color.yellow;
            style.normal.textColor = Color.yellow;
        }else if(level < 30.0f)
        {
            //the text has to be larger in order to warn the user
            style.fontSize = h * 4 / 100;
            style.normal.textColor = Color.red;
        }
        else
        {
            batteryLevel.color = Color.green;
        }
        batteryLevel.text = level.ToString("F2") + "%";

        string text = string.Format("Battery Status: {0:0.0} %", level);
        text += " " + status;
        GUI.Label(rect, text, style);
    }

    void DrawLidarValues()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(40, h - 200 - (h * 3 / 100 + 10), w, h * 3 / 100);
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = Color.cyan;

        // returns a random number between 0 and 100
        float randomNumber = Random.Range(0, 100);

        string text = string.Format("LIDAR : {0:0.0} Lidar measure", randomNumber);
        GUI.Label(rect, text, style);
    }

    void OnGUI()
    {
        //if (showFps) DrawFps();
        DrawBatteryInfo();
        DrawLidarValues();
    }
}
