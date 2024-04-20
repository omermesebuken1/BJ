using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Keyboard : MonoBehaviour
{


    string word = null;
    int wordIndex = 0;
    public TextMeshProUGUI inputArea = null;

    // Use this for initialization
    public void alphabetFunction(string alphabet)
    {
        if (word != null)
        {
            if (word.Length < 18)
            {
                wordIndex++;
                word = word + alphabet;
                inputArea.text = word;
            }
        }
        else
        {
            wordIndex++;
            word = word + alphabet;
            inputArea.text = word;
        }
    }


    public void RemoveLastCharacter()
    {
        if (!string.IsNullOrEmpty(word))
        {
            wordIndex--;
            word = word.Substring(0, word.Length - 1);
            inputArea.text = word;
        }

    }



}
