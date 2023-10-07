using System.Collections.Generic;
using Exiled.API.Features.Items;
using PlayerRoles;

public class PlayerHandler
{
    public string Name { get; set; }
    public RoleTypeId Class { get; set; }
    public float Position_X { get; set; }
    public float Position_Y { get; set; }
    public float Position_Z { get; set; }
    //public IEnumerable<Item> Inventory { get; set; }
    public float Health { get; set; }
}
