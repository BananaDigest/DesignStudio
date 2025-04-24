using DesignStudio.BLL.DTOs;
using DesignStudio.BLL.Interfaces;

namespace DesignStudio.UI.CommandPattern
{
    public class AddServiceCommand : ICommand
    {
        private readonly IDesignStudioService _facade;
        private readonly DesignServiceDto _dto;

        public AddServiceCommand(IDesignStudioService facade, DesignServiceDto dto)
        {
            _facade = facade;
            _dto = dto;
        }

        public void Execute()
        {
            // Викликаємо асинхронний метод синхронно
            _facade.AddServiceAsync(_dto).Wait();
        }
    }
}
