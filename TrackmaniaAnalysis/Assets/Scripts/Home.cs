using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    void Start()
    {
        DataManager.PreviousScenes.Add(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        
    }
}
