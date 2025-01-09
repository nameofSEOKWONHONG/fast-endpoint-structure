using Feature.Account.Entities;
using Feature.Domain.Member.Abstract;
using Feature.Domain.Member.Dtos;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Riok.Mapperly.Abstractions;

namespace Feature.Account.Member.Repositories;


public class MemberRepository : RepositoryBase<MemberRepository, AppDbContext>, IMemberRepository
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="dbContext"></param>
    public MemberRepository(ILogger<MemberRepository> logger, ISessionContext sessionContext, AppDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }

    public Task<UserDto> GetMember(string id)
    {
        return this.DbContext.Users
                .Where(u => u.Id == id)
                .Select(m => new UserDto()
                {
                    Id = m.Id,
                    UserName = m.UserName,
                    Email = m.Email,
                    NormalizedEmail = m.NormalizedEmail,
                    NormalizedUserName = m.NormalizedUserName,
                })
            .FirstAsync();  
    }

    public Task<UserDto> GetMemberByEmail(string email)
    {
        return this.DbContext.Users
            .Where(u => u.Email == email)
            .Select(m => new UserDto()
            {
                Id = m.Id,
                UserName = m.UserName,
                Email = m.Email,
                NormalizedEmail = m.NormalizedEmail,
                NormalizedUserName = m.NormalizedUserName,
            })
            .FirstAsync();
    }

    public Task<List<UserDto>> GetMembers()
    {
        return this.DbContext.Users.Select(m => new UserDto()
        {
            Id = m.Id,
            UserName = m.UserName,
            Email = m.Email,
            NormalizedEmail = m.NormalizedEmail,
            NormalizedUserName = m.NormalizedUserName,
        }).ToListAsync();
    }

    public Task<List<UserDto>> GetMembers(string[] ids)
    {
        return this.DbContext.Users
            .Where(m => ids.Contains(m.Id))
            .Select(m => new UserDto()
        {
            Id = m.Id,
            UserName = m.UserName,
            Email = m.Email,
            NormalizedEmail = m.NormalizedEmail,
            NormalizedUserName = m.NormalizedUserName,
        }).ToListAsync();
    }
}

[Mapper]
public static partial class UserMapper
{
    public static partial UserDto UserToUserDto(this User user);
    public static partial User UserDtoToUser(this UserDto dto);
}