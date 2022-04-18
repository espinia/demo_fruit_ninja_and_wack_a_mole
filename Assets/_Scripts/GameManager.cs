using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string MAX_SCORE_PREF = "MAX_SCORE";
    public enum GameState
    {
        loading,
        inGame,
        gameOver,
    }

    public GameState gameState;

    public List<GameObject> targetPrefabs;

    public float spawnRate = 1.5f;    

    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    public TextMeshProUGUI scoreText;
    private int _score = 0;
    //variable autocomputada
    public int score 
    {
		set
		{
            //_score = Mathf.Max(value, 0);
            //tambien existe Clamp para estar entre dos valores
            _score = Mathf.Clamp(value, 0,9999);
        }
        get
        {
            return _score;
        }
    }

    public GameObject titleScreen;

    private int numberOfLives = 4;
    public List<GameObject> lives;

    /// <summary>
    /// Método que inicia la partida cambiando el valor del estado del juego
    /// </summary>
    /// <param name="difficulty">Número para configurar la dificultad</param>
    public void StartGame(int difficulty)
	{
        gameState = GameState.inGame;

        spawnRate /= difficulty;
        numberOfLives -= difficulty;

        for(int i=0;i<numberOfLives;i++)
		{
            lives[i].SetActive(true);
		}

        titleScreen.gameObject.SetActive(false);

        StartCoroutine(SpawnTarget());

        score = 0;
        UpdateScore(0);
    }

	private void Start()
	{
        ShowMaxScore();
	}

	// Update is called once per frame
	void Update()
    {
        
    }

    IEnumerator SpawnTarget()
	{
		while (gameState==GameState.inGame)
		{
            yield return new WaitForSeconds(spawnRate);            
            int idx = Random.Range(0, targetPrefabs.Count);
            Instantiate(targetPrefabs[idx]);
		}
	}

    /// <summary>
    /// Actualiza la puntuación, y lo muestra en pantalla
    /// </summary>
    /// <param name="scoreToAdd">Puntos a agregar</param>
    public void UpdateScore(int scoreToAdd)
	{
        score += scoreToAdd;
        scoreText.text = "PUNTUACION:\n" + score;
    }

    public void ShowMaxScore()
	{
        int maxScore = PlayerPrefs.GetInt(MAX_SCORE_PREF, 0);
        scoreText.text = "PUNTUACION MAXIMA:\n" + maxScore;
	}
    
    public void SetmaxScore()
	{
        int maxScore = PlayerPrefs.GetInt(MAX_SCORE_PREF, 0);
        if (score > maxScore)
        {
            PlayerPrefs.SetInt(MAX_SCORE_PREF, score);

            //TODO: Si hay puntuación máxima lanzar cohetes
        }

    }

    public void GameOver()
	{
        numberOfLives--;

        if (numberOfLives >= 0)
        {
            Image heartImage = lives[numberOfLives].GetComponent<Image>();
            Color tempColor = heartImage.color;
            tempColor.a = 0.20f;
            heartImage.color = tempColor;
        }

        if (numberOfLives <= 0)
        {
            SetmaxScore();

            gameState = GameState.gameOver;
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
	{
        //obtiene la escena actual y su nombre para recargarla
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
