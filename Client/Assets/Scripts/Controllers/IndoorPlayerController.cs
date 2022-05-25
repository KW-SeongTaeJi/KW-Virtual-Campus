using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorPlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("Move speed of the character in m/s")]
    protected float moveSpeed = 5.0f;

    protected bool _isWalk;
    protected Animator _animator;

    protected GameObject _leftEnd;
    protected GameObject _rightEnd;

    protected IndoorPlayerCanvasController _canvas;

    // Using for movement sycn
    public float MoveX { get; set; }
    public bool Sync { get; set; } = false;
    public float PosX { get; set; }


    void Start()
    {
        Init();
    }

    void Update()
    {
        OnUpdate();
    }

    void LateUpdate()
    {
        OnLateUpdate();
    }

    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();

        _leftEnd = GameObject.Find("LeftEnd");
        _rightEnd = GameObject.Find("RightEnd");

        _canvas = gameObject.FindChild<IndoorPlayerCanvasController>(recursive: true);
    }

    protected virtual void OnUpdate()
    {
        Move();
    }

    protected virtual void OnLateUpdate()
    {
        SyncPosition();
    }

    protected virtual void Move()
    {
        if (MoveX == 0)
        {
            _isWalk = false;
        }
        else
        {
            transform.localScale = new Vector3(MoveX * 0.8f, 0.8f, 0.8f);
            _canvas.transform.localScale = new Vector3(MoveX * 0.5f, 0.5f, 0.5f);
            _isWalk = true;
        }

        float newPosX = Mathf.Clamp(transform.position.x + MoveX * moveSpeed * Time.deltaTime, _leftEnd.transform.position.x, _rightEnd.transform.position.x);

        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
       
        _animator.SetBool("isWalk", _isWalk);
    }

    void SyncPosition()
    {
        if (Sync)
        {
            Vector3 startPos = transform.position;
            Vector3 destPos = new Vector3(PosX, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(startPos, destPos, Time.deltaTime * 5); 
            Sync = false;
        }
    }
}
