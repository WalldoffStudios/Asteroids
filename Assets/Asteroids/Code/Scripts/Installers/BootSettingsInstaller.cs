using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    [CreateAssetMenu(fileName = "Boot settings", menuName = "Asteroids/Settings/Boot settings")]
    public class BootSettingsInstaller : ScriptableObjectInstaller<BootSettingsInstaller>
    {
        [SerializeField] private AddressablesManager.Settings addressableSettings = null;

        public override void InstallBindings()
        {
            Container.BindInstance(addressableSettings).IfNotBound();
        }
    }

    [Serializable]
    public class PlayerTextureAsset
    {
        public Sprite PlayerTexture { get; private set; }
        public PlayerTextureAsset(Sprite texture) => PlayerTexture = texture;
    }
}
