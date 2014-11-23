namespace Vanderkorn.ER
{
    using System;
    using System.Reflection;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Infrastructure;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Owin;

    public partial class Startup
    {
        public void ConfigureSignalR(IAppBuilder app)
        {  
            
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
          
        }
    }

    public class SignalRContractResolver : IContractResolver
    {
        private readonly Assembly _assembly;
        private readonly IContractResolver _camelCaseContractResolver;
        private readonly IContractResolver _defaultContractSerializer;

        public SignalRContractResolver()
        {
            this._defaultContractSerializer = new DefaultContractResolver();
            this._camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
            this._assembly = typeof(Connection).Assembly;
        }

        #region IContractResolver Members

        public JsonContract ResolveContract(Type type)
        {
            if (type.Assembly.Equals(this._assembly))
                return this._defaultContractSerializer.ResolveContract(type);

            return this._camelCaseContractResolver.ResolveContract(type);
        }

        #endregion
    }
}
