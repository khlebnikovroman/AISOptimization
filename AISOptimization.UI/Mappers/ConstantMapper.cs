// using AISOptimization.Domain.Parameters;
// using AISOptimization.VMs;
//
//
// namespace AISOptimization.Mappers;
//
// public static class ConstantMapper
// {
//     public static void ToVM(this Constant source, ConstantVM destination)
//     {
//         destination.Id = source.Id;
//         destination.Key = source.Key;
//         destination.Description = source.Description;
//         destination.Value = destination.Value;
//     }
//
//     public static ConstantVM ToVM(this Constant source)
//     {
//         var vm = new ConstantVM();
//         source.ToVM(vm);
//
//         return vm;
//     }
//
//     public static void ToEntity(this ConstantVM source, Constant destination)
//     {
//         destination.Id = source.Id;
//         destination.Key = source.Key;
//         destination.Description = source.Description;
//         destination.Value = destination.Value;
//     }
//     
//     public static Constant ToEntity(this ConstantVM source)
//     {
//         var entity = new Constant();
//         source.ToEntity(entity);
//
//         return entity;
//     }
// }
