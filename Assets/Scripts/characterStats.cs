using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.UIElements;

public class characterStats : MonoBehaviour
{
    public string characterName;
    public int maxHealth = 100;
    public int currentHealth;

    //UI¿ä¼Ò

    public Slider healthBar;
    public TextMeshPro healthText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
    }
}
