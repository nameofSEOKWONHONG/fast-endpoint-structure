using Feature.Domain.Member.Dtos;

namespace Feature.Domain.Member.Abstract;

public interface IMemberRepository
{
    Task<UserDto> GetMember(string id);
    Task<UserDto> GetMemberByEmail(string email);
    Task<List<UserDto>> GetMembers();
    Task<List<UserDto>> GetMembers(string[] ids);
}
