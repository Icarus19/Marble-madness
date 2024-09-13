using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    Transform cameraTransform;
    float force = 2f;
    [SerializeField]
    Rigidbody rigidBody;
    Vector3 spawnPoint;
    [SerializeField]
    SpawnLevel spawnLevel;
    [SerializeField]
    VisualEffect[] vfxEffect;

    void Awake()
    {
        if (UnityEngine.InputSystem.Accelerometer.current != null)
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Accelerometer.current);
        }

        spawnPoint = transform.position;
        spawnLevel = GameObject.FindAnyObjectByType<SpawnLevel>();

    }
    void Update()
    {
        if (UnityEngine.InputSystem.Accelerometer.current != null)
        {
            var rotation = UnityEngine.InputSystem.Accelerometer.current.acceleration.ReadValue();
            rotation.y = 0.0f;
            rotation.z *= -1;
            
            rigidBody.AddForce(rotation * force);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "GameBounds":
                transform.position = spawnPoint;
                break;
            case "Goal":
                Debug.Log("Goal reached");
                for(int i = 0; i < vfxEffect.Length; i++)
                {
                    vfxEffect[i].Play();
                }
                spawnLevel.ChangeTracking();
                StartCoroutine(WaitAndDestroy(transform.parent.gameObject, 1.0f));
                break;
            default:
                Debug.LogWarning($"collider {other.name} defaulted");
                break;
        }
    }

    IEnumerator WaitAndDestroy(GameObject o, float t)
    {
        Debug.Log("Insider");
        while (true)
        {
            Debug.Log("looper" + o.name);
            yield return new WaitForSeconds(t);
            Destroy(o);
        }
    }
}
