using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public int nbpts;
	public int radius;
	[Range(0.05f, 1.7f)]
	public float scale;
	public int mindist1;
	public int maxdist1;
	public int mindist2;
	public int maxdist2;
	public int nbLiensParFace = 1;

	public GameObject prefab;

	private GameObject[] archi;

	// Use this for initialization
	void Start () {
		Generate ();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.R)){
			archi =  GameObject.FindGameObjectsWithTag ("architecture");
			Destroy(archi[0]);
			GameObject[] lumObj = GameObject.FindGameObjectsWithTag ("lumieres");
			lumObj[0].GetComponent<LumiereMonitor>().enabled = false;

			GameObject[] lums = GameObject.FindGameObjectsWithTag ("lum");

			foreach (GameObject g in lums) {
				Destroy (g);
			}
			Generate ();


		}
	}
	public void Generate(){
		GameObject NewArchitecture = (GameObject)Instantiate (prefab, Vector3.zero, Quaternion.identity);
		NewArchitecture.transform.parent = transform;
		NewArchitecture.GetComponent<StrucGenScript> ().nbpts = nbpts;
		NewArchitecture.GetComponent<StrucGenScript> ().radius = radius;
		NewArchitecture.GetComponent<StrucGenScript> ().scale = scale;
		NewArchitecture.GetComponent<StrucGenScript> ().mindist1 = mindist1;
		NewArchitecture.GetComponent<StrucGenScript> ().maxdist1 = maxdist1;
		NewArchitecture.GetComponent<StrucGenScript> ().mindist2 = mindist2;
		NewArchitecture.GetComponent<StrucGenScript> ().maxdist2 = maxdist2;
		NewArchitecture.GetComponent<StrucGenScript> ().nbLiensParFace = nbLiensParFace;

	}
}
