namespace AdCommunity.Application.DTOs.UserCommunity;

public class UserCommunityBaseDto
{

    public DateTime? JoinDate { get; set; }

    public UserCommunityBaseDto(DateTime? joinDate)
    {
        JoinDate = joinDate;
    }
    public UserCommunityBaseDto()
    {
    }
}

public class UserCommunityDto : UserCommunityBaseDto
{
    public int Id { get; set; }
    public Domain.Entities.Aggregates.User.User User { get; set; }
    public Domain.Entities.Aggregates.Community.Community Community { get; set; }

    public UserCommunityDto(int id)
    {
        Id = id;
    }

    public UserCommunityDto()
    {
        
    }
}

public class UserCommunityCreateDto : UserCommunityBaseDto
{
    public int UserId { get; set; }
    public int CommunityId { get; set; }
    public UserCommunityCreateDto()
    {
        
    }
}

public class UserCommunityUpdateDto : UserCommunityBaseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CommunityId { get; set; }
    public UserCommunityUpdateDto(int id)
    {
        Id = id;
    }
    public UserCommunityUpdateDto()
    {
        
    }
}

