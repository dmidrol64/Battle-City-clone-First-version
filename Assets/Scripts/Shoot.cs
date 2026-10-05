using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shoot : MonoBehaviour
{
    
    [SerializeField] GameObject bulletPrefab;
    //private List<GameObject> activeBullets = new List<GameObject>();
    private List<GameObject> sleepBullets = new List<GameObject>();
    PlayerMove playerMove;
    [SerializeField]Transform bulletPlace;


    private GameObject poolingObject;
    private int countBullet = 20;
    [SerializeField]float rate;
    float currentRate;

    Rigidbody2D _rb;
    float bulletSpeed;
    public int levelWeapon = 0;
    public PlayerMove PlayerComponent { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {

        _rb = GetComponent<Rigidbody2D>();
        playerMove = GetComponentInParent<PlayerMove>();
        
        if (bulletPlace == null) bulletPlace = transform;

        CreateBullet();
        currentRate = rate;
       
    }

    // Update is called once per frame
    void Update()
    {
        sleepBullets.RemoveAll(b => b == null);
        currentRate -= Time.deltaTime;

        if (playerMove != null)
        {
            // ���������� ��� �������� � ���� �������� �������:
            // ������ ������ ����� � ������ ����������� ����� � ����� ���� �� ������ ��������� ��������
            if (Input.GetKeyDown("space") && currentRate <= 0 && CanPlayerShoot())
            {
                // ����������� �������� ���� � ����������� �� �������� (levelWeapon)
                bulletSpeed = (levelWeapon >= 1) ? 700f : 400f;

                GetBullet();

                // ���������� ������ �����������
                currentRate = rate;
            }
        }
        
        else
        {
            ////currentRate -= Time.deltaTime;
            //if (currentRate <= 0 && EnemySpawn.IsTimeFrozen == false)
            //{
            //    //        //CreateBullet(BulletCollision.BulletOwner.Enemy);
            //    // CreateBullet();
            //    //return;

            //    CreateBullet();
            //    currentRate = rate;
            //}
            //else
            //{



            //}
            // ���� �������� ������ ����: ������� ������ � ����� �� ���������� � ��� ������� ���� ��� ������� (activeBullets.Count < 1)
            // if (currentRate <= 0 && EnemySpawn.IsTimeFrozen == false && sleepBullets.Count < 1)
            // {
            if (currentRate <= 0 && EnemySpawn.IsTimeFrozen == false && GetActiveBulletsCount() < 1)
            {
                bulletSpeed = 400f;
                GetBullet();
                currentRate = rate;
            }
            // }

        }
    }
   private int GetActiveBulletsCount()
    {
        int activeCount = 0;
        for (int i = 0; i < sleepBullets.Count; i++)
        {
            if (sleepBullets[i] != null && sleepBullets[i].activeInHierarchy)
            {
                activeCount++;
            }
        }
        return activeCount;
    }



private bool CanPlayerShoot()
    {
        int activeBullets = GetActiveBulletsCount();
        // ���� ������� ����� 0 ��� 1 � ��������� ��������, ������ ���� �� ������ ������ 1 ���� ������
        if (levelWeapon < 2 && activeBullets < 1) return true;

        // ���� ������� ����� 2 ��� 3 (������) � ���������, ���� �� ������ ������ 2 ���� ������
        if (levelWeapon >= 2 && activeBullets < 2) return true;

        // �� ���� ��������� ������� �������� ���������
        return false;
    }

    private GameObject GetBullet()
    {
        for (int i = 0; i < sleepBullets.Count; i++)
        {
            if (sleepBullets[i].activeInHierarchy == false)
            {
                poolingObject = sleepBullets[i];

                // 1. Перемещаем пулю к дулу и разворачиваем её
                poolingObject.transform.position = bulletPlace.position;
                poolingObject.transform.rotation = bulletPlace.rotation;

                // 2. ВКЛЮЧАЕМ пулю
                poolingObject.SetActive(true);
                poolingObject.transform.SetParent(null);

                // 3. НАСТРАИВАЕМ ФИЗИКУ (Перенесли сюда!)
                Rigidbody2D bulletRb = poolingObject.GetComponent<Rigidbody2D>();
                if (bulletRb != null)
                {
                    // Задаем скорость в направлении дула
                    bulletRb.linearVelocity = bulletPlace.up * bulletSpeed; // Делим на 50 для адекватной скорости в Physics2D
                }

                // 4. ПЕРЕДАЕМ ПАРАМЕТРЫ УРОНА (Перенесли сюда!)
                BulletCollision bulletScript = poolingObject.GetComponent<BulletCollision>();
                if (bulletScript != null)
                {
                    bulletScript.bulletPower = this.levelWeapon;
                    bulletScript.isPlayerBullet = (playerMove != null);
                }

                // 5. НАСТРАИВАЕМ СЛОИ (Перенесли сюда!)
                if (playerMove != null)
                {
                    poolingObject.layer = LayerMask.NameToLayer("PlayerBullet");
                }
                else
                {
                    poolingObject.layer = LayerMask.NameToLayer("EnemyBullet");
                }

                return poolingObject;
            }
        }
        return null;
    }



    void CreateBullet()
    {
        if (bulletPrefab == null && bulletPlace == null) return;
        for (int i = 0; i < countBullet; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform);
            bullet.gameObject.SetActive(false);
            sleepBullets.Add(bullet);
          
            Debug.Log($"Пуля создалась {sleepBullets}");
        }

        //void CreateBullet(BulletCollision.BulletOwner bulletOwner)
        //{
        //    if (bulletPrefab == null) return;
        //    bullet = Instantiate(bulletPrefab, bulletPlace.position, transform.rotation);

        //    BulletCollision bulletScript = bullet.GetComponent<BulletCollision>();
        //        if (bulletScript != null)
        //            {
        //                bulletScript.owner = bulletOwner;
        //                bulletScript.OnBulletHit += HandleBulletHit;
        //            }
        //}
        //private void HandleBulletHit(GameObject bulletObj,Collider2D collision) 
        //{
        // BulletCollision bulletScript = bulletObj.GetComponent<BulletCollision>();
        //    if (bulletScript == null) return;

        //        bulletScript.OnBulletHit -= HandleBulletHit;


        //    //if()
        // }

        //����������� � ��������� ������
        //if (playerMove != null)
        //{

        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        if (currentRate <= 0)
        //        {
        //            //if(levelWeapon >= 1)
        //            //{
        //            //    bulletSpeed = 15;
        //            //}
        //            //if (bullet != null)
        //            //{
        //            //    return;

        //            //}
        //            //else
        //            //{
        //            // CreateBullet(BulletCollision.BulletOwner.Player);
        //            //return;
        //            CreateBullet();
        //            currentRate = rate;
        //        }


        //    }

        //}

        //}


    }
}
