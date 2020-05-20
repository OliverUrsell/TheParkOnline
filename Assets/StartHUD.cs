using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartHUD : MonoBehaviour
{

    public Image colourDisplayImage;

    private Color playerColour = new Color(1,0,1,1);

    public InputField displayName;

    public void SetPlayerColour(string rgb) {
        string[] rgbArray = rgb.Split(',');
        playerColour = new Color(float.Parse(rgbArray[0]), float.Parse(rgbArray[1]), float.Parse(rgbArray[2]));
        colourDisplayImage.color = playerColour;
    }

    public void AssignCustomization() {
        Debug.Log("Hello 3");
        foreach (BlockPlayer bp in FindObjectsOfType<BlockPlayer>()) {
            if (bp.isLocalPlayer) {
                Debug.Log("Hello 4");
                if (displayName.text != "") {
                    bp.CmdSetName(displayName.text);
                }
                bp.CmdSetColour(playerColour);
                gameObject.SetActive(false);
                break;
            }
        }
    }
}
