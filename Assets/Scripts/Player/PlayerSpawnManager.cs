using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    
    PlayerRessurection playerHP;
    TankBonusHandler handler;
     [SerializeField] GameObject playerPrefab;

    private int currentHits;
    public int currentLives;

    Vector3 spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        handler = FindAnyObjectByType<TankBonusHandler>();
        playerHP = FindAnyObjectByType<PlayerRessurection>();
        if (playerHP != null)
        {
            currentHits = playerHP.PlayerNumberOfHits;
            currentLives = playerHP.PlayerGlobalLives;
            PlayerSpawn();
        }
        else
        {
            Debug.LogError("Критическая ошибка: Скрипт PlayerRessurection не найден на сцене!");
        }

    }


    public void PlayerSpawn()
    {

        spawnPosition = new Vector3(-188, -268, 0);


        GameObject handlerObject =  Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        if(handlerObject != null )
        {
            TankBonusHandler tankBonus = handlerObject.GetComponent<TankBonusHandler>();

            if (tankBonus != null)
            {
                tankBonus.ActivateSpawnShield();
            }
        }
        

    }

    public void Ressurecton(GameObject DeadPlayerTank)
    {
        currentHits --;
        if (currentLives > 1)
        {
            if (currentHits <= 0)
            {
                if(DeadPlayerTank != null)
                {
                    Destroy(DeadPlayerTank);
                }
                PlayerSpawn();
                currentLives--;
               
                currentHits = playerHP.PlayerNumberOfHits;
                Debug.Log($"У объекта {gameObject.name} осталось попыток: {currentHits}");

                Debug.Log($"У объекта {gameObject.name} осталось жизней: {currentLives}");
            }

        }
        else
        {
            GameOverManager gameOver = FindAnyObjectByType<GameOverManager>();
            if (gameOver != null) { 
            gameOver.ShowGameOver();
            }
            return;
        }
        
       
    }

    
}
