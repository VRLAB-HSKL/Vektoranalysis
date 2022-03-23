using Controller;
using UnityEngine;

namespace UI.States
{
    /// <summary>
    /// Curve state used during selection and interaction of exercises in the exercises dataset
    /// </summary>
    public class ExerciseCurvesState : AbstractCurveSelectionState
    {
        public ExerciseCurvesState(
            GameObject menuContent, GameObject prefab, WorldStateController world) : base(menuContent, prefab, world) { }

        public override void OnStateUpdate() {}
    }
}
