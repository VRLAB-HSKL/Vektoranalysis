using System.Collections;
using System.Collections.Generic;
//using IronPython.Hosting;
using UnityEngine;

public class PythonExample : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
	    //Foo02();
    }

//     private void Foo01()
//     {
// 	    var engine = Python.CreateEngine();
//
// 	    ICollection<string> searchPaths = engine.GetSearchPaths();
//
// 	    //Path to the folder of greeter.py
// 	    searchPaths.Add(Application.dataPath);
// 	    //Path to the Python standard library
// 	    searchPaths.Add(Application.dataPath + @"\Plugins\IronPython\Lib\");
// 	    engine.SetSearchPaths(searchPaths);
//
// 	    dynamic py = engine.ExecuteFile(Application.dataPath + @"\greeter.py");
// 	    dynamic greeter = py.Greeter("Mika");
// 	    Debug.Log(greeter.greet());
// 	    Debug.Log(greeter.random_number(1,5));
//     }
//
//     private void Foo02()
//     {
// 	    var eng = IronPython.Hosting.Python.CreateEngine();
// 	    var scope = eng.CreateScope();
// 	    
// 	    eng.Execute(@"
// import sympy as sp
//
// def greetings(name):
// 	return 'Hello ' + name.title() + '!'
// ", scope);
// 	    dynamic greetings = scope.GetVariable("greetings");
// 	    Debug.Log(greetings("world"));
//     }
    
    
    // Update is called once per frame
    void Update () {
		
	}
}
