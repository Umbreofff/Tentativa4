using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float bulletVelocity;
    [SerializeField] private AudioClip hitSound;

    private void Start()
    {
        StartCoroutine(BulletDespawn());
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * bulletVelocity *Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    IEnumerator BulletDespawn()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
