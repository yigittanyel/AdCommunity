using System;

namespace AdCommunity.Core.CustomMapper;

public interface IYtMapper
{
    TDestination Map<TSource, TDestination>(TSource source);
    List<TDestination> MapList<TSource, TDestination>(List<TSource> sourceList);
}

// EXAMPLE USAGE

//public class SampleController : ControllerBase
//{
//    private readonly IYtMapper _ytMapper;

//    public SampleController(IYtMapper ytMapper)
//    {
//        _ytMapper = ytMapper;
//    }

//    public IActionResult MapPersonToPersonDTO()
//    {
//        Person person = new Person { FirstName = "John", LastName = "Doe" };
//        PersonDTO personDto = _ytMapper.Map<Person, PersonDTO>(person);

//        return Ok(personDto);
//    }
//}