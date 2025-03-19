using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRange : MonoBehaviour
{
    Peashooter peashooter;

    private void Awake()
    {
        peashooter = GetComponentInParent<Peashooter>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie"))
        {
            peashooter.OnDetectedZombieIncrease();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie"))
        {
            peashooter.OnDetectedZombieDecrease();
        }
    }
}
