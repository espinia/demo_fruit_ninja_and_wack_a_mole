using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour
{
    public TextMeshProUGUI scoreAddText;
    public TextMeshProUGUI scoreText;
    public GameObject panelTime;
    public TextMeshProUGUI gameOverText;
    public Slider timeSlider;
    public GameObject titleScreen;
    public Button restartButton; 

    public List<GameObject> targetPrefabs;

    private int score;
    private float spawnRate = 1.5f;
    public bool isGameActive;

    private float spaceBetweenSquares = 2.5f; 
    private float minValueX = -3.75f; //  x value of the center of the left-most square
    private float minValueY = -3.75f; //  y value of the center of the bottom-most square

    private ArrayList rowList= new ArrayList();
    public int rowCount=4;
    public int colCount=4;

    private int countDown = 60;
    
    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    public void StartGame(int difficulty)
    {        
        for (int i = 0; i < rowCount; i++)
        {
            ArrayList row = new ArrayList();
            for(int j=0;j<colCount;j++)
			{
                row.Add(false);
			}
            rowList.Add(row);
        }

        spawnRate /= difficulty;
        isGameActive = true;
        StartCoroutine(SpawnTarget());
        score = 0;
        scoreText.gameObject.SetActive(true);
        UpdateScore(0);
        titleScreen.SetActive(false);
        
        StartCoroutine(TimerCountDown());
    }

    /// <summary>
    /// The timer count down corroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator TimerCountDown()
    {
        panelTime.SetActive(true);
        while (isGameActive &&  countDown > 0)
        {
            yield return new WaitForSeconds(1.0f);
            countDown--;
            //timeText.text = "Time: " + countDown;
            timeSlider.value = countDown;
        }

        if(isGameActive)
            GameOver();
    }
        
        // While game is active spawn a random target
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetPrefabs.Count);

            if (isGameActive)
            {
                Vector2 cellPosition = RandomCellPosition();
                int col = (int)cellPosition.x;
                int row = (int)cellPosition.y;
                float spawnPosX = minValueX + (row * spaceBetweenSquares);
                float spawnPosY = minValueY + (col * spaceBetweenSquares);

                Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);            

                GameObject instance=Instantiate(targetPrefabs[index],spawnPosition, targetPrefabs[index].transform.rotation);
                TargetX target = instance.GetComponent<TargetX>();
                target.row = row;
                target.col = col;
                
            }
            
        }
    }

    // Generate a random spawn position based on a random index from 0 to 3
    Vector2 RandomCellPosition()
    {
        int row = RandomSquareIndex();
        int col = RandomSquareIndex();

        ArrayList rowItem = (ArrayList)rowList[row];
        bool ocupied = (bool)rowItem[col];
        while (ocupied)
        {
            row = RandomSquareIndex();
            col = RandomSquareIndex();

            Debug.Log("Ocupado " + row + "," + col);
            rowItem = (ArrayList)rowList[row];
            ocupied = (bool)rowItem[col];
        }

        rowItem[col] = true;

        return new Vector2(col, row);

    }

    public void ClearBoardPosition(int row, int col)
	{
        ArrayList rowItem = (ArrayList)rowList[row];
        rowItem[col]=false;
        Debug.Log("Libera " + row + "," + col);
    }

    // Generates random square index from 0 to 3, which determines which square the target will appear in
    int RandomSquareIndex()
    {
        return Random.Range(0, 4);
    }

    // Update score with value from target clicked
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score:"+score;
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowAddScore(int addScore,Vector3 position)
	{
        scoreAddText.transform.position = position;
        if(addScore>0)
            scoreAddText.text = "+" + addScore;
        else
            scoreAddText.text = "" + addScore;

        scoreAddText.gameObject.SetActive(true);
        StartCoroutine(ShowAddScoreCorroutine());
    }
    IEnumerator ShowAddScoreCorroutine()
    {       
        yield return new WaitForSeconds(0.5f);
        scoreAddText.gameObject.SetActive(false);
    }

}
