// using AISOptimization.Domain;
// using AISOptimization.VMs;
//
//
// namespace AISOptimization.Mappers;
//
// public static class OptimizationProblemMapper
// {
//     public static void ToVM(this OptimizationProblem source, OptimizationProblemVM destination)
//     {
//         destination.Id = source.Id;
//
//         foreach (var constantVm in destination.Constants)
//         {
//             
//         }
//     }
//
//     public static OptimizationProblemVM ToVM(this OptimizationProblem source)
//     {
//         var vm = new OptimizationProblemVM();
//         source.ToVM(vm);
//
//         return vm;
//     }
//
//     public static void ToEntity(this OptimizationProblemVM source, OptimizationProblem destination)
//     {
//         destination.Id = source.Id;
//         destination.Key = source.Key;
//         destination.Description = source.Description;
//         destination.Value = destination.Value;
//     }
//     
//     public static OptimizationProblem ToEntity(this OptimizationProblemVM source)
//     {
//         var entity = new OptimizationProblem();
//         source.ToEntity(entity);
//
//         return entity;
//     }
// }
