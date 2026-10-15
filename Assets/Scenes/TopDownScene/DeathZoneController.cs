using System;
using Unity.Collections.Tests.CoreCLR.TestJobs;
using UnityEngine;

public class DeathZoneController : MonoBehaviour
{
    [SerializeField] private Transform respawnPos;
    [SerializeField] private EndController endController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlatformerController>(out PlatformerController controller))
            {
                controller.Respawn(respawnPos.position);
            }

            endController.Reset();
        }
    }
}
