using UnityEngine;
using Zenject;

namespace Asteroids
{
    [CreateAssetMenu(fileName = "New game settings", menuName = "Asteroids/Settings/Game settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [SerializeField] private GameInstaller.Settings gameSettings = null;
        [SerializeField] private PlayerMoveHandler.Settings moveSettings = null;
        [SerializeField] private PlayerHealthHandler.Settings healthSettings = null;
        [SerializeField] private PlayerWeaponHandler.Settings playerWeaponSettings = null;
        [SerializeField] private PlayerBordersHandler.Settings playerBorderSettings = null;
        [SerializeField] private AsteroidManager.Settings asteroidSettings = null;
        [SerializeField] private ScreenBorders.Settings screenBorderSettings = null;
        
        [SerializeField] private UICoinManager.Settings coinManagerSettings = null;
        

        public override void InstallBindings()
        {
            Container.BindInstance(gameSettings).IfNotBound();
            Container.BindInstance(screenBorderSettings).IfNotBound();
            
            Container.BindInstance(moveSettings).IfNotBound();
            Container.BindInstance(healthSettings).IfNotBound();
            Container.BindInstance(playerWeaponSettings).IfNotBound();
            Container.BindInstance(playerBorderSettings).IfNotBound();
            
            Container.BindInstance(asteroidSettings).IfNotBound();     
            
            Container.BindInstance(coinManagerSettings).IfNotBound();
        }
    }   
}
