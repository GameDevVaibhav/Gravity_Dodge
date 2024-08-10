using UnityEngine;

public class OscillatingObject : MonoBehaviour
{
    private Vector3 initialLocalPosition;
    private Vector3 targetPosition;
    private bool movingOutward = true;
    private bool isPausing = false;
    private float pauseStartTime;
    private float moveSpeed;
    private float pauseDuration;

    public void Initialize(Transform planetCenter, float emergenceDistance, float moveSpeed, float pauseDuration)
    {
        this.moveSpeed = moveSpeed;
        this.pauseDuration = pauseDuration;

        initialLocalPosition = transform.localPosition;
        Vector3 direction = (initialLocalPosition - planetCenter.localPosition).normalized;
        targetPosition = initialLocalPosition + direction * emergenceDistance;

        movingOutward = true;
        isPausing = false;
    }

    void Update()
    {
        if (isPausing)
        {
            if (Time.time - pauseStartTime >= pauseDuration)
            {
                isPausing = false;
                movingOutward = false;
            }
            else
            {
                return;
            }
        }

        if (movingOutward)
        {
            MoveTowardsTarget(targetPosition);
            if (IsCloseToTarget(targetPosition))
            {
                isPausing = true;
                pauseStartTime = Time.time;
            }
        }
        else
        {
            MoveTowardsTarget(initialLocalPosition);
        }
    }

    private void MoveTowardsTarget(Vector3 target)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, moveSpeed * Time.deltaTime);
    }

    public bool IsAtInitialPosition()
    {
        return Vector3.Distance(transform.localPosition, initialLocalPosition) < 0.01f;
    }

    private bool IsCloseToTarget(Vector3 target)
    {
        return Vector3.Distance(transform.localPosition, target) < 0.01f;
    }
}
