using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace sdLitica.Entities.Abstractions
{
    /// <summary>
    /// Abstract class for any entity that uniquely identifies it
    /// </summary>
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Entity unique Id
        /// </summary>
        [Column("ID")]
        public Guid Id { get; protected set; }

        /// <summary>
        /// Creates an Entity with new guid id
        /// </summary>
        protected Entity()
        {
            
        }

        /// <summary>
        /// Creates an Entity with existing guid id
        /// </summary>
        /// <param name="id"></param>
        protected Entity(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException(nameof(id));
            Id = id;
        }
    }
}
