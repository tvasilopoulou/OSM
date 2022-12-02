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

        string label = data.item.ToUpper() + data.id + System.Environment.NewLine + "Lat: " + data.latitude + ", Lon: " + data.longitude + System.Environment.NewLine + 
                        "Speed: " + data.speed + System.Environment.NewLine + "Altitude: " + data.altitude;
        

        if(!IDList.Contains(data.id)){
            OnlineMapsMarker marker = OnlineMapsMarkerManager.CreateItem(new Vector2(data.longitude,data.latitude), itemLocated(data.item), label);
            marker.OnClick += OnMarkerClick;
            GameObject.Find("MapText").GetComponent<TextMeshProUGUI>().text = label;
            marker.scale = 0.5f;
            OnlineMapsMarkerList.Add(marker);
            IDList.Add(data.id);
        }
        
        else{
            foreach(OnlineMapsMarker marker in OnlineMapsMarkerList){
                if(marker.label.Contains(data.id)){
                    marker.position = new Vector2(data.longitude,data.latitude);
                    marker.label = label;
                    GameObject.Find("MapText").GetComponent<TextMeshProUGUI>().text = label;
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
}