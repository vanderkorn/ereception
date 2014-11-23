namespace Vanderkorn.ER.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;

    using Vanderkorn.ER.Models;

    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections =
            new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return this._connections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (this._connections)
            {
                HashSet<string> connections;
                if (!this._connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    this._connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (this._connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public void Remove(T key, string connectionId)
        {
            lock (this._connections)
            {
                HashSet<string> connections;
                if (!this._connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        this._connections.Remove(key);
                    }
                }
            }
        }
    }
    public class ReceptionHub : Hub
    {
        public readonly static ConnectionMapping<string> _connections =
               new ConnectionMapping<string>();

        public override Task OnConnected()
        {
            using (var db = new ApplicationDbContext())
            {
                var userId = this.Context.User.Identity.GetUserId();
                // Retrieve user.
                var reception = db.Receptions.SingleOrDefault(u => u.MinisterId == userId);
                if (reception == null)
                {
                    var user = db.Users.Find(userId);
                    reception = db.Receptions.SingleOrDefault(u => u.Id == user.ReceptionId);
                }

                if (reception != null)
                {
                    this.Groups.Add(this.Context.ConnectionId, reception.Id.ToString());
                }
            }
            string name = this.Context.User.Identity.Name;

            _connections.Add(name, this.Context.ConnectionId);
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            string name = this.Context.User.Identity.Name;

            _connections.Remove(name, this.Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = this.Context.User.Identity.Name;

            if (!_connections.GetConnections(name).Contains(this.Context.ConnectionId))
            {
                _connections.Add(name, this.Context.ConnectionId);
            }

            return base.OnReconnected();
        }
        public Task JoinReceptio(string roomName)
        {
            return this.Groups.Add(this.Context.ConnectionId, roomName);
        }

        public Task LeaveReceptio(string roomName)
        {
            return this.Groups.Remove(this.Context.ConnectionId, roomName);
        }

        public Task updateAppeal(string roomName, Appeal appeal)
        {
            return this.Clients.OthersInGroup(roomName).updateAppeal(appeal);
        }
    }
}