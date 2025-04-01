using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAreaManager : MonoBehaviour
{
    [SerializeField] private List<PlayerArea> Areas;

    private void Start()
    {
        //Check how many players there are
        int playerCount = int.Parse(GlobalGameSettings.GetSetting("Players"));

        //Add areas based on what value you loaded
        for (int i = Areas.Count - 1; i >= playerCount; i--)
        {
            Areas[i].gameObject.SetActive(false);
        }
    }
}