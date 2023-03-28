namespace Lattice.Domain.Entities;

/// <summary>
///  Define the possible state of a Card.
///  <list type="bullet">
///   <item>
///    To Do - 0
///   </item>
///   <item>
///    Commited - 1  
///   </item>
///   <item>
///    On Hold - 2  
///   </item>
///   <item>
///    Completed - 3  
///   </item>
///   <item>
///    Dropped - 4
///   </item>
///  </list>
/// </summary>
public enum CardStatus
{
    Todo = 0,
    Commited,
    OnHold,
    Completed,
    Dropped
}
