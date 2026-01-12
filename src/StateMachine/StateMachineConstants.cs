
namespace StateManagement {
  public enum Command
  {
   PAUSE = 0,
   MOVE,
   STOP,
   INTERACT,
   ENTER_WATER,
   LEAVE_WATER,
   SLIDE,
   END_SLIDE,
   MOUNT,
   DISMOUNT,
   FALL,
   LAND
  }

  public enum State
  {
    PAUSED = 0,
    GROUNDED,
    WALKING,
    SWIMMING,
    DIVING,
    SLIDING,
    CLIMBING,
    FALLING,
    ERROR_STATE = -1

  }
}