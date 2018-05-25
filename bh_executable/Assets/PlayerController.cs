using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _myRigidbody;
    public float speed;
    private Animator _anim;
    private bool _facingRight = true;
    //ground check
    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGrounded;

    private int jumpCount;
    public float groundTime = 0.0f;
    float timer;

    //double jump from fallstate
    
    


    // Use this for initialization
    void Start ()
    {
        _myRigidbody = this.GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }
    private void OnGUI()
    {
        string air = jumpCount.ToString();
        string ground = isGrounded.ToString();
        string sped = speed.ToString();
        
        GUI.Label(new Rect(10, 10, 200, 90),"AIR ACTIONS: "+air);
        GUI.Label(new Rect(10, 25, 100, 45), "IS GROUNDED: " + ground);
        GUI.Label(new Rect(190, 10, 100, 45), "SPEED: " + sped);
    }
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
        //horizontal movement
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 15;
            
        }
        else
        {
            speed = 5;
        }
        
        
        float l_r = Input.GetAxisRaw("Horizontal");
        _anim.SetFloat("Speed", Mathf.Abs(l_r));
        _myRigidbody.velocity = new Vector2(l_r * speed, _myRigidbody.velocity.y);
        

        //sprite flip logic
        if (_facingRight  ==true && l_r < 0){
            _facingRight = false;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
        else if (_facingRight == false && l_r >0) {
            _facingRight = true;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }


        //jump input
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        // isGrounded will update slightly behind the jump count, so that grounded jumps still add to
        // the jump count variable( happened at the same time before
        if (isGrounded == true)
        {
            timer += (Time.deltaTime)*3;
            if (timer > groundTime)
            {
                timer = 0f;
                jumpCount = 2;
            }

        }

        if (Input.GetKeyDown(KeyCode.W) && jumpCount>0)
        {
            isGrounded = false;
            _anim.SetBool("Jump", true);
            //15 = current jump height
            _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x, 17);
            jumpCount--;

        }
        

    }
}
