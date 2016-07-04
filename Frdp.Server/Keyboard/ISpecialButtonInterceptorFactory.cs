namespace Frdp.Server.Keyboard
{
    public interface ISpecialButtonInterceptorFactory
    {
        ISpecialButtonInterceptor CreateInterceptor(
            //MainViewModel viewModel
            );
    }
}