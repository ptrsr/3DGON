using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
    private enum CollisionSide 
    {
        None,
        Left,
        Right,
    }

    public GameObject hexagon;
    public GameObject lightPivot;

    public float _rotation = 0;
    public float _speed = 3;
    public float _distance = 3;

    private delegate void PlayerDelegate();
    private PlayerDelegate playerDelegate;

    private bool selecting;
    private bool currentTriangleChanged;

    public int currentTriangle;
    private int previousTriangle;

    private CollisionSide collisionSide;
    private int inCollision;

    void Start()
    {
        Movement();

    }

	void Update () 
    {
        if (playerDelegate != null)
            playerDelegate();
	}

    void OnTriggerStay(Collider other)
    {
        inCollision = 3;

        WallScript wallScript = other.gameObject.GetComponent<WallScript>();

        if (Main.s.CurrentState != State.Stopping && (currentTriangleChanged || wallScript._side != currentTriangle))
        {
            int difference = 0;
            float rotation = 0;

            if (currentTriangleChanged)
                difference = currentTriangle - previousTriangle;
            else
                difference = wallScript._side - currentTriangle;

            if (difference < 0 || difference == 5)
            {
                rotation = -6.2f;
                collisionSide = CollisionSide.Right;
            }
            else
            {
                rotation = 66.2f;
                collisionSide = CollisionSide.Left;
            }

            rotation -= other.gameObject.transform.eulerAngles.y;

            UpdatePosition(rotation);
        }

        else if (Main.s.CurrentState == State.Playing)
            wallScript.Hit();
    }

    private void Movement()
    {
        if (inCollision == 0)
            collisionSide = CollisionSide.None;

        if (Input.GetMouseButton(0) == Input.GetMouseButton(1))
            return;

        float oldRotation = _rotation;

        if (collisionSide != CollisionSide.Right && Input.GetMouseButton(0))
            _rotation += _speed;
            
        else if (collisionSide != CollisionSide.Left && Input.GetMouseButton(1))
            _rotation -= _speed;

        _rotation %= 360;

        checkPosition(_rotation - oldRotation);
        inCollision = Mathf.Clamp(inCollision - 1, 0, 2);
        UpdatePosition(_rotation);
        SelectTriangle();
        
    }

    public void ToggleMovement(bool toggle)
    {
        playerDelegate -= Movement;

        if (toggle)
            playerDelegate += Movement;
    }

    private void checkPosition(float rot) 
    {
        currentTriangle = Mathf.RoundToInt(((transform.rotation.eulerAngles.y - rot) % 360f) / 60f - 1) % 6;

        if (previousTriangle != currentTriangle)
        {
            currentTriangleChanged = true;
            previousTriangle = currentTriangle;
        }
        else
        {
            currentTriangleChanged = false;
        }
    }

    public void SelectTriangle()
    {
        if (selecting == false || Mathf.Round(transform.rotation.eulerAngles.y % 60 - 30) == 0)
            lightPivot.SetActive(false);
        
        else
            lightPivot.SetActive(true);

        lightPivot.transform.eulerAngles = new Vector3(0, 60 * (currentTriangle + 1), 0);
    }

    public void ToggleSelecting(bool toggle)
    {
        selecting = toggle;
        lightPivot.SetActive(toggle);
    }

    private void UpdatePosition(float rotation) 
    {
        _rotation = rotation;
        Vector3 localPosition = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), 0, Mathf.Sin(rotation * Mathf.Deg2Rad)) * _distance;
        transform.position = hexagon.transform.position + localPosition;
        transform.Rotate(new Vector3(0,-rotation + 90,0) - this.transform.rotation.eulerAngles);
    }
}
