using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{

    public CardData cardData;     //ī�� ������
    public int cardIndex;

    //3D
    public MeshRenderer cardRenderer;     //ī�� ������ (icon or �Ϸ���Ʈ)
    public TextMeshPro nameText;         //�̸� �ؽ�Ʈ
    public TextMeshPro costText;         //���
    public TextMeshPro attackText;       //����
    public TextMeshPro descriptionText;      //���� �ؽ�Ʈ


    //ī�� ����
    public bool isDragging = false;
    private Vector3 originalPositon;   //�巡�� �� ���� ��ġ

    //���̾� ����ũ
    public LayerMask enemyLayer;    //�� ���̾�
    public LayerMask playerLayer;    //�÷��̾� ���̾�

    private CardManager cardManager;


    void Start()
    {
        //���̾� ����ũ ����
        playerLayer = LayerMask.GetMask("Player");
        enemyLayer = LayerMask.GetMask("Enemy");
        
        cardManager = FindObjectOfType<CardManager>();
        SetupCard(cardData);
    }

    //ī�� ������ ����
    public void SetupCard(CardData data)
    {
        cardData = data;

        //3D �ؽ�Ʈ ������Ʈ
        if (nameText != null) nameText.text = data.cardName;
        if (costText != null) costText.text = data.manaCost.ToString();
        if(attackText != null) attackText.text = data.effectAmount.ToString();
        if (descriptionText != null) descriptionText.text = data.description;

        //ī�� �ؽ��� ����
        if(cardRenderer != null && data.artwork != null)
        {
            Material cardMaterial = cardRenderer.material;
            cardMaterial.mainTexture = data.artwork.texture;
        }

    }

    private void OnMouseDown()
    {
        //�巡�׽��� �� ���� ��ġ ����
        originalPositon = transform.position;
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if(isDragging)
        {
            //���콺 ��ġ�� Ŭ�� �̵�
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);

        }
    }

    private void OnMouseUp()
    {
        characterStats playerStats = FindObjectOfType<characterStats>();
        if (playerStats == null || playerStats.currentMana < cardData.manaCost)
        {
            Debug.Log($"������ �����մϴ�! (�ʿ� : {cardData.manaCost}, ���� : {playerStats?.currentMana ?? 0})");
            transform.position = originalPositon;
            return;
        }

        isDragging = false;

        //�����ɽ�Ʈ�� Ÿ�� ����
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //ī�� ��� ���� ���� ����
        bool cardUsed = false;

        //�� ���� ��� �ߴ��� �˻�
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayer))
        {
            //������ ���� ȿ�� ����
            characterStats enemyStats = hit.collider.GetComponent<characterStats>();

            if(enemyStats != null)
            {
                if(cardData.cardType == CardData.CardType.Attack)  // ī�� ȿ���� ���� �ٸ���
                {
                    //���� ī��� ������ �ֱ�
                    enemyStats.TakeDamage(cardData.effectAmount);
                    Debug.Log($"{cardData.cardName} ī��� ������ {cardData.effectAmount} �������� �������ϴ�");
                    cardUsed = true;
                }
                else
                {
                    Debug.Log("�� ī��� ������ ����� �� �����ϴ�");
                }
            }
        }
        else if(Physics.Raycast(ray, out hit, Mathf.Infinity, playerLayer))
        {
            //�÷��̾�� �� ȿ�� ����
           // characterStats playerStats = hit.collider.GetComponent<characterStats>();

            if(playerStats != null)
            {
                if(cardData.cardType == CardData.CardType.Heal)
                {
                    //�� ī��� �����ϱ�
                    playerStats.Heal(cardData.effectAmount);
                    Debug.Log($"{cardData.cardName} ī��� �÷��̾��� ü���� {cardData.effectAmount} ȸ���߽��ϴ�");
                    cardUsed = true;
                }
                else
                {
                    Debug.Log("�� ī��� �÷��̾�� ����� �� �����ϴ�.");
                }
            }
        }
        else if(cardManager != null)
        {
            //���� ī�� ���� ��ó�� ����ߴ��� �˻�
            float distToDiscard = Vector3.Distance(transform.position, cardManager.discardPosition.position);
            if(distToDiscard < 2.0f)
            {
                cardManager.DiscardCard(cardIndex);
                return;
            }

        }

        //ī�带 ������� �ʾҴٸ� ���� ��ġ�� �ǵ�����
        if (!cardUsed)
        {
            transform.position = originalPositon;

            //���� ������ (�ʿ��� ���)
            cardManager.ArrangeHand();
        }
        else
        {
            //ī�带 ����ߴٸ� ���� ī�� ���̷� �̵�
            if (cardManager != null)
                cardManager.DiscardCard(cardIndex);

            //ī�� ��� �� ���� �Ҹ�(ī�尡 ���������� ���� �� �߰�)
            playerStats.UseMana(cardData.manaCost);
            Debug.Log($"������ {cardData.manaCost} ��� �߽��ϴ�. (���� ���� : {playerStats.currentMana})");

        }
             
    }
}
