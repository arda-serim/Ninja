using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    [SerializeField] List<GameObject> decoratives;
    [SerializeField] List<GameObject> obstacles;

    public override void Init()
    {
        StartCoroutine(SpawnDecoratives());
        GameManager.Instance.startGame += () => this.enabled = true;
    }

    void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnDecoratives()
    {
        while (true)
        {
            Instantiate(decoratives[Random.Range(0, decoratives.Count)], Camera.main.transform.position + new Vector3(30, -2.15f, 10), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(4f, 9f));
        }
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            GameObject go = Instantiate(obstacles[Random.Range(0, obstacles.Count)], Camera.main.transform.position + new Vector3(30, 0, 10), Quaternion.identity);
            yield return new WaitForSeconds(1);
            go.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            yield return new WaitForSeconds(Random.Range(5f, 8f));
        }
    }
}
