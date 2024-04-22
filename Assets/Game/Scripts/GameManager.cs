using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

using distriqt.plugins.vibration;
public class GameManager : MonoBehaviour
{
    private FeedbackGenerator _selectGenerator;
    private FeedbackGenerator _impactGenerator;
    [HideInInspector] public double totalMoney;
    [HideInInspector] public int fps;
    [SerializeField] private AudioClip cardFlipSound;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip modeWin;

    [SerializeField] private GameObject genelUI;
    [SerializeField] private GameObject BJCanvas;

    public void Vibrate(string type)
    {
        if (Vibration.isSupported)
        {
            if (PlayerPrefs.HasKey("Vibration"))
            {
                if (PlayerPrefs.GetInt("Vibration") == 1)
                {
                    switch (type)
                    {
                        case "soft":
                            if (_selectGenerator == null)
                            {
                                _selectGenerator = Vibration.Instance.CreateFeedbackGenerator(FeedbackGeneratorType.SELECTION);
                            }
                            _selectGenerator.PerformFeedback();
                            break;

                        case "hard":
                            if (_impactGenerator == null)
                            {
                                _impactGenerator = Vibration.Instance.CreateFeedbackGenerator(FeedbackGeneratorType.IMPACT);
                                _impactGenerator.Prepare();
                            }
                            _impactGenerator.PerformFeedback();
                            break;
                    }
                }
            }
        }

    }

    public void CardSound()
    {

        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {

                GetComponent<AudioSource>().PlayOneShot(cardFlipSound, 0.5f);

            }
        }


    }

    public void WinSound()
    {

        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {

                GetComponent<AudioSource>().PlayOneShot(winSound);

            }
        }


    }

    public void LoseSound()
    {

        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {

                GetComponent<AudioSource>().PlayOneShot(loseSound);

            }
        }


    }

    public void ModeWinSound()
    {

        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {

                GetComponent<AudioSource>().PlayOneShot(modeWin);

            }
        }


    }

    public void openBJ()
    {
        BJCanvas.SetActive(true);
        genelUI.SetActive(false);

    }



}
