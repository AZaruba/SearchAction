
namespace Authorship.src
{
  class InputActions
  {
    public static readonly string MoveForward = "MoveForward";
    public static readonly string MoveBack = "MoveBack";
    public static readonly string MoveLeft = "MoveLeft";
    public static readonly string MoveRight = "MoveRight";
    public static readonly string LookUp = "LookUp";
    public static readonly string LookDown = "LookDown";
    public static readonly string LookLeft = "LookLeft";
    public static readonly string LookRight = "LookRight";

    public static readonly string Interact = "Interact";
    public static readonly string OpenInventory = "OpenInventory";

#if DEBUG
    public static readonly string DEBUG_Reset = "DEBUG_Reset";
    public static readonly string DEBUG_Exit = "DEBUG_Exit";
    public static readonly string DEBUG_Toggle_Log = "DEBUG_Log";
#endif
  }
}