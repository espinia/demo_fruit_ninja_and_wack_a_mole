using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float minForce = 12, maxForce = 16,
           maxTorque = 10, xRange = 4, ySpawnPos = -5.0f;

    private GameManager gameManager;

    [Range(-100,100)]
    public int pointValue;

    public ParticleSystem explosionParticle;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.AddForce(RandomForce(),
                            ForceMode.Impulse);
        _rigidbody.AddTorque(RandomTorque(),
                             RandomTorque(),
                             RandomTorque(),
                             ForceMode.Impulse);
        //z=0
        _rigidbody.transform.position = RandomSpawnPos();

        //por nombre y luego obtenemos el script
        // gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //por tipo
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Genera un vector aleatorio en 3D
    /// </summary>
    /// <returns>Fuerza aleatoria hacia arriba</returns>
    private Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minForce, maxForce);
    }

    /// <summary>
    /// Genera un número aleatorio 
    /// </summary>
    /// <returns>Valor aleatorio entre -maxTorque, maxTorque</returns>
    private float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    /// <summary>
    /// Genera una posición aleatoria
    /// </summary>
    /// <returns>Posición aleatoria en 3d con coordenada z =0</returns>
    private Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    //private void OnMouseDown()
    private void OnMouseOver()
    {
        if (gameManager.gameState == GameManager.GameState.inGame)
        {
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation).Play();
            gameManager.UpdateScore(pointValue);
            /*
            if (gameObject.CompareTag("Bad"))
            {
                gameManager.GameOver();
            }*/
        }
    }

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("KillZone"))
		{
            Destroy(gameObject);
            if (gameObject.CompareTag("Good"))
            {
                //gameManager.UpdateScore(-10);
                gameManager.GameOver();
            }            
        }
	}
}
