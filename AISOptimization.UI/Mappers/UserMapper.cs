// using AISOptimization.Domain;
// using AISOptimization.VMs;
//
//
// namespace AISOptimization.Mappers;
//
// public static class UserMapper
// {
//     public static void ToVM(this User source, UserVM destination)
//     {
//         destination.Id = source.Id;
//         destination.Password = source.Password;
//         destination.UserName = destination.UserName;
//     }
//
//     public static UserVM ToVM(this User source)
//     {
//         var vm = new UserVM();
//         source.ToVM(vm);
//
//         return vm;
//     }
//
//     public static void ToEntity(this UserVM source, User destination)
//     {
//         destination.Id = source.Id;
//         //destination.RoleType = source.RoleType;
//     }
//     
//     public static User ToEntity(this UserVM source)
//     {
//         var entity = new User();
//         source.ToEntity(entity);
//
//         return entity;
//     }
// }
