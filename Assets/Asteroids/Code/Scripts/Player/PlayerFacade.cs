using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerFacade : MonoBehaviour
    {
        private Player _player;

        [Inject]
        public void Construct(Player player)
        {
            _player = player;
            Debug.Log($"Constructed player");
        }
        
        public bool IsDead => _player.IsDead;
        public Vector3 Position => _player.Position;
        public float Rotation => _player.Rotation;

        private void Update()
        {
            Vector3 localEulers = transform.localEulerAngles;
            localEulers.z = Rotation;
            transform.rotation = Quaternion.Euler(localEulers);
        }
    }   
}
