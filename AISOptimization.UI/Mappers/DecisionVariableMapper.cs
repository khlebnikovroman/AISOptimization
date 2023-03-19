// using AISOptimization.Domain.Parameters;
// using AISOptimization.VMs;
//
//
// namespace AISOptimization.Mappers;
//
// public static class DecisionVariableMapper
// {
//     public static void ToVM(this DecisionVariable source, DecisionVariableVM destination)
//     {
//         destination.Id = source.Id;
//         destination.Key = source.Key;
//         destination.Description = source.Description;
//         source.FirstRoundConstraint.ToVM(destination.FirstRoundConstraint);
//     }
//
//     public static DecisionVariableVM ToVM(this DecisionVariable source)
//     {
//         var vm = new DecisionVariableVM();
//         source.ToVM(vm);
//
//         return vm;
//     }
//
//     public static void ToEntity(this DecisionVariableVM source, DecisionVariable destination)
//     {
//         destination.Id = source.Id;
//         destination.Key = source.Key;
//         destination.Description = source.Description;
//         destination.Value = destination.Value;
//         source.FirstRoundConstraint.ToEntity(destination.FirstRoundConstraint);
//     }
//     
//     public static DecisionVariable ToEntity(this DecisionVariableVM source)
//     {
//         var entity = new DecisionVariable();
//         source.ToEntity(entity);
//         return entity;
//     }
// }
