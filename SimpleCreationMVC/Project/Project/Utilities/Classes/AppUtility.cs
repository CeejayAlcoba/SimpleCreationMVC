
using Utilities.Interfaces;

namespace Utilities.Classes
{
    public class AppUtility : IAppUtility
    { 
        public IConfigurationRoot GetConfiguration() {  
            return new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
        }
    }
}
