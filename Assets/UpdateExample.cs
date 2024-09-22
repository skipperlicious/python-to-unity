using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateExample : MonoBehaviour
{
    //This script is an example of how to update a UI Text object with the latest Python output data
    [Tooltip("Toggles between the latest output and the full log")]
    public bool updateLatest = true;
    void Update()
    {
        if(updateLatest)
        {
            gameObject.GetComponent<Text>().text = PythonToUnity.LatestPythonOutputData;
        }
        else
        {
            gameObject.GetComponent<Text>().text = PythonToUnity.PythonOutputLog;
        }
    }
}
