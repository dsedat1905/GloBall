using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Arayuz : MonoBehaviour
{
	public static Arayuz Instance { get; private set; }
	public KameraKontrol kameraKontrol;
	public Animation kameraAnimation;
	public GameObject anaMenu;
	public GameObject oyunIciUI;
	public GameObject gameOverMenu;

	public Text creditsText;
	public Text oyunIciSkorText;
	public Text gameOverSkorText;

	public CanvasGroup oyunIciCanvasGroup;
	public AudioSource sesCalar;
	public AudioClip butonClickSesi;

	public GameObject[] baslangicZeminler;

	private void Awake()
	{
		if( Instance == null )
			Instance = this;
		else if( this != Instance )
		{
			Destroy( this );
			return;
		}

		Time.timeScale = 1f;

		anaMenu.SetActive( true );
	}

	private void Start()
	{
		kameraKontrol.enabled = false;
		Player.Instance.enabled = false;
	}

	public void OyunuBaslat()
	{
		kameraKontrol.enabled = true;
		Player.Instance.enabled = true;
		Player.Instance.HareketeBasla();

		Destroy( kameraAnimation );

		for( int i = 0; i < baslangicZeminler.Length; i++ )
			Destroy( baslangicZeminler[i], 4f );

		sesCalar.PlayOneShot( butonClickSesi );

		anaMenu.SetActive( false );

		oyunIciUI.SetActive( true );
	}

	public void Restart()
	{
		SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
	}

	public void Credits()
	{
		creditsText.text = "www.sedatdemir.net";

		CancelInvoke();
		Invoke( "CreditsDuzelt", 1f );

		sesCalar.PlayOneShot( butonClickSesi );
	}

	public void Cikis()
	{
		sesCalar.PlayOneShot( butonClickSesi );

		Application.Quit();
	}

	public void GameOverMenusunuGoster( int skor )
	{
		gameOverMenu.SetActive( true );
		gameOverSkorText.text = "Score : " + skor + "\nHigh Score : " + PlayerPrefs.GetInt( "YuksekSkor" );
	}

	public void OyunIcıSkoruGuncelle( int skor )
	{
		oyunIciSkorText.text = "Score: <color=yellow>" + skor + "</color>";
	}

	private void CreditsDuzelt()
	{
		creditsText.text = "Producer";
	}

	public IEnumerator SkorYazisiniSaydamYap()
	{
		float saydamlik = 1f;
		while( saydamlik > 0f )
		{
			saydamlik -= Time.unscaledDeltaTime * 4f;
			oyunIciCanvasGroup.alpha = saydamlik;

			yield return null;
		}

		oyunIciCanvasGroup.alpha = 0f;
	}
}