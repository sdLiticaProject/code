using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Entities.Abstractions
{
    /// <summary>
    /// Interface that uniquely identifies any entity
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Entity unique Id
        /// </summary>
        Guid Id { get; }
    }
}
