using System;
using System.Collections.Generic;
using System.Reflection;
using StereoKit;

public class Touchable
{
    Model _model;
    Bounds _bounds;
    bool _pressed;
    bool first;

    object[] _touchedargs;
    object[] _releaseddargs;
    object[] _clickargs;
    object[] _heldargs;

    object _touchedarg;
    object _releaseddarg;
    object _clickarg;
    object _heldarg;

    Delegate _OnClick;
    Delegate _OnTouched;
    Delegate _OnReleased;
    Delegate _OnHold;
    // OnReleased;
    //Delegate OnClicked;
    /// <summary>
    /// Tracks whether hands are currently touching an objec t or have ket go the object
    /// 
    /// Touched() takes a function Func. When a hand enters the object's Bounds, it will execute Func;
    ///     Touched adds Obj to the list of touched objects 
    /// Released() takes a function Func. If a hand that was previously touching an object exits its Bound, it will execute Func
    ///     
    /// </summary>
    
    
    public Touchable(ref Model model, Bounds bounds)
    {
        _model = model;
        _bounds = bounds;
    }

    public Touchable(ref Model model)
    {
        _model = model;
    }

    public void OnTouch(Delegate TouchFunc, object[] args)
    {
        _touchedargs = args;
        _OnTouched = TouchFunc;
    }

    public void OnHold(Delegate HoldFunc, object[] args)
    {
        _heldarg = args;
        _OnTouched = HoldFunc;
    }

    public void OnClick(Delegate ClickFunc, object[] args)
    {
        _clickargs = args;
        _OnClick = ClickFunc;
    }


    public void Update()
    {
        _model.Draw(Matrix.TS(Vec3.Zero, 1f));
        for (int h = 0; h < (int)Handed.Max; h++)
        {
            Hand hand = Input.Hand((Handed)h);
            Pose fingertip = hand[FingerId.Index, JointId.Tip].Pose;
            if (hand.IsTracked)
            {
                if (_bounds.Contains(fingertip.position))
                {
                    if (first)
                    {
                        if (_OnTouched != null)
                            _OnTouched.DynamicInvoke(_touchedargs);
                        first = false;
                        _pressed = true;
                    }
                    else if (_OnHold != null)
                        _OnHold.DynamicInvoke(_heldargs);
                    Console.WriteLine("Touching\n");
                }
                if (!_bounds.Contains(fingertip.position))
                {
                    first = false;
                    Console.WriteLine("Not Touching\n");
                    if (_pressed == true)
                    {
                        _pressed = false;
                        if (_OnClick != null)
                           _OnClick.DynamicInvoke(_clickargs);
                    }
                }
            }
        }
    }
}
