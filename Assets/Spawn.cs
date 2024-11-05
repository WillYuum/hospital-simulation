using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject patientPrefab;
    public int numPatients;

    // Start is called before the first frame update
    void Start()
    {
        numPatients = 25;
        // for (int i = 0; i < numPatients; i++)
        // {
        //     SpawnPatient();
        // }

        InvokeRepeating(nameof(SpawnPatient), 5, 15);
    }

    void SpawnPatient()
    {
        Instantiate(patientPrefab, this.transform.position, Quaternion.identity);
        // Invoke("SpawnPatient", Random.Range(2, 10));
    }



    // Update is called once per frame
    void Update()
    {

    }
}
