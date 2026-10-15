using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EndActivator : MonoBehaviour
{
    [SerializeField] private Rigidbody2D endRb;

    private Coroutine coroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int numDeaths = 0;

        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<PlatformerController>(out PlatformerController controller))
            {
                numDeaths = controller.getNumDeaths();
            }

            switch (numDeaths)
            {
                case 0:
                    endRb.linearVelocityY = 30f;
                    break;
                case 1:
                    endRb.linearVelocityX = -30f;
                    break;
                case 2:
                    endRb.linearVelocityY = -30f;
                    break;
            }

            if (coroutine == null)
            {
                coroutine = StartCoroutine(ResetRoutine());
            }
        }
    }

    public void StopReset()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private IEnumerator ResetRoutine()
    {
        yield return new WaitForSeconds(5f);

        if (endRb.gameObject.TryGetComponent<EndController>(out  EndController controller))
        {
            controller.Reset();
        }

        coroutine = null;
    }
}
 