using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBonusHandler : MonoBehaviour
{

    [SerializeField] private GameObject shieldVisualObject;
    EnemySpawn enemySpawn;

   public bool isInvulnereble = false;
   private bool isPlayer;

    private void Awake()
    {
        isPlayer = GetComponent<PlayerMove>() != null;
    }
    
   public void ActivateBonus(BonusType type)
    {
        enemySpawn = FindAnyObjectByType<EnemySpawn>();
        switch (type) {

            case BonusType.Star:
                UpgradeWeapon();
                break;


            case BonusType.Helmet:
               StartCoroutine(GiveShield());
                break;


            case BonusType.Pistol:
                MakeMaxLevel();
                break;

            case BonusType.Tank:
                ArmorTank();
                break;

            case BonusType.Shovel:
                ArmorBase();
                break;

            case BonusType.Timer:
                StartCoroutine(Freeze());
                break;

            case BonusType.Granade:
                Exploid();
                break;
        }
    }


    private void UpgradeWeapon()
    {
        Shoot weapon = GetComponentInChildren<Shoot>();
        if (weapon != null)
        {
            if (weapon.levelWeapon < 3)
            {
                weapon.levelWeapon++;

                Debug.Log($"{gameObject.name} повысил уровень оружия до: {weapon.levelWeapon}");
            }
        }
    }

    public IEnumerator GiveShield()
    {
        // 1. Включаем бессмертие именно этому танку
        isInvulnereble = true;

        // Если у префаба врага тоже настроен дочерний щит — показываем его
        if (shieldVisualObject != null)
        {
            shieldVisualObject.SetActive(true);
        }

        // 2. Танк бессмертен 5 секунд
        yield return new WaitForSeconds(5f);

        // 3. Время вышло — отключаем защиту
        isInvulnereble = false;
        if (shieldVisualObject != null)
        {
            shieldVisualObject.SetActive(false);
        }
    }

    private void MakeMaxLevel()
    {
        Shoot weapon = GetComponentInChildren<Shoot>();
        if (weapon != null)
        {
            weapon.levelWeapon = 3;
            Debug.Log($"{gameObject.name} получил МАКСИМАЛЬНЫЙ уровень оружия от Пистолета!");
        }
    }

    private void ArmorBase()
    {
        WallsAroundBase aroundBase = FindAnyObjectByType<WallsAroundBase>();
        if (isPlayer)
        {
            
            aroundBase.PickUpShovelBonus();
        }
        else
        {
            aroundBase.EnemyPickUpShovelBonus();
        }
       
    }
    private IEnumerator Freeze()
    {
        if (isPlayer)
        {
            EnemySpawn.IsTimeFrozen = true;
            for (int i = 0; i < enemySpawn.activeTank.Count; i++)
            {
                enemySpawn.activeTank[i].GetComponent<EnemyMove>().FreezeTank();

            }

            yield return new WaitForSeconds(5f);
            for (int i = 0; i < enemySpawn.activeTank.Count; i++)
            {
                enemySpawn.activeTank[i].GetComponent<EnemyMove>().UnfreezeTank();
            }
            EnemySpawn.IsTimeFrozen = false;
        }
        else
        {
            // если бонус взял ВРАГ, застывает Игрок!
            PlayerMove player = FindAnyObjectByType<PlayerMove>();
            if (player != null)
            {
                player.isFrozen = true; // Заколдовали игрока
            }

            yield return new WaitForSeconds(5f); // Игрок стоит беспомощным 5 секунд

            // Пытаемся снова найти игрока (вдруг его убили пока он стоял, и спавнился новый)
            PlayerMove regeneratedPlayer = FindAnyObjectByType<PlayerMove>();
            if (regeneratedPlayer != null)
            {
                regeneratedPlayer.isFrozen = false; // Разморозили игрока
            }
        }
    
    }

    private void Exploid()
    {
        if (isPlayer)
        {
            for (int i = 0; i < enemySpawn.activeTank.Count; i++)
            {
                if (enemySpawn.activeTank[i] != null)
                {
                    enemySpawn.activeTank[i].GetComponent<LivesComponent>().CountHits = 0;
                }

            }
        }
        else
        {
            PlayerMove activePlayer = FindAnyObjectByType<PlayerMove>();
            PlayerSpawnManager playerSpawn = FindAnyObjectByType<PlayerSpawnManager>();
            if(playerSpawn != null && activePlayer != null)
            {
                playerSpawn.Ressurecton(activePlayer.gameObject);
               
            }
        }
        
    }

    private void ArmorTank()
    {
        if (isPlayer)
        {
            PlayerSpawnManager manager = FindAnyObjectByType<PlayerSpawnManager>();
            manager.currentLives++;
            Debug.Log(manager.currentLives);
        }
        else
        {
            for (int i = 0; i < enemySpawn.activeTank.Count; i++)
            {
                if(enemySpawn.activeTank[i] != null)
                {
                    enemySpawn.activeTank[i].GetComponent<LivesComponent>().CountHits++;
                }
            }
        }
    }

    public void ActivateSpawnShield()
    {
        if (isPlayer)
        {
            StartCoroutine(GiveShield());
        }
        else
        {

        }
    }

}
