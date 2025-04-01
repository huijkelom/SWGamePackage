using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour, I_SmartwallInteractable
{
    private delegate void SWHitEvent(Vector3 position, InputType input);
    [SerializeField] private SWHitEvent OnInput;

    private bool Cooldown = false;

    public void Hit(Vector3 position, InputType input)
    {
        if (!Cooldown)
        {
            OnInput?.Invoke(position, input);

            Cooldown = true;
            Invoke("ResetCooldown", 0.1f);
        }
    }

    private void ResetCooldown()
    {
        Cooldown = false;
    }
}