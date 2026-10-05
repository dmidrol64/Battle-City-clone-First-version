using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpwn : MonoBehaviour
{
   [SerializeField] public BonusSpawnData bonusData;
    

    public void BonusInst()
    {
        
        if (bonusData == null || bonusData._bonusPrefab == null || bonusData._bonusPrefab.Count == 0)
        {
            return;
        }

            Vector2 randomPoint = new Vector2(Random.Range(-576f, 552f), Random.Range(283f, -300f));
            int randomPrefab = Random.Range(0, bonusData._bonusPrefab.Count);

            GameObject randomObject = bonusData._bonusPrefab[randomPrefab];

            GameObject clone = Instantiate(randomObject, randomPoint, Quaternion.identity);

            
            Destroy(clone, 10f);
        
    }

    
}
