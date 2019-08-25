using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    [Header("Enemy Info")]
    public int enemyHealth;
    public int enemyAttack;

    [Header("Player Info")]
    public int playerHealth;

    [Header("Holder")]
    public Text battleText;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        enemyHealth = Random.Range(10, 30);
        enemyAttack = Random.Range(5, 10);

        ShowEnemyHealth();
        battleText.text += "적의 현재 공격력 : <b>" + enemyAttack + "</b>\n";
        ShowPlayerHealth();
    }

    public void PlayerAttack(int length)
    {
        int amount = (int)Mathf.Pow(2, length - 2);
        battleText.text += "<color=#0000ffff>플레이어의 공격! : <b>" + amount + "</b>의 데미지</color>\n";

        enemyHealth -= amount;

        ShowEnemyHealth();
    }

    public void EnemyAttack()
    {
        battleText.text += "<color=#ff0000ff>적의 공격! 플레이어는 <b>" + enemyAttack + "</b>의 데미지를 받았다.</color>\n";

        playerHealth -= enemyAttack;

        ShowPlayerHealth();
    }

    private void ShowPlayerHealth()
    {
        battleText.text += "플레이어의 현재 체력 : <b>" + playerHealth + "</b>\n";
    }

    private void ShowEnemyHealth()
    {
        battleText.text += "적의 현재 체력 : <b>" + enemyHealth + "</b>\n";
    }
}
