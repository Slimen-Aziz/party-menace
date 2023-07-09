using UnityEngine;

namespace Elements
{
    [ExecuteInEditMode]
    public class Cable : MonoBehaviour
    {
        [SerializeField] private float cableWidth;
        [SerializeField] private LineRenderer line;
        [SerializeField] private HingeJoint2D[] joints;

        private Rigidbody2D[] _rigidBodies;

        private void OnValidate()
        {
            line.positionCount = joints.Length;
            line.startWidth = cableWidth;
            _rigidBodies = new Rigidbody2D[joints.Length];
            for (var i = 0; i < joints.Length; i++)
            {
                _rigidBodies[i] = joints[i].GetComponent<Rigidbody2D>();
                if (i == 0) continue;
                joints[i].connectedBody = _rigidBodies[i - 1];
            }
        }
        
        private void Update()
        {
            for (var i = 0; i < joints.Length; i++) 
                line.SetPosition(i, joints[i].transform.position);
        }

        public void Connect()
        {
            foreach (var rb in _rigidBodies) 
                rb.bodyType = RigidbodyType2D.Static;

            foreach (var joint in joints) 
                joint.enabled = false;
        }

        public void Disconnect()
        {
            foreach (var rb in _rigidBodies)
                rb.bodyType = RigidbodyType2D.Dynamic;
        
            foreach (var joint in joints) 
                joint.enabled = true;
        }
    }
}
