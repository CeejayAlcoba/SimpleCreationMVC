namespace SimpleCreation.Models
{
    public enum FolderNames
    {
        Models,
        TableValuedParameters,
        ProcedureQueries,
        ProcedureEnums,
        Repositories,
        Classes,
        Interfaces,
        Services,
        Controllers,
        ApplicationContexts,
        JsClasses,
        TsTypes,
        Utilities
    }

    public static class FolderPaths
    {
        public static string RepositoriesFolder => $"{FolderNames.Repositories}";
        public static string RepositoriesClassesFolder => $"{FolderNames.Repositories}/{FolderNames.Classes}";
        public static string RepositoriesInterfacesFolder => $"{FolderNames.Repositories}/{FolderNames.Interfaces}";
        public static string ServicesFolder => $"{FolderNames.Services}";
        public static string ServicesClassesFolder => $"{FolderNames.Services}/{FolderNames.Classes}";
        public static string ServicesInterfacesFolder => $"{FolderNames.Services}/{FolderNames.Interfaces}";
        public static string UtilitiesClassesFolder => $"{FolderNames.Utilities}/{FolderNames.Classes}";
        public static string UtilitiesInterfacesFolder => $"{FolderNames.Utilities}/{FolderNames.Interfaces}";
        public static string UtilitiesFolder => FolderNames.Utilities.ToString();
        public static string ModelsFolder => FolderNames.Models.ToString();
        public static string ControllersFolder => FolderNames.Controllers.ToString();
        public static string JsClassesFolder => FolderNames.JsClasses.ToString();
        public static string TsTypesFolder => FolderNames.TsTypes.ToString();
        public static string ApplicationContextsFolder => FolderNames.ApplicationContexts.ToString();
        public static string ProcedureQueriesFolder => FolderNames.ProcedureQueries.ToString();
        public static string ProcedureEnumsFolder => FolderNames.ProcedureEnums.ToString();
        public static string TableValuedParametersFolder => FolderNames.TableValuedParameters.ToString();
    }
}
