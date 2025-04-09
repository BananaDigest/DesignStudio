using DesignStudio.DAL.Models;

namespace DesignStudio.BLL.Interfaces
{
    public interface IOrderFactory
    {
        Order CreateTurnkeyOrder(string customer, string phone, string requirement, string description);
        Order CreateServiceOrder(string customer, string phone, DesignService service);
    }
}

