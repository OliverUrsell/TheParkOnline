

// Provides functionality for toggling player names on or off

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameToggle : MonoBehaviour
{
    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            toggleNames();
        }
    }

    void toggleNames() {
        bool active = !FindObjectOfType<BlockPlayer>().transform.Find("NameCanvas").gameObject.activeInHierarchy;
        foreach (BlockPlayer bp in FindObjectsOfType<BlockPlayer>()){
            GameObject nameCanvas = bp.transform.Find("NameCanvas").gameObject;
            nameCanvas.SetActive(active);
        }
    }
}
