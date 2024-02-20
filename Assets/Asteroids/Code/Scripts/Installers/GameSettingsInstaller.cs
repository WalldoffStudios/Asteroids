using UnityEngine;
using Zenject;

namespace Asteroids
{
    [CreateAssetMenu(fileName = "New game settings", menuName = "Asteroids/Settings/Game settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public GameInstaller.Settings GameSettings;
        public PlayerMoveHandler.Settings MoveSettings;
        public PlayerRotationHandler.Settings RotationSettings;
        public PlayerHealthHandler.Settings HealthSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(GameSettings).IfNotBound();
            Container.BindInstance(MoveSettings).IfNotBound();
            Container.BindInstance(RotationSettings).IfNotBound();
            Container.BindInstance(HealthSettings).IfNotBound();
        }
    }   
}
