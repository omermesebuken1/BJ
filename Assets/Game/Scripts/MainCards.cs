using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MainCards : MonoBehaviour
{
    [SerializeField] public GameObject CardPrefab;

    [SerializeField] private GameManager gm;

    [SerializeField]  private Sprite CardBack;
    public Deck deck;
    Card card;

    public bool cardReady;


    public List<Card> theFiveCard = new List<Card>();
    [SerializeField] private List<GameObject> fiveCardGO = new List<GameObject>();

    [HideInInspector] public int mainCardCounter = 0;

    void Start()
    {   

        deck = new Deck();
        cardReady = true;
        for (int i = 0; i < 5; i++)
        {
            card = deck.DrawCard();
            theFiveCard.Add(card);
            
        }


    }

    public void OpenNewMainCard()
    {
        if (mainCardCounter < 5)
        {
            mainCardCounter++;
            StartCoroutine(FlipCard(fiveCardGO[mainCardCounter-1],0.1f,mainCardCounter-1));
            
        }

    }

    private IEnumerator FlipCard(GameObject cardGO, float waitTime, int num)
    {

        WaitForSeconds wait = new WaitForSeconds(waitTime);

        cardGO.transform.DOLocalRotate(new Vector3(0,90,0),0.1f);
        yield return wait;

        cardGO.GetComponent<CardScript>().GetCardSprite(theFiveCard[num]);

        cardGO.transform.DOLocalRotate(new Vector3(0,0,0),0.1f);
        yield return wait;

        yield return new WaitUntil(() => cardGO.transform.rotation == Quaternion.Euler(0,0,0));

        cardReady = true;
    }

    private IEnumerator ReverseFlipCard(GameObject cardGO, float waitTime)
    {

        WaitForSeconds wait = new WaitForSeconds(waitTime);

        cardGO.transform.DOLocalRotate(new Vector3(0,90,0),0.1f);
        yield return wait;

        cardGO.GetComponent<CardScript>().GetCardBack();

        cardGO.transform.DOLocalRotate(new Vector3(0,0,0),0.1f);
        yield return wait;

        yield return new WaitUntil(() => cardGO.transform.rotation == Quaternion.Euler(0,0,0));

        

    }

    public void ReDeal()
    {
        
        foreach (var item in fiveCardGO)
        {   
            if(item.GetComponent<SpriteRenderer>().sprite != CardBack)
            {
                StartCoroutine(ReverseFlipCard(item,0.1f));
            }
            
        }

        foreach (var item in fiveCardGO)
        {   
            if(item.GetComponent<SpriteRenderer>().sprite != CardBack)
            {
                StartCoroutine(ReverseFlipCard(item,0.1f));
            }
            
        }

        if (mainCardCounter != 0)
        {
            theFiveCard.Clear();
            mainCardCounter = 0;
            
            deck = new Deck();

            for (int i = 0; i < 5; i++)
            {
                card = deck.DrawCard();
                theFiveCard.Add(card);
            }

        }

        cardReady = true;


    }





}
