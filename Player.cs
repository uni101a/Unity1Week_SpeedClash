using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playert : MonoBehaviour
{
    [SerializeField] ClashZone _clashZone = default;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnInput();
        }
    }

    private void OnInput()
    {
        if (_clashZone.IsOver)
        {
            Clash();
        }
    }

    private void Clash()
    {

    }
}
