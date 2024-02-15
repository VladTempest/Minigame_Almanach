using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyBounce : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _initialPosition;
    private Vector3 _initialRotation;
    private Vector3 _initialScale;
    
    private Vector3 _errorMargin = new Vector3(0.1f, 0.1f, 0.1f);
    
    [SerializeField]
    private BoxCollider _groundCollider;
    
    private bool _isBounceActive = false;
    private void Start()
    {
        InitialSetUp();
    }

    private void Update()
    {
        ManageInputs();
        Bounce();
    }

    private void InitialSetUp()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _initialPosition = transform.position;
        _initialRotation = transform.eulerAngles;
        _initialScale = transform.localScale;
    }

    private void ManageInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartBouncing();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetSphere();
        }
    }

    private void Bounce()
    {
        if(IsCollidedWithGround() && _isBounceActive)
        {
            _rigidbody.velocity = new Vector3(0, Random.Range(5, 10), 0);
        }
    }

    private bool IsCollidedWithGround()
    {
        return _groundCollider.bounds.Intersects(GetComponent<Collider>().bounds);
    }

    private void StartBouncing()
    {
        _isBounceActive = true;
    }

    private void ResetSphere()
    {
        _isBounceActive = false;
        transform.position = _initialPosition;
        transform.eulerAngles = _initialRotation;
        transform.localScale = _initialScale;
        _rigidbody.velocity = Vector3.zero;
    }
}
