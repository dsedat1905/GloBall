using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static InputManager Instance { get; private set; }
	public float mobilSensorSensitivity = 7.5f;
	public float mobilTouchSensitivity = 30f;

#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBGL

	private int parmakId = -1;
	private Vector2 parmakIlkPosition;
	private float sensorOutput = 0f;
#endif

	private void Awake()
	{
		if( Instance == null )
			Instance = this;
		else if( this != Instance )
		{
			Destroy( this );
			return;
		}

#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBGL

		mobilTouchSensitivity *= mobilTouchSensitivity;
#endif
	}

	private void Update()
	{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL

		if( Input.GetKeyDown( KeyCode.Space ) || Input.GetMouseButtonDown( 0 ) )
			Player.Instance.Zipla();

		if( Input.GetKeyDown( KeyCode.A ) )
			Player.Instance.SolaDon();

		if( Input.GetKeyDown( KeyCode.D ) )
			Player.Instance.SagaDon();
#else
		
		float hedefSensorDegeri = ( Input.acceleration.x ) * mobilSensorSensitivity;
		sensorOutput = Mathf.Lerp( sensorOutput, hedefSensorDegeri, 0.25f );
		
		for( int i = 0; i < Input.touchCount; i++ )
		{
			Touch parmak = Input.GetTouch( i );

			if( parmakId == -1 && parmak.phase == TouchPhase.Began )
			{
				parmakId = parmak.fingerId;
				parmakIlkPosition = parmak.position;
			}
			else if( parmak.fingerId == parmakId )
			{
			
				if( parmak.phase != TouchPhase.Ended && parmak.phase != TouchPhase.Canceled )
				{
					Vector2 deltaPosition = parmak.position - parmakIlkPosition;
					if( deltaPosition.sqrMagnitude >= mobilTouchSensitivity )
					{
						float x = deltaPosition.x;
						float y = deltaPosition.y;
						
						if( y > Mathf.Abs( x ) )
						{
							Player.Instance.Zipla();
						}
						else if( x > 0f )
						{

							Player.Instance.SagaDon();
						}
						else
						{

							Player.Instance.SolaDon();
						}

						// sisteme bildir
						parmakId = -1;
					}
				}
				else
				{
					parmakId = -1;
				}
			}
		}
#endif
	}

	public float YatayInputAl()
	{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
		return Input.GetAxis( "Mouse X" );
#else

		return Mathf.Clamp( sensorOutput, -1f, 1f );
#endif
	}
}