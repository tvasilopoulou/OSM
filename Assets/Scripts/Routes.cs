using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;

using UnityEngine;
using System.Net;
using UnityEngine.UI;

public class Routes : MonoBehaviour
{
    [Serializable]
    public class Location{
        public double lat;
        public double lon;
    }


    [Serializable]
    public class Route{
        public string name;
        public string type;
        public Location [] vertices = null;
        public Location center = null;
        public float radius = 0.0F;

    }



    [Serializable]
    public class RoutesClass{
        public Route [] areas;
    }

    private OnlineMaps map;
    public float borderWidth = 1;
    private bool flag = false;

    
    void Start()
    {
        map = OnlineMaps.instance;
        Positions.OnRoutesReceived += CreateRoutes;
    }

    void CreateRoutes(string jsonMessage){
        // break down received message
        // Debug.Log(jsonMessage);
        RoutesClass routesClass = JsonUtility.FromJson<RoutesClass>(jsonMessage);
        // Debug.Log(routesClass.areas[0].name);
        Location location = new Location();

        // // for every route
        foreach(Route route in routesClass.areas){
            // Debug.Log(route.vertices[0]);
            if (route.radius > 0.0f){ Debug.Log("bye"); continue;}
            flag = false;
            List <Vector2> routeLine = new List <Vector2>();
                foreach(Location loc in route.vertices){
                    Debug.Log(loc);
                    if (loc.lat > 40.0f){
                        flag = true;
                        break;
                    }
                    //loc.latitude, loc.longitude -> waypoint
                    OnlineMapsMarker marker = OnlineMapsMarkerManager.CreateItem(new Vector2((float)loc.lon,(float)loc.lat));
                    routeLine.Add(new Vector2((float)loc.lon,(float)loc.lat));

                }
                if (flag == true) continue;
                OnlineMapsDrawingPoly poly = new OnlineMapsDrawingPoly(routeLine, Color.red, 1, new Color(1, 1, 1, 0.5f));
                poly.checkMapBoundaries = false;
                OnlineMapsDrawingElementManager.AddItem(poly);

                routeLine = null;
        }
    }


    
}
