using UnityEngine;

namespace ParamCurve.Scripts
{
    public class PosAndRotConstraint : MonoBehaviour
    {
        public GameObject leftBorderCube;
        public GameObject rightBorderCube;

        private Vector3 initPos;
        private Vector3 lastPos;
        private Vector3 leftBorderPos;
        private Vector3 rightBorderPos;

        private void Start()
        {
            initPos = transform.position;
            lastPos = initPos;
            leftBorderPos = leftBorderCube.transform.position + leftBorderCube.GetComponent<MeshRenderer>().bounds.extents;
            rightBorderPos = rightBorderCube.transform.position - rightBorderCube.GetComponent<MeshRenderer>().bounds.extents;
        }

        private void Update()
        {
            transform.rotation = Quaternion.identity;

            float xValue = 0f;
            if(transform.position.x < leftBorderPos.x ||
               transform.position.x > rightBorderPos.x)
            {
                xValue = lastPos.x;            
            }
            else
            {
                xValue = transform.position.x;
            }

            transform.position = new Vector3(xValue, initPos.y, initPos.z);

            lastPos = transform.position;
        }

    }
}
