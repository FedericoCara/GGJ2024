using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player _playerPrefab;

    [SerializeField]
    private Enemy _enemyPrefab;

    [SerializeField]
    private Transform[] _enemySpawnPoints;

    [SerializeField]
    private float _enemySpawnInterval = 0.5f;

    [SerializeField]
    private int _enemyMaxCount = 5;

    [SerializeField]
    private Text _killsText;

    [SerializeField]
    private Image _healthBar;

    [SerializeField]
    private GameObject[] _gameOverObjects;

    private float _timeRemainingForNextSpawn;

    private int _liveEnemies;

    private int _kills;

    private bool _hasGameStarted;

    private bool _isGameOver;

    private Player _player;

    private List<Enemy> _spawnedEnemies = new();

    public void StartGame()
    {
        _hasGameStarted = true;
        _isGameOver = false;
        _timeRemainingForNextSpawn = 0;

        if (_player == null)
        {
            _player = Instantiate(_playerPrefab);
        }
        UpdateKills();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    protected void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _player.SetOnEnemyDiedAction(OnPlayerDied);
        if(_healthBar != null)
        {
            _player.SetOnTakeDamageAction(health => _healthBar.fillAmount = health);
        }
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (!_hasGameStarted || _isGameOver)
        {
            return;
        }

        _timeRemainingForNextSpawn -= Time.deltaTime;
        if (_timeRemainingForNextSpawn < 0 && _liveEnemies < _enemyMaxCount)
        {
            var enemy = Instantiate(_enemyPrefab, _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Length)].position, Quaternion.identity);
            enemy.SetOnEnemyDiedAction(OnEnemyDied);
            _timeRemainingForNextSpawn = _enemySpawnInterval;
            _liveEnemies++;
        }
    }

    private void OnEnemyDied(Entity enemy)
    {
        _kills++;
        _liveEnemies--;
        UpdateKills();
    }

    private void UpdateKills()
    {
        _killsText.text = _kills.ToString();
    }

    private void OnPlayerDied(Entity player)
    {
        _isGameOver = true;
        player.gameObject.tag = "Untagged";
        _player = null;

        _spawnedEnemies.ForEach(enemy => Destroy(enemy));
        _spawnedEnemies.Clear();

        foreach (var go in _gameOverObjects)
        {
            go.SetActive(true);
        }
    }
}
