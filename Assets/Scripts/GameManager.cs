using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
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
    private GameObject _gameOverScreen;

    private float _timeRemainingForNextSpawn;

    private int _liveEnemies;

    private int _kills;

    private bool _hasGameStarted;

    private bool _isGameOver;

    public void StartGame()
    {
        _hasGameStarted = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
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

    private void OnEnemyDied(Enemy enemy)
    {
        _kills++;
        _liveEnemies--;
        _killsText.text = _kills.ToString();
    }

    private void OnPlayerDied()
    {
        _isGameOver = true;
        _gameOverScreen.SetActive(true);
    }
}
