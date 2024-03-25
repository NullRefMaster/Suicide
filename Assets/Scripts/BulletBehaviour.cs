using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletBehaviour : IManualBehaviour
{
    const float kMoveSpeed = 20.0f;

    public override void ManualUpdate()
    {
        transform.Translate(kMoveSpeed * Time.smoothDeltaTime * Vector3.up);

        // destroy the egg if it goes off screen
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet")) return;
        if (other.gameObject.CompareTag("Portal")) return;

        Destroy(gameObject);

        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit by bullet");
            other.gameObject.GetComponent<MoveBehaviour>().Die();
            return;
        }

        if (other.gameObject.CompareTag("Hero"))
        {
            other.gameObject.GetComponent<MoveBehaviour>().Die();
            if (SceneManager.GetActiveScene().name != "Directory")
            {
                var iterationManager = FindObjectOfType<IterationManager>();
                if (iterationManager.IsHeroIteration)
                {
                    iterationManager.GameOver();
                }
                else
                {
                    iterationManager.NextIteration();
                }
            }
        }
    }
}