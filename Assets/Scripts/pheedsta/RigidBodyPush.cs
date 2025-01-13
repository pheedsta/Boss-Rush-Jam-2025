using UnityEngine;

// code taken from "Starter Assets - FirstPerson" in Unity Asset Store

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: RigidBodyPush
//++++++++++++++++++++++++++++++++++++++++//

public class RigidBodyPush : MonoBehaviour {
	
	//:::::::::::::::::::::::::::::://
	// Serialized Fields
	//:::::::::::::::::::::::::::::://
	
	[Header("Settings")]
	[SerializeField] private LayerMask includeLayers;
	[Range(0.5f, 5f)]
	[SerializeField] private float strength = 1f;
    
	//:::::::::::::::::::::::::::::://
	// Components
	//:::::::::::::::::::::::::::::://

	private void OnControllerColliderHit(ControllerColliderHit controllerColliderHit)  {
		PushRigidBody(controllerColliderHit);
	}
	
	//:::::::::::::::::::::::::::::://
	// Pushing Rigid Bodies
	//:::::::::::::::::::::::::::::://

	private void PushRigidBody(ControllerColliderHit controllerColliderHit) {
		// if we didn't hit a non kinematic rigid body, we're done
		var colliderRigidbody = controllerColliderHit.collider.attachedRigidbody;
		if (!colliderRigidbody || colliderRigidbody.isKinematic) return;

		// if the object is not on a push layer, we're done
		var rigidbodyLayerMask = 1 << colliderRigidbody.gameObject.layer;
		if ((rigidbodyLayerMask & includeLayers.value) == 0) return;

		// we dont want to push objects below us
		if (controllerColliderHit.moveDirection.y < -0.3f) return;

		// calculate push direction from move direction, horizontal motion only
		var pushDirection = new Vector3(controllerColliderHit.moveDirection.x, 0.0f, controllerColliderHit.moveDirection.z);

		// apply the push and take strength into account
		colliderRigidbody.AddForce(pushDirection * strength, ForceMode.Impulse);
	}
}