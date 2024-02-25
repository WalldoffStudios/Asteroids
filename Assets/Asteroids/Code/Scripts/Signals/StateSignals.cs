namespace Asteroids
{
    public class GameStateChangedSignal
    {
        public GameStates State { get; private set; }

        public GameStateChangedSignal(GameStates state) => State = state;
    }

    public class GameSceneInitializedSignal { }
}
