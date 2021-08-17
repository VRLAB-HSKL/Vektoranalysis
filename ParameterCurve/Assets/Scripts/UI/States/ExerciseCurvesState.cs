using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseCurvesState : AbstractCurveSelectionState
{
    public ExerciseCurvesState(
        GameObject menuContent, GameObject prefab, WorldStateController world) : base(menuContent, prefab, world) { }

    public override void OnStateUpdate() {}
}
