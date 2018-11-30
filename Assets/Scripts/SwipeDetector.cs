using UnityEngine;
using System.Collections;

public class SwipeDetector : MonoBehaviour
{
	public float minSwipeDistY;
	public float minSwipeDistX;
	private Vector2 startPos;
	public static float swipeValue;
	private bool stopJump = false;

	void Update ()
	{
		if (Input.touchCount > 0) {	
			Touch touch = Input.touches [0];

			switch (touch.phase) {
				
			case TouchPhase.Began:

				startPos = touch.position;
				
				break;
			case TouchPhase.Moved:

				float swipeDistVertical = (new Vector3 (0, touch.position.y, 0) - new Vector3 (0, startPos.y, 0)).magnitude;
				
				if ((swipeDistVertical > minSwipeDistY) && (!stopJump)) {
					swipeValue = Mathf.Sign (touch.position.y - startPos.y);
					stopJump = true;
				}
				break;
			case TouchPhase.Ended:
				stopJump = false;
				swipeValue = 0;
				break;
			}
		}
	}
}