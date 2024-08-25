using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	public static Player Instance { get; private set; }
public static float YuksekSkor;
	public SonsuzYolScript yolGenerator;

	public AudioClip gameOverSesi;
	public AudioClip puanSesi;

	private Rigidbody rb;
	private Animation anim;
	private AudioSource ses;

	private float skor = YuksekSkor;

	public float maksimumHiz = 10f;
	public float yatayHiz = 5f;
	public float ziplamaHizi = 10f;

	public float hizArtmaAraligi = 10f;
	private float maksimumHizArtmaZamani;

	private bool ucurum = false;
	private bool death = false;

	[HideInInspector]
	public float limitMinDeger = -0f;
	[HideInInspector]
	public float limitMaxDeger = 0f;

	private int donusYonu = 0;
	private bool donemecteyiz = false;

	[HideInInspector]
	public YolYonu yon;
	[HideInInspector]
	public Vector3 ileriYon, sagYon;
	public int index;
	private void Awake()
	{
		if( Instance == null )
			Instance = this;
		else if( this != Instance )
		{
			Destroy( this );
			return;
		}

		rb = GetComponent<Rigidbody>();
		anim = GetComponentInChildren<Animation>();
		ses = GetComponent<AudioSource>();
YuksekSkor = PlayerPrefs.GetInt( "YuksekSkor" );

	}

	public void HareketeBasla()
	{

		anim.Play( "KosmaAnimasyonu" );

		maksimumHizArtmaZamani = Time.time + hizArtmaAraligi;
	}

	private void FixedUpdate()
	{
		if( !ucurum && !death )
		{
			Vector3 konum = rb.position;

			Vector3 surat = ileriYon * maksimumHiz * Time.fixedDeltaTime;

			surat += sagYon * InputManager.Instance.YatayInputAl() * yatayHiz * Time.fixedDeltaTime;

			konum += surat;

			if( yon == YolYonu.Ileri )
				konum.x = Mathf.Clamp( konum.x, limitMinDeger, limitMaxDeger );
			else
				konum.z = Mathf.Clamp( konum.z, limitMinDeger, limitMaxDeger );

			rb.position = konum;

			Vector3 v = rb.velocity;
			v.x = 0f;
			v.z = 0f;
			rb.velocity = v;
		}
	}

	private void Update()
	{
		float yukseklik = transform.localPosition.y;

#if UNITY_EDITOR

		if( Input.GetKeyDown( KeyCode.R ) )
			SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
#endif

		if( !death && yukseklik < -5f )
		{
			death = true;
			GameOver();
			Time.timeScale = 0f;
		}

		if( !ucurum && !death )
		{

			skor += Time.deltaTime * maksimumHiz;
			Arayuz.Instance.OyunIcıSkoruGuncelle( (int) skor );

			if( yukseklik < 1.9f )
			{
				ucurum = true;

				GetComponentInChildren<Collider>().enabled = false;
				Arayuz.Instance.StartCoroutine( Arayuz.Instance.SkorYazisiniSaydamYap() );
			}

			if( Time.time >= maksimumHizArtmaZamani )
			{
				maksimumHizArtmaZamani = Time.time + hizArtmaAraligi;
				maksimumHiz++;
			}

			if( donemecteyiz && donusYonu != 0 )
			{
				if( donusYonu == -1 )
					yolGenerator.sol = true;
				else
					yolGenerator.sag = true;

				donemecteyiz = false;
				donusYonu = 0;
			}
		}
	}

	public void Zipla()
	{
		if( !enabled )
			return;

		if( !ucurum && !death && transform.localPosition.y <= 3f && Mathf.Abs( rb.velocity.y ) < 0.5f )
			rb.AddForce( new Vector3( 0f, ziplamaHizi, 0f ), ForceMode.Impulse );
	}

	public void SolaDon()
	{

		if( !ucurum && !death )
		{
			donusYonu = -1;

			CancelInvoke();
			Invoke( "DonusYonunuResetle", 1f );
		}
	}

	public void SagaDon()
	{
		if( !ucurum && !death )
		{
			donusYonu = 1;

			CancelInvoke();
			Invoke( "DonusYonunuResetle", 1f );
		}
	}

	public Vector3 KonumuResetle()
	{
		Vector3 kaydirmaMiktari = -transform.localPosition;
		kaydirmaMiktari.y = 0f;
		transform.localPosition += kaydirmaMiktari;

		if( yon == YolYonu.Ileri )
		{
			limitMinDeger += kaydirmaMiktari.x;
			limitMaxDeger += kaydirmaMiktari.x;
		}
		else
		{
			limitMinDeger += kaydirmaMiktari.z;
			limitMaxDeger += kaydirmaMiktari.z;
		}

		return kaydirmaMiktari;
	}

	private void DonusYonunuResetle()
	{
		donusYonu = 0;
	}

	private void GameOver()
	{
		death = true;


		// if( (int) skor > PlayerPrefs.GetInt( "YuksekSkor" ) )
		// {
// PlayerPrefs.GetInt( "YuksekSkor" );
			PlayerPrefs.SetInt( "YuksekSkor", (int) skor );
			PlayerPrefs.Save();
skor = YuksekSkor + skor;



		// }

		Arayuz.Instance.GameOverMenusunuGoster( (int) skor );

		ses.PlayOneShot( gameOverSesi );
	}

	private void OnTriggerEnter( Collider c )
	{
		if( c.CompareTag( "Donemec" ) )
		{
			donemecteyiz = true;
		}
		else if( c.CompareTag( "PuanObjesi" ) )
		{

			skor += 25;
			Arayuz.Instance.OyunIcıSkoruGuncelle( (int) skor );

			c.gameObject.SetActive( false );

			ses.PlayOneShot( puanSesi );
		}
	}

	// private void OnTriggerExit( Collider c )
	// {
	//	if( c.CompareTag( "Donemec" ) )
	//		donemecteyiz = false;
	// }

	private void OnCollisionEnter( Collision c )
	{
		if( death )
			return;

		if( c.collider.CompareTag( "OlumculEngel" ) )
		{
			rb.isKinematic = true;
			anim.Play( "OlmeAnimasyonu" );
			GameOver();
		}
	 else if( c.collider.CompareTag( "bitis" ) )
		{
			PlayerPrefs.SetInt( "YuksekSkor", (int) skor );
			PlayerPrefs.Save();
skor = YuksekSkor + skor;
			Application.LoadLevel(index);
		}
	
		
	}
}