using System;
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
    public Dropdown KeyboardLayoutDropdown;
    void Start()
    {
        DataManager.PreviousScenes.Add(SceneManager.GetActiveScene().name);
        KeyboardLayoutDropdown.value = PlayerPrefs.GetInt("KeyboardLayout");
        
        DataManager.KeyboardLayoutChanged(PlayerPrefs.GetInt("KeyboardLayout"));

        KeyboardLayoutDropdown.onValueChanged.AddListener(KeyboardLayoutDropdownValueChanged);

        if (PlayerPrefs.GetFloat("LineThickness") == 0)
            PlayerPrefs.SetFloat("LineThickness", 1);
    }

    private void KeyboardLayoutDropdownValueChanged(int arg0)
    {
        DataManager.KeyboardLayoutChanged(arg0);
    }

    void Update()
    {
        
    }
}
