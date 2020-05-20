using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CustomNetworkManager : NetworkManager{

    public Camera hudCamera;
    public StartHUD StartHud;

    public override void OnClientConnect(NetworkConnection nc) {
        base.OnClientConnect(nc);
        //StartHud.AssignCustomization();
        //hudCamera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void OnClientDisconnect(NetworkConnection nc) {
        base.OnClientDisconnect(nc);
        hudCamera.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Set IP address of network manager
    public void SetNetworkAddress(string ip) {
        base.networkAddress = ip;
    }
}
