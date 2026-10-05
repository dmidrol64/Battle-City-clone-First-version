using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class EnemySpawnData 
{
    public GameObject _prefab;
    //public int chance;

    [Range(1, 100)]
   public int weight = 50;

    public int startingLives;




}
