namespace OOTPnSP.Laba1;

internal class Program
{
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        Figure figure = new Square(100, 100, 200);
        app.Run(async (context) =>
        {
            await context.Response.WriteAsync($"{(figure as Square).InscribedCircleRadius()}");
        });

        app.Run();
    }
}