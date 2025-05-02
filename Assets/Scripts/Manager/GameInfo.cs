public enum TypeRegion
{
    NONE,
    EGEE,
    STYX,
    OLYMPUS,
    ETNAS,
    ARCADIE,
    COUNT
}


public static class GameInfo
{
    private static TypeRegion currentRegion;

    public static void SetCurrentRegion( TypeRegion newRegion) => currentRegion = newRegion;

    public static TypeRegion GetCurrentRegion() => currentRegion;
}
