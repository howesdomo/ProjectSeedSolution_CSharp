using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Security.Client.Common
{

    public class SecurityServiceLink : Security.Client.SecurityServer.SecurityServiceSoapClient        
    {
        public SecurityServiceLink()
            : base()
        {
            var uri = string.Format("http://{0}:{1}/{2}", Config.GetServerIP(), Config.GetServerPort(), Config.GetServerApplication());
            base.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(uri) { });
        }

        public SecurityServiceLink(string ip, string port, string app)
            : base()
        {
            var uri = string.Format("http://{0}:{1}/{2}", ip, port, app);
            base.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(uri) { });
        }
    }
}
