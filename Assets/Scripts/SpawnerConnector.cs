using UnityEngine;

public class SpawnerConnector : MonoBehaviour
{
    [SerializeField] private CubesSpawner _cubeSpawner;
    [SerializeField] private BombsSpawner _bombSpawner;

    private void Start()
    {
        _cubeSpawner.OnSpawn += TransferCubePosition;
    }

    private void TransferCubePosition(Vector3 cubePosition)
    {
        _bombSpawner.Spawn(cubePosition);
    }
}
