using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesComponent : MonoBehaviour
{

    [SerializeField] int numberOfHits;
    private int currentHits;
    
    public int CountHits
    { 
        get { return currentHits; }
        set {  currentHits = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHits = numberOfHits;
        
    }

    // Update is called once per frame
    void Update()
    {
        DestroyTank();
    }

    public void DestroyTank()
    {
        if (CountHits <= 0)
        {
            EnemySpawn spawnManager = FindAnyObjectByType<EnemySpawn>();
            if (spawnManager != null) spawnManager.RemoveDeadTank(gameObject);

            EnemyCounterUI counter = FindAnyObjectByType<EnemyCounterUI>();
            if (counter != null) counter.OnEnemyDestroyed();

            Destroy(gameObject);
        }
    }
}

