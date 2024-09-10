using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Attribute
{
    public int AttributeId { get; set; }

    public int? ProductId { get; set; }

    public string? AttributeName { get; set; }

    public string? Specifications { get; set; }

    public int? Status { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Product? Product { get; set; }
}
