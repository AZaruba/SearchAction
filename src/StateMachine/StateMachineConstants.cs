
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
   MOUNT,
   DISMOUNT
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
    ERROR_STATE = -1

  }
}