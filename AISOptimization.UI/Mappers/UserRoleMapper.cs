// using AISOptimization.Domain;
// using AISOptimization.VMs;
//
//
// namespace AISOptimization.Mappers;
//
// public static class UserRoleMapper
// {
//     public static void ToVM(this UserRole source, UserRoleVM destination)
//     {
//         destination.Id = source.Id;
//         destination.RoleType = source.RoleType;
//     }
//
//     public static UserRoleVM ToVM(this UserRole source)
//     {
//         var vm = new UserRoleVM();
//         source.ToVM(vm);
//
//         return vm;
//     }
//
//     public static void ToEntity(this UserRoleVM source, UserRole destination)
//     {
//         destination.Id = source.Id;
//         destination.RoleType = source.RoleType;
//     }
//     
//     public static UserRole ToEntity(this UserRoleVM source)
//     {
//         var entity = new UserRole();
//         source.ToEntity(entity);
//
//         return entity;
//     }
// }
