using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Lumiere : MonoBehaviour {

	public float speed;
	public float speedMin;
	public float speedMax;
	public float range;
	public int Tuer_1_Parmi;
	public float intensity;



	private bool generate; 
	private bool ismoving; 

	private GameObject[] structure;

	private NodeClass origin;
	private NodeClass destination;
	private float counter;
	private float dist;
	private float distToDest;


	private int rand;
	private int rand2;



	// Use this for initialization
	void Start () {
		structure =  GameObject.FindGameObjectsWithTag ("tetra");
		generate = false;
		ismoving = false;
		counter = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!generate) {
			CreateLight ();
		} else {
			if (!ismoving) {
				
				rand2 = Random.Range (0, origin.fils.Count);
				destination = origin.fils [rand2];
				dist = Vector3.Distance (origin.center, destination.center);
				ismoving = true;
			} else {
				Vector3 pointOrigin = origin.center;
				Vector3 pointDestination = destination.center;
				distToDest = Vector3.Distance(transform.position,pointDestination);

				if (distToDest > 0.5) {
					
					counter += .1f * speed;
					float x = Mathf.Lerp (0, dist, counter);
					transform.position = x * Vector3.Normalize (pointDestination - pointOrigin) + pointOrigin;
				} else {
					int die = Random.Range (0, Tuer_1_Parmi - 1);
					if((die == 0  )|| (destination.fils.Count == 1)){
						Destroy (transform.gameObject);
					} else {
						origin = destination;
						ismoving = false;
						counter = 0;
					}
				}

			}
		}

	}

	public void CreateLight(){
		do {
			rand = Random.Range (0, structure.Length);
		} while(structure [rand].GetComponent<NodeClass> ().fils.Count == 0);

	
		Light lightComp = transform.gameObject.AddComponent<Light> ();
		lightComp.range = range;
		lightComp.intensity = intensity;
		transform.position = structure [rand].GetComponent<NodeClass> ().center;
		origin = structure [rand].GetComponent<NodeClass> ();
		generate = true;
		speed = Random.Range (speedMin/10, speedMax/10);

	}



}
