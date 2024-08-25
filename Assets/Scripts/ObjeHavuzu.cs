using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObjeHavuzu : MonoBehaviour
{
	public static ObjeHavuzu Instance { get; private set; }

	public Transform[] puanPrefablari;
	public int puanHavuzuBoyutu = 50;

	public YolObjesi[] ileriYolPrefablari;
	public int ileriYolHavuzuBoyutu = 75;


	private List<Transform> puanObjeleriHavuzu;

	private List<YolObjesi> ileriYolObjeleriHavuzu;
	


	private void Awake()
	{
		if( Instance == null )
			Instance = this;
		else if( this != Instance )
		{
			Destroy( this );
			return;
		}

		puanObjeleriHavuzu = new List<Transform>( puanHavuzuBoyutu );
		HavuzuDoldur( puanObjeleriHavuzu, puanPrefablari, puanHavuzuBoyutu );

		ileriYolObjeleriHavuzu = new List<YolObjesi>( ileriYolHavuzuBoyutu );
		HavuzuDoldur( ileriYolObjeleriHavuzu, ileriYolPrefablari, ileriYolHavuzuBoyutu );

	
	
	}

	private void HavuzuDoldur( IList havuz, IList prefablar, int havuzBoyutu )
	{
		int herTipObjeSayisi = havuzBoyutu / prefablar.Count;

		for( int i = 0; i < prefablar.Count; i++ )
		{
			for( int j = 0; j < herTipObjeSayisi; j++ )
			{
				Component obje = Instantiate( (Component) prefablar[i] );
				obje.gameObject.SetActive( false );
				havuz.Add( obje );
			}
		}
	}

	private Component HavuzdanObjeCek( IList havuz, IList prefablar )
	{
		Component obje;

		if( havuz.Count <= 0 )
		{
			int randomIndex = Random.Range( 0, prefablar.Count );
			obje = Instantiate( (Component) prefablar[randomIndex] );
		}
		else
		{

			int randomIndex = Random.Range( 0, havuz.Count );
			obje = (Component) havuz[randomIndex];
			havuz.RemoveAt( randomIndex );
		}

		return obje;
	}

	public Transform HavuzdanPuanObjesiCek()
	{
		return (Transform) HavuzdanObjeCek( puanObjeleriHavuzu, puanPrefablari );
	}

	public YolObjesi HavuzdanYolObjesiCek()
	{
		return (YolObjesi) HavuzdanObjeCek( ileriYolObjeleriHavuzu, ileriYolPrefablari );
	}

	



	public void HavuzaPuanObjesiEkle( Transform obje )
	{
		puanObjeleriHavuzu.Add( obje );
	}

	public void HavuzaYolObjesiEkle( YolObjesi obje )
	{
		if( obje.yolTuru == YolTuru.DuzYol )
			ileriYolObjeleriHavuzu.Add( obje );
		
		else
			ileriYolObjeleriHavuzu.Add( obje );
	}
}