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
    object[] _clickargs;
    object[] _heldargs;

    Delegate _OnClick;
    Delegate _OnTouched;
    Delegate _OnHold;
    /// <summary>
    /// Executes the provided function when a hand is touching holding or has clicked (touched and let go) an object
    ///<example>
    ///
    ///  Model model = Model.FromMesh(Mesh.GenerateSphere(0.1f, 3), Default.Material);
    ///  Touchable cube;
    ///  var method1 = (Func<Model, bool>)click;
    ///  var method2 = (Func<Model, float, bool>)touch;
    ///  object[] arg = new object[2];
    ///  arg[0] = model;
    ///  arg[1] = 42.42f;
    ///  cube = new Touchable(ref model);
    ///  cube.OnClick(method1, arg);
    ///  cube.OnClick(method2, arg);
    ///  while (StereoKitApp.Step(() =>
    ///  {
    ///     cube.Update();
    ///  })) ;
    /// </example>
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
        _heldargs = args;
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
                }
                if (!_bounds.Contains(fingertip.position))
                {
                    first = false;
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
