namespace gmoralesExamen.Views;

public partial class vLogin : ContentPage
{
	// Matriz 4 x 2: usuario, contraseña
	string[,] usuarios =
	{
		{ "recepcion", "gym2026" },
		{ "entrenador", "fit_01" },
		{ "uisrael", "2026" },
		{ "gerente", "admin123" }
	};

	public vLogin()
	{
		InitializeComponent();
	}

    private async void btnIniciarSesion_Clicked(object sender, EventArgs e)
    {
		try
		{
			string usuarioIngreso = txtUsuario.Text ?? "";
			string contrasenaIngreso = txtContrasena.Text ?? "";
			bool acceso = false;

			for (int i = 0; i < usuarios.GetLength(0); i++)
			{
				if (usuarioIngreso == usuarios[i, 0] && contrasenaIngreso == usuarios[i, 1])
				{
					acceso = true;
					break;
				}
			}

			if (acceso)
			{
				await Navigation.PushAsync(new vRegistro(usuarioIngreso));
			}
			else
			{
				await DisplayAlertAsync("Alerta", "Dato incorrecto: Usuario/Contraseña", "Cerrar");
			}
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("Error", ex.Message, "Cerrar");
		}
    }

    private async void btnAcercaDe_Clicked(object sender, EventArgs e)
    {
		await DisplayAlertAsync("Acerca de",
			"Nombre: Gabriel Alexander Morales" + Environment.NewLine +
			"Carrera: Ingeniería Sistemas de la Información" + Environment.NewLine +
			"Paralelo: 8vo A",
			"Cerrar");
    }
}
