using UnityEngine;
using System.Collections;

// Infinite road yapmayı görev edinmiş kahraman script!
// Oyunun en başında ufak bir yol rastgele bir şekilde oluşturulur 
// ve karakter kavşaklardan sağa/sola saptıkça yolun devamı bu
// script vasıtasıyla oluşturulmaya devam edilir. Böylece infinite road
// atmosferi oluşturulur oysa Scene panelinden bakılacak olursa oyun
// ilerledikçe eski yolun yok olup da yeni yolun sonradan oluşturulduğu
// kolayca gözlemlenebilir
// Bu script düzgün çalışmak için ObjeHavuzu scriptine (havuz)(pool) ihtiyaç
// duyar (RequireComponent)
[RequireComponent( typeof( ObjeHavuzu ) )]
public class SonsuzYolScript : MonoBehaviour
{
	// Düz bir yolun sahip olacağı minimum zemin objesi sayısı
	public int yolMinimumUzunluk = 10;
	// Düz bir yolun sahip olacağı maksimum zemin objesi sayısı
	public int yolMaksimumUzunluk = 20;
	// Puan objeleri yolda birbiri ardına dizilirler (dizi şeklinde) ve bu
	// değişken bir dizinin sahip olacağı puan objesi sayısını depolar
	public int ardArdaDiziliPuanObjesiSayisi = 6;

	// karakterin üzerinde koşmakta olduğu yol (ileri yöndeki yol)
	private YolContainer ileriYol;
	// ileriYol'un ucundaki kavşaktan sağa sapınca gireceğimiz yol
	private YolContainer sagYol;
	// ileriYol'un ucundaki kavşaktan sola sapınca gireceğimiz yol
	private YolContainer solYol;
	// ileriYol'un ucundaki kavşak objesi
	private YolObjesi kavsakObjesi;

	[System.NonSerialized]
	public bool sol = false;
	[System.NonSerialized]
	public bool sag = false;

	private void Start()
	{
		// Rastgele bir yol oluştur
		ileriYol = new YolContainer();
		sagYol = new YolContainer();
		solYol = new YolContainer();

		YolOlustur( ileriYol, new Vector3( 0f, 0f, 15f ), YolYonu.Ileri );
		
	}

	// Random bir şekilde düz bir yol oluşturmaya yarayan fonksiyon
	private void YolOlustur( YolContainer c, Vector3 baslangicNoktasi, YolYonu yolYonu )
	{
		// yolun uzunluğunu rastgele olarak belirle
		int yolUzunluk = Random.Range( yolMinimumUzunluk, yolMaksimumUzunluk + 1 );

		// yolu oluştur (yola zeminleri dik)
		c.YolOlustur( baslangicNoktasi, yolYonu, yolUzunluk );
		// yola puan objelerini diz
		c.YolaPuanObjeleriDiz( ardArdaDiziliPuanObjesiSayisi );
	}

	
	// Girilen yönde dizilen zemin objelerinin sahip olması gereken eğimi
	// bulmaya yarayan fonksiyon
	public static Vector3 YolEgiminiBul( YolYonu yolYonu )
	{
		if( yolYonu == YolYonu.Ileri )
			return new Vector3( 0f, 0f, 0f );
	
		else
			return new Vector3( 0f, 180f, 0f );
	}
}