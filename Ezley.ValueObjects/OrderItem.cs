using System;

namespace Ezley.ValueObjects
{
    public record OrderItem(DateTime TimeAdded, string Name, decimal Amount);
}