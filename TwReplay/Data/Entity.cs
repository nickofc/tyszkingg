using System;

namespace TwReplay.Data
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}