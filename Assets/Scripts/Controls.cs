using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public float sideSpeed = 0f; //Боковая скорость
    public Moving moving;
    float moveSide;
    float moveForward;
    public float scores = 0f; //Очки
    void Update()
    {
        moveSide = Input.GetAxis("Horizontal"); 
        //Когда игрок будет нажимать на стрелочки влево или вправо, сюда будет добавляться 1f или -1f
        moveForward = Input.GetAxis("Vertical"); 
        //То же самое, но со стрелочками вверх и вниз

        if (moveSide != 0)
        {
            sideSpeed = moveSide * -1f; //Если игрок нажал на стрелочки влево или вправо, задаём боковую скорость
        }

        if (moveForward != 0)
        {
            moving.speed += 0.01f * moveForward; //Если игрок нажал вверх или вниз
        }
        else //Если игрок не нажал ни вверх, ни вниз, то скорость будет постепенно возвращаться к нулю
        {
            if (moving.speed > 0)
            {
                moving.speed -= 0.01f;
            }
            else
            {
                moving.speed += 0.01f;
            }
        }

        if (moving.speed > moving.maxSpeed)
        {
            moving.speed = moving.maxSpeed; //Проверка на превышение максимальной скорости
        }

    }
}
