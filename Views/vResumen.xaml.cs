namespace gmoralesExamen.Views;

public partial class vResumen : ContentPage
{
	string resumen;

	public vResumen(string usuario, string nombre, string apellido, int edad, DateTime fechaInicio,
		string plan, bool casillero, double montoInicial, double pagoMensual, double pagoTotal)
	{
		InitializeComponent();

		lblUsuario.Text = "Usuario conectado: " + usuario;
		lblNombre.Text = nombre;
		lblApellido.Text = apellido;
		lblEdad.Text = edad.ToString();
		lblFechaInicio.Text = fechaInicio.ToString("dd/MM/yyyy");
		lblPlan.Text = plan;
		lblCasillero.Text = casillero ? "Sí" : "No";
		lblMontoInicial.Text = montoInicial.ToString("0.00");
		lblPagoMensual.Text = pagoMensual.ToString("0.00");
		lblPagoTotal.Text = pagoTotal.ToString("0.00");

		resumen = "RESUMEN DEL SOCIO" + Environment.NewLine +
			"Usuario conectado: " + usuario + Environment.NewLine +
			"Nombre: " + lblNombre.Text + Environment.NewLine +
			"Apellido: " + lblApellido.Text + Environment.NewLine +
			"Edad: " + lblEdad.Text + Environment.NewLine +
			"Fecha de inicio: " + lblFechaInicio.Text + Environment.NewLine +
			"Plan: " + lblPlan.Text + Environment.NewLine +
			"Casillero: " + lblCasillero.Text + Environment.NewLine +
			"Monto Inicial: " + lblMontoInicial.Text + Environment.NewLine +
			"Pago mensual: " + lblPagoMensual.Text + Environment.NewLine +
			"Pago Total: " + lblPagoTotal.Text;
	}

    private async void btnGenerarTxt_Clicked(object sender, EventArgs e)
    {
		try
		{
			// Guarda el resumen en un archivo .txt dentro de la carpeta de datos de la app
			string nombreArchivo = "resumen_" + lblNombre.Text + "_" + lblApellido.Text + ".txt";
			string ruta = Path.Combine(FileSystem.AppDataDirectory, nombreArchivo);
			File.WriteAllText(ruta, resumen);

			// Presenta el contenido del archivo generado
			string contenido = File.ReadAllText(ruta);
			await DisplayAlertAsync("Archivo TXT generado", contenido + Environment.NewLine + Environment.NewLine + "Ruta: " + ruta, "Cerrar");
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("Error", ex.Message, "Cerrar");
		}
    }

    private void btnSalir_Clicked(object sender, EventArgs e)
    {
		// Vuelve al Login y elimina el historial de navegacion
		Application.Current!.Windows[0].Page = new NavigationPage(new vLogin());
    }
}
