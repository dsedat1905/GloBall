using UnityEngine;
using System.Collections.Generic;

public enum YolYonu { Ileri = 0};

public class YolContainer
{
	private List<YolObjesi> yol;

	private List<Transform> puanObjeleri;

	private Vector3 bitisNoktasi;

	private YolYonu yon;

	private Vector3 ileriYon;

	public Vector3 BitisNoktasi { get { return bitisNoktasi; } }

	public int Uzunluk { get { return yol.Count; } }

	public YolYonu Yon { get { return yon; } }

	public Vector3 IleriYon { get { return ileriYon; } }

	public Vector3 SagYon
	{
		get
		{
			if( yon == YolYonu.Ileri )
				return new Vector3( 1f, 0f, 0f );

			else
				return new Vector3( 1f, 0f, 0f );
		}
	}

	public YolContainer()
	{
		yol = new List<YolObjesi>( 32 );
		puanObjeleri = new List<Transform>( 32 );
	}

	public void YolOlustur( Vector3 baslangicNoktasi, YolYonu yolYonu, int uzunluk )
	{
		// yolYonu'nden yolun ileri yön vektörünü hesapla
		if( yolYonu == YolYonu.Ileri )
			ileriYon = new Vector3( 0f, 0f, 1f );

		else
			ileriYon = new Vector3( 0f, 0f, 1f );

		

		bitisNoktasi = baslangicNoktasi;
		yon = yolYonu;
	}

	public void YolaPuanObjeleriDiz( int birDizidekiPuanObjesiSayisi )
	{
		int puanObjeleriArasiAralik = 1;

		int yon = Random.Range( 0, 2 );

		int diziliPuanObjesi = 0;
		for( int i = 0; i < yol.Count; i++ )
		{
			int puanSpawnNoktasiSayisi;
			Transform parentObje;

			if( yon == 0 )
				parentObje = yol[i].solPuanSpawnNoktalari;
			else
				parentObje = yol[i].sagPuanSpawnNoktalari;

			puanSpawnNoktasiSayisi = parentObje.childCount;

			for( int j = 0; j < puanSpawnNoktasiSayisi && diziliPuanObjesi < birDizidekiPuanObjesiSayisi; j++ )
			{
				Transform obje = ObjeHavuzu.Instance.HavuzdanPuanObjesiCek();

				obje.localPosition = parentObje.GetChild( j ).position;
				obje.localEulerAngles = new Vector3( 0f, 0f, 0f );
				obje.gameObject.SetActive( true );

				puanObjeleri.Add( obje );

				diziliPuanObjesi++;
			}


			if( diziliPuanObjesi == birDizidekiPuanObjesiSayisi )
			{

				diziliPuanObjesi = 0;
				i += puanObjeleriArasiAralik;
				yon = Random.Range( 0, 2 );
			}
		}
	}

	public void YoluKaydir( Vector3 kaydirmaMiktari )
	{
		for( int i = 0; i < yol.Count; i++ )
			yol[i].transform.localPosition += kaydirmaMiktari;

		for( int i = 0; i < puanObjeleri.Count; i++ )
			puanObjeleri[i].localPosition += kaydirmaMiktari;

		bitisNoktasi += kaydirmaMiktari;
	}


	public void YoluYokEt()
	{

		for( int i = 0; i < yol.Count; i++ )
		{
			YolObjesi obje = yol[i];
			obje.gameObject.SetActive( false );
			ObjeHavuzu.Instance.HavuzaYolObjesiEkle( obje );
		}

		for( int i = 0; i < puanObjeleri.Count; i++ )
		{
			Transform obje = puanObjeleri[i];
			obje.gameObject.SetActive( false );
			ObjeHavuzu.Instance.HavuzaPuanObjesiEkle( obje );
		}

		yol.Clear();
		puanObjeleri.Clear();
	}
}