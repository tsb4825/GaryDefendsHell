using UnityEngine;
using System.Collections;
using System.Linq;

public class MapSelectionScript : MonoBehaviour
{
		private bool IsMapSelected;

		void OnGUI ()
		{
				if (IsMapSelected) {

				}
		}

		void Update ()
		{
				if (Input.GetMouseButtonDown (0)) {
						RaycastHit2D[] hit = Physics2D.RaycastAll (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			
						if (hit.Any (x => x.collider is BoxCollider2D)) {
								IsMapSelected = !IsMapSelected;
								
								if (IsMapSelected) {
										// bring up menu
								}
			
						}
				}
		}
}
