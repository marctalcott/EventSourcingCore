using System;
using System.Collections.Generic;

namespace Ezley.ValueObjects
{
    public record Address(string Line1, string Line2, string Line3, string City, string PostalCode, string State, string Country);
}