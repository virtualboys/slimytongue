using UnityEngine;
using System.Collections;

public class BombBugController : BugController {

    public float timeToExplode;
    public float explosionRadius;
    public float explosionPower;

    public MeshRenderer meshToBlink;
    private Material defaultMat;
    public Material blinkMat;

    private float m_spawnTime;
    private bool m_blinkOn;

    protected override void StartBug()
    {
        defaultMat = meshToBlink.material;
        m_spawnTime = Time.time;
    }

    protected override void UpdateBug()
    {
        if(Time.time - m_spawnTime > timeToExplode)
        {
            Explode();
        }

        Blink();
    }

    private void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 3.0F);

        }

        Destroy(gameObject);
    }

    public override bool IsShootable()
    {
        return true;
    }

    private void Blink()
    {
        float ttd = timeToExplode - (Time.time - m_spawnTime);
        float ttf = timeToExplode*.25f ;
        if (ttd > ttf)
        {
            return;
        }
        //Debug.Log(ttd);
        if (ttd > ttf * .75f)
        {
            if (!m_blinkOn)
            {
                m_blinkOn = true;
                meshToBlink.material = blinkMat;
            }
        }
        else if (ttd > ttf * .5f)
        {
            if (m_blinkOn)
            {
                m_blinkOn = false;
                meshToBlink.material = defaultMat;
            }
        }
        else if (ttd > ttf * 3.0f / 8.0f)
        {
            if (!m_blinkOn)
            {
                m_blinkOn = true;
                meshToBlink.material = blinkMat;
            }
        }
        else if (ttd > ttf * .25f)
        {
            if (m_blinkOn)
            {
                m_blinkOn = false;
                meshToBlink.material = defaultMat;
            }
        }
        else if (ttd > ttf * 1.0f / 8.0f)
        {
            if (!m_blinkOn)
            {
                m_blinkOn = true;
                meshToBlink.material = blinkMat;
            }
        }
        else
        {
            if (m_blinkOn)
            {
                m_blinkOn = false;
                meshToBlink.material = defaultMat;
            }
        }
    }
}
