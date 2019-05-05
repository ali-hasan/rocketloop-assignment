using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspnetcore_assignment.Models
{
    /// <summary>
    /// Profile Model
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The Identifier</value>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets first name of user's profile
        /// </summary>
        /// <value>The first name</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of user's profile
        /// </summary>
        /// <value>The last name</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or set email of user's profile
        /// </summary>
        /// <value>The email</value>
        public string Email { get; set; }

        /// <summary>
        /// The user todos containing both user and it's related todos
        /// </summary>
        /// <value>The UserTodos.</value>
        public IEnumerable<UserTodo> UserTodos { get; set; }

        /// <summary>
        /// Stores connection IDs of user e.g all devices user is connected
        /// </summary>
        /// <value>A Hashset</value>
        [NotMapped]
        public HashSet<string> ConnectionIds { get; set; }

    }
}