using static Application.Enums.Enums;

namespace Application.Constants;

public class Constants
{
    public static readonly Dictionary<ReferentialDataType, Guid> IdsReferencesData = new Dictionary<ReferentialDataType, Guid>
    {
        { ReferentialDataType.StateProyect, new Guid(" 10000000-0000-0000-0000-000000000001") },
        { ReferentialDataType.StateArea, new Guid("10000000-0000-0000-0000-000000000002") },
        { ReferentialDataType.PropertiesTask, new Guid("10000000-0000-0000-0000-000000000003" )},
    };
}
