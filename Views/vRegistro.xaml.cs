using System.Globalization;

namespace gmoralesExamen.Views;

public partial class vRegistro : ContentPage
{
	const double costoMembresia = 1200;
	const int numeroCuotas = 5;

	string usuario;
	double pagoMensual, pagoTotal;

	public vRegistro(string usuarioConectado)
	{
		InitializeComponent();
		usuario = usuarioConectado;
		lblUsuario.Text = "Usuario conectado: " + usuario;

		// No permitir fechas pasadas
		dpFechaInicio.MinimumDate = DateTime.Today;
		dpFechaInicio.Date = DateTime.Today;
	}

	// Valida los datos y calcula el pago mensual; muestra la alerta y retorna false si algo esta mal
	private async Task<bool> Calcular()
	{
		string nombre = txtNombre.Text?.Trim() ?? "";
		string apellido = txtApellido.Text?.Trim() ?? "";
		string edadTexto = txtEdad.Text?.Trim() ?? "";
		string montoTexto = txtMontoInicial.Text?.Trim() ?? "";

		if (nombre == "" || apellido == "")
		{
			await DisplayAlertAsync("Alerta", "Ingrese el nombre y el apellido", "Cerrar");
			return false;
		}

		if (!edadTexto.All(char.IsDigit) || !int.TryParse(edadTexto, out int edad) || edad < 16)
		{
			await DisplayAlertAsync("Alerta", "La edad debe ser solo números y mayor o igual a 16", "Cerrar");
			return false;
		}

		if (pkPlan.SelectedIndex == -1)
		{
			await DisplayAlertAsync("Alerta", "Seleccione un plan", "Cerrar");
			return false;
		}

		if (!double.TryParse(montoTexto, NumberStyles.Number, CultureInfo.InvariantCulture, out double montoInicial) || montoInicial < 120 || montoInicial > 600)
		{
			await DisplayAlertAsync("Alerta", "El monto inicial debe estar entre 120 y 600", "Cerrar");
			return false;
		}

		// (Costo - inicial) / 5 + 2% del costo + 10 si tiene casillero
		double saldo = costoMembresia - montoInicial;
		pagoMensual = saldo / numeroCuotas + costoMembresia * 0.02;
		if (swCasillero.IsToggled)
		{
			pagoMensual += 10;
		}
		pagoTotal = montoInicial + pagoMensual * numeroCuotas;

		txtPagoMensual.Text = pagoMensual.ToString("0.00");
		return true;
	}

	private void LimpiarCampos()
	{
		txtNombre.Text = "";
		txtApellido.Text = "";
		txtEdad.Text = "";
		dpFechaInicio.Date = DateTime.Today;
		pkPlan.SelectedIndex = -1;
		swCasillero.IsToggled = false;
		txtMontoInicial.Text = "";
		txtPagoMensual.Text = "";
	}

	// Edad: solo permite numeros
	private void txtEdad_TextChanged(object sender, TextChangedEventArgs e)
	{
		string texto = e.NewTextValue ?? "";
		if (!texto.All(char.IsDigit))
		{
			txtEdad.Text = e.OldTextValue;
		}
	}

	// Edad: al salir del campo valida que sea mayor o igual a 16
	private async void txtEdad_Unfocused(object sender, FocusEventArgs e)
	{
		string texto = txtEdad.Text ?? "";
		if (texto != "" && int.Parse(texto) < 16)
		{
			await DisplayAlertAsync("Alerta", "La edad debe ser mayor o igual a 16", "Cerrar");
			txtEdad.Text = "";
		}
	}

	// Monto: solo permite numeros y un punto decimal
	private void txtMontoInicial_TextChanged(object sender, TextChangedEventArgs e)
	{
		string texto = e.NewTextValue ?? "";
		if (!texto.All(c => char.IsDigit(c) || c == '.') || texto.Count(c => c == '.') > 1)
		{
			txtMontoInicial.Text = e.OldTextValue;
			return;
		}

		// Si cambia el monto, el pago mensual calculado ya no es valido
		txtPagoMensual.Text = "";
	}

	// Monto: al salir del campo valida que este entre 120 y 600
	private async void txtMontoInicial_Unfocused(object sender, FocusEventArgs e)
	{
		string texto = txtMontoInicial.Text ?? "";
		if (texto == "")
		{
			return;
		}

		if (!double.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out double monto) || monto < 120 || monto > 600)
		{
			await DisplayAlertAsync("Alerta", "El monto inicial debe estar entre 120 y 600", "Cerrar");
			txtMontoInicial.Text = "";
		}
	}

    private async void btnCalcular_Clicked(object sender, EventArgs e)
    {
		try
		{
			await Calcular();
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("Error", ex.Message, "Cerrar");
		}
    }

    private async void btnVerResumen_Clicked(object sender, EventArgs e)
    {
		try
		{
			if (!await Calcular())
			{
				return;
			}

			await Navigation.PushAsync(new vResumen(
				usuario,
				txtNombre.Text.Trim(),
				txtApellido.Text.Trim(),
				int.Parse(txtEdad.Text.Trim()),
				dpFechaInicio.Date ?? DateTime.Today,
				pkPlan.SelectedItem.ToString()!,
				swCasillero.IsToggled,
				double.Parse(txtMontoInicial.Text.Trim(), CultureInfo.InvariantCulture),
				pagoMensual,
				pagoTotal));

			// Al regresar desde Resumen (Nuevo registro) los campos quedan limpios
			LimpiarCampos();
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("Error", ex.Message, "Cerrar");
		}
    }
}
