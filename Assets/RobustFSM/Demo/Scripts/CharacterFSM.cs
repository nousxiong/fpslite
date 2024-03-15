using RobustFSM.Base;
using RobustFSM.Demo.Scripts.States.Idle;
using RobustFSM.Demo.Scripts.States.Patrol;

namespace RobustFSM.Demo.Scripts
{
    public class CharacterFSM : MonoFSM<Character>
    {
        public override void AddStates()
        {
            //add the states
            AddState<IdleMainState>();
            AddState<PatrolMainState>();

            //set the initial state
            SetInitialState<IdleMainState>();
        }
    }
}
