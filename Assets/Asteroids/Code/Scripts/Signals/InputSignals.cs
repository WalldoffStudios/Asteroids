using UnityEngine;

namespace Asteroids
{
    public class MovementStateChangedSignal
    {
        public bool MovementStarted { get; private set; }
        public MovementStateChangedSignal(bool started) => MovementStarted = started;
    }

    public class MovementUpdateSignal
    {
        public Vector2 Direction { get; private set; }

        public MovementUpdateSignal(Vector2 direction) => Direction = direction;
    }

    public class ShootInputChangedSignal
    {
        public bool StartedShooting { get; private set; }
        public ShootInputChangedSignal(bool startedFiring) => StartedShooting = startedFiring;
    }

    public class CameraZoomSignal
    {
        public float ZoomLevel { get; private set; }
        public CameraZoomSignal(float zoomLevel) => ZoomLevel = zoomLevel;
    }
}