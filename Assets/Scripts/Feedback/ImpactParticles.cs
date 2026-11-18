
using UnityEngine;

public class ImpactParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem impactEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        impactEffect.transform.position = contact.point;
        impactEffect.Play();
    }
}
