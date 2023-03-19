// using AISOptimization.Domain.Constraints;
// using AISOptimization.VMs;
//
//
// namespace AISOptimization.Mappers;
//
// public static class FirstRoundConstraintMapper
// {
//     public static void ToVM(this FirstRoundConstraint source, FirstRoundConstraintVM destination)
//     {
//         destination.Id = source.Id;
//         destination.Max = source.Max;
//         destination.Min = source.Min;
//         destination.LessSign = source.LessSign;
//         destination.BiggerSign = source.BiggerSign;
//     }
//
//     public static FirstRoundConstraintVM ToVM(this FirstRoundConstraint source)
//     {
//         var vm = new FirstRoundConstraintVM();
//         source.ToVM(vm);
//
//         return vm;
//     }
//
//     public static void ToEntity(this FirstRoundConstraintVM source, FirstRoundConstraint destination)
//     {
//         destination.Id = source.Id;
//         destination.Max = source.Max;
//         destination.Min = source.Min;
//         destination.LessSign = source.LessSign;
//         destination.BiggerSign = source.BiggerSign;
//     }
//     
//     public static FirstRoundConstraint ToEntity(this FirstRoundConstraintVM source)
//     {
//         var entity = new FirstRoundConstraint();
//         source.ToEntity(entity);
//
//         return entity;
//     }
// }
