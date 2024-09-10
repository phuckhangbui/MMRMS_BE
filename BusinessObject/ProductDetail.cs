using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class ProductDetail
{
    public int ProductDetailId { get; set; }

    public int? ProductId { get; set; }

    public int? ComponentId { get; set; }

    public DateTime? DateCreate { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ComponentProduct? Component { get; set; }

    public virtual Product? Product { get; set; }
}
