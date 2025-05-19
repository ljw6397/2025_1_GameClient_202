using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<CardData> deckCards = new List<CardData>();
    public List<CardData> handCards = new List<CardData>();
    public List<CardData> discardCards = new List<CardData>();

    public GameObject cardPrefab;
    public Transform deckPosition;
    public Transform handPosition;
    public Transform discardPosition;

    public List<GameObject> cardObjects = new List<GameObject>();

    void Start()
    {
        ShuffleDeck();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DrawCard();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ReturnDiscardsToDeck();
        }
        ArrangeHand();
    }
    public void ShuffleDeck()
    {
        List<CardData> tempDeck = new List<CardData>(deckCards);
        deckCards.Clear();

        while (tempDeck.Count > 0)
        {
            int randIndex = Random.Range(0, tempDeck.Count);
            deckCards.Add(tempDeck[randIndex]);
            tempDeck.RemoveAt(randIndex);
        }
        Debug.Log("���� �������ϴ�. : "+deckCards.Count + " ��"); 
    }

    public void DrawCard()
    {
        if (handCards.Count >= 6)
        {
            Debug.Log("���� ���� á���ϴ�. ! (�ִ� 6��)");
            return;
        }
        if (deckCards.Count == 0)
        {
            Debug.Log("���� ī�尡 �����ϴ�.");
            return;
        }

        CardData cardData = deckCards[0];
        deckCards.RemoveAt(0);

        handCards.Add(cardData);

        GameObject cardObj = Instantiate(cardPrefab, deckPosition.position, Quaternion.identity);

        CardDisplay cardDisplay = cardObj.GetComponent<CardDisplay>();

        if (cardDisplay != null)
        {
            cardDisplay.SetupCard(cardData);
            cardDisplay.cardIndex = handCards.Count - 1;
            cardObjects.Add(cardObj);
        }
        ArrangeHand();

        Debug.Log("ī�带 ��ο� �߽��ϴ�. : " + cardData.cardName + "���� : " + handCards.Count + "/6");
    }

    public void ArrangeHand()
    {
        if (handCards.Count == 0) return;

        float cardWidth = 1.2f;
        float spacing = cardWidth + 1.8f;
        float totalWidth = (handCards.Count - 1) * spacing;
        float starX = -totalWidth / 2f;

        for (int i = 0; i < cardObjects.Count; i++)
        {
            if (cardObjects[i] != null)
            {
                CardDisplay display = cardObjects[i].GetComponent<CardDisplay>();
                if (display != null && display.isDragging)
                    continue;

                Vector3 tarPosition = handPosition.position + new Vector3(starX + (i * spacing), 0, 0);

                cardObjects[i].transform.position = Vector3.Lerp(cardObjects[i].transform.position, tarPosition, Time.deltaTime * 10f);
            }
        }
    }

    public void DiscardCard(int handIndex)
    {
        if(handIndex < 0 || handIndex >= handCards.Count)
        {
            Debug.Log("��ȿ���� ���� ī�� �ε��� �Դϴ�!");
            return;
        }
        CardData cardData = handCards[handIndex];
        handCards.RemoveAt(handIndex);

        discardCards.Add(cardData);

        if(handIndex < cardObjects.Count)
        {
            Destroy(cardObjects[handIndex]);
            cardObjects.RemoveAt(handIndex);
        }
        for (int i = 0; i<cardObjects.Count;i++)
        {
            CardDisplay display = cardObjects[i].GetComponent<CardDisplay>();
            if (display != null) display.cardIndex = i;
        }
        ArrangeHand();
        Debug.Log("ī�带 ���Ƚ��ϴ�. "+ cardData.cardName);
    }

    public void ReturnDiscardsToDeck()
    {
        if (discardCards.Count ==0)
        {
            Debug.Log("���� ī�� ���̰� �� �ֽ��ϴ�. ");
            return;
        }
        deckCards.AddRange(discardCards);
        discardCards.Clear();
        ShuffleDeck();

        Debug.Log("���� ī��" + deckCards.Count + "���� ������ �ǵ����� �������ϴ�. "); 
    }
}
