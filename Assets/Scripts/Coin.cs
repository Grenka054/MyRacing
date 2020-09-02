using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    int direction = 1; //Направление движения монеты

    float high = 1.2f; //Наивысшая точка
    float low = 0.7f; //Низшая точка

    public GameObject coinSound; //Звук монеты

    void Update()
    {
        transform.Rotate(0f, 1f, 0f); //Монета с каждым кадром будет вращаться

        if (direction > 0) //Если направление больше нуля, то монета будет двигаться вверх, 
            if (transform.position.y < high) //пока не достигнет наивысшей точки
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
            else //После направление изменится
                direction *= -1;
        else
            if (transform.position.y > low) //И монета будет двигаться вниз
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
            else
                direction *= -1; //А когда достигнет низшей точки, снова начнёт двигаться вверх
    }

    public void Delete() //Удаление монеты
    {
        var sound = Instantiate(coinSound, transform.position, transform.rotation); //Добавление звука монеты

        Destroy(sound, 2f); //Уничтожение звука через две секунды
        Destroy(gameObject); //Сама монета удалится сразу
    }
}
