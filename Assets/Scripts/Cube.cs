using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Color _defaultColour;
    private bool _hitFloorOnce = false;
    private readonly int _interval = 1;

    public event Action<Cube> ReadyForDiactivation;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _defaultColour = _meshRenderer.material.color;
    }
    private void OnDisable()
    {
        _meshRenderer.material.color = _defaultColour;
        _hitFloorOnce = false;
    }

    private void ChangeColor()
    {
        _meshRenderer.material.color = UnityEngine.Random.ColorHSV();
    }

    private IEnumerator DiactivateWithRandomDelay()
    {
        int diactivationTime = UnityEngine.Random.Range(2, 6);
        WaitForSeconds waitForSeconds = new WaitForSeconds(_interval);

        while (diactivationTime > 0)
        {
            diactivationTime--;

            yield return waitForSeconds;
        }

        ReadyForDiactivation?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Floor>(out Floor floor))
        {
            if(_hitFloorOnce == false)
            {
                _hitFloorOnce = true;
                ChangeColor();
                StartCoroutine(DiactivateWithRandomDelay());
            }        
        }
    }
}
