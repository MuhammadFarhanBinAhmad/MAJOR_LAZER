using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdSpawner : MonoBehaviour
{
    public GameObject[] allowedBirds;

    public float spawnInterval = 2f;

    public int minimumBirds = 9;
    public int maximumBirds = 30;

    public float spawnRadius = 10f;

    private int activeBirdLimit;

    private List<GameObject> activeBirds =
        new List<GameObject>();

    private Coroutine spawnRoutine;

    private void Start()
    {
        // if (GameFlowManager.Instance == null)
        // {
        //     Debug.LogError("GameFlowManager is missing!");
        //     return;
        // }

        // GameFlowManager.Instance.BeginRound();

        activeBirdLimit = Random.Range(
            minimumBirds,
            maximumBirds + 1
        );

        spawnRoutine = StartCoroutine(SpawnBirds());
    }

    private IEnumerator SpawnBirds()
    {
        // while (GameFlowManager.Instance.IsRoundActive)
        // {
        //     activeBirds.RemoveAll(bird => bird == null);

        //     if (activeBirds.Count < activeBirdLimit)
        //     {
        //         SpawnOneBird();
        //     }

        //     yield return new WaitForSeconds(spawnInterval);
        // }
        while (true)
        {
            activeBirds.RemoveAll(bird => bird == null);

            if (activeBirds.Count < activeBirdLimit)
            {
                SpawnOneBird();
            }

            yield return new WaitForSeconds(spawnInterval);
        }

                    
    }

    private void SpawnOneBird()
    {
        if (allowedBirds == null || allowedBirds.Length == 0)
            return;

        int randomIndex = Random.Range(
            0,
            allowedBirds.Length
        );

        GameObject selectedBird = allowedBirds[randomIndex];

        if (selectedBird == null)
            return;

        Vector3 spawnPosition =
            transform.position +
            Random.insideUnitSphere * spawnRadius;

        GameObject newBird = Instantiate(
            selectedBird,
            spawnPosition,
            transform.rotation
        );

        activeBirds.Add(newBird);
    }

    private void OnDisable()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }
    }
}
