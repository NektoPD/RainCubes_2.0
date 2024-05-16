using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;

    private MeshRenderer _meshRenderer;

    public event Action<Bomb> Exploded;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        StartCoroutine(DiactivateAndExplode());
    }

    private IEnumerator DiactivateAndExplode()
    {
        float diactivationTime = UnityEngine.Random.RandomRange(2, 6);
        float elapsedTime = 0;

        Color defaultColour = _meshRenderer.material.color;
        Color targetColout = new Color(defaultColour.r, defaultColour.g, defaultColour.b, 0);

        while (elapsedTime < diactivationTime) 
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / diactivationTime;

            _meshRenderer.material.color = Color.Lerp(defaultColour, targetColout, t);

            yield return null;
        }

        Explode();
    }

    private void Explode()
    {
        foreach (Rigidbody expodableObject in GetExpodableObjects())
        {
            expodableObject.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
        }

        Exploded?.Invoke(this);
    }

    private List<Rigidbody> GetExpodableObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        List<Rigidbody> objects = new();

        foreach (Collider hit in hits)
            if (hit.attachedRigidbody != null)
                objects.Add(hit.attachedRigidbody);

        return objects;
    }
}
