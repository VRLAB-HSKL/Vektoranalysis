using System;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using UnityEngine;
using Python;
using Python.Runtime;
using UnityEditor.Scripting.Python;
using UnityEngine.SocialPlatforms;

using System.Collections.Generic;

public static class PythonUtility
{
    public static void CalculatePoints()
    {
        var pointList = new List<Vector3>();
        
        using (Py.GIL())
        {
            PythonRunner.EnsureInitialized();
            dynamic builtins = Py.Import("builtins");
            dynamic np = Py.Import("numpy");
            dynamic sp = Py.Import("sympy");
            dynamic spAbc = Py.Import("sympy.abc");
            // dynamic system = Py.Import("System");
            //dynamic gen = Py.Import("System.Collection.Generic");
            
            // using (var scope = Py.CreateScope())
            // {
            //     
            // }

            

            
            // Debug.Log(np.cos(np.pi * 2));
            //
            // dynamic sin = np.sin;
            // Debug.Log(sin(5));
            //
            // double c = (double)(np.cos(5) + sin(5));
            // Debug.Log(c);
            //
            // dynamic a = np.array(new List<float> { 1, 2, 3 });
            // Debug.Log(a.dtype);
            //
            // dynamic b = np.array(new List<float> { 6, 5, 4 }, dtype: np.int32);
            // Debug.Log(b.dtype);
            //
            // Debug.Log(a * b);

            //sf = static_sf_dict[sf_key]
            //x_param_range = sf["x_param_range"]
            //y_param_range = sf["y_param_range"]
            // x_values = np.linspace(x_param_range[0], x_param_range[1], number_of_samples)
            // y_values = np.linspace(y_param_range[0], y_param_range[1], number_of_samples)
            // x_values, y_values = np.meshgrid(x_values, y_values)
            // zExpr = sf["z_expr"]
            // zFunc = sp.lambdify([sp.abc.x, sp.abc.y], zExpr, "numpy")
            // z_values = zFunc(x_values, y_values)
            // return x_values, y_values, z_values

            
            using (var scope = Py.CreateScope())
            {
                var numberOfSamples = 200;
                
                dynamic xValues = np.linspace(-2.0, 2.0, numberOfSamples);
                dynamic yValues = np.linspace(-2.0, 2.0, numberOfSamples);
            
                dynamic zExpr = -(sp.sin(spAbc.x)) * sp.sin(spAbc.y);

                //dynamic symbolType = builtins.type(sp.core.symbol.Symbol);
                var lst = builtins.list();
                //var lst = system.Collections.Generic.List[sp.core.symbol.Symbol]();
                
                //dynamic varArr = Array.CreateInstance(symbolType, 2); //system.Array.CreateInstance(symbolType, 2);
                //varArr[0] = abc.x;
                //varArr[1] = abc.y;

                lst.append(spAbc.x);
                lst.append(spAbc.y);
                
                Debug.Log(lst[0]);
                Debug.Log(lst[1]);
                
                dynamic zFunc = sp.lambdify(lst, zExpr, "numpy");
                dynamic zValues = zFunc(xValues, yValues);

                for (var i = 0; i < numberOfSamples; i++)
                {
                    Debug.Log("x: " + xValues[i] + ", y: " + yValues[i] + ", z: " + zValues[i]);
                }
            }
            

            
        }
        

        
        
    }
}
