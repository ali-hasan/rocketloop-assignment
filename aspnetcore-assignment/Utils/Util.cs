using System.Collections.Concurrent;
using System.Collections.Generic;
using aspnetcore_assignment.Models;

namespace aspnetcore_assignment.Utils
{
    public static class Util
    {
        public static readonly ConcurrentDictionary<string, User> Users 
        = new ConcurrentDictionary<string, User>();
    }
}