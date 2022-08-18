using UnityEngine;

namespace Services.Input
{
    public class StandaloneInputService : InputService
    {
        public override Vector3 Axis {
            get
            {
                Vector3 axis = new Vector3(UnityEngine.Input.GetAxis(Horizontal), 0, UnityEngine.Input.GetAxis(Vertical));
                
                return axis;
            } 
        }

        public override bool IsAttackButtonUp()
        {
            return UnityEngine.Input.GetMouseButtonUp(0);
        }
    }
}