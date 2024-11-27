using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Destroy Plant");
        TractorController tractorController = other.GetComponent<TractorController>();
        if (tractorController != null)
        {
            tractorController.currentLoad++;
        }
        Destroy(gameObject);
    }
}
