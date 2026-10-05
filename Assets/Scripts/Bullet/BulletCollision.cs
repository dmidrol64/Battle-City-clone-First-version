using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public int bulletPower;     // Уровень оружия (0, 1, 2 или 3)
    public bool isPlayerBullet; // Кто выстрелил — Игрок (true) или Враг (false)

    // Этот метод вызывается Unity автоматически КАЖДЫЙ РАЗ, когда пуля активируется из пула
    private void OnEnable()
    {
        // Если за 4 секунды пуля ни во что не попала (улетела за экран), выключаем её
        Invoke(nameof(DeactivateBullet), 4f);
    }

    private void OnDisable()
    {
        // Обязательно отменяем таймер, если пуля попала в цель раньше, чем через 4 секунды
        CancelInvoke(nameof(DeactivateBullet));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Вода — пуля пролетает насквозь
        if (collision.gameObject.GetComponent<Water>())
        {
            return;
        }

        // Кирпичная стена — разрушается, пуля уходит в пул
        if (collision.gameObject.GetComponent<BrickWall>())
        {
            Destroy(collision.gameObject);
            DeactivateBullet();
            return;
        }

        // Стальная стена — разрушается только если у пули максимальный уровень (3)
        if (collision.gameObject.GetComponent<SteelWall>())
        {
            Debug.Log($"[ТЕСТ СТАЛИ] Врезался в железо! Моя bulletPower = {bulletPower}. Пуля игрока? = {isPlayerBullet}");

            if (bulletPower == 3)
            {
                Destroy(collision.gameObject);
            }

            DeactivateBullet();
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            DeactivateBullet();
            return;
        }

        // Штаб (База) — получает урон при любом попадании
        BaseHQ baseHQ = FindAnyObjectByType<BaseHQ>();
        if (collision.gameObject.GetComponent<EagleHonor>() != null)
        {
            if (baseHQ != null)
            {
                baseHQ.TakeDamage();
            }
            Destroy(collision.gameObject);
            DeactivateBullet();
            return;
        }

        LivesComponent hp = collision.gameObject.GetComponent<LivesComponent>();
        PlayerSpawnManager playerRessur = FindAnyObjectByType<PlayerSpawnManager>();
        TankBonusHandler enemyTankBonus = collision.gameObject.GetComponent<TankBonusHandler>();

        if (playerRessur != null)
        {
            if (collision.gameObject.GetComponent<PlayerMove>() != null)
            {
                TankBonusHandler bonusHandler = collision.gameObject.GetComponent<TankBonusHandler>();
                if (bonusHandler != null && bonusHandler.isInvulnereble)
                {
                    DeactivateBullet();
                    return;
                }
                playerRessur.Ressurecton(collision.gameObject);
                DeactivateBullet();
                return;
            }
        }
        if (hp != null)
        {
            if (enemyTankBonus != null && enemyTankBonus.isInvulnereble)
            {
                DeactivateBullet(); 
                return; // Выходим, враг не получает урона
            }
            hp.CountHits--;

            if (hp.CountHits <= 0)
            {
                EnemyMove enemyMove = collision.gameObject.GetComponent<EnemyMove>();
                BonusSpwn bonusSpwn = FindAnyObjectByType<BonusSpwn>();

                if (enemyMove != null)
                {
                    EnemyCounterUI counter = FindAnyObjectByType<EnemyCounterUI>();

                    EnemySpawn spawnManager = FindAnyObjectByType<EnemySpawn>();
                    if (spawnManager != null)
                    {
                        spawnManager.RemoveDeadTank(collision.gameObject);

                    }
                    if (enemyMove.isBonusTank)
                    {
                        bonusSpwn.BonusInst();
                    }

                    if (counter != null)
                    {
                        counter.OnEnemyDestroyed();
                    }

                }
                Destroy(collision.gameObject);
                DeactivateBullet();
                return;

            }
        }
    }

    // Универсальный метод возврата пули в пул
    private void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }
}
