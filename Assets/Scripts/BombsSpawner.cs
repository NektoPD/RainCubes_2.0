using System;
using UnityEngine;

[RequireComponent(typeof(SpawnerView))]
public class BombsSpawner : ObjectPool<Bomb>, ISpawner
{
    [SerializeField] private Bomb _prefab;

    private int _spawnCount;
    private SpawnerView _view;

    public event Action<int> TotalCountChange;
    public event Action<int> ActiveCountChange;

    private void Awake()
    {
        _view = GetComponent<SpawnerView>();
        Initalize(_prefab);
        _view.SetSpawner(this);
    }

    public void Spawn(Vector3 position)
    {
        Bomb bomb = null;

        if(TryGetObject(out bomb, _prefab))
        {
            bomb.transform.position = position;
            bomb.Exploded += PutObject;
            bomb.gameObject.SetActive(true);
            _spawnCount++;
            TotalCountChange?.Invoke(_spawnCount);
            ActiveCountChange?.Invoke(GetActiveObjectsCount());
        }
    }
}
