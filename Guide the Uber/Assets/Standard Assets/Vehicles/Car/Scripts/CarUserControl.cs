using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        public bool keyboard ;
        public float h;
        public float v;
        public bool r;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }

        private void FixedUpdate()
        {
            // pass the input to the car!
            if (keyboard)
            {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
                float temp = Input.GetAxis("Reverse");
                if (temp >= 1)
                {
                    r = true;
                }
                else if (temp <= 0)
                {
                    r = false;
                }
            }

            m_Car.Move(h, v, v, 0f, r);
        }
    }
}
