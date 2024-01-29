namespace WhiteLagoon.Application;

public interface IVillaNumberRepository : IRepository<VillaNumber>
{
    public void Update(VillaNumber villaNumber);
}
