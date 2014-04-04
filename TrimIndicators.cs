using System;
using UnityEngine;

namespace KSP_TrimIndicators {
	// Set to start every time the flight scene is loaded
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class TrimIndicators : MonoBehaviour {
		
		private static float scaleAmount = 0.75f;
		
		// Local vars to keep a reference to our Gauges
		private LinearGauge trimRoll;
		private LinearGauge trimPitch;
		private LinearGauge trimYaw;
		
		private void InitGauges() {
			// Find the original gauges.
			GameObject stagingQuadrant = GameObject.Find("staging Quadrant");
			GameObject roll = stagingQuadrant.transform.FindChild("roll").gameObject;
			GameObject pitch = stagingQuadrant.transform.FindChild("pitch").gameObject;
			GameObject yaw = stagingQuadrant.transform.FindChild("yaw").gameObject;
			
			//Create our own.
			GameObject myRoll = (GameObject)Instantiate(roll, Vector3.zero, Quaternion.identity);
			myRoll.name = "rollTrim";
			GameObject myPitch = (GameObject)Instantiate(pitch, Vector3.zero, Quaternion.identity);
			myPitch.name = "pitchTrim";
			GameObject myYaw = (GameObject)Instantiate(yaw, Vector3.zero, Quaternion.identity);
			myYaw.name = "yawTrim";

			//Set parants. (Not sure if this is required but doing it just to make sure)
			myRoll.transform.SetParent(roll.transform.parent);
			myPitch.transform.SetParent(pitch.transform.parent);
			myYaw.transform.SetParent(yaw.transform.parent);

			//Set rotations.
			myRoll.transform.rotation = roll.transform.rotation;
			myPitch.transform.rotation = pitch.transform.rotation;
			myYaw.transform.rotation = yaw.transform.rotation;

			//Scale them down.
			myRoll.transform.localScale = Vector3.Scale(roll.transform.localScale, new Vector3(-scaleAmount, scaleAmount, scaleAmount));
			myPitch.transform.localScale = Vector3.Scale(pitch.transform.localScale, new Vector3(-scaleAmount, scaleAmount, scaleAmount));
			myYaw.transform.localScale = Vector3.Scale(yaw.transform.localScale, new Vector3(-scaleAmount, scaleAmount, scaleAmount));
			

			//Get the LinearGauge components.
			trimRoll = myRoll.GetComponent<LinearGauge>();
			trimPitch = myPitch.GetComponent<LinearGauge>();
			trimYaw = myYaw.GetComponent<LinearGauge>();
			
			//Update gauges positions using scaling, took a while to figure out these values.
			trimRoll.minPos = Vector3.Scale(trimRoll.minPos, new Vector3(1f, 1f - 0.22f, 1f));
			trimRoll.maxPos = Vector3.Scale(trimRoll.maxPos, new Vector3(1f, 1f - 0.22f, 1f));
			trimPitch.minPos = Vector3.Scale(trimPitch.minPos, new Vector3(1f - 0.11f, 1f, 1f));
			trimPitch.maxPos = Vector3.Scale(trimPitch.maxPos, new Vector3(1f - 0.11f, 1f, 1f));
			trimYaw.minPos = Vector3.Scale(trimYaw.minPos, new Vector3(1f, 1f - 0.50f, 1f));
			trimYaw.maxPos = Vector3.Scale(trimYaw.maxPos, new Vector3(1f, 1f - 0.50f, 1f));
			
			//Set LinearGauge's pointer.
			trimRoll.pointer = myRoll.transform;
			trimPitch.pointer = myPitch.transform;
			trimYaw.pointer = myYaw.transform;
		}
		
		//Unity Events...
		//Called just before the first frame is rendered. So be quick about what you do here.
		//I used Start instead of Awake because the Guages may not be fully setup in Awake.
		private void Start() {
			InitGauges();
		}
		
		//Called at a set interval. 
		private void FixedUpdate() {
			FlightCtrlState ctrlState = FlightGlobals.ActiveVessel.ctrlState;
			trimRoll.setValue(ctrlState.rollTrim);
			trimPitch.setValue(ctrlState.pitchTrim);
			trimYaw.setValue(ctrlState.yawTrim);
		}
	}
}
