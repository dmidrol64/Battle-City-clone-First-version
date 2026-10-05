using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


public class EnemySpawn : MonoBehaviour
{
    [SerializeField] public EnemyCoast coast;
  
    public List<GameObject> activeTank =new List<GameObject>();
    Queue<GameObject> tankQueue = new Queue<GameObject>();

    TankBonusHandler bonusHandler;
    private int totalSpawnedTank = 0;

    public static bool IsTimeFrozen = false;

    void Start()
    {
        bonusHandler = FindAnyObjectByType<TankBonusHandler>();
        for (int i = 0; i < 16; i++)
        {
            GameObject addRandomTank = GetRandomTankByWeight();

            tankQueue.Enqueue(addRandomTank);
        }
        

        EnemyCounterUI counterUI = FindAnyObjectByType<EnemyCounterUI>();

        if (counterUI != null)
        {
            counterUI.InitializeIcons(16);
        }
        StartCoroutine(CreateTank());
    }

    // Корутина создает строго 4 танка с задержкой
    IEnumerator CreateTank()
    {
        while(true)
        {

               if(activeTank.Count < 4 && tankQueue.Count > 0)
               {
                    float randomX = UnityEngine.Random.Range(-576f, 552f);
                    Vector3 rand = new Vector3(randomX, 311f, 0f);

                    GameObject tankPrefab = tankQueue.Dequeue();
                    
                    GameObject _instTank =  Instantiate(tankPrefab, rand, Quaternion.identity);
                    activeTank.Add(_instTank);

                    totalSpawnedTank++;
                    
                    if(totalSpawnedTank == 4 || totalSpawnedTank == 9  || totalSpawnedTank == 13)
                        {
                            EnemyMove move = _instTank.GetComponent<EnemyMove>();
                            if (move != null)
                                {
                                    move.isBonusTank = true;
                                    
                                }
                        }
                    
                    
                        yield return new WaitForSeconds(2f);
                    
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
                }
  
        }
    } 

    // Самостоятельный метод для расчета вероятности спавна
    GameObject GetRandomTankByWeight()
    {
        // Проверка безопасности: если скрипт настроек забыли прикрепить в Инспекторе
        if (coast == null || coast.tankPool == null || coast.tankPool.Count == 0) return null;

        int totalWeight = 0;

        foreach (var tank in coast.tankPool)
        {
            totalWeight += tank.weight;
        }

        int randomValue = UnityEngine.Random.Range(0, totalWeight);

        foreach (var tank in coast.tankPool)
        {
            if (randomValue < tank.weight)
            {
                return tank._prefab; // Нашли нужный танк
            }
            randomValue -= tank.weight;
        }

        return coast.tankPool[0]._prefab; // Страховка
    }

    public void RemoveDeadTank(GameObject deadTank)
    {
        if (activeTank.Contains(deadTank))
        {
            activeTank.Remove(deadTank);
            
        }
    }

} 
    
    


