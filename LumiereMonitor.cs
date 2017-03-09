using UnityEngine;
using System.Collections;

public class LumiereMonitor : MonoBehaviour {
	public GameObject prefabLumieres; 

	public float speedMin;
	public float speedMax;
	public float range;
	[Range(0.0f, 8.0f)]
	public float intensity;
	public int Creer_1_Parmi;
	public int Tuer_1_Parmi;


	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void FixedUpdate () {

		int create  = Random.Range (0, Creer_1_Parmi- 1 );
		if(create == 0){
			GameObject light = (GameObject)Instantiate (prefabLumieres, Vector3.zero, Quaternion.identity);
			light.transform.parent = transform;
			light.GetComponent<Lumiere> ().speedMin = speedMin;
			light.GetComponent<Lumiere> ().speedMax = speedMax;
			light.GetComponent<Lumiere> ().range = range;
			light.GetComponent<Lumiere> ().Tuer_1_Parmi = Tuer_1_Parmi;
			light.GetComponent<Lumiere> ().intensity = intensity;
			light.tag  = "lum";



		}
	}
}
