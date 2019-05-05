using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aspnetcore_assignment.Models;
using aspnetcore_assignment.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace aspnetcore_assignment.Hubs
{
    public class NotificationsHub : Hub, INotificationsHub
    {
        private IHubContext<NotificationsHub> _context;
        private readonly ILogger<NotificationsHub> _logger;
        public NotificationsHub(IHubContext<NotificationsHub> context, ILogger<NotificationsHub> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// when connection is started, add user into HashSet which contains connected user IDs
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync() {
            string email = Context.User.Identity.Name; // email could be get from User.Claims as well
            string connectionId = Context.ConnectionId;

            var user = Util.Users.GetOrAdd(email, _ => new User() {
                Email = email,
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds) {
                user.ConnectionIds.Add(connectionId);
            }

            return base.OnConnectedAsync();
        }

        /// <summary>
        /// when user is disconnected, remove user from HashSet
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception ex)
        {
            string email = Context.User.Identity.Name; // email could be get from User.Claims as well
            string connectionId = Context.ConnectionId;

            User user;
            Util.Users.TryGetValue(email, out user);
            
            if (user != null) {
                lock (user.ConnectionIds) {
                    user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));
                    if (!user.ConnectionIds.Any()) {
                        User removedUser;
                        Util.Users.TryRemove(email, out removedUser);
                    }
                }
            }

            return base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// Notify about new board to all subscribers
        /// </summary>
        /// <param name="board"></param>
        /// <returns>Dispatch <see cref="Board"/> notification </returns>
        public async Task BoardNotification(Board board)
        {
            try
            {
                // send new/updated board to all subscribers
                await _context.Clients.All.SendAsync("Push Board Notification", board);
            }
            catch (Exception exception)
            {
                _logger.LogError("Pushing board notification is failed", exception);
            }
        }

        /// <summary>
        /// Notify about new todo to all subscribers
        /// </summary>
        /// <param name="todo"></param>
        /// <returns>Dispatch <see cref="Todo"/> notification</returns>
        public async Task TodoNotification(Todo todo)
        {
            try
            {
                //TODO Only send notification to users whome todo is assigned
                // ...

                /// Send new/updated todo to all subscribers
                await _context.Clients.All.SendAsync("Push Todo Notification", todo);
            }
            catch (Exception exception)
            {
                _logger.LogError("Pushing Todo notification is failed", exception);
            }
        }
    }

    public interface INotificationsHub{
        Task BoardNotification(Board board);
        Task TodoNotification(Todo todo);
    }
}