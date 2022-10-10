using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to make projectiles move
/// </summary>
public class Projectile : MonoBehaviour
{
    [Tooltip("The distance this projectile will move each second.")]
    public float projectileSpeed = 3.0f;

    //Standard Unity function called once per frame
    private void Update()
    {
        MoveProjectile();
    }

    //Move the projectile in the direction it is heading
    private void MoveProjectile()
    {
        // move the transform
        transform.position = transform.position + transform.up * projectileSpeed * Time.deltaTime;
    }
}