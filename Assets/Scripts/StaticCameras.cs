using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCameras : MonoBehaviour
{

    class Location{
        double latitude;
        double longitude;
    }

    class StaticCamera{
        string deviceId;
        string [] url;
        Location location;
    }

    class StaticCamerasClass{
        StaticCamera [] statCameras;
        StaticCamera [] cods;
    }

}
