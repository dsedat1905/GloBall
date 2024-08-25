using UnityEngine;

public class KameraKontrol : MonoBehaviour
{
	public static KameraKontrol Instance { get; private set; }

	public Transform hedef;
	public float uzaklik = 10f;
	public float yukseklik = 8f;

	private void Awake()
	{
		if( Instance == null )
			Instance = this;
		else if( this != Instance )
		{
			Destroy( this );
			return;
		}
	}

	private void FixedUpdate()
	{
		Vector3 hedefKonum = hedef.position - hedef.forward * uzaklik + new Vector3( 0f, yukseklik, 0f );

		if( hedefKonum.y < yukseklik )
			hedefKonum.y = yukseklik;
		transform.localPosition = Vector3.Slerp( transform.localPosition, hedefKonum, 0.1f );

		Quaternion rot = Quaternion.LookRotation( hedef.position - transform.localPosition );
		transform.localRotation = Quaternion.Slerp( transform.localRotation, rot, 0.1f );
	}
}