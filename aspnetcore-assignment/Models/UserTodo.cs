using System;
using System.ComponentModel.DataAnnotations;

namespace aspnetcore_assignment.Models
{
    public class UserTodo
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        /// <value>The user identifier</value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user
        /// </summary>
        /// <value>The user</value>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets todo identifier
        /// </summary>
        /// <value>The todo identifier</value>
        public Guid TodoId { get; set; }

        /// <summary>
        /// Gets or sets Todo
        /// </summary>
        /// <value>The todo</value>
        public Todo Todo { get; set; }
    }
}