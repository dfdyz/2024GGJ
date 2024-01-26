using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Resources")]

    [Header("Parameters")]
    [SerializeField] float moveSmooth = 0.25f;
    [Header("Display")]
    public int nextSkillBeat;
    public int faceDir = 1; // right
    public int currPhase = 0;

    public SkillActionSheetSO.SkillClip currentSkill;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isStarted) return;
        MovementVisual();
    }

    private void FixedUpdate()
    {

    }

    void onBeat()
    {

    }

    void onHeavyBeat()
    {
        if (GameManager.Instance.currentBeat == nextSkillBeat)
        {
            NextSkill();
        }
        else
        {
            
        }
    }


    public void NextSkill()
    {
        int skill = Random.Range(0, GameManager.Instance.skillActionSheets.mobSkillRegistries.Length);
        currentSkill = GameManager.Instance.skillActionSheets.mobSkillRegistries[skill];

        faceDir = Random.Range(0, 2) == 0 ? 1 : -1;
        currPhase = 0;
    }

    void MovementVisual()
    {
        Vector2 targetPos = GridManager.Instance.GetEnemyVisualPos();
        gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, targetPos, moveSmooth);
    }

}
