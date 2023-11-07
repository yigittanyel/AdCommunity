namespace AdCommunity.Application.DTOs.UserCommunity;

public class UserCommunityBaseDto
{
    public int UserId { get; set; }
    public int CommunityId { get; set; }
    public DateTime? JoinDate { get; set; }

    public UserCommunityBaseDto(int userId, int communityId, DateTime? joinDate)
    {
        UserId = userId;
        CommunityId = communityId;
        JoinDate = joinDate;
    }
    public UserCommunityBaseDto()
    {
    }
}

public class UserCommunityDto : UserCommunityBaseDto
{
    public int Id { get; set; }

    public UserCommunityDto(int id)
    {
        Id = id;
    }
}

public class UserCommunityCreateDto : UserCommunityBaseDto
{
    public UserCommunityCreateDto()
    {
        
    }
}

public class UserCommunityUpdateDto : UserCommunityBaseDto
{
    public int Id { get; set; }

    public UserCommunityUpdateDto(int id)
    {
        Id = id;
    }
    public UserCommunityUpdateDto()
    {
        
    }
}

