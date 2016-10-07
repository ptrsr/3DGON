using UnityEngine;
using System.Collections;
using System.Linq;

public class CameraScript : MonoBehaviour {

        [SerializeField]
    private float speed = 1;
        [SerializeField]
    private float border = 10;
        [SerializeField]
    private Main main;

    private MusicAnalyser analyser;
    private Vector3 velocity;

    private delegate void CameraDelegate();
    private CameraDelegate cameraDelegate;

    private float oldRotSpeed = 0;
    private float currentRotSpeed = 0;
    private bool skippedTriangle = false;

    void Start()
    {
        analyser = main.musicAnalyser;
    }

	void Update() 
    {
        if (cameraDelegate != null)
            cameraDelegate();
	}

    void Movement()
    {
        velocity += new Vector3(-Input.GetAxisRaw("Mouse Y"), 0 ,Input.GetAxisRaw("Mouse X")) * speed;
        velocity.Scale
            (
            new Vector3 
                (
                ApplyBorder(this.transform.eulerAngles.x,velocity.x),
                0,
                ApplyBorder(this.transform.eulerAngles.z,velocity.z)
                )
            );

        this.transform.eulerAngles += velocity;
    }

    private void Rotate()
    {
        this.transform.eulerAngles += new Vector3(0, currentRotSpeed + analyser.RmsValue * 10, 0);
    }

    public void StartRotating()
    {
        cameraDelegate -= Rotate;
        cameraDelegate += Rotate;
    }

    private float ApplyBorder(float currentAngle, float velocityAngle)
    {
        float newAngle = Mathf.Abs((currentAngle + 180) % 360 - 180 + velocityAngle);

        if (newAngle > border)
            return Mathf.Clamp(1 - (newAngle - border) / 20, 0, 0.9f);
        else
            return 0.9f;
    }

    public void ToggleMovement(bool on)
    {
        cameraDelegate -= Movement;
        
        if (on)
            cameraDelegate += Movement;
    }

    public void SetDefaultRotSpeed(float speed)
    {
        currentRotSpeed = speed;
    }

    public bool StopRotating()
    {
        float degreesLeft = transform.eulerAngles.y % 60;

        if (Mathf.Sign(degreesLeft) == 1)
            degreesLeft = 60 - degreesLeft;

        if (oldRotSpeed == 0)
        {
            oldRotSpeed = currentRotSpeed;
            if (degreesLeft < 15)
                skippedTriangle = true;
        }

        if (skippedTriangle == true)
        {
            if (degreesLeft > 15)
                skippedTriangle = false;
            else
                return true;
        }

        currentRotSpeed = Mathf.Lerp(0, oldRotSpeed, degreesLeft / 10);

        if (degreesLeft > 0.3f)
            return true;

        currentRotSpeed = degreesLeft;
        Rotate();

        cameraDelegate -= Rotate;

        skippedTriangle = false;
        oldRotSpeed = 0;

        return false;
    }

    public int ParseRotation()
    {
        return (int)(transform.eulerAngles.y / 60);
    }
}
