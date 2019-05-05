using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace aspnetcore_assignment.Models {
    /// <summary>
    /// Todo Model
    /// </summary>
    public class Todo {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:aspnetcore_assignment.Models.Todo"/> is done.
        /// </summary>
        /// <value><c>true</c> if done; otherwise, <c>false</c>.</value>
        public bool Done { get; set; } = false;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the todos creation time
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the time the todo was updated 
        /// </summary>
        /// <value>The update.</value>
        public DateTime Update { get; set; }

        /// <summary>
        /// Gets or sets due date of a todo
        /// </summary>
        /// <value>The due date</value>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the board identifier.
        /// </summary>
        /// <value>The board identifier.</value>
        [Required]
        public Guid BoardId { get; set; }

        /// <summary>
        /// Gets or sets the board.
        /// </summary>
        /// <value>The board.</value>
        public Board Board { get; set; }

        /// <summary>
        /// The user todos containing both user and it's related todos
        /// </summary>
        /// <value>The UserTodos.</value>
        public IEnumerable<UserTodo> UserTodos { get; set; }
    }
}
