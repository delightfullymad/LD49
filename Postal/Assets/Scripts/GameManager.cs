using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    static public GameManager GM;
    public UI ui;
    public int waveNum;
    public int spawnedRound;
    //public int killedRound;
    public float spawnRate = 5f;
    public float nextSpawn;
    public Transform[] spawnPoints;
    public GameObject[] items;
    public Transform[] itemSpawnPoints;
    public GameObject enemy;
    private ColorAdjustments _col;
    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1f;
        GM = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnedRound < waveNum && Time.time > nextSpawn)
        {
            Vector3 rand = new Vector3(Random.Range(-4f, 4f), 0f, (Random.Range(-4f, 4f)));
            Instantiate(enemy, spawnPoints[Random.Range(0, spawnPoints.Length)].position + rand, Quaternion.identity);
            nextSpawn = Time.time + spawnRate;
            spawnedRound++;
            if (spawnRate > 1f)
            {
                spawnRate -= 0.01f;
            }
        }
        else if(spawnedRound >= waveNum)
        {
            nextSpawn = Time.time + 10f;
            waveNum++;
            if(waveNum % 5 == 0)
            {
                SpawnItems();
            }
            spawnedRound = 0;
        }

        if(Player.player.health <= 0)
        {
            Time.timeScale = 0.5f;
            ui.wasted.transform.GetComponent<CanvasGroup>().alpha += 0.2f * Time.deltaTime;
            Camera.main.GetComponent<Volume>().profile.TryGet<ColorAdjustments>(out _col);
            _col.saturation.value -= 0.2f + Time.deltaTime;
            if(ui.wasted.transform.GetComponent<CanvasGroup>().alpha >= 1f && Input.anyKeyDown)
            {
                SceneManager.LoadScene("menu");
            }
        }

    }

    void SpawnItems()
    {
        for (int i = 0; i < waveNum; i++)
        {
            if(Random.value > 0.25)
            {
                Instantiate(items[Random.Range(0, items.Length)], itemSpawnPoints[Random.Range(0, itemSpawnPoints.Length)].position, Quaternion.identity);
            }
        }
    }
}
