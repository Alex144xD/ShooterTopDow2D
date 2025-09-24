using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Bullet bulletPrefab;     
    [SerializeField, Min(0)] private int prewarm = 20;
    [SerializeField, Min(1)] private int chunkSize = 8;   
    [SerializeField] private int maxSize = -1;          
    [SerializeField] private Transform poolParent;        

    private readonly Queue<Bullet> _inactive = new Queue<Bullet>();
    private int _totalCreated = 0;

    private void Awake()
    {
        if (!poolParent) poolParent = this.transform;
        Prewarm(prewarm);
    }

    private void Prewarm(int count)
    {
        Expand(count);
    }

    private void Expand(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Checar límite
            if (maxSize > 0 && _totalCreated >= maxSize) break;

            var inst = Instantiate(bulletPrefab, poolParent);
            inst.gameObject.SetActive(false);
            inst.SetPoolReference(this); 
            _inactive.Enqueue(inst);
            _totalCreated++;
        }
    }


    public Bullet GetBullet(Vector3 position, Quaternion rotation)
    {
        var b = GetInternal();
        b.transform.SetPositionAndRotation(position, rotation);
        b.gameObject.SetActive(true);  
        return b;
    }

    public Bullet GetBullet()
    {
        var b = GetInternal();
        b.gameObject.SetActive(true);
        return b;
    }

    private Bullet GetInternal()
    {
        if (_inactive.Count == 0)
        {
            if (maxSize > 0 && _totalCreated >= maxSize)
            {
                Debug.LogWarning($"[BulletPool] Capacidad máxima alcanzada ({maxSize}).");
            }
            else
            {
                Expand(chunkSize);
            }
        }

        if (_inactive.Count == 0)
        {

            if (maxSize < 0 || _totalCreated < maxSize)
            {
                Expand(1);
            }
            else
            {
                return null;
            }
        }

        return _inactive.Dequeue();
    }

    public void ReturnBullet(Bullet bullet)
    {
        if (!bullet) return;
        if (bullet.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        if (bullet.TryGetComponent<TrailRenderer>(out var tr))
        {
            tr.Clear();
        }

        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(poolParent, false);
        _inactive.Enqueue(bullet);
    }
}