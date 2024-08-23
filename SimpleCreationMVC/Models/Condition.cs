namespace SimpleCreation.Models
{
    public class Condition
    {
        public bool? isServiceFileAllowed { get; set; } = true;
        public bool? isControllerAllowed { get; set; } = true;
        public bool? isGetAllProcedureAllowed { get; set; } = true;
        public bool? isInsertProcedureAllowed { get; set; } = true;
        public bool? isUpdateProcedureAllowed { get; set; } = true;
        public bool? isGetByIdProcedureAllowed { get; set; } = true;
        public bool? isHardDeleteByIdProcedureAllowed { get; set; } = true;
    }
}
