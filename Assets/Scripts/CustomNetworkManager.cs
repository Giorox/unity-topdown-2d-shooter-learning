using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CustomNetworkManager : NetworkManager {

    public void StartHosting()
    {
        SetPort();
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    private void SetIPAddress()
    {
        string ipAddress = GameObject.Find("InputField").transform.Find("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    private void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    void OnEnable()
    {
        //Tell our "OnLevelLoaded" function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        //Tell our "OnLevelLoaded" function to stop listening for a scene change as soon as this script is disabled.
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        //This is the delegate function to handle scene loading as to create the Listeners on the UI buttons.
        if (scene.name == "Main Menu")
            SetupMainSceneButtons();
        else
            SetupOtherSceneButtons();
    }

    private void loadMainMenu()
    {
        GameObject menu1 = MyUtils.FindIncludingInactive("Multiplayer Menu");
        GameObject menu2= MyUtils.FindIncludingInactive("Options Menu");

        if (menu1.active)
            menu1.SetActive(false);
        else
            menu2.SetActive(false);

        GameObject menu = MyUtils.FindIncludingInactive("Main Menu");
        menu.SetActive(true);
    }

    private void loadOptionsMenu()
    {
        GameObject menu = MyUtils.FindIncludingInactive("Main Menu");
        menu.SetActive(false);

        menu = MyUtils.FindIncludingInactive("Options Menu");
        menu.SetActive(true);
    }

    private void loadMultiplayerMenu()
    {
        GameObject menu = MyUtils.FindIncludingInactive("Main Menu");
        menu.SetActive(false);

        menu = MyUtils.FindIncludingInactive("Multiplayer Menu");
        menu.SetActive(true);

        GameObject.Find("Host Game Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Host Game Button").GetComponent<Button>().onClick.AddListener(StartHosting);

        GameObject.Find("Join Game Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Join Game Button").GetComponent<Button>().onClick.AddListener(JoinGame);

        GameObject.Find("Back Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Back Button").GetComponent<Button>().onClick.AddListener(loadMainMenu);
    }

    private void SetupMainSceneButtons()
    {
        GameObject menu = MyUtils.FindIncludingInactive("Main Menu");
        menu.SetActive(true);

        menu = MyUtils.FindIncludingInactive("Multiplayer Menu");
        menu.SetActive(false);

        menu = MyUtils.FindIncludingInactive("Options Menu");
        menu.SetActive(false);


        GameObject.Find("Multiplayer Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Multiplayer Button").GetComponent<Button>().onClick.AddListener(loadMultiplayerMenu);

        GameObject.Find("Options Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Options Button").GetComponent<Button>().onClick.AddListener(loadOptionsMenu);

        GameObject.Find("Quit Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Quit Button").GetComponent<Button>().onClick.AddListener(loadMainMenu);
    }

    private void SetupOtherSceneButtons()
    {
        GameObject menu = MyUtils.FindIncludingInactive("NetworkButton");
        menu.SetActive(true);

        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(NetworkServer.Reset);
    }
}
