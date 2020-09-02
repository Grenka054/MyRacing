using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Moving : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject car; //Модель машины
    public GameObject brokenPrefab; //Префаб сломанной машины
    public GameObject modelHolder; //Объект, в который помещается модель
    public Controls control; //Скрипт управления, он будет добавлен позже
    public float speed = 0.1f; //Скорость на старте
    float minSpeed = 0.1f; //Минимальная скорость
    public float maxSpeed; //Максимальная скорость
    bool isKilled = false; //Эта переменная нужна, чтобы триггер сработал только один раз

    public List<GameObject> wheels; //Колёса машины

    float newSpeed;
    float sideSpeed;
    bool isAlive = true;
    string path;
    public float highScore = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        path = "highscore"; //Путь к файлу, в котором сохраняется высший результат
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            byte[] bytes = new byte[Convert.ToInt32(fs.Length)];

            fs.Read(bytes, 0, Convert.ToInt32(fs.Length));

            string high = Encoding.UTF8.GetString(bytes);

            try
            {
                highScore = Convert.ToSingle(high);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }

        }
    }

    void Update()
    {
        if (isAlive)
        {
            newSpeed = speed; //Скорость движения вперёд
            sideSpeed = 0f; //Скорость движения вбок

            if (newSpeed > maxSpeed)
            {
                newSpeed = maxSpeed; //Проверка на превышение максимальной скорости
            }

            if (newSpeed < minSpeed)
            {
                newSpeed = minSpeed; //Проверка на слишком низкую скорость
            }

            if (control != null)
            {
                sideSpeed = control.sideSpeed; //взять скорость игрока из Controls
            }
            //Изменение положения машины - она двигается вперёд
            //Для этого к её положению по оси X прибавляется новая скорость, положение по Y остаётся прежним
            //К положение по оси Z прибавляется 0.1f, умноженная на боковую скорость 
            transform.position = new Vector3(transform.position.x + newSpeed, transform.position.y, transform.position.z + 0.1f * sideSpeed);

            if (control != null)
            {
                control.sideSpeed = 0f; //Сброс боковой скорости
                control.scores += 0.1f; //Добавление очков
                car.GetComponent<AudioSource>().pitch = 2 + newSpeed; //Изменение высоты звука
            }

            if (wheels.Count > 0) //Если есть колёса
            {
                foreach (var wheel in wheels)
                {
                    wheel.transform.Rotate(-3f, 0f, 0f); //Вращение каждого колеса по оси X
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
      if (other.tag == "Car" || other.tag == "Wall") //Если машина игрока столкнулась со стеной или другой машиной
        {
            isAlive = false; //Игрок больше не жив

            if (car != null) //Если есть модель
            {
                if (!isKilled) //Если триггер ещё не сработал
                {
                    Destroy(car); //Удалить старую модель

                    //Добавить новую модель
                    var broken = Instantiate(brokenPrefab, transform.position, Quaternion.Euler(new Vector3(0f, -270f, 0f)));
                    broken.transform.SetParent(modelHolder.transform);

                    isKilled = true; //Указать, что триггер сработал
                    StartCoroutine(Die()); //Запустить процесс умирания
                }


            }
        }

        if (other.tag == "Coin") //Если столкновение с монетой
        {
            if (control != null) //Если столкнулась машина игрока
            {
                other.GetComponent<Coin>().Delete(); //Удалить монету

                control.scores += 100f; //Добавить 100 очков
            }
        }
    }

    IEnumerator Die() //Процесс умирания
    {
       using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
       {
          if (highScore < Math.Floor(control.scores))
          {
             byte[] newScores = Encoding.UTF8.GetBytes(Math.Floor(control.scores).ToString());
             fs.Write(newScores, 0, newScores.Length);
          }
       }
        yield return new WaitForSeconds(2f); //Подождать 2 секунды
        SceneManager.LoadScene("Menu"); //Перейти в меню
    }
}
