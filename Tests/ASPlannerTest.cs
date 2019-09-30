using NUnit.Framework;
using UnityEngine;
using NullRef = System.NullReferenceException;

public class ASPlannerTest : TestBase{

    ASPlanner x;

    [SetUp] public void Setup(){
        x = new ASPlanner();
        x.maxIter = x.maxNodes = 100;
    }

    [Test] public void AgentMissing()
    => Assert.Throws<NullRef>( () => x.Eval((Agent)null, null) );

    [Test] public void GoalMissing()
    => Assert.Throws<NullRef>( () => x.Eval( new Idler(), null) );

    [Test] public void IdlePassThrough()
    => o ( x.Eval(new Idler(), goal: x => true), null );

    [Test] public void HeuristicIdlePassThrough()
    => o ( x.Eval(new Idler(), goal: x => true, h: x => 0f), null );

    // TODO doesn't look right. First off, returned func should be
    // OneTrick, secondly if goal is always fulfilled no action is
    // needed
    [Test] public void OTPoneyPassThrough()
    => o( x.Eval(new OTPoney(), goal: x => true),
          "OTPoney" );

    // TODO see above
    [Test] public void HeuristicOTPoneyPassThrough()
    => o( x.Eval(new OTPoney(), goal: x => true, h: x => 0f),
          "OTPoney" );

    [Test] public void UseHeuristic(){
        bool h = false;
        x.Eval(new OTPoney(), x => false,
               h: x => { h = true; return 0f; });
        o( h, true );
    }

    [Test] public void IgnoreHeuristic(){
        bool h = false; x.brfs = true;
        x.Eval(new OTPoney(), x => false,
               h: x => { h = true; return 0f; });
        o( h, false );
    }

    [Test] public void DisallowZeroCost(){
        
    }

}
