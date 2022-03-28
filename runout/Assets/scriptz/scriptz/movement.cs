using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class movement : MonoBehaviour {

		
	public GameObject peener;
	public drivingMapper dm;
	
	// Use this for initialization
	void Start () {
	
	    dm = peener.AddComponent<drivingMapper>();
	    print(dm.xAxis);
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(1f*dm.CurrentGear*dm.xAxis*Time.deltaTime, 0f, 0f);
		print (dm.CurrentGear);
	}
}
