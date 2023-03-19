using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AISOptimization.Domain;
using AISOptimization.Domain.Migrations;
using AISOptimization.VMs;

using Mapster;

using Microsoft.EntityFrameworkCore;


namespace AISOptimization.Services;

public class OptimizationProblemService
{
    private readonly Func<AisOptimizationContext> _contextCreator;

    public OptimizationProblemService(Func<AisOptimizationContext> contextCreator)
    {
        _contextCreator = contextCreator;
    }

    public async Task<OptimizationProblemVM> Get(long id)
    {
        await using var context = _contextCreator();

        var problem = await context.OptimizationProblems.AsNoTracking()
                                    .Include(x=>x.Constants)
                                    .Include(x=>x.DecisionVariables)
                                    .ThenInclude(x=>x.FirstRoundConstraint)
                                    .Include(x=>x.ObjectiveFunction)
                                    .Include(x=>x.SecondRoundConstraints)
                                    .ThenInclude(x=>x.ConstraintFunction)
                                    .SingleOrDefaultAsync(x=>x.Id == id);
        return problem.Adapt<OptimizationProblemVM>();
    }

    public async Task<List<OptimizationProblemVM>> GetAll(long? userId = null )
    {
        await using var context = _contextCreator();

        var problems =  context.OptimizationProblems.AsNoTracking()
                                    .Include(x => x.Constants)
                                    .Include(x => x.DecisionVariables)
                                    .ThenInclude(x => x.FirstRoundConstraint)
                                    .Include(x => x.ObjectiveFunction)
                                    .Include(x => x.SecondRoundConstraints)
                                    .ThenInclude(x=>x.ConstraintFunction);
        if (userId is null)
        {
            return problems.Adapt<List<OptimizationProblemVM>>();
        }
        else
        {
            return problems.Where(x => x.UserId == userId).Adapt<List<OptimizationProblemVM>>();
        }
    }

    public async Task<long> Create(OptimizationProblemVM vm)
    {
        await using var context = _contextCreator();
        var problem = vm.Adapt<OptimizationProblem>();
        await context.AddAsync(problem);
        await context.SaveChangesAsync();
        return problem.Id;
    }

    public async Task Update(OptimizationProblemVM vm)
    {
        await using var context = _contextCreator();
        var problem = await context.OptimizationProblems
                                   .SingleOrDefaultAsync(x=>x.Id == vm.Id);
        vm.Adapt(problem);
        await context.SaveChangesAsync();
    }

    public async Task Delete(long id)
    {
        await using var context = _contextCreator();
        var problem = new OptimizationProblem() {Id = id};
        context.OptimizationProblems.Remove(problem);
        await context.SaveChangesAsync();
    }
}
