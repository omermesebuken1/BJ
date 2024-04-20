using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMods : MonoBehaviour
{
    [SerializeField] public List<Sprite> ButtonColors = new List<Sprite>();

    public int buttonLevel;


     public bool isPlayed;
    public bool isLocked;

    

    public void ChangeColor(string buttonStatus)
    {

        switch (buttonStatus)
        {
            case "locked":
            GetComponent<Image>().sprite = ButtonColors[2];
            break;

            case "playable":
            GetComponent<Image>().sprite = ButtonColors[0];
            break;

            case "played":
            GetComponent<Image>().sprite = ButtonColors[1];
            break;
            
        }

    }

    




}
