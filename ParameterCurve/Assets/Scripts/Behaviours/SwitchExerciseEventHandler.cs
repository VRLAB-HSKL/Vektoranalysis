using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviours
{
    public class SwitchExerciseEventHandler : AbstractVisualChangeSelectionEventHandler
    {
        //public ThreeSelectionView threeSel;
        public bool IsIncrement;
        
        
        
        protected override void HandlePointerClick(PointerEventData eventData)
        {
            if (IsIncrement)
            {
                threeSel.NextSubExercise();
            }
            else
            {
                threeSel.PreviousSubExercise();
            }
        }

        protected override void HandlePointerEnter(PointerEventData eventData)
        {
        
        }

        protected override void HandlePointerExit(PointerEventData eventData)
        {
        
        }
    }
}
