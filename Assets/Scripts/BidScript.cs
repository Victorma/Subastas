using UnityEngine;
using System.Collections;

public class BidScript : MonoBehaviour {

	public PujadorScript bidParent;
	public float bidCuantity;



	public bool closed;
	private bool used = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0)){
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			Collider col = GetComponentInChildren<Collider>();

			RaycastHit info = new RaycastHit();
			if(col.Raycast(ray, out info, Camera.main.farClipPlane)){
				if(!used){
					GetComponent<Animator>().SetBool("Closed",true);
					used = true;
					AuctioningScript auc = Auction.currentAuction.GetComponent<AuctioningScript>();
					
					auc.ganadorActual = bidParent;
					auc.price += bidCuantity;
					
					foreach(PujadorScript p in GameObject.FindObjectsOfType<PujadorScript>())
						p.TakeBid(this);
				}
			}
		}
		if(closed){
			DestroyImmediate(this.gameObject);
		}
	
	}
}
