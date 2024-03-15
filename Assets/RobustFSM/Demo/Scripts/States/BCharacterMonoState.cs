using RobustFSM.Base;

namespace RobustFSM.Demo.Scripts.States
{
    public class BCharacterMonoState : MonoState
    {
        /// <summary>
        /// A little extra stuff. Accessing info inside the OwnerFSM
        /// </summary>
        public Character Owner
        {
            get
            {
                return ((CharacterFSM)SuperMachine).Owner;
            }
        }
    }
}
