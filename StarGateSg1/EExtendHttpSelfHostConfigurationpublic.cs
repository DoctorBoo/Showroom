using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using System.Web.Http.SelfHost.Channels;

namespace StarGateSg1
{
    public class HttpSelfHostWindowsAuthenticationConfiguration : HttpSelfHostConfiguration
    {
        public HttpSelfHostWindowsAuthenticationConfiguration(string baseAddress)
            : base(baseAddress)
        {
        }

        protected override BindingParameterCollection OnConfigureBinding(HttpBinding httpBinding)
        {
            //httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            //httpBinding.Security.Mode = HttpBindingSecurityMode.Transport;
            // Windows authentication:
            httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;

            httpBinding.Security.Mode = HttpBindingSecurityMode.TransportCredentialOnly;
 
            return base.OnConfigureBinding(httpBinding);
        }
    }
    internal class MyHttpsSelfHostConfiguration : HttpSelfHostConfiguration
    {
        public MyHttpsSelfHostConfiguration(string baseAddress) : base(baseAddress) { }
        public MyHttpsSelfHostConfiguration(Uri baseAddress) : base(baseAddress) { }
        protected override BindingParameterCollection OnConfigureBinding(HttpBinding httpBinding)
        {
            httpBinding.Security.Mode = HttpBindingSecurityMode.Transport;
            return base.OnConfigureBinding(httpBinding);
        }
    }
}
