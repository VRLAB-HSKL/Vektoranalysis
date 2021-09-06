using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurveViewController// : MonoBehaviour
{
    [Header("DisplayElements")]
    private Transform RootElement;
    private LineRenderer DisplayLR;
    private Transform TravelObject;
    private Transform ArcLengthTravelObject;

    private AbstractCurveView currentView;
    public AbstractCurveView CurrentView
    {
        get { return currentView; }
        set
        {
            currentView = value;
            currentView.UpdateView();

            TravelObject.gameObject.SetActive(CurrentView.HasTravelPoint);
            ArcLengthTravelObject.gameObject.SetActive(CurrentView.HasArcLengthPoint);
        }
    }

    public SimpleCurveView simpleView;
    public SimpleRunCurveView simpleRunView;
    public SimpleRunCurveWithArcLength simpleRunWithArcLengthView;
    public SelectionExerciseView selectionExerciseView;

    public CurveViewController(Transform root, LineRenderer displayLR, Transform travel, Transform arcTravel, float scalingFactor)
    {
        RootElement = root;
        DisplayLR = displayLR;
        TravelObject = travel;
        ArcLengthTravelObject = arcTravel;

        simpleView = new SimpleCurveView(DisplayLR, RootElement.position, scalingFactor);
        simpleView.UpdateView();

        simpleRunView = new SimpleRunCurveView(DisplayLR, RootElement.position, scalingFactor, TravelObject);
        simpleRunView.UpdateView();

        simpleRunWithArcLengthView = new SimpleRunCurveWithArcLength(DisplayLR, RootElement.position, scalingFactor, TravelObject, ArcLengthTravelObject);
        simpleRunWithArcLengthView.UpdateView();

        
        CurrentView = simpleRunWithArcLengthView;

        TravelObject.gameObject.SetActive(CurrentView.HasTravelPoint);
        ArcLengthTravelObject.gameObject.SetActive(CurrentView.HasArcLengthPoint);
    }

    public void SwitchView()
    {
        
    }
}
