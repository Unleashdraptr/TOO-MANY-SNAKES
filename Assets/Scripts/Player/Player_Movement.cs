using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    //General Player settings
    public float Hp;
    public float Speed;
    public float lookSpeed;
    public float JumpSpeed;

    //Getting the camera and where the character's physics body and feet gameobject. Also what layer the feet will interact with.
    public LayerMask FloorMask;
    public Transform Feet;
    public Transform PlayerCamera;
    public Rigidbody rb;
    public Vector3 StatusSlow = new(1,1,1);
    public Vector3 ShieldSlow = new(1, 1, 1);
    //Players input
    Vector3 PlayerMoveInput;
    Vector2 CameraMoveInput;

    //Players stamina (Removing infinite dashing)
    public float Stamina = 100;

    //Bools to keep track what state they are currently in
    float RotX;
    public enum MoveState { IDLE, WALK, DASH, SNEAK, DEATH };
    public MoveState moveState;
    private Player_Stats Stats;

    private void Start()
    {
        //Set the state to idle
        moveState = MoveState.IDLE;
        //Hide the cursor and get the rigidbody on the player when first starting
        rb = transform.GetComponent<Rigidbody>();
        Stats = transform.GetChild(0).GetComponent<Player_Stats>();
        Speed = Stats.Speed;
    }
    //Update once a frame
    void Update()
    {
        if (!GameManager.Pause && !Stats.Death)
        {
            //Gets the movement input and then runs its function
            PlayerMoveInput = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Checks if the player is moving and if not to go to Idle
            if (PlayerMoveInput.Equals(default))
            {
                moveState = MoveState.IDLE;
            }
            //Gets the camera's input and run its function
            CameraMoveInput = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            MovePlayerCamera();
            //Stamina Ending and Recharging
            if (Stamina <= 0)
            {
                //Checks what the player is currently doing to get the correct state
                if (PlayerMoveInput.Equals(default))
                {
                    moveState = MoveState.IDLE;
                }
                else
                    moveState = MoveState.WALK;
            }
            //Recharging if not running or is not currently full
            if (Stamina < 100 && moveState != MoveState.DASH)
            {
                Stamina += 15 * Time.deltaTime;
            }
            //Jump Function
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Check if the feet GameObject are on the ground that has a layer called 'Ground'
                if (Physics.CheckSphere(Feet.position, 0.1f, FloorMask))
                {
                    Jump();
                }
            }
            //Dash Function
            if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > 20)
            {
                //checks if the player is walking or not
                if (moveState == MoveState.WALK)
                {
                    moveState = MoveState.DASH;
                }
                else
                    moveState = MoveState.WALK;
            }
            //If they arent dashing, then check if they're sneaking
            else if (Input.GetKey(KeyCode.LeftControl) == true)
            {
                moveState = MoveState.SNEAK;
            }
            //If not then reset the sneak bools
            else
            {
                if (PlayerMoveInput.Equals(default))
                {
                    moveState = MoveState.IDLE;
                }
                else if (moveState != MoveState.DASH)
                {
                    moveState = MoveState.WALK;
                }
            }
        }
    }
    //FixedUpdate can run multiple times per frame
    private void FixedUpdate()
    {
        if (!GameManager.Pause && !Stats.Death)
        {
            //Run the Physics calculations
            MovePlayer();
            if (!Stats.CheckForStatus() && StatusSlow.x < 1)
            {
                StatusSlow *= 1.025f;
                if (StatusSlow.x > 1)
                {
                    StatusSlow = new Vector3(1, 1, 1);
                }
            }
        }
    }
    void Jump()
    {
        rb.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
    }
    void MovePlayer()
    {
        //Total Move Direction for the frame
        Vector3 MoveDir = transform.TransformDirection(PlayerMoveInput) * Speed;
        //Setting the Y velocity aswell
        MoveDir.y = rb.velocity.y;
        MoveDir.Scale(StatusSlow);
        MoveDir.Scale(ShieldSlow);
        //If its true then increase speed and lower stamina
        if (moveState == MoveState.DASH)
        {
            MoveDir.x *= 1.5f;
            MoveDir.z *= 1.5f;
            Stamina -= 10 * Time.deltaTime;
        }
        if (moveState == MoveState.SNEAK)
        {
            MoveDir.x /= 4;
            MoveDir.z /= 4;
        }
        //Calculates and then applies then input with rigidbody's Physics
        rb.MovePosition(transform.position + MoveDir * Time.deltaTime);
    }
    void MovePlayerCamera()
    {
        //Gets the Y axis of the camera input
        RotX -= CameraMoveInput.y * lookSpeed;
        //Applies it to the camera and then to the player
        transform.Rotate(0, CameraMoveInput.x * lookSpeed, 0);
        PlayerCamera.transform.localRotation = Quaternion.Euler(RotX, CameraMoveInput.x, 0);
    }
}
