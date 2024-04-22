using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using enumHelper;

public class HelperImp : MonoBehaviour
{

    [SerializeField] private BlackJack bj;
    [SerializeField] private DataTable db;
    private bool helperStatus;

    public bool isHit;
    public bool isHit1;
    public bool isHit2;

    public bool helperDone;

    public bool isSplitted;

    [SerializeField] private Button standButton;
    [SerializeField] private Button hitButton;
    [SerializeField] private Button splitButton;
    [SerializeField] private Button DDButton;
    [SerializeField] private Button helperButton;

    private Color32 greenColor = new Color32(112, 214, 201, 255);
    private Color32 originalColor = new Color32(161, 165, 224, 255);

    public void changeHelperStatus()
    {
        if (helperStatus)
        {
            helperStatus = false;
        }
        else
        {
            helperStatus = true;
        }
    }

    private void Update()
    {

        if (helperStatus)
        {
            helperButton.GetComponentInChildren<TextMeshProUGUI>().text = "HELPER: ON";
            Colorize(HelperHelps());
        }
        else
        {
            helperButton.GetComponentInChildren<TextMeshProUGUI>().text = "HELPER: OFF";
        }
    }

    private choice HelperHelps()
    {
        if (!helperDone && helperStatus)
        {
            int dealerNum = bj.dealerCards[0].number;
            int playerCard1 = bj.playerCards[0].number;
            int playerCard2 = bj.playerCards[1].number;
            int totalStart = playerCard1 + playerCard2;

            if(dealerNum > 10) dealerNum = 10;

            //Debug.Log("Dealer Num: "+dealerNum);

            if (isSplitted)
            {

            }
            else
            {

                if (isHit)
                {
                    choice ch = choice.H;

                    //Debug.Log("Player Score: " + bj.playerScore);


                    switch (bj.playerScore)
                    {
                        case 20:
                            ch = db.Sum_17();
                            break;
                        case 19:
                            ch = db.Sum_17();
                            break;
                        case 18:
                            ch = db.Sum_17();
                            break;
                        case 17:
                            ch = db.Sum_17();
                            break;
                        case 16:
                            ch = db.Sum_16(dealerNum);
                            break;
                        case 15:
                            ch = db.Sum_15(dealerNum);
                            break;
                        case 14:
                            ch = db.Sum_14(dealerNum);
                            break;
                        case 13:
                            ch = db.Sum_13(dealerNum);
                            break;
                        case 12:
                            ch = db.Sum_12(dealerNum);
                            break;
                        case 11:
                            ch = db.Sum_11(dealerNum);
                            break;
                        case 10:
                            ch = db.Sum_10(dealerNum);
                            break;
                        case 9:
                            ch = db.Sum_9(dealerNum);
                            break;
                        case 8:
                            ch = db.Sum_8();
                            break;
                        case 7:
                            ch = db.Sum_8();
                            break;
                        case 6:
                            ch = db.Sum_8();
                            break;
                        case 5:
                            ch = db.Sum_8();
                            break;
                        case 4:
                            ch = db.Sum_8();
                            break;
                    }

                    if (ch == choice.D)
                    {
                        ch = choice.H;
                    }

                    return ch;

                }
                else
                {

                    if (playerCard1 == playerCard2)
                    {
                        switch (playerCard1)
                        {
                            case 1:
                                return db.Same_AA();
                            case 10:
                                return db.Same_1010();
                            case 9:
                                return db.Same_99(dealerNum);
                            case 8:
                                return db.Same_88();
                            case 7:
                                return db.Same_77(dealerNum);
                            case 6:
                                return db.Same_66();
                            case 5:
                                return db.Same_55(dealerNum);
                            case 4:
                                return db.Same_44(dealerNum);
                            case 3:
                                return db.Same_33(dealerNum);
                            case 2:
                                return db.Same_22(dealerNum);

                        }

                    }

                    else if (playerCard1 == 1 || playerCard2 == 1)
                    {

                        switch (totalStart)
                        {
                            case 10:
                                return db.Ace_9(dealerNum);
                            case 9:
                                return db.Ace_8(dealerNum);
                            case 8:
                                return db.Ace_7(dealerNum);
                            case 7:
                                return db.Ace_6(dealerNum);
                            case 6:
                                return db.Ace_5(dealerNum);
                            case 5:
                                return db.Ace_4(dealerNum);
                            case 4:
                                return db.Ace_3(dealerNum);
                            case 3:
                                return db.Ace_2(dealerNum);
                        }
                    }

                    else
                    {
                        switch (bj.playerScore)
                        {
                            case 20:
                                return db.Sum_17();
                            case 19:
                                return db.Sum_17();
                            case 18:
                                return db.Sum_17();
                            case 17:
                                return db.Sum_17();
                            case 16:
                                return db.Sum_16(dealerNum);
                            case 15:
                                return db.Sum_15(dealerNum);
                            case 14:
                                return db.Sum_14(dealerNum);
                            case 13:
                                return db.Sum_13(dealerNum);
                            case 12:
                                return db.Sum_12(dealerNum);
                            case 11:
                                return db.Sum_11(dealerNum);
                            case 10:
                                return db.Sum_10(dealerNum);
                            case 9:
                                return db.Sum_9(dealerNum);
                            case 8:
                                return db.Sum_8();
                            case 7:
                                return db.Sum_8();
                            case 6:
                                return db.Sum_8();
                            case 5:
                                return db.Sum_8();
                            case 4:
                                return db.Sum_8();
                        }

                    }

                }

            }

        }


        return choice.NoChoice;

    }



    private void Colorize(choice ch)
    {
        switch (ch)
        {
            case choice.S:
                standButton.image.color = greenColor;
                break;
            case choice.SP:
                splitButton.image.color = greenColor;
                break;
            case choice.H:
                hitButton.image.color = greenColor;
                break;
            case choice.D:
                DDButton.image.color = greenColor;
                break;
        }

        helperDone = true;

    }

    public void ResetColors()
    {
        standButton.image.color = originalColor;
        splitButton.image.color = originalColor;
        hitButton.image.color = originalColor;
        DDButton.image.color = originalColor;

    }



}
