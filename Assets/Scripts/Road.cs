using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public List<GameObject> blocks; //Коллекция всех дорожных блоков
    public List<GameObject> cars; //Коллекции машин
    public List<GameObject> coins; //Коллекции монет
    public GameObject player; //Игрок
    public GameObject roadPrefab; //Префаб дорожного блока
    public GameObject carPrefab; //Префаб машины NPC
    public GameObject coinPrefab; //Префаб монеты

    System.Random rand = new System.Random(); //Генератор случайных чисел

    /// <summary>
    /// Проверка, проехал ли игрок этот блок
    /// </summary>
    bool fetched; 
    /// <summary>
    /// Положение игрока
    /// </summary>
    float x; 
    void Update()
    {
        //x = player.GetComponent<Moving>().rb.position.x; //Получение положения игрока //Я сделал проще
        x = player.transform.position.x;
        GameObject last = blocks[blocks.Count - 1]; //Номер дорожного блока, который дальше всех от игрока

        if (x > last.transform.position.x - 24.69f * 6f) //Если игрок подъехал к последнему блоку ближе, чем на 10 блоков
        {
            //Инстанцирование нового блока
            GameObject block = Instantiate(roadPrefab, new Vector3(last.transform.position.x + 24.69f, last.transform.position.y, last.transform.position.z), Quaternion.identity);
            block.transform.SetParent(gameObject.transform); //Перемещение блока в объект Road
            blocks.Add(block); //Добавление блока в коллекцию

            float side = rand.Next(1, 3) == 1 ? -1f : 1f; //Случайное определение стороны появления машины

            //Добавление машины на сцену
            GameObject car = Instantiate(carPrefab, new Vector3(last.transform.position.x + 24.69f, last.transform.position.y + 0.25f, last.transform.position.z + 1.30f * side), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
            car.transform.SetParent(gameObject.transform); //Добавление машины в объект Road
            cars.Add(car);

            if (rand.Next(0, 100) > 70) //Добавление монеты с вероятностью 30%
            {
                GameObject coin = Instantiate(coinPrefab, new Vector3(last.transform.position.x + 24.69f, last.transform.position.y + 0.20f, last.transform.position.z + 1.50f * side * -1f), Quaternion.identity);
                coin.transform.SetParent(gameObject.transform);
                coins.Add(coin);
            }
        }

        foreach (GameObject block in blocks)
        {
            fetched = block.GetComponent<RoadBlock>().Fetch(x); //Проверка, проехал ли игрок этот блок

            if (fetched) //Если проехал
            {
                blocks.Remove(block); //Удаление блока из коллекции
                Destroy(block); //Удаление блока со сцены
            }
        }
        StartCoroutine(DeleteCars());
        StartCoroutine(DeleteCoins());
        IEnumerator DeleteCars() {
            foreach (GameObject car in cars)
            {
                if (car.transform.position.y < -0.8) //Если проехал
                {
                    cars.Remove(car); //Удаление car из коллекции
                    Destroy(car); //Удаление car со сцены
                }
            }
            yield return new WaitForSeconds(1f);
        }
        IEnumerator DeleteCoins() {
            foreach (GameObject coin in coins)
            {
                if (coin == null)
                {
                    coins.Remove(coin);
                }
                fetched = coin.GetComponent<RoadBlock>().Fetch(x); //Проверка, проехал ли игрок этот coin

                if (fetched) //Если проехал
                {
                    coins.Remove(coin); //Удаление coin из коллекции
                    Destroy(coin);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
