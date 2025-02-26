using UnityEngine;


public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _powerUps;
    [SerializeField] private float _spawnRate = 5f;
    [SerializeField] private float _xSpawnRange = 9f;
    [SerializeField] private float _zSpawnRange = 5f;

    private float _timer = 0;
    private int _powerUpIndex = 0;
    private int _powerUpsNumber;


    void Start()
    {
        _powerUpsNumber = _powerUps.Length;
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _spawnRate)
        {
            Instantiate(_powerUps[_powerUpIndex], new Vector3(Random.Range(-_xSpawnRange, _xSpawnRange),
                        1, Random.Range(-_zSpawnRange, _zSpawnRange)), Quaternion.identity);
            _powerUpIndex = (_powerUpIndex + 1) % _powerUpsNumber;
            _timer = 0;
        }
    }
}
