using System;
using System.Collections.Generic;

namespace MySuperApi.Models;

public partial class Cart
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public virtual ICollection<CartProduct> CartProducts { get; } = new List<CartProduct>();

    public virtual MyPetUser? User { get; set; }
}
