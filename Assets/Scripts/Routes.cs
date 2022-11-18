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
        public Location [] vertices;
        public string name;
        public string type;
    }

    [Serializable]
    public class RoutesClass{
        public Route [] routes;
    }

    private OnlineMaps map;

    
    void Start()
    {
        map = OnlineMaps.instance;
        Positions.OnRoutesReceived += CreateRoutes;
    }

    void CreateRoutes(string jsonMessage){

        // List <Vector2> routeLine = new List <Vector2>();

        // break down received message
        RoutesClass routesClass = JsonUtility.FromJson<RoutesClass>(jsonMessage);
        Location location = new Location();

        // for every route
        foreach(Route route in routesClass.routes){
            Debug.Log(route.name);
        
            List <Vector2> routeLine = new List <Vector2>();

            foreach(Location loc in route.vertices){
                //loc.latitude, loc.longitude -> waypoint

                OnlineMapsMarker marker = OnlineMapsMarkerManager.CreateItem(new Vector2((float)loc.lat,(float)loc.lon));
                routeLine.Add(new Vector2((float)loc.lat,(float)loc.lon));

            }
            OnlineMapsDrawingElementManager.AddItem(new OnlineMapsDrawingLine(routeLine, Color.black, 2));
            routeLine = null;

        }



    }

    
}
