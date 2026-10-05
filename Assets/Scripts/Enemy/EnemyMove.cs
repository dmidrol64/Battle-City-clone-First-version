using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] public float enemySpeedMoving;
    [SerializeField] AudioClip _eDrive;

    private bool isFrozen = false;
    public float baseSpeed;
    public bool isBonusTank = false;
    private AudioSource _enemyDrive;
    Rigidbody2D _rb;

    SpriteRenderer _blinklColor;


    // Start is called before the first frame update
    void Start()
    {
       if( EnemySpawn.IsTimeFrozen)
        {
            FreezeTank();
        }
        else
        {
            UnfreezeTank();
        }
        baseSpeed = enemySpeedMoving;
        _blinklColor = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _enemyDrive = GetComponent<AudioSource>();

        StartCoroutine(BlinkBonusTank());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float currentEnemySpeed = isFrozen ? 0f : baseSpeed;

        _rb.linearVelocity =  transform.up * currentEnemySpeed;

        if( currentEnemySpeed > 0 && _enemyDrive.clip != _eDrive)
        {
            _enemyDrive.clip = _eDrive;
            _enemyDrive.loop = true;
            _enemyDrive.Play();
        }
        else if (currentEnemySpeed == 0)
        {
            _enemyDrive.Stop(); // Замерзший танк не шумит мотором
        }

    }

    public void FreezeTank() { isFrozen = true; }
    public void UnfreezeTank() { isFrozen = false; }

    public IEnumerator BlinkBonusTank()
    {
        while (true)
        {
            if (isBonusTank)
            {
                _blinklColor.color = Color.red;
                yield return new WaitForSeconds(0.2f); // Ждем в красном цвете

                _blinklColor.color = Color.white;
                yield return new WaitForSeconds(0.2f); // Ждем в белом цвете (ЗАДЕРЖКА, КОТОРОЙ НЕ ХВАТАЛО)
            }
            else
            {
                // 2. Если танк не бонусный, просто ждем 0.5 сек и проверяем заново,
                // чтобы цикл не зациклил игру намертво в один кадр!
                yield return new WaitForSeconds(0.5f);
            }
        }


    }
}
