using UnityEngine;
using System.Collections;
using System.Linq;

public class CameraScript : MonoBehaviour
{
		public float dragSpeed = 2;
		private Vector3 dragOrigin;
		public float MapWidth;
		public float MapHeight;
		public int MaxZoom = 25;
		public int MinZoom = 50;
		public float ZoomSpeed = 1f;
		public float CurrentZoom = 50f;
		public float MinX;
		public float MinY;
		public float MaxX;
		public float MaxY;

		void Start ()
		{
				camera.orthographic = true;
				camera.orthographicSize = CurrentZoom;
				CalculateCameraBounds ();
		}

		private void CalculateCameraBounds ()
		{
				var vertExtent = Camera.main.camera.orthographicSize;  
				var horzExtent = vertExtent * Screen.width / Screen.height;

				MinX = horzExtent - MapWidth / 2.0f;
				MaxX = MapWidth / 2.0f - horzExtent;
				MinY = vertExtent - MapHeight / 2.0f;
				MaxY = MapHeight / 2.0f - vertExtent;
		}

		void Update ()
		{
				if (GameObject.FindGameObjectsWithTag ("Tower") != null && !GameObject.FindGameObjectsWithTag ("Tower").Any (x => x.GetComponent<Tower> ().IsTowerSelected)) {
						if (Input.GetAxis ("Mouse ScrollWheel") > 0 && CurrentZoom > MaxZoom) {
								CurrentZoom = Mathf.Max (CurrentZoom - ZoomSpeed, MaxZoom);
								camera.orthographicSize = CurrentZoom;
								CalculateCameraBounds ();
						}

						// handle zoom out when on edge of screen		
						if (Input.GetAxis ("Mouse ScrollWheel") < 0 && CurrentZoom < MinZoom) {
								CurrentZoom = Mathf.Min (CurrentZoom + ZoomSpeed, MinZoom);
								camera.orthographicSize = CurrentZoom;
								CalculateCameraBounds ();
								if (IsCameraOffScreen (transform.position)) {
										Vector3 closestOnScreenPosition = GetClosestOnScreenPosition (transform.position);
										transform.position = closestOnScreenPosition;
								}
						}

						if (Input.GetMouseButtonDown (0)) {
								dragOrigin = Input.mousePosition;
								return;
						}
		
						if (!Input.GetMouseButton (0))
								return;
		
						Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - dragOrigin);
						Vector3 move = new Vector3 (pos.x * dragSpeed, pos.y * dragSpeed, 0);
						
						move = AdjustMovePositionToStayOnScreen (move);

						transform.Translate (move, Space.World);  
				}
		}

		private bool IsCameraOffScreen (Vector3 position)
		{
				if (position.x < MinX) {
						return true;
				}
				if (position.y < MinY) {
						return true;
				}
				if (position.x > MaxX) {
						return true;
				}
				if (position.y > MaxY) {
						return true;
				}
				return false;
		}

		private Vector3 GetClosestOnScreenPosition (Vector3 position)
		{
				position.x = Mathf.Max (position.x, MinX);
				position.y = Mathf.Max (position.y, MinY);
				position.x = Mathf.Min (position.x, MaxX);
				position.y = Mathf.Min (position.y, MaxY);

				return position;
		}

		private Vector3 AdjustMovePositionToStayOnScreen (Vector3 position)
		{
				// fix for zoom max width and height, ratio is not right
				if (transform.position.x <= MinX && position.x < 0) {
						position.x = 0;
				}
				if (transform.position.y <= MinY && position.y < 0) {
						position.y = 0;
				}
				if (transform.position.x >= MaxX && position.x > 0) {
						position.x = 0;
				}
				if (transform.position.y >= MaxY && position.y > 0) {
						position.y = 0;
				}
		
				return position;
		}
}
