using System.Collections;
using System.Net.WebSockets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;


public class CreatePin : MonoBehaviour
{
    private OnlineMaps map;

    public Texture2D ambulance;
    public Texture2D cam;
    public Texture2D cod;
    public Texture2D firecar;
    public Texture2D limocar;
    public Texture2D policecar;
    public Texture2D riskhigh;
    public Texture2D risklow;
    public Texture2D riskmoderate;
    public Texture2D sportcar;
    public Texture2D teamk9;
    public Texture2D teamsecurity;
    public Texture2D truckcar;
    public Texture2D ugv;
    public Texture2D uav;
    
    private List<OnlineMapsMarker> OnlineMapsMarkerList;
    private List<string> IDList;
    private GameObject tooltip;

    public OnlineMapsRawImageTouchForwarder forwarder;

    public GameObject tooltipPrefab;
    public Canvas container;
    public Material onToggle;
    public Material offToggle;

    private int flag = 0;
    private int roundCoords = 0;

    protected double latit;
    protected double longit;

    class Telemetry{
        public float latitude;
        public float longitude;
        public string item;
        public string speed;
        public string altitude;
        public string id;
        public string message_type;
    
    }


    void Start()
    {
        map = OnlineMaps.instance;
        OnlineMapsMarkerList = new List<OnlineMapsMarker>();
        IDList = new List<string>();
        Positions.OnReceived += Place;
        
    }

    void Place(string jsonMessage)
    {
        Telemetry data = JsonUtility.FromJson<Telemetry>(jsonMessage);
        
        latit = data.latitude;
        longit = data.longitude;

        if(data.speed == "0") return;

        double roundedLat = Math.Round(data.latitude, 3);
        double roundedLon = Math.Round(data.longitude, 3);

        if (roundCoords != 0){
            roundedLat = data.latitude;
            roundedLon = data.longitude;
        }


        string label = data.item.ToUpper() + data.id + System.Environment.NewLine + "Lat: " + roundedLat + ", Lon: " + roundedLon + System.Environment.NewLine + 
                        "Speed: " + data.speed + "m/s" + System.Environment.NewLine + "Altitude: " + data.altitude + "m";
        
        if(!IDList.Contains(data.item + data.id)){
            OnlineMapsMarker marker = OnlineMapsMarkerManager.CreateItem(new Vector2(data.longitude,data.latitude), itemLocated(data.item), label);
            marker.OnClick += OnMarkerClick;
            if (flag == 1) GameObject.Find("MapText").GetComponent<TextMeshProUGUI>().text = label;
            marker.scale = 0.5f;
            OnlineMapsMarkerList.Add(marker);
            IDList.Add(data.item + data.id);
        }
        
        else{
            foreach(OnlineMapsMarker marker in OnlineMapsMarkerList){
                if(marker.label.Contains(data.item.ToUpper() + data.id)){
                    marker.position = new Vector2(data.longitude,data.latitude);
                    marker.label = label;
                    if (flag == 1) GameObject.Find("MapText").GetComponent<TextMeshProUGUI>().text = label;
                    map.Redraw();
                    break;
                }
            }
        }
        
    }

    private void OnMarkerClick(OnlineMapsMarkerBase marker){
        // Show in console marker label.
        Debug.Log(marker.label);
    }



    private Texture2D itemLocated(string item){
        if(item.IndexOf("ambulance", StringComparison.OrdinalIgnoreCase) >= 0) return ambulance;
        else if(item.IndexOf("camera", StringComparison.OrdinalIgnoreCase) >= 0) return cam;
        else if(item.IndexOf("cod", StringComparison.OrdinalIgnoreCase) >= 0) return cod;
        else if(item.IndexOf("firecar", StringComparison.OrdinalIgnoreCase) >= 0) return firecar;
        else if(item.IndexOf("limocar", StringComparison.OrdinalIgnoreCase) >= 0) return limocar;
        else if(item.IndexOf("policecar", StringComparison.OrdinalIgnoreCase) >= 0) return policecar;
        else if(item.IndexOf("riskhigh", StringComparison.OrdinalIgnoreCase) >= 0) return riskhigh;
        else if(item.IndexOf("risklow", StringComparison.OrdinalIgnoreCase) >= 0) return risklow;
        else if(item.IndexOf("riskmoderate", StringComparison.OrdinalIgnoreCase) >= 0) return riskmoderate;
        else if(item.IndexOf("sportcar", StringComparison.OrdinalIgnoreCase) >= 0) return sportcar;
        else if(item.IndexOf("teamk9", StringComparison.OrdinalIgnoreCase) >= 0) return teamk9;
        else if(item.IndexOf("teamsecurity", StringComparison.OrdinalIgnoreCase) >= 0) return teamsecurity;
        else if(item.IndexOf("truckcar", StringComparison.OrdinalIgnoreCase) >= 0) return truckcar;
        else if(item.IndexOf("ugv", StringComparison.OrdinalIgnoreCase) >= 0) return ugv;
        else if(item.IndexOf("uav", StringComparison.OrdinalIgnoreCase) >= 0) return uav;
        else return riskmoderate;

    }

    public void OnPlusClick(){
        if (map.zoom <= 19) map.zoom += 1;
    }

    public void OnMinusClick(){
        if (map.zoom >= 1) map.zoom -= 1;
    }

    public void onInfoClick(){
        if (flag == 0){
            GameObject.Find("Quad").GetComponent<MeshRenderer>().material = onToggle;
            flag= 1;
        }
        else {
            GameObject.Find("Quad").GetComponent<MeshRenderer>().material = offToggle;
            GameObject.Find("MapText").GetComponent<TextMeshProUGUI>().text = "";
            flag = 0;
        }
    }

    public void OnButtonMoreClick(){
        if (flag == 1 && GameObject.Find("MapText").GetComponent<TextMeshProUGUI>().text.Length > 5) {
            if(roundCoords == 0){
                roundCoords = 1;
                string label = GameObject.Find("MapText").GetComponent<TextMeshProUGUI>().text;
                label = label.Replace(label.Substring(label.IndexOf('L'), label.IndexOf('S')), "Lat: " + latit + 
                             ", Lon: " + longit + System.Environment.NewLine + "Speed: ");
                Debug.Log(label.Substring(label.IndexOf('L'), label.IndexOf('S')));
                GameObject.Find("MapCanvas/MapText").GetComponent<TextMeshProUGUI>().text = label;

                GameObject.Find("MapCanvas/Info/ButtonMore/TextMoreLess").GetComponent<TextMeshPro>().text = "Less";
            }
            else{
                roundCoords = 0;
                string label = GameObject.Find("MapText").GetComponent<TextMeshProUGUI>().text;
                label = label.Replace(label.Substring(label.IndexOf('L'), label.IndexOf('S')), "Lat: " + Math.Round(latit, 3) + 
                            ", Lon: " + Math.Round(longit, 3) + System.Environment.NewLine + "Speed: ");
                GameObject.Find("MapCanvas/MapText").GetComponent<TextMeshProUGUI>().text = label;

                GameObject.Find("MapCanvas/Info/ButtonMore/TextMoreLess").GetComponent<TextMeshPro>().text = "More";
            }
        }
    }
}