using UnityEngine;

namespace Services.Input
{
    public class StandaloneInputService : InputService
    {
        public override Vector3 Axis {
            get
            {
                Vector3 axis = new Vector3(UnityEngine.Input.GetAxis(Horizontal), 0);
                
                return axis;
            } 
        }
    }
}