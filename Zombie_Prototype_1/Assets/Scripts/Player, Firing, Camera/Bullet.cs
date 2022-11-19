using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public ParticleSystem ps;
    public Transform gun;

    public float bulletSpeed;
    public float damage;
    public float destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gun = GameObject.Find("FirePoint").transform;
        damage = FindObjectOfType<Shooting>().currentDamage;
        rb.AddForce(gun.transform.forward * bulletSpeed, ForceMode.Impulse);
    }

    private void PlayAudio(string name)
    {
        FindObjectOfType<AudioManager>().Play(name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        GetComponent<Light>().intensity = 0;

        if (collision.gameObject.GetComponent<Zombie>())
        {
            Zombie z = collision.gameObject.GetComponent<Zombie>();
            z.TakeDamage(damage);
            ps = GameObject.Find("ImpactSoftBody").GetComponent<ParticleSystem>();
            PlayAudio("HitMarker");
            PlayAudio("BloodSplat1");
        }

        if (collision.gameObject.CompareTag("Concrete"))
        {
            PlayAudio("BulletImpactStone");
            ps = GameObject.Find("ImpactConcrete").GetComponent<ParticleSystem>();
        }
        else if (collision.gameObject.CompareTag("Metal"))
        {
            PlayAudio("BulletImpactMetal");
            ps = GameObject.Find("ImpactMetal").GetComponent<ParticleSystem>();
        }
        else if (collision.gameObject.CompareTag("Wood"))
        {
            PlayAudio("BulletImpactWood");
            ps = GameObject.Find("ImpactWood").GetComponent<ParticleSystem>();
        }

        if (ps != null)
        {
            ps.Play();
        }
        else
        {
            print("Particle System not found, make sure to assign object's tag");
        }

        Destroy(gameObject, destroyTime);
    }
}
