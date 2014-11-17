using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour
{
    //public AudioClip shoutingClip;      // Audio clip of the player shouting.
    public float turnSmoothing = 15f;   // A smoothing value for turning the player.
    public float speedDampTime = 0.1f;  // The damping for the speed parameter
    public Transform cursor;// ngui do huda

    public float speedVector = 10.0f;

    private Animator anim;              // Reference to the animator component.
    //private HashIDs hash;               // Reference to the HashIDs.
    
    
    void Awake ()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        //hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        
        // Set the weight of the second layer to 1.
        //anim.SetLayerWeight(1, 1f);
    }
    
    
    void FixedUpdate ()
    {
        // Cache the inputs.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //bool running = Input.GetButton("running");
        
        MovementManagement(h, v);
    }
    
    
    void Update ()
    {
        Transform cursorWithM4Diff = cursor;
        cursorWithM4Diff.rotation = new Quaternion(cursorWithM4Diff.rotation.x, cursorWithM4Diff.rotation.y, cursorWithM4Diff.rotation.z, cursorWithM4Diff.rotation.w);
        transform.LookAt(cursorWithM4Diff);
    }
    
    
    void MovementManagement (float horizontal, float vertical)
    {
        
        if(horizontal != 0f || vertical != 0f)
        {

			Rotating(horizontal, vertical);
            transform.position += new Vector3(horizontal * speedVector * Time.deltaTime, 0f, vertical * speedVector * Time.deltaTime);
            //Vector3 pretendedMove = Vector3.forward *Time.deltaTime * speedVector * vertical;
            //pretendedMove = new Vector3(horizontal * speedVector * Time.deltaTime, 0.0f, pretendedMove.z);
            //transform.position += pretendedMove;
                //Vector3.up * horizontal * speedVector * Time.deltaTime, 
                //0f, 
                //vertical * speedVector * Time.deltaTime
            //);
            // transform.position += transform.forward * speedVector * Time.deltaTime * vertical;
			anim.SetFloat(Animator.StringToHash("Manoeuvre"), horizontal, speedDampTime, Time.deltaTime);
            anim.SetFloat(Animator.StringToHash("Speed"), 5.5f * vertical, speedDampTime, Time.deltaTime);
        }
        else
        {
            anim.SetFloat(Animator.StringToHash("Speed"), 0);
        }
            
    }
    
    
    void Rotating (float horizontal, float vertical)
    {
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
        
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        
        Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
        
        rigidbody.MoveRotation(newRotation);
    }   
    

}