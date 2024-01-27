using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] GameObject suspendPanel;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameManager.Instance.suspend)
            {
                Resume();
            }
            else
            {
                Suspend();
            }
        }
    }

    public void Suspend()
    {
        GameManager.Instance.Suspend();
        suspendPanel.SetActive(true);
    }

    public void Resume() {
        GameManager.Instance.Resume();
        suspendPanel.SetActive(false);
    }

}
