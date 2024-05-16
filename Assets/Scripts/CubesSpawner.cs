using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpawnerView))]
public class CubesSpawner : ObjectPool<Cube>, ISpawner
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private float _minXPosition;
    [SerializeField] private float _maxXPosition;
    [SerializeField] private float _secondsBetweenSpawn;

    private int _spawnCount;
    private SpawnerView _view;

    public event Action<int> TotalCountChange;
    public event Action <Vector3> OnSpawn;

    public event Action<int> ActiveCountChange;

    private void Awake()
    {
        _view = GetComponent<SpawnerView>();
        Initalize(_prefab);
        _view.SetSpawner(this);
    }

    private void Start()
    {
        StartCoroutine(GenerateCubes());
    }

    private IEnumerator GenerateCubes()
    {
        WaitForSeconds interval = new WaitForSeconds(_secondsBetweenSpawn);

        while (enabled)
        {
            Spawn();

            yield return interval;
        }
    }

    private void Spawn()
    {
        Cube cube = null;

        if(TryGetObject(out cube, _prefab))
        {
            float spawnPositionX = UnityEngine.Random.Range(_minXPosition, _maxXPosition);

            cube.transform.position = new Vector3(spawnPositionX, transform.position.y, transform.position.z);
            cube.ReadyForDiactivation += PutObject;
            cube.ReadyForDiactivation += GetCubePosition;
            cube.gameObject.SetActive(true);
            _spawnCount++;
            TotalCountChange?.Invoke(_spawnCount);
            ActiveCountChange?.Invoke(GetActiveObjectsCount());
        }
    }

    private void GetCubePosition(Cube cube)
    {
        OnSpawn?.Invoke(cube.transform.position);
    }
}
