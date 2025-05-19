using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard" , menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    public enum CardType // ī�� Ÿ�� ������ �߰�
    {
        Attack,
        Heal,
        Buff,
        Utility
    }
    public enum AdditionalEffectType
    {
        None,
        DrawCard,
        DiscardCard,
        GainMana,
        ReduceEnemyMana,
        ReduceCardCost
    }
    public List<AdditionalEffect> additionalEffects = new List<AdditionalEffect>();

    public string cardName;
    public string description;
    public Sprite artwork;
    public int manaCost;
    public int effectAmount;
    public CardType cardType;

    public Color GetCardColor()
    {
        switch(cardType)
        {
            case CardType.Attack:
                return new Color(0.9f, 0.3f, 0.3f); //����
            case CardType.Heal:
                return new Color(0.3f, 0.9f, 0.3f); //���
            case CardType.Buff:
                return new Color(0.3f, 0.3f, 0.9f); //�Ķ�
            case CardType.Utility:
                return new Color(0.9f, 0.9f, 0.3f); //���
            default:
                return Color.white;
        }
    }

    public string GetAdditionalEffectsDescription()
    {
        if (additionalEffects.Count == 0)
            return "";
        string result = "\n";

        foreach(var effect in additionalEffects)
        {
            result += effect.GatDescription() + "\n";
        }
        return result;
    }
}
