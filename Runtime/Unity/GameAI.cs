#if UNITY_2018_1_OR_NEWER

using UnityEngine;

/*
How-to:
1 - Implement the Goal() and Model() methods
2 - Each action returned by the solver should match
a public, no-arg function call supported by at least
one component.
3 - When an action starts/ends set busy = true/false
*/
namespace Activ.GOAP{
public abstract class GameAI<T> : MonoBehaviour where T : Agent{

    public bool verbose = true;
    public bool busy;
    Solver<T> solver;

    // Decide a goal and (optionally) a heuristic
    protected abstract Goal<T> Goal();

    // Instantiates or updates the model using relevant world states
    protected abstract T Model();

    // Action mapping may be overriden
    protected virtual void Effect(object action){
        if(action as string == State.Init) return;
        var t = Time.frameCount;
        if(action is System.Action method){
            Print($"Delegate: {method.Method.Name} at frame {t}");
            method();
        }else{
            Print($"No-arg message: {(string)action} at frame {t}");
            SendMessage(action.ToString());
        }
    }

    void Update(){
        if(busy) return;
        solver = solver ?? new Solver<T>();
        Effect(solver.Next(Model(), Goal()));
    }

    void Print(object arg){ if(verbose) print(arg); }

}}

#endif  // UNITY_2018_1_OR_NEWER
