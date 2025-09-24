using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyContactDamage : MonoBehaviour
{
    [Header("Daño")]
    [SerializeField] private int damage = 10;

    [Tooltip("Tiempo entre daños repetidos al MISMO objetivo.")]
    [SerializeField, Min(0f)] private float damageInterval = 1.0f;

    [Tooltip("Aplicar daño inmediato al entrar al trigger (si está en true). Si está en false, espera al primer intervalo.")]
    [SerializeField] private bool damageOnEnter = true;

    [Header("Filtrado")]
    [Tooltip("Si se deja vacío, no se filtra por tag. Si se setea, SOLO dañará objetos con este tag (ej: 'Player').")]
    [SerializeField] private string targetTag = "Player";

    private readonly Dictionary<Collider2D, float> _cooldowns = new Dictionary<Collider2D, float>();

    // Cache para evitar allocs
    private readonly List<Collider2D> _tempToProcess = new List<Collider2D>();

    private void OnEnable()
    {
        _cooldowns.Clear();
    }

    private void OnDisable()
    {
        _cooldowns.Clear();
    }

    private void Update()
    {
        if (_cooldowns.Count == 0) return;

        _tempToProcess.Clear();
        _tempToProcess.AddRange(_cooldowns.Keys);

        for (int i = 0; i < _tempToProcess.Count; i++)
        {
            var col = _tempToProcess[i];
            if (col == null) 
            {
                _cooldowns.Remove(col);
                continue;
            }
            float t = _cooldowns[col] - Time.deltaTime;

            if (t <= 0f)
            {
                // Intentamos aplicar daño
                if (IsValidTarget(col))
                {
                    if (TryApplyDamage(col.gameObject))
                    {
                        // Reinicia cooldown tras aplicar daño
                        t = damageInterval;
                    }
                    else
                    {
                    
                        _cooldowns.Remove(col);
                        continue;
                    }
                }
                else
                {
               
                    _cooldowns.Remove(col);
                    continue;
                }
            }

            _cooldowns[col] = t;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsValidTarget(other)) return;


        if (_cooldowns.ContainsKey(other)) return;

        if (damageOnEnter)
        {
            if (TryApplyDamage(other.gameObject))
            {
                _cooldowns[other] = damageInterval; 
            }
            else
            {

            }
        }
        else
        {

            _cooldowns[other] = damageInterval;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
       
        if (_cooldowns.ContainsKey(other))
            _cooldowns.Remove(other);
    }

    private bool IsValidTarget(Collider2D col)
    {
        if (!col || !col.gameObject.activeInHierarchy) return false;
        if (!string.IsNullOrEmpty(targetTag) && !col.CompareTag(targetTag)) return false;
        return true;
    }


    private bool TryApplyDamage(GameObject target)
    {
        if (target == null) return false;

        if (target.TryGetComponent<IDamageable>(out var dmg))
        {
            dmg.TakeDamage(damage);
            return true;
        }


        var presenter = target.GetComponent<PlayerHealthPresenter>();
        if (presenter != null)
        {
            presenter.TakeDamage(damage);
            return true;
        }

        return false;
    }
}

public interface IDamageable
{
    void TakeDamage(int amount);
}