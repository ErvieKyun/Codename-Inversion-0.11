using UnityEngine;

public class PLAYERCore : MonoBehaviour
{
    // ================================================================================
    // NAMING CONVENTIONS
    // ================================================================================
    /* 
    /// !!!! VARIABLES !!!!
    /// 
    /// EG: PLYRSpawn
    ///         "PLYR"  = Broadly, this is what system is being used.
    ///             "Spawn" = Specifically, what this is for in the system (subsystem)
    ///         
    ///     IE: CAMSmoothing, CAMOffset, CAMFollow
    ///         "CAM"    = Camera System
    ///             "Smoothing" = Smoothing number.
    ///             "Offset" = Dimensions with Player as reference.
    ///             "Follow" = The camera being used to follow the Player
    ///         
    /// ================================================================================
    ///     - Main System
    ///         - Variables are in upper case
    ///         - Words are in truncated; vowels removed
    ///     - Subsystem Variables are in lower case
    ///         - Full alphabet used
    ///     - Underscores visually declare private variables.
    /// ================================================================================    
    /// 
    /// !!!! SUB ROUTINES !!!
    /// 
    ///     EG: JUMP_All()
    ///         "JUMP"  = This is the name of the Mechanic being called
    ///             "_All"  = This is the name of the condition/state for mechanic to be activated
    ///         
    ///     IE: DASH_Grounded, DASH_Air
    ///         "DASH"      = Dash Mechanic
    ///             "_Grounded" = Specifically executed when Player is grounded.
    ///             "_Air"      = Specifically executed when Player is in the air.
    ///             
    /// ================================================================================
    ///     - Mechanic is called in Upper Case
    ///     - Submechanic/State is addressed in Lower Case
    ///     - Underscores split Mechanic name and state for easy reading
    /// ================================================================================
    /// </summary> */

    [Header ("Positioning") ]
    [Tooltip("Destination Spawn Point using an Empty 3D Object Location.")]
    public Transform PLYRSpawn;

    [Header ("Movement") ]
    [Tooltip("Defines quickly the Character moves.")]
    public float MVMNTMoveSpeed;
    [Tooltip("Defines the strength of Gravity to the Character.")]
    public float MVMNTGravity;

	private bool _MVMNTMobilityOn	= 	true;
    private bool _MVMNTInverseOn	=	false;
    private bool _MVMNTGravityOn	=	true;
    private Vector3 _MVMNTVelocity;

    [Header("Jumping")]
    [Tooltip("Defines the Initial Jump Height.")]
    public float JMPHeight;
    [Tooltip("Defines the Double-Jump Height.")]
    public float DBLJMPHeight;

    private bool _DBLJMPHasJumped;

    [Header("Dashing")] 
    [Tooltip("Defines the Intensity of the Dash.")]
    public float DSHVolume;
    [Tooltip("Defines the duration of the Dash.")]
    public float DSHDuration;
    [Tooltip("Defines the amount of 'lift' a dash generates (WHEN GROUNDED)")]
    public float DSHHeight;
    [Tooltip("Defines the amount of time between keypresses to define a double-tap")]
    public float DSHTapDelay = 0.5f;

    private float	_DSHRealTime = 0;
    private int		_DSHRIGHTTapCount = 0;
    private float	_DSHRIGHTTimer = 0;
    private int		_DSHLEFTTapCount = 0;
    private float	_DSHLEFTTimer = 0;
    private float	_DSHInjector = 0;
    private bool	_DSHHasDashed = false;

    [Header("Camera")]
    [Tooltip("Defines how Quickly the Camera follows the Character. [5 = Default]")]
    public float CAMSmoothing;
    [Tooltip("Defines the distance between Character and Camera.")]
    public Vector3 CAMOffset;
    [Tooltip("Defines the CINEMACHINE CAMERA that is used to follow the Character.")]
    public Transform CAMFollow;

    private CharacterController _BDYController;

    private void Start()
    {
        /*
        if (PLYRSpawn != null)                                  // Check if there is a dedicated Player Spawn location
        {
            transform.position = PLYRSpawn.position;            // If found, place Player Character there.
        }
        */
        _MVMNTVelocity = Vector3.zero;                          // Hard setting value for Character's movement; makes sure character is still on spawn.
        _BDYController = GetComponent<CharacterController>();   // Grabs the Character Controller for the Player.

        
	}
    private void Update()
    {
        if (_BDYController != null)     //  Check for the Character Controller; if the Character Controller is found, run the following subroutines
        {
            MOVEMENT_Controller();      //  General Player Movement
            GRAVITY_Controller();       //  Calls the Gravity routine
            CAMERA_Controller();        //  Calls the Camera Module
            DASH_Controller();          //  Calls the Dash routine
            JUMP_Controller();			//	Calls the Jump routine
        }
    }

    public void CAMERA_Controller()
    {
        if (CAMFollow != null)          // If the Player Camera has been detected; set camera to perform the follow functions
        {
            CAMFollow.transform.position = 
                Vector3.Lerp(CAMFollow.transform.position, transform.position + CAMOffset, CAMSmoothing * Time.deltaTime);
        }
    }
    public void MOVEMENT_Controller()
    {
        if (_MVMNTMobilityOn == true)                                           // Check if Player is permitted to control the character.
        {
            _MVMNTVelocity.x = Input.GetAxis("Horizontal") * MVMNTMoveSpeed;    // Formula for Character Movement
            _BDYController.Move(_MVMNTVelocity * Time.deltaTime);               // Formula and execution of gradual character movement.
        }

    }
    public void GRAVITY_Controller()
    {
        // Normal Gravity function
        if ((_MVMNTGravityOn == true) && (_MVMNTInverseOn != true))	    //	If Gravity is switched on and Gravity is NOT inverse. 
        {
            if (_BDYController.isGrounded)                              //  Check to see if the Character is on the ground.
            {
                _MVMNTVelocity.y = -MVMNTGravity / 5;					//	This was to prevent instant drops when the Player pushed the character over a ledge.
            }
            else if(!_BDYController.isGrounded)                         //  Check to see if the Character is NOT grounded.
            {
                _MVMNTVelocity.y -= MVMNTGravity * Time.deltaTime;	    //	Regular Gravity operator.
            }
        }

        // Inverse Gravity function
        if ((_MVMNTGravityOn == true) && (_MVMNTInverseOn == true))	    //	If Gravity is switched on AND Gravity is inverse.
        {
            if (_BDYController.isGrounded)                              //  Check if the Player is grounded.
            {
                _MVMNTVelocity.y = MVMNTGravity / 5;					//	This was to prevent instant drops when the Player pushed the character over a ledge.
            }
            else if(!_BDYController.isGrounded)                         //  Check to see if the Character is NOT grounded.
            {
                _MVMNTVelocity.y += MVMNTGravity * Time.deltaTime;	    //	Inverse Gravity operator.
            }
        }
		
		//	Anti-Gravity Function
		if (_MVMNTGravityOn == false)
		{
			// left empty to prevent adding any forces moving the player around.
            // while not necessary, this is a placeholder for the Anti-Gravity state.
		}


    }
    public void JUMP_Controller()
    {
        if (Input.GetKeyDown("space"))                                      //  When the Player hits the Spacebar
        {
			if (!_BDYController.isGrounded)                                 //  Check to see if the Player is NOT on the ground.
			{
				if (_DBLJMPHasJumped != true)                               //  Check to see if the Player has NOT engaged the Double Jump.
				{
					_MVMNTVelocity.y = DBLJMPHeight;     //  Injects an upward force to the Character, equal to the Double Jump Height.
					_DBLJMPHasJumped = true;                                //  Sets state of Double Jump to true.
				}
			}
			if (_BDYController.isGrounded)                                  //  Check to see if the player is on the ground.
			{
				_MVMNTVelocity.y = JMPHeight;                               //  Injects an upward force to the Character, equal to the Jump Height.
				_DBLJMPHasJumped = false;                                   //  Resets the State of the Double Jump to False as the Player has landed and is on the floor.
			}
		}
	}
    public void DASH_Controller()
    {
        if ((_BDYController.isGrounded) && (_DSHHasDashed == true))                                     //  Check to see if the Character is grounded AND has dashed.
        {
            _DSHHasDashed = false;                                                                      //  As Character is on the ground, the Dash State is reset.
        }
        if (Input.GetKeyDown(KeyCode.D))							                                    //	When the Player taps the D Key
        {															
            _DSHRIGHTTapCount += 1;                                                                     //	Add to Right Tap Count.
        }
        if ((_DSHRIGHTTapCount == 1) && (_DSHRIGHTTimer < DSHTapDelay))		                            //	When the Player hits the D key once, and the Dash Timer is less than the Tap Delay
        {															
            _DSHRIGHTTimer += Time.deltaTime;                                                           //	Starts timer to detect double-tap
        }
        if ((_DSHRIGHTTapCount == 1) && (_DSHRIGHTTimer >= DSHTapDelay))		                        //	When the Player only hits the D key once, and the Dash Timer is equal to/more than the Tap Delay
        {															
            _DSHRIGHTTimer = 0;                                                                         //	Resets Right Tap Timer to 0
            _DSHRIGHTTapCount = 0;                                                                      //  Resets Right Tap Count to 0
        }
        if ((_DSHRIGHTTapCount == 2) && (_DSHRIGHTTimer < DSHTapDelay) && (_DSHHasDashed != true))		//	When the Player hits the D key twice, and the Dash Timer is less than the Tap Delay.
        {															                                    
            _DSHInjector = DSHVolume;                                                                   //	Executes Dash Right.
            if (_BDYController.isGrounded)                                                              //  Check if the Character is on the Ground.
            {
                _MVMNTVelocity.y = DSHHeight;                                                           //  While Dashing, Character lifts up slightly, due to acceleration
            }
            else if (!_BDYController.isGrounded)                                                        //  Check if player is NOT on the ground.
            {
                _MVMNTVelocity.y = 0;                                                                   //  Hard sets the vertical velocity to 0 for a straight dash.
            }
        }
		if (Input.GetKeyDown(KeyCode.A))							                                    //	When the player taps the A key
		{															
		    _DSHLEFTTapCount += 1;                                                                      //	Add to Left Tap Counter.
        }
		if ((_DSHLEFTTapCount == 1) && (_DSHLEFTTimer < DSHTapDelay))			                        //	When the Player hits the A key once, and the Dash Timer is less than the Dash Tap Delay.
        {															
            _DSHLEFTTimer += Time.deltaTime;                                                            //	Starts timer to detect double-tap
        }
        if ((_DSHLEFTTapCount == 1) && (_DSHLEFTTimer >= DSHTapDelay))		                            //	When the Player onlt hits the A key once, and the Dash Timer is equal to or above the Tap Delay
        {															    
            _DSHLEFTTimer = 0;                                                                          //	Resets the Left Tap Timer to 0
            _DSHLEFTTapCount = 0;                                                                       //  Resets the Left Tap Counter to 0
        }
        if ((_DSHLEFTTapCount == 2) && (_DSHLEFTTimer < DSHTapDelay) && (_DSHHasDashed != true))		//	When the Player hits the A key twice, and the Dash Timer is below the Tap Delay.
        {                                                           
            _DSHInjector = -DSHVolume;                                                                  //	Executes Dash Left.
            if (_BDYController.isGrounded)                                                              //  Check if Character is on the ground
            {
                _MVMNTVelocity.y = DSHHeight;                                                           //  While Dashing, Character lifts up slightly, due to acceleration
            }
            else if (!_BDYController.isGrounded)                                                        //  Check if the Character is NOT on the ground
            {
                _MVMNTVelocity.y = 0;                                                                   //  Hard sets the vertical velocity to 0 for a straight dash.
            }
        }
        if (_DSHInjector != 0)										                                    //	When the Dash Injector is triggered.
        {															
            _DSHRealTime += Time.deltaTime;                                                             //	Start Dash Duration timer.
            _DSHLEFTTapCount = 0;                                                                       //  Reset the Left Tap Counter
            _DSHRIGHTTapCount = 0;                                                                      //  Reset the Right Tap counter
            _MVMNTMobilityOn = false;                                                                   //  Revoke control of Character from player during dash
            _MVMNTGravityOn = false;                                                                    //  Switch off Gravity function to Character during dash
        }
        if (_DSHRealTime > DSHDuration)								                                    //	When the Dash Duration Timer exceeds the Max Dash time.
        {															
            _DSHInjector = 0;                                                                           //	Resets Dash Injector Value to 0.
            _DSHRealTime = 0;                                                                           //	Resets Dash Timer to 0.
            _MVMNTVelocity = Vector3.zero;                                                              //	Resets velocity on all axes.
            _MVMNTMobilityOn = true;                                                                    //	Return Control of Character to Player.
            _MVMNTGravityOn = true;                                                                     //	Return Gravity of Character.
            _DSHHasDashed = true;                                                                       //  Switch state of Dash to True to prevent future dashing until landed.
        }
        if (_MVMNTMobilityOn == false)                                                                  //  When Player initiates Dash and Mobility is turned off.
        {
            _MVMNTVelocity.x = _DSHInjector;                                                            //  Injects Dash value to the X axis.
            _BDYController.Move(_MVMNTVelocity / 100);                                                  //  Executes the movement based on the Dash Injection value.
        }
    }
}