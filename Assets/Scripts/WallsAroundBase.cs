using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsAroundBase : MonoBehaviour
{

    [SerializeField] private GameObject brickDefensePrefab; // Сюда перетащим объект-папку кирпичей
    [SerializeField] private GameObject steelDefenseGroup; // Сюда перетащим объект-папку стали

    private GameObject currentBrickDefense;

    private Coroutine shovelCoroutine;

    Vector3 positionBase;
    // Start is called before the first frame update
    void Start()
    {
        positionBase = new Vector3(15, -205, 0);
        currentBrickDefense = Instantiate(brickDefensePrefab, positionBase, Quaternion.identity);
        steelDefenseGroup.SetActive(false);
    }

    IEnumerator ShovelBonus()
    {
        // 1. Включаем сталь, а текущий кирпич  просто прячем
        steelDefenseGroup.SetActive(true);
        if (currentBrickDefense != null) currentBrickDefense.SetActive(false);

        yield return new WaitForSeconds(10f); // Ждем 10 секунд защиты

        // 2. Время вышло! удаляем старый побитый кирпич со сцены
        if (currentBrickDefense != null) Destroy(currentBrickDefense);

        // 3. Спавним новую, абсолютно целую кирпичную стену на то же место!
        currentBrickDefense = Instantiate(brickDefensePrefab, positionBase, Quaternion.identity);

        // 4. Выключаем сталь
        steelDefenseGroup.SetActive(false);
    }

    public void PickUpShovelBonus()
    {
        // Если корутина уже работала (игрок взял вторую лопату подряд), 
        // сначала останавливаем её, чтобы таймер сбросился
        if (shovelCoroutine != null) StopCoroutine(shovelCoroutine);

        // Запускаем корутину защиты и сохраняем ссылку на неё
        shovelCoroutine = StartCoroutine(ShovelBonus());
        StartCoroutine(ShovelBonus());
    }

    public void EnemyPickUpShovelBonus()
    {
        if (shovelCoroutine != null)
        {
            StopCoroutine(shovelCoroutine);
            shovelCoroutine = null; // Очищаем ссылку
        }
        if (currentBrickDefense != null) Destroy(currentBrickDefense);
        if (steelDefenseGroup != null) steelDefenseGroup.SetActive(false);

        Debug.Log("Враг подобрал лопату: Защита базы полностью уничтожена!");
    }
}
