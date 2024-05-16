using System;
using TMPro;
using UnityEngine;

public class SpawnerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _totalAmountText;
    [SerializeField] private TMP_Text _activeAmountText;
    
    private ISpawner _spawner;

    private void OnDisable()
    {
        _spawner.TotalCountChange -= SetTotalAmount;
        _spawner.ActiveCountChange -= SetActiveAmount;
    }

    public void SetTotalAmount(int amount)
    {
        _totalAmountText.text = amount.ToString();
    }
    public void SetActiveAmount(int amount)
    {
        _activeAmountText.text = amount.ToString();
    }

    public void SetSpawner(ISpawner spawner)
    {
        if(spawner == null)
            throw new ArgumentNullException(nameof(spawner));

        _spawner = spawner;
        _spawner.TotalCountChange += SetTotalAmount;
        _spawner.ActiveCountChange += SetActiveAmount;
    }
}

