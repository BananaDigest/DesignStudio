using DesignStudio.BLL.Facades;
using DesignStudio.DAL.Models;


namespace DesignStudio.UI.CommandPattern
{
    public class AddServiceCommand : ICommand
    {
        private readonly DesignStudioService _facade;
        private readonly DesignService _designService;

        public AddServiceCommand(DesignStudioService facade, DesignService designService)
        {
            _facade = facade;
            _designService = designService;
        }

        public void Execute()
        {
            _facade.AddService(_designService);
        }
    }
}