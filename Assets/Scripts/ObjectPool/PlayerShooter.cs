using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private Transform firePoint;   
    [SerializeField] private BulletPool bulletPool;
    [SerializeField, Min(0.01f)] private float fireRate = 0.3f;

    private float fireTimer;

    private void Update()
    {
        fireTimer += Time.deltaTime;

        if (Input.GetButton("Fire1") && fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    private void Shoot()
    {
      
        bulletPool.GetBullet(firePoint.position, firePoint.rotation);
    }
}