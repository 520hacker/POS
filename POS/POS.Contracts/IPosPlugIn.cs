using POS.Contracts.Architecture;

namespace POS.Contracts
{
    public interface IPosPlugIn : IPlugIn
    {
        void Load();
    }
}