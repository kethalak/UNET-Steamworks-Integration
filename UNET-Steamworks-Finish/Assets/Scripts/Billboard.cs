using UnityEngine;

namespace USG.NetworkExample{
	public class Billboard : MonoBehaviour {

		void Update () {
			transform.LookAt(Camera.main.transform);
		}
	}
}
