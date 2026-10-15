using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Entidades;
using Negocio;

namespace laboratorio7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NPersona nPersona = new NPersona();
        private int idSeleccionado = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarPersonas();
        }

        private void CargarPersonas(string? filtro = null)
        {
            try
            {
                DgPersonas.ItemsSource = nPersona.Buscar(filtro);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            CargarPersonas(TxtBuscar.Text);
        }

        private void TxtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CargarPersonas(TxtBuscar.Text);
        }

        private void BtnMostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            TxtBuscar.Clear();
            CargarPersonas();
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nPersona.Registrar(LeerFormulario());
                MessageBox.Show("Persona registrada correctamente.", "Guardar",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                LimpiarFormulario();
                CargarPersonas(TxtBuscar.Text);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Persona persona = LeerFormulario();
                persona.IdPersona = idSeleccionado;

                nPersona.Actualizar(persona);
                MessageBox.Show("Persona actualizada correctamente.", "Actualizar",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                LimpiarFormulario();
                CargarPersonas(TxtBuscar.Text);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var respuesta = MessageBox.Show(
                $"¿Desea eliminar a {TxtNombres.Text} {TxtApellidos.Text}?",
                "Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (respuesta != MessageBoxResult.Yes)
                return;

            try
            {
                nPersona.Eliminar(idSeleccionado);
                MessageBox.Show("Persona eliminada correctamente.", "Eliminar",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                LimpiarFormulario();
                CargarPersonas(TxtBuscar.Text);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void DgPersonas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgPersonas.SelectedItem is not Persona persona)
                return;

            idSeleccionado = persona.IdPersona;
            TxtNombres.Text = persona.Nombres;
            TxtApellidos.Text = persona.Apellidos;
            TxtDni.Text = persona.DNI;
            TxtEmail.Text = persona.Email;

            BtnGuardar.IsEnabled = false;
            BtnActualizar.IsEnabled = true;
            BtnEliminar.IsEnabled = true;
        }

        private Persona LeerFormulario()
        {
            return new Persona
            {
                Nombres = TxtNombres.Text,
                Apellidos = TxtApellidos.Text,
                DNI = TxtDni.Text,
                Email = TxtEmail.Text
            };
        }

        private void LimpiarFormulario()
        {
            idSeleccionado = 0;
            TxtNombres.Clear();
            TxtApellidos.Clear();
            TxtDni.Clear();
            TxtEmail.Clear();
            DgPersonas.SelectedItem = null;

            BtnGuardar.IsEnabled = true;
            BtnActualizar.IsEnabled = false;
            BtnEliminar.IsEnabled = false;
            TxtNombres.Focus();
        }

        private static void MostrarError(Exception ex)
        {
            if (ex is ArgumentException)
                MessageBox.Show(ex.Message, "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
