namespace SimpleCreationMVC.Services.GenericServices
{
    public class GenericMainService
    {
        private readonly GenericClassesService _genericClassesService;
        private readonly GenericInterfacesService _genericInterfacesService = new GenericInterfacesService();
        public GenericMainService(string connectionString) {

            _genericClassesService= new GenericClassesService(connectionString);
        }
        public void CreateStoredProcedure()
        {
            _genericInterfacesService.CreateStoredProcedure();
            _genericClassesService.CreateStoredProcedure();
        }
        public void CreateDapperQuery()
        {
            _genericInterfacesService.CreateDapperQuery();
            _genericClassesService.CreateDapperQuery();
        }
        public void CreateEFCore()
        {
            _genericInterfacesService.CreateEFCore();
            _genericClassesService.CreateEFCore();
            _genericClassesService.CreateEFCoreContext();
        }
    }
}
