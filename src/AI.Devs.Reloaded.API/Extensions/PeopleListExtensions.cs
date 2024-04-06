using AI.Devs.Reloaded.API.Models;
using AI.Devs.Reloaded.API.Models.OpenAi;

namespace AI.Devs.Reloaded.API.Extensions;

public static class PeopleListExtensions
{
    public static PeopleModel FindByFirstAndLastname(this List<PeopleModel> list, PeopleResponse response)
    {
        var person = list.Single(x => 
            x.Firstname.Equals(response.Firstname, StringComparison.OrdinalIgnoreCase) &&
            x.Lastname.Equals(response.Lastname, StringComparison.OrdinalIgnoreCase)
            );

        return person;
    }
}
