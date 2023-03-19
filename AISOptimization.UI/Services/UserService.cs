using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AISOptimization.Domain;
using AISOptimization.VMs;

using Mapster;

using Microsoft.EntityFrameworkCore;


namespace AISOptimization.Services;

/// <summary>
/// Класс для взаимодействия с текущим пользователем
/// </summary>
public class UserService
{
    private readonly Func<AisOptimizationContext> _contextCreator;

    public UserService(Func<AisOptimizationContext> contextCreator)
    {
        _contextCreator = contextCreator;
    }
    public UserVM User { get; set; }

    
    public async Task<UserVM> Get(long id)
    {
        await using var context = _contextCreator();

        var problem = await context.Users.AsNoTracking()
                                    .Include(x=>x.Role)
                                    .SingleOrDefaultAsync(x=>x.Id == id);
        return problem.Adapt<UserVM>();
    }
    public async Task<UserVM> GetByName(string userName)
    {
        await using var context = _contextCreator();

        var problem = await context.Users.AsNoTracking()
                                   .Include(x=>x.Role)
                                   .SingleOrDefaultAsync(x=>x.UserName == userName);
        return problem.Adapt<UserVM>();
    }
    public async Task<List<UserVM>> GetAll( )
    {
        await using var context = _contextCreator();

        var problems = context.Users.AsNoTracking()
                              .Include(x => x.Role);
       
        return problems.Adapt<List<UserVM>>();

    }

    public async Task<long> Create(UserVM vm)
    {
        await using var context = _contextCreator();
        var user = vm.Adapt<User>();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user.Id;
    }

    public async Task Update(UserVM vm)
    {
        await using var context = _contextCreator();
        var user = await context.Users
                                   .SingleOrDefaultAsync(x=>x.Id == vm.Id);
        vm.Adapt(user);
        await context.SaveChangesAsync();
    }

    public async Task Delete(long id)
    {
        await using var context = _contextCreator();
        var user = new User() {Id = id};
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

}
