using Zenject;

namespace Asteroids
{
    public class AddressablesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AddressablesManager>().FromComponentOn(gameObject).AsSingle();
        }
    }   
}
