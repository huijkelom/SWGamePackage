using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralGateway : MonoBehaviour
{
    [HideInInspector]
    public CentralGateway Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("CentralGateway | Awake | A second CentralGateway was loaded, maybe you should make a initialisation scene?");
            Destroy(this);
        }
    }
}
