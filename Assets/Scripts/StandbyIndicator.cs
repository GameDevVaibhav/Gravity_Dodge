using UnityEngine;

public class StandbyIndicator : MonoBehaviour
{
    public void SetStandby(bool isStandby)
    {
        // Activate/deactivate the child object (indicator) based on standby status
        transform.GetChild(0).gameObject.SetActive(isStandby);
    }
}
