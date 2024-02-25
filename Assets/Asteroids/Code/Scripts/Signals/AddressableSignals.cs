namespace Asteroids
{
    public class AssetsDownloadedSignal
    {
        public BootManager BootManager { get; private set; }
        public AssetsDownloadedSignal(BootManager bootManager) => BootManager = bootManager;
    }
    
    //public class AssetsBoundSignal { }   
}
