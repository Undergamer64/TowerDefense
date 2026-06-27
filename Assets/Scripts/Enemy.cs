using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour, IPoolableObject<Enemy>
{
    public Pool<Enemy> _Pool { get; set; }

    [FormerlySerializedAs("OnDestroy")] public UnityEvent<Enemy> _OnDestroy;

    public EnemyType _Type;

    private Coroutine _changeColor;
    
    [FormerlySerializedAs("Speed")] public float _Speed = 2f;
    [FormerlySerializedAs("Life")] public float _Life = 2f;

    public void TakeDamage(float damage)
    {
        _Life -= damage;
        _changeColor = StartCoroutine("ChangeColor");
        if (!(_Life <= 0)) return;
        StopCoroutine(_changeColor);
        GetComponent<SpriteRenderer>().color = Color.white;
        _OnDestroy.Invoke(this);
        _Pool.Release(this);
    }

    private IEnumerator ChangeColor()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
